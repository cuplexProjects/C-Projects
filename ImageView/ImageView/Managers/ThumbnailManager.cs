using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using GeneralToolkitLib.Converters;
using GeneralToolkitLib.Storage;
using GeneralToolkitLib.Storage.Models;
using ImageView.Configuration;
using ImageView.DataContracts;
using ImageView.Models;
using ImageView.Utility;
using Serilog;
using Serilog.Context;

namespace ImageView.Managers
{
    public class ThumbnailManager : ManagerBase, IDisposable
    {
        private const string DatabaseFilename = "thumbs.db";
        private const string DatabaseImgDataFilename = "thumbs.ibd";
        private const string TemporaryDatabaseFilename = "temp.ibd";
        private const string DatabaseKey = "2C1D350D-B0E5-4181-8D60-CAE050132DC1";
        private const string ImageSearchPatterb = @"^[a-zA-Z0-9_]((.+\.jpg$)|(.+\.png$)|(.+\.jpeg$)|(.+\.gif$))";
        private readonly Regex _fileNameRegExp;
        private bool _abortScan;
        private Dictionary<string, ThumbnailEntry> _fileDictionary;
        private bool _isRunningThumbnailScan;
        private ThumbnailDatabase _thumbnailDatabase;
        private readonly FileManager _fileManager;

        public ThumbnailManager(FileManager fileManager)
        {
            _fileManager = fileManager;
            string dataStoragePath = ApplicationBuildConfig.UserDataPath;
            _fileDictionary = new Dictionary<string, ThumbnailEntry>();
            _thumbnailDatabase = new ThumbnailDatabase
            {
                DatabaseId = Guid.NewGuid().ToString(),
                LasteUpdate = DateTime.Now,
                DataStroragePath = dataStoragePath,
                ThumbnailEntries = new List<ThumbnailEntry>()
            };
            //_fileManager = new FileManager(dataStoragePath + DatabaseImgDataFilename);
            _fileNameRegExp = new Regex(ImageSearchPatterb, RegexOptions.IgnoreCase);
        }

        public bool IsModified { get; private set; }

        public void Dispose()
        {
            //_fileManager?.Dispose();
            //_fileManager = null;

            //_fileDictionary = null;
            //_thumbnailDatabase = null;
        }

        public void StartThumbnailScan(string path, IProgress<ThumbnailScanProgress> progress, bool scanSubdirectories)
        {
            if (_isRunningThumbnailScan || _fileManager.IsLocked)
                return;

            _fileManager.CloseStream();

            _isRunningThumbnailScan = true;
            _abortScan = false;

            if (!Directory.Exists(path))
                return;

            var dirList = scanSubdirectories ? GetSubdirectoryList(path) : new List<string>();
            dirList.Add(path);
            int scannedFiles = dirList.TakeWhile(directory => !_abortScan).Sum(directory => PerformThumbnailScan(directory, progress));

            _fileManager.CloseStream();
            SaveThumbnailDatabase();

            progress?.Report(new ThumbnailScanProgress { TotalAmountOfFiles = scannedFiles, ScannedFiles = scannedFiles, IsComplete = true });
            _isRunningThumbnailScan = false;
        }

        public void StopThumbnailScan()
        {
            if (_isRunningThumbnailScan)
                _abortScan = true;
        }

        public bool SaveThumbnailDatabase()
        {
            try
            {
                //if (_fileManager.IsLocked)
                //    return false;

                string filename = _thumbnailDatabase.DataStroragePath + DatabaseFilename;
                var settings = new StorageManagerSettings(true, Environment.ProcessorCount, true, DatabaseKey);
                var storageManager = new StorageManager(settings);
                bool successful = storageManager.SerializeObjectToFile(_thumbnailDatabase, filename, null);

                if (successful)
                {
                    IsModified = false;
                }

                return successful;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "ThumbnailManager.SaveThumbnailDatabase() : " + ex.Message, ex);
                return false;
            }
        }

        public bool LoadThumbnailDatabase()
        {
            try
            {
                if (_fileManager.IsLocked)
                    return false;

                string filename = _thumbnailDatabase.DataStroragePath + DatabaseFilename;
                if (!File.Exists(filename))
                {
                    return false;
                }

                var settings = new StorageManagerSettings(true, Environment.ProcessorCount, true, DatabaseKey);
                var storageManager = new StorageManager(settings);
                var thumbnailDatabase = storageManager.DeserializeObjectFromFile<ThumbnailDatabase>(filename, null);

                if (ValidateThumbnailDatabase(thumbnailDatabase))
                {
                    _thumbnailDatabase = thumbnailDatabase;
                    _fileDictionary = _thumbnailDatabase.ThumbnailEntries.ToDictionary(x => x.Directory + x.FileName, x => x);

                    IsModified = false;

                    return true;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "ThumbnailManager.LoadFromFile(string filename, string password) : " + ex.Message, ex);
            }

            return false;
        }

        public Image GetThumbnail(string fullPath)
        {
            if (_fileManager.IsLocked)
            {
                try
                {
                    return LoadImageFromFile(fullPath);
                }
                catch (Exception)
                {
                    Log.Information("Failed to load file: " + fullPath + "\nThe file might me corrupt.");
                    throw;
                }
            }

            if (ThumbnailIsCached(fullPath))
            {
                var imgFromCache = LoadImageFromCache(fullPath);
                if (imgFromCache != null)
                {
                    return imgFromCache;
                }

                // Possibly a corrupt database
            }

            Image img = LoadImageFromFile(fullPath);
            string directory = GeneralConverters.GetDirectoryNameFromPath(fullPath);
            string filename = GeneralConverters.GetFileNameFromPath(fullPath);
            AddImageToCache(img, directory, filename);
            return img;
        }

        public void OptimizeDatabase()
        {
            var thumbnailsToRemove = new Queue<ThumbnailEntry>();
            _fileManager.ClearDirectoryAccessCache();
            foreach (ThumbnailEntry entry in _thumbnailDatabase.ThumbnailEntries)
            {
                if (!_fileManager.HasAccessToDirectory(entry.Directory)) continue;
                if (FileManager.IsUpToDate(entry)) continue;
                thumbnailsToRemove.Enqueue(entry);
            }

            if (thumbnailsToRemove.Count == 0)
                return;

            if (_fileManager.LockDatabase())
            {
                while (thumbnailsToRemove.Count > 0)
                {
                    ThumbnailEntry entry = thumbnailsToRemove.Dequeue();
                    _thumbnailDatabase.ThumbnailEntries.Remove(entry);
                }

                _fileManager.RecreateDatabase(_thumbnailDatabase.ThumbnailEntries);
                _fileDictionary = _thumbnailDatabase.ThumbnailEntries.ToDictionary(x => x.Directory + x.FileName, x => x);
                _fileManager.UnlockDatabase();
                SaveThumbnailDatabase();
            }
        }

        public int GetNumberOfCachedThumbnails()
        {
            return _thumbnailDatabase.ThumbnailEntries.Count;
        }

        public void CloseFileHandle()
        {
            _fileManager.CloseStream();
        }

        private List<string> GetSubdirectoryList(string path)
        {
            var subdirectoryList = new List<string>();
            var directories = Directory.GetDirectories(path);

            foreach (string directory in directories)
            {
                subdirectoryList.Add(directory);
                subdirectoryList.AddRange(GetSubdirectoryList(directory));
            }

            return subdirectoryList;
        }

        private int PerformThumbnailScan(string path, IProgress<ThumbnailScanProgress> progress)
        {
            var files = Directory.GetFiles(path);
            if (!Directory.Exists(path))
                return 0;

            if (!path.EndsWith("\\"))
                path += "\\";

            int filesToScan = files.Length;
            int scannedFiles = 0;

            if (_abortScan)
                return 0;

            foreach (string fullPath in files)
            {
                string fileName = GeneralConverters.GetFileNameFromPath(fullPath);
                if (_abortScan)
                    break;

                if (!_fileNameRegExp.IsMatch(fileName) || ThumbnailIsCached(fullPath)) continue;
                Image img = null;

                try
                {
                    img = LoadImageFromFile(fullPath);
                }
                catch (Exception exception)
                {
                    Log.Error(exception, "Error loading file: " + fullPath);
                }

                if (img == null)
                    continue;

                var fileInfo = new FileInfo(fullPath);

                var thumbnail = new ThumbnailEntry
                {
                    Date = DateTime.Now,
                    SourceImageDate = fileInfo.LastWriteTime,
                    FileName = fileName,
                    Directory = path
                };

                FileEntry fileEntry = _fileManager.WriteImage(img);
                thumbnail.FilePosition = fileEntry.Position;
                thumbnail.Length = fileEntry.Length;

                _thumbnailDatabase.ThumbnailEntries.Add(thumbnail);
                _fileDictionary.Add(path + fileName, thumbnail);
                IsModified = true;

                // Update progress
                scannedFiles++;
                progress?.Report(new ThumbnailScanProgress { TotalAmountOfFiles = filesToScan, ScannedFiles = scannedFiles });
            }

            return scannedFiles;
        }

        private bool ValidateThumbnailDatabase(ThumbnailDatabase thumbnailDatabase)
        {
            if (thumbnailDatabase.ThumbnailEntries == null)
            {
                thumbnailDatabase.ThumbnailEntries = new List<ThumbnailEntry>();
                return true;
            }

            int itemsRemovedTotal = 0;

            //Check for duplicates
            var query = (from t in thumbnailDatabase.ThumbnailEntries
                group t by new {EntryFilePath = t.Directory + t.FileName}
                into g
                select new {FilePath = g.Key, Count = g.Count()}).ToList();

            var duplicateKeys = query.Where(x => x.Count > 1).Select(x => x.FilePath.EntryFilePath).ToList();

            foreach (string duplicateKey in duplicateKeys)
            {
                var removeItemsList = thumbnailDatabase.ThumbnailEntries.Where(x => x.Directory + x.FileName == duplicateKey).ToList();

                itemsRemovedTotal += removeItemsList.Count - 1;
                removeItemsList.RemoveAt(0);
                foreach (var removableItem in removeItemsList)
                {
                    thumbnailDatabase.ThumbnailEntries.Remove(removableItem);
                }
            }

            if (itemsRemovedTotal > 0)
            {
                Log.Information("Removed {itemsRemovedTotal} duplicate items from the thumbnailcache", itemsRemovedTotal);
            }

            return true;
        }

        private bool ThumbnailIsCached(string filename)
        {
            return _fileDictionary.ContainsKey(filename);
        }

        private Image LoadImageFromFile(string filename)
        {
            const int MaxSize = 512;
            Image img = Image.FromFile(filename);
            int width = img.Width;
            int height = img.Height;

            if (width >= height)
            {
                if (width > MaxSize)
                {
                    double factor = (double)width / MaxSize;
                    width = MaxSize;
                    height = Convert.ToInt32(Math.Ceiling(height / factor));
                }
                else
                {
                    return img;
                }
            }
            else
            {
                if (height > MaxSize)
                {
                    double factor = (double)height / MaxSize;
                    height = MaxSize;
                    width = Convert.ToInt32(Math.Ceiling(width / factor));
                }
                else
                {
                    return img;
                }
            }

            return ImageTransform.ResizeImage(img, width, height);
        }

        private Image LoadImageFromCache(string filename)
        {
            ThumbnailEntry thumbnail = _fileDictionary[filename];
            try
            {
                return _fileManager.ReadImage(Convert.ToInt32(thumbnail.FilePosition), thumbnail.Length);
            }
            catch (Exception ex)
            {
                using (LogContext.PushProperty("Data", thumbnail))
                {
                    Log.Error(ex, "LoadImageFromCache failed");
                }
            }
            return null;
        }

        private void AddImageToCache(Image img, string path, string fileName)
        {
            if (_fileManager == null)
                return;

            var fileInfo = new FileInfo(path + fileName);
            var thumbnail = new ThumbnailEntry
            {
                Date = DateTime.Now,
                SourceImageDate = fileInfo.LastWriteTime,
                FileName = fileName,
                Directory = path
            };

            FileEntry fileEntry = _fileManager.WriteImage(img);
            thumbnail.FilePosition = fileEntry.Position;
            thumbnail.Length = fileEntry.Length;

            _thumbnailDatabase.ThumbnailEntries.Add(thumbnail);

            // In case of index db corruption
            if (!_fileDictionary.ContainsKey(path + fileName))
            {
                _fileDictionary.Add(path + fileName, thumbnail);
                IsModified = true;
            }
        }
    }
}