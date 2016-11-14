using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using GeneralToolkitLib.Converters;
using GeneralToolkitLib.Log;
using GeneralToolkitLib.Storage;
using GeneralToolkitLib.Storage.Models;
using ImageView.DataContracts;
using ImageView.Models;
using ImageView.Utility;

namespace ImageView.Managers
{
    public class ThumbnailManager : IDisposable
    {
        private Dictionary<string, ThumbnailEntry> _fileDictionary;
        private ThumbnailDatabase _thumbnailDatabase;
        private bool _isLoadedFromFile;
        private bool _isModified;
        private bool _isRunningThumbnailScan;
        private bool _abortScan;
        private FileManager _fileManager;
        private readonly Regex _fileNameRegExp;
        private const string DatabaseFilename = "thumbs.db";
        private const string DatabaseImgDataFilename = "thumbs.ibd";
        private const string TemporaryDatabaseFilename = "temp.ibd";
        private const string DatabaseKey = "2C1D350D-B0E5-4181-8D60-CAE050132DC1";
        private const string ImageSearchPatterb = @"^[a-zA-Z0-9_]((.+\.jpg$)|(.+\.png$)|(.+\.jpeg$)|(.+\.gif$))";

        private ThumbnailManager(string dataStoragePath)
        {
            _fileDictionary = new Dictionary<string, ThumbnailEntry>();
            _thumbnailDatabase = new ThumbnailDatabase
            {
                DatabaseId = Guid.NewGuid().ToString(),
                LasteUpdate = DateTime.Now,
                DataStroragePath = dataStoragePath,
                ThumbnailEntries = new List<ThumbnailEntry>()
            };
            _fileManager = new FileManager(dataStoragePath + DatabaseImgDataFilename);
            _fileNameRegExp = new Regex(ImageSearchPatterb, RegexOptions.IgnoreCase);
        }

        public bool IsModified
        {
            get { return _isModified; }
        }

        public static ThumbnailManager CreateNew(string dataStoragePath)
        {
            return new ThumbnailManager(dataStoragePath);
        }

        public void StartThumbnailScan(string path, IProgress<ThumbnailScanProgress> progress)
        {
            if (_isRunningThumbnailScan || _fileManager.IsLocked)
                return;

            _fileManager.CloseStream();

            _isRunningThumbnailScan = true;
            _abortScan = false;

            string[] files = Directory.GetFiles(path);
            if (!Directory.Exists(path))
                return;

            if (!path.EndsWith("\\"))
                path += "\\";

            int scannedFiles = 0;
            int filesToScan = files.Length;

            foreach (string fullPath in files)
            {
                string fileName = GeneralConverters.GetFileNameFromPath(fullPath);
                if (_abortScan)
                    break;

                if (_fileNameRegExp.IsMatch(fileName) && !ThumbnailIsCached(fullPath))
                {
                    Image img = LoadImageFromFile(fullPath);
                    FileInfo fileInfo = new FileInfo(fullPath);

                    ThumbnailEntry thumbnail = new ThumbnailEntry
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
                    _isModified = true;

                    // Update progress
                    scannedFiles++;
                    progress?.Report(new ThumbnailScanProgress {TotalAmountOfFiles = filesToScan, ScannedFiles = scannedFiles});
                }
            }

            _fileManager.CloseStream();
            SaveThumbnailDatabase();

            progress?.Report(new ThumbnailScanProgress {TotalAmountOfFiles = filesToScan, ScannedFiles = scannedFiles, IsComplete = true});
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
                if (_fileManager.IsLocked)
                    return false;

                string filename = _thumbnailDatabase.DataStroragePath + DatabaseFilename;
                var settings = new StorageManagerSettings(true, Environment.ProcessorCount, true, DatabaseKey);
                var storageManager = new StorageManager(settings);
                bool successful = storageManager.SerializeObjectToFile(_thumbnailDatabase, filename, null);

                if (successful)
                {
                    _isModified = false;
                }

                return successful;
            }
            catch (Exception ex)
            {
                LogWriter.LogError("ThumbnailManager.SaveToFile(string filename, string password) : " + ex.Message, ex);
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

                _isLoadedFromFile = true;
                var settings = new StorageManagerSettings(true, Environment.ProcessorCount, true, DatabaseKey);
                var storageManager = new StorageManager(settings);
                var thumbnailDatabase = storageManager.DeserializeObjectFromFile<ThumbnailDatabase>(filename, null);

                if (ValidateThumbnailDatabase(thumbnailDatabase))
                {
                    _thumbnailDatabase = thumbnailDatabase;
                    _fileDictionary = _thumbnailDatabase.ThumbnailEntries.ToDictionary(x => x.Directory + x.FileName, x => x);

                    _isModified = false;

                    return true;
                }
            }
            catch (Exception ex)
            {
                LogWriter.LogError("ThumbnailManager.LoadFromFile(string filename, string password) : " + ex.Message, ex);
            }

            return false;
        }

        private bool ValidateThumbnailDatabase(ThumbnailDatabase thumbnailDatabase)
        {
            if (thumbnailDatabase.ThumbnailEntries == null)
                thumbnailDatabase.ThumbnailEntries = new List<ThumbnailEntry>();

            return true;
        }

        public Image GetThumbnail(string fullPath)
        {
            if (_fileManager.IsLocked)
            {
                return LoadImageFromFile(fullPath);
            }

            if (ThumbnailIsCached(fullPath))
            {
                return LoadImageFromCache(fullPath);
            }

            Image img = LoadImageFromFile(fullPath);
            string directory = GeneralConverters.GetDirectoryNameFromPath(fullPath);
            string filename = GeneralConverters.GetFileNameFromPath(fullPath);
            AddImageToCache(img, directory, filename);
            return img;
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

                    double factor = (double) width/MaxSize;
                    width = MaxSize;
                    height = Convert.ToInt32(Math.Ceiling(height/factor));
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
                    double factor = (double) height/MaxSize;
                    height = MaxSize;
                    width = Convert.ToInt32(Math.Ceiling(width/factor));
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

            return _fileManager.ReadImage(Convert.ToInt32(thumbnail.FilePosition), thumbnail.Length);
        }

        private void AddImageToCache(Image img, string path, string fileName)
        {
            if (_fileManager == null)
                return;

            FileInfo fileInfo= new FileInfo(path + fileName);
            ThumbnailEntry thumbnail = new ThumbnailEntry
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
            _isModified = true;
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

        private class FileManager : IDisposable
        {
            private readonly string _fileName;
            private FileStream _fileStream;
            private readonly Dictionary<string, bool> _directoryAccessDictionary;
            public bool IsLocked { get; private set; }

            public FileManager(string fileName)
            {
                _fileName = fileName;
                _directoryAccessDictionary = new Dictionary<string, bool>();
            }

            public Image ReadImage(int position, int length)
            {
                if (_fileStream == null)
                    _fileStream = File.Open(_fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);

                _fileStream.Position = position;
                byte[] buffer = new byte[length];
                _fileStream.Read(buffer, 0, length);

                MemoryStream ms = new MemoryStream(buffer);
                Image img = Image.FromStream(ms);
                return img;
            }

            public FileEntry WriteImage(Image img)
            {
                if (_fileStream == null)
                    _fileStream = File.Open(_fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);

                _fileStream.Position = _fileStream.Length;
                long position = _fileStream.Position;
                img.Save(_fileStream, ImageFormat.Jpeg);

                FileEntry fileEntry = new FileEntry
                {
                    Position = position,
                    Length = Convert.ToInt32(_fileStream.Position - position)
                };

                return fileEntry;
            }

            public void RecreateDatabase(IEnumerable<ThumbnailEntry> thumbnailEntries)
            {
                if (_fileStream == null)
                    _fileStream = File.Open(_fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                else
                {
                    SaveToDisk();
                }

                string tempFileName = GeneralConverters.GetDirectoryNameFromPath(_fileName)+ TemporaryDatabaseFilename;

                FileStream temporaryDatabaseFile = null;
                try
                {
                    if (File.Exists(tempFileName))
                        File.Delete(tempFileName);

                    temporaryDatabaseFile = File.OpenWrite(tempFileName);
                    foreach (ThumbnailEntry entry in thumbnailEntries)
                    {
                        byte[] buffer = new byte[entry.Length];
                        _fileStream.Position = entry.FilePosition;
                        _fileStream.Read(buffer, 0, entry.Length);

                        entry.FilePosition = temporaryDatabaseFile.Position;
                        temporaryDatabaseFile.Write(buffer, 0, entry.Length);
                    }

                    CloseStream();
                    temporaryDatabaseFile.Close();
                    temporaryDatabaseFile = null;
                    File.Delete(_fileName);
                    File.Move(tempFileName, _fileName);
                }
                catch (Exception exception)
                {
                    LogWriter.LogError("Error in RecreateDatabase()", exception);
                }
                finally
                {
                    temporaryDatabaseFile?.Close();
                    _fileStream?.Close();
                    _fileStream = null;
                }
            }

            public void SaveToDisk()
            {
                _fileStream?.Flush(true);
            }

            public void CloseStream()
            {
                if (_fileStream != null)
                {
                    _fileStream.Flush(true);
                    _fileStream.Close();
                    _fileStream = null;
                }
            }

            public void Dispose()
            {
                CloseStream();
            }

            /// <summary>
            /// Verifies that the file does excist and that the physical file has not been written to after the thumbnail was created.
            /// Assumes access to the directory
            /// </summary>
            /// <param name="thumbnailEntry"></param>
            /// <returns>True if the thumbnail is up to date and the original file exists</returns>
            public static bool IsUpToDate(ThumbnailEntry thumbnailEntry)
            {
                FileInfo fileInfo = new FileInfo(thumbnailEntry.Directory + thumbnailEntry.FileName);
                return fileInfo.Exists && fileInfo.LastWriteTime == thumbnailEntry.SourceImageDate;
            }

            public void ClearDirectoryAccessCache()
            {
                _directoryAccessDictionary.Clear();
            }

            public bool HasAccessToDirectory(string directory)
            {
                if (_directoryAccessDictionary.ContainsKey(directory))
                    return _directoryAccessDictionary[directory];

                string volume = GeneralConverters.GetVolumeLabelFromPath(directory);
                var drives = DriveInfo.GetDrives().ToList();

                if (drives.Any(d => d.IsReady && d.Name.Equals(volume, StringComparison.CurrentCultureIgnoreCase)))
                {
                    try
                    {
                        DirectoryInfo directoryInfo = new DirectoryInfo(directory);
                        directoryInfo.EnumerateFiles();
                        _directoryAccessDictionary.Add(directory, true);
                        return true;
                    }
                    catch (Exception)
                    {
                        // ignored
                    }
                }

                _directoryAccessDictionary.Add(directory, false);
                return false;
            }

            public bool LockDatabase()
            {
                if (IsLocked)
                    return false;

                IsLocked = true;
                return true;
            }

            public void UnlockDatabase()
            {
                IsLocked = false;
            }
        }

        private class FileEntry
        {
            public long Position { get; set; }
            public int Length { get; set; }
        }

        public void Dispose()
        {
            _fileManager.Dispose();
            _fileManager = null;

            _fileDictionary = null;
            _thumbnailDatabase = null;
        }

        public int GetNumberOfCachedThumbnails()
        {
            return _thumbnailDatabase.ThumbnailEntries.Count;
        }

        public void CloseFileHandle()
        {
            _fileManager.CloseStream();
        }
    }
}