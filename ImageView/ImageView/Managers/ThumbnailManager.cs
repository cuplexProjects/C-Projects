using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using GeneralToolkitLib.Configuration;
using GeneralToolkitLib.Converters;
using GeneralToolkitLib.Storage;
using GeneralToolkitLib.Storage.Models;
using ImageProcessor;
using ImageView.DataContracts;
using ImageView.Models;
using ImageView.Services;
using ImageView.Utility;
using JetBrains.Annotations;
using MoreLinq;
using Serilog;
using Serilog.Context;

namespace ImageView.Managers
{
    [UsedImplicitly]
    public sealed class ThumbnailManager : ManagerBase, IDisposable
    {
        private const string DatabaseFilename = "thumbs.db";
        private const string DatabaseKey = "2C1D350D-B0E5-4181-8D60-CAE050132DC1";
        private const string ImageSearchPatterb = @"^[a-zA-Z0-9_]((.+\.jpg$)|(.+\.png$)|(.+\.jpeg$)|(.+\.gif$))";
        private readonly FileManager _fileManager;
        private readonly Regex _fileNameRegExp;
        private bool _abortScan;
        private Dictionary<string, ThumbnailEntry> _fileDictionary;
        private bool _isRunningThumbnailScan;
        private ThumbnailDatabase _thumbnailDatabase;
        private readonly ImageCacheService _imageCacheService;
        private readonly Semaphore _imageLoaderSemaphore;
        private readonly Semaphore _loadAndSaveDatabase;
        private readonly ImageFactory _imageFactory;


        public ThumbnailManager(FileManager fileManager, ImageCacheService imageCacheService)
        {
            _imageFactory = new ImageFactory();
            _imageLoaderSemaphore = new Semaphore(1, 8, "ThumbnailManager");
            _loadAndSaveDatabase = new Semaphore(1, 1, "SaveAndLoadThumbnailDatabase");
            _fileManager = fileManager;
            _imageCacheService = imageCacheService;
            string dataStoragePath = ApplicationBuildConfig.UserDataPath;
            _fileDictionary = new Dictionary<string, ThumbnailEntry>();
            _thumbnailDatabase = new ThumbnailDatabase
            {
                DatabaseId = Guid.NewGuid().ToString(),
                LasteUpdate = DateTime.Now,
                DataStroragePath = dataStoragePath,
                ThumbnailEntries = new List<ThumbnailEntry>()
            };

            _fileNameRegExp = new Regex(ImageSearchPatterb, RegexOptions.IgnoreCase);
        }

        public bool IsModified { get; private set; }
        public bool IsLoaded { get; private set; }

        public void Dispose()
        {
            _fileDictionary = null;
            _thumbnailDatabase = null;
            GC.Collect();
        }

        public async Task StartThumbnailScan(string path, IProgress<ThumbnailScanProgress> progress, bool scanSubdirectories)
        {
            if (_isRunningThumbnailScan)
            {
                return;
            }

            if (!_loadAndSaveDatabase.SafeWaitHandle.IsClosed)
            {
                _loadAndSaveDatabase.WaitOne();
            }

            _fileManager.CloseStream();

            _isRunningThumbnailScan = true;
            _abortScan = false;

            if (!Directory.Exists(path))
                return;

            var dirList = scanSubdirectories ? GetSubdirectoryList(path) : new List<string>();
            dirList.Add(path);

            //int scannedFiles = dirList.TakeWhile(directory => !_abortScan).Sum(directory => PerformThumbnailScan(directory, progress));
            int totalFileCount = GetFileCount(dirList);
            int scannedFiles = await PerformThumbnailScanMultithreaded(dirList, totalFileCount, progress);

            _fileManager.CloseStream();
            SaveThumbnailDatabase();

            _isRunningThumbnailScan = false;
            progress?.Report(new ThumbnailScanProgress { TotalAmountOfFiles = scannedFiles, ScannedFiles = scannedFiles, IsComplete = true });

        }

        public int GetFileCount(List<string> dirList)
        {
            int fileCount = 0;
            foreach (string dir in dirList)
            {
                fileCount += Directory.GetFiles(dir).Length;
            }

            return fileCount;
        }

        public void StopThumbnailScan()
        {
            if (_isRunningThumbnailScan)
                _abortScan = true;
        }

        public bool SaveThumbnailDatabase()
        {
            _loadAndSaveDatabase.WaitOne();

            try
            {


                string filename = Path.Combine(ApplicationBuildConfig.UserDataPath, DatabaseFilename);
                var settings = new StorageManagerSettings(true, Environment.ProcessorCount, true, DatabaseKey);
                var storageManager = new StorageManager(settings);
                bool successful = storageManager.SerializeObjectToFile(_thumbnailDatabase, filename, null);

                if (successful)
                {
                    IsModified = false;
                }

                // Also Save the raw db file
                _fileManager.CloseStream();

                return successful;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "ThumbnailManager.SaveThumbnailDatabase() : " + ex.Message, ex);
                return false;
            }
            finally
            {
                _loadAndSaveDatabase.Release();
            }
        }

        public bool LoadThumbnailDatabase()
        {
            _loadAndSaveDatabase.WaitOne();

            try
            {
                if (_fileManager.IsLocked)
                    return false;

                IsLoaded = true;

                string filename = Path.Combine(ApplicationBuildConfig.UserDataPath, DatabaseFilename);
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
            finally
            {
                _loadAndSaveDatabase.Release();
            }

            return false;
        }

        public Image GetThumbnail(string fullPath)
        {
            if (ThumbnailIsCached(fullPath))
            {
                var imgFromCache = LoadImageFromDatabase(fullPath);
                if (imgFromCache != null)
                {
                    return imgFromCache.LoadImage();
                }

                // Possibly a corrupt database
            }


            if (_fileManager.IsLocked)
            {
                try
                {
                    return ImageProcessHelper.CreateThumbnailImage(fullPath, new Size(512, 512));
                }
                catch (Exception)
                {
                    Log.Information("Failed to load file: " + fullPath + "\nThe file might me corrupt.");
                    throw;
                }
            }



            RawImageModel img = RawImageModel.CreateFromImage(Image.FromFile(fullPath), new Size(512, 512));
            string directory = GeneralConverters.GetDirectoryNameFromPath(fullPath);
            string filename = GeneralConverters.GetFileNameFromPath(fullPath);
            AddImageToCache(img, directory, filename);
            return img.LoadImage();
        }

        public void OptimizeDatabase()
        {
            var thumbnailsToRemove = new Queue<ThumbnailEntry>();
            _fileManager.ClearDirectoryAccessCache();
            foreach (ThumbnailEntry entry in _thumbnailDatabase.ThumbnailEntries)
            {
                if (!_fileManager.HasAccessToDirectory(entry.Directory)) continue;
                if (File.Exists(Path.Combine(entry.Directory, entry.FileName)) && FileManager.IsUpToDate(entry)) continue;
                thumbnailsToRemove.Enqueue(entry);
            }

            //if (thumbnailsToRemove.Count == 0)
            //    return;

            if (_fileManager.LockDatabase())
            {
                while (thumbnailsToRemove.Count > 0)
                {
                    ThumbnailEntry entry = thumbnailsToRemove.Dequeue();
                    _thumbnailDatabase.ThumbnailEntries.Remove(entry);
                }

                //Remove possible duplicates due to data corruption.
                _thumbnailDatabase.ThumbnailEntries = _thumbnailDatabase.ThumbnailEntries.DistinctBy(x => x.Directory + x.FileName).ToList();

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
            IsLoaded = false;
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

        private async Task<int> PerformThumbnailScanMultithreaded(IEnumerable<string> dirList, int totalNumberOfFiles, IProgress<ThumbnailScanProgress> progress)
        {
            Queue<string> dirQueue = new Queue<string>(dirList);
            ConcurrentQueue<ThumbnailData> scannedThumbnailEntries = new ConcurrentQueue<ThumbnailData>();

            int threads = Environment.ProcessorCount * 2;
            int filesProccessed = 0;

            /*
             * Direcory list contains main dir and all sub dirs
             * So work must be divided into two segmentd but only one dedicated thread can list files per directory
             */

            // Work Scheduler Task
            return await Task.Factory.StartNew(() =>
            {
                while (dirQueue.Count > 0 && !_abortScan)
                {
                    var filenames = GetImageFilenamesInDirectory(dirQueue.Dequeue());

                    while (filenames.Count < threads * 4 && dirQueue.Count > 0)
                    {
                        filenames.AddRange(GetImageFilenamesInDirectory(dirQueue.Dequeue()));
                    }

                    filesProccessed += filenames.Count;
                    ParallelOptions parallelOptions = new ParallelOptions { MaxDegreeOfParallelism = threads };
                    var cancellationToken = parallelOptions.CancellationToken;

                    Queue<string> filenameQueue = new Queue<string>(filenames);

                    // Create work batch 
                    Task dataStorageTask = null;
                    var taskList = new List<Task>();
                    while (filenameQueue.Count > 0 && !_abortScan)
                    {
                        while (taskList.Count < threads && filenameQueue.Count > 0)
                        {
                            taskList.Add(Task.Factory.StartNew(() =>
                            {
                                if (filenameQueue.Count > 0)
                                {
                                    GetThumbnailEntry(filenameQueue.Dequeue(), scannedThumbnailEntries);
                                }

                            }, cancellationToken));
                        }

                        Task.WaitAny(taskList.ToArray());
                        taskList.RemoveAll(task => task.IsCompleted);

                        if (dataStorageTask == null || dataStorageTask.IsCompleted)
                        {
                            if (scannedThumbnailEntries.Count > 100)
                            {
                                dataStorageTask = Task.Factory.StartNew(() => { ProcessThumbnailData(scannedThumbnailEntries); }, cancellationToken);
                            }
                        }
                        progress?.Report(new ThumbnailScanProgress { TotalAmountOfFiles = totalNumberOfFiles, ScannedFiles = filesProccessed, IsComplete = false });
                    }

                }

                ProcessThumbnailData(scannedThumbnailEntries);

                return filesProccessed;
            });
        }

        private void ProcessThumbnailData(ConcurrentQueue<ThumbnailData> thumbnailDatas)
        {
            while (thumbnailDatas.Count > 0)
            {
                if (thumbnailDatas.TryDequeue(out var data))
                {
                    if (!_fileDictionary.ContainsKey(Path.Combine(data.ThumbnailEntry.Directory, data.ThumbnailEntry.FileName)))
                    {
                        SaveThumbnailData(data);
                    }
                }
            }
        }

        private List<string> GetImageFilenamesInDirectory(string path)
        {
            var filenameList = new List<string>();
            var fullPathFilenames = Directory.GetFiles(path);

            foreach (string fullPath in fullPathFilenames)
            {
                string fileName = GeneralConverters.GetFileNameFromPath(fullPath);
                if (!_fileNameRegExp.IsMatch(fileName) || ThumbnailIsCached(fullPath)) continue;
                filenameList.Add(fullPath);
            }

            return filenameList;
        }

        private void GetThumbnailEntry(string fullPath, ConcurrentQueue<ThumbnailData> thumbnailDatas)
        {
            if (string.IsNullOrEmpty(fullPath))
            {
                return;
            }

            string fileName = GeneralConverters.GetFileNameFromPath(fullPath);
            string directory = GeneralConverters.GetDirectoryNameFromPath(fullPath);
            var fileInfo = new FileInfo(fullPath);


            var thumbnail = new ThumbnailEntry
            {
                Date = DateTime.Now,
                SourceImageDate = fileInfo.LastWriteTime,
                FileName = fileName,
                Directory = directory,
                SourceImageLength = fileInfo.Length,
                UniqueId = Guid.NewGuid()
            };


            var thumbnailData = new ThumbnailData {ThumbnailEntry = thumbnail};
            thumbnailData.LoadImage(ImageProcessHelper.CreateThumbnailImage);

            thumbnailDatas.Enqueue(thumbnailData);
        }

        private void SaveThumbnailData(ThumbnailData thumbnailData)
        {
            var thumbnail = thumbnailData.ThumbnailEntry;

            FileEntry fileEntry = _fileManager.WriteImage(thumbnailData.RawImage);
            if (fileEntry == null)
            {
                return;
            }

            thumbnail.FilePosition = fileEntry.Position;
            thumbnail.Length = fileEntry.Length;

            _thumbnailDatabase.ThumbnailEntries.Add(thumbnail);
            _fileDictionary.Add(Path.Combine(thumbnail.Directory, thumbnail.FileName), thumbnail);
            IsModified = true;
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
                    img = ImageProcessHelper.CreateThumbnailImage(fullPath, new Size(512, 512));
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
                    Directory = path,
                    SourceImageLength = fileInfo.Length,
                    UniqueId = Guid.NewGuid()
                };

                FileEntry fileEntry = _fileManager.WriteImage(RawImageModel.CreateFromImage(img));
                if (fileEntry == null)
                {
                    continue;
                }
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
            bool UpdateToDisk = false;

            if (thumbnailDatabase.ThumbnailEntries == null)
            {
                thumbnailDatabase.ThumbnailEntries = new List<ThumbnailEntry>();
                return true;
            }

            int itemsRemovedTotal = 0;

            //Check for duplicates
            var query = (from t in thumbnailDatabase.ThumbnailEntries
                         group t by new { EntryFilePath = t.Directory + t.FileName }
                into g
                         select new { FilePath = g.Key, Count = g.Count() }).ToList();

            var duplicateKeys = query.Where(x => x.Count > 1).Select(x => x.FilePath.EntryFilePath).ToList();

            if (duplicateKeys.Count > 0)
            {
                UpdateToDisk = true;
            }

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

            // Verify that every ThumbnailEntry has a set guid 
            foreach (var thumbnailEntry in thumbnailDatabase.ThumbnailEntries.Where(x => x.UniqueId == Guid.Empty))
            {
                thumbnailEntry.UniqueId = Guid.NewGuid();
                UpdateToDisk = true;
            }

            if (UpdateToDisk)
            {
                SaveThumbnailDatabase();
            }

            return true;
        }

        private bool ThumbnailIsCached(string filename)
        {
            return _fileDictionary.ContainsKey(filename);
        }

        /// <summary>
        ///     Loads an image from file and then returns a resized version
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <returns></returns>
        //private Image LoadImageFromFile(string filename)
        //{
        //    const int maxSize = 512;
        //    Image img = _imageCacheService.GetImage(filename);
        //    int width = img.Width;
        //    int height = img.Height;

        //    if (width >= height)
        //    {
        //        if (width > maxSize)
        //        {
        //            double factor = (double)width / maxSize;
        //            width = maxSize;
        //            height = Convert.ToInt32(Math.Ceiling(height / factor));
        //        }
        //        else
        //        {
        //            return img;
        //        }
        //    }
        //    else
        //    {
        //        if (height > maxSize)
        //        {
        //            double factor = (double)height / maxSize;
        //            height = maxSize;
        //            width = Convert.ToInt32(Math.Ceiling(width / factor));
        //        }
        //        else
        //        {
        //            return img;
        //        }
        //    }

        //    return ImageTransform.ResizeImage(img, width, height);
        //}

        private RawImageModel LoadImageFromDatabase(string filename)
        {
            ThumbnailEntry thumbnail = _fileDictionary[filename];
            try
            {
                return _fileManager.ReadImageFromDatabase(thumbnail);
            }
            catch (Exception ex)
            {
                using (LogContext.PushProperty("Data", thumbnail))
                {
                    Log.Error(ex, "LoadImageFromDatabase failed");
                }
            }
            return null;
        }

        private void AddImageToCache(RawImageModel img, string path, string fileName)
        {
            if (_fileManager == null)
                return;

            var fileInfo = new FileInfo(path + fileName);
            var thumbnail = new ThumbnailEntry
            {
                Date = DateTime.Now,
                SourceImageDate = fileInfo.LastWriteTime,
                FileName = fileName,
                Directory = path,
                UniqueId = Guid.NewGuid(),
                SourceImageLength = fileInfo.Length
            };

            FileEntry fileEntry = _fileManager.WriteImage(img);

            if (fileEntry == null)
            {
                return;
            }

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

        public long GetThumbnailDbFileSize()
        {
            return _fileManager.GetDbSize();
        }

        public bool RemoveAllMissingFilesAndRecreateDb()
        {
            bool canLockDatabase = false;
            try
            {
                Queue<ThumbnailEntry> deleteQueue = new Queue<ThumbnailEntry>();

                canLockDatabase = _fileManager.LockDatabase();
                if (!canLockDatabase)
                    return false;

                foreach (var entryKey in _fileDictionary.Keys)
                {
                    var entry = _fileDictionary[entryKey];
                    if (!File.Exists(Path.Combine(entry.Directory, entry.FileName)))
                    {
                        deleteQueue.Enqueue(entry);
                    }
                }

                if (deleteQueue.Count == 0)
                    return false;

                while (deleteQueue.Count > 0)
                {
                    var item = deleteQueue.Dequeue();
                    _fileDictionary.Remove(Path.Combine(item.Directory, item.FileName));
                }

                IsModified = true;
                return true;
            }
            finally
            {
                if (canLockDatabase)
                {
                    _fileManager.UnlockDatabase();
                }
            }
        }

        /// <summary>
        ///     Reduces the size of the cach. Prioritize removing the smallest original files size images first since they are
        ///     easiest to proces yet take up equal amount of
        ///     storage as the large files.
        /// </summary>
        /// <param name="maxFileSize">Maximum size of the file.</param>
        /// <returns></returns>
        public bool ReduceCachSize(long maxFileSize)
        {
            bool canLockDatabase = false;

            try
            {
                canLockDatabase = _fileManager.LockDatabase();
                if (!canLockDatabase)
                    return false;

                // Update SourceImageLength is a new property and needs to be calculated post process when it is zero
                var filesToProcess = _fileDictionary.Values.Where(x => x.SourceImageLength == 0).Select(x => Path.Combine(x.Directory, x.FileName)).ToList();
                foreach (string fileName in filesToProcess)
                {
                    if (File.Exists(fileName))
                    {
                        var fileInfo = new FileInfo(fileName);
                        _fileDictionary[fileName].SourceImageLength = fileInfo.Length;
                    }
                    else
                    {
                        _fileDictionary.Remove(fileName);
                    }

                }

                var fileEntryList = _fileDictionary.Values.OrderBy(x => x.SourceImageLength).ToList();
                long currentSize = fileEntryList.Sum(x => x.Length);


                while (currentSize > maxFileSize)
                {
                    var element = fileEntryList.FirstOrDefault();
                    if (element != null)
                    {
                        fileEntryList.Remove(element);
                        currentSize -= element.Length;
                    }
                    else
                    {
                        break;
                    }
                }

                _fileDictionary = fileEntryList.ToDictionary(x => x.Directory + x.FileName, x => x);
                _thumbnailDatabase.ThumbnailEntries.Clear();
                _thumbnailDatabase.ThumbnailEntries.AddRange(_fileDictionary.Values);
                _fileManager.RecreateDatabase(_fileDictionary.Values.ToList());
                SaveThumbnailDatabase();
                IsModified = true;
                return true;
            }
            finally
            {
                if (canLockDatabase)
                {
                    _fileManager.UnlockDatabase();
                }
            }
        }

        public bool ClearDatabase()
        {
            if (_fileManager == null || _isRunningThumbnailScan)
            {
                return false;
            }

            try
            {
                bool isLocked = _fileManager.LockDatabase();
                if (!isLocked)
                {
                    return false;
                }

                CloseFileHandle();
                _thumbnailDatabase.ThumbnailEntries.Clear();
                _fileManager.DeleteBinaryContainer();

                // Saves the Index file
                SaveThumbnailDatabase();
            }
            catch (Exception exception)
            {
                Log.Error(exception, "Failed to clear database");
                return false;
            }
            finally
            {
                _fileManager.UnlockDatabase();
            }

            return true;
        }

        private class ThumbnailData
        {
            private RawImageModel _rawImageModel;

            public Image LoadImage(Func<string, Size, Image> imageLoadFunc)
            {
                var image = imageLoadFunc(Path.Combine(ThumbnailEntry.Directory, ThumbnailEntry.FileName), new Size(512, 512));

                var ms = new MemoryStream();
                image.Save(ms, ImageFormat.Jpeg);
                ms.Flush();

                _rawImageModel = new RawImageModel(ms.ToArray());
                ms.Close();
                ms.Dispose();

                return image;
            }

            public RawImageModel RawImage
            {
                get { return _rawImageModel; }
            }

            public ThumbnailEntry ThumbnailEntry { get; set; }

            public byte[] RawImageBytes => _rawImageModel?.ImageData;

            public Image GetImageFromRawData()
            {
                return _rawImageModel?.LoadImage();
            }
        }
    }
}