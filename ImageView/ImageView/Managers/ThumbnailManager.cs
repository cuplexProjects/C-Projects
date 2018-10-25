using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using GeneralToolkitLib.Configuration;
using GeneralToolkitLib.Converters;
using GeneralToolkitLib.Storage;
using GeneralToolkitLib.Storage.Models;
using ImageViewer.DataContracts;
using ImageViewer.Models;
using ImageViewer.Repositories;
using ImageViewer.Utility;
using JetBrains.Annotations;
using MoreLinq;
using Serilog;
using Serilog.Context;

namespace ImageViewer.Managers
{
    [UsedImplicitly]
    public class ThumbnailManager : ManagerBase, IDisposable
    {

        private const string ImageSearchPattern = @"^[a-zA-Z0-9_]((.+\.jpg$)|(.+\.png$)|(.+\.jpeg$)|(.+\.gif$))";
        private readonly Regex _fileNameRegExp;
        private bool _abortScan;
        private bool _isRunningThumbnailScan;
        private readonly ThumbnailRepository _thumbnailRepository; 


        public ThumbnailManager(ThumbnailRepository thumbnailRepository)
        {
            _thumbnailRepository = thumbnailRepository;

            _fileNameRegExp = new Regex(ImageSearchPattern, RegexOptions.IgnoreCase);
        }

        public bool IsModified { get; private set; }
        public bool IsLoaded { get; private set; }

        public void Dispose()
        {
            
            
            GC.Collect();
        }

        public async Task StartThumbnailScan(string path, IProgress<ThumbnailScanProgress> progress, bool scanSubdirectories)
        {
            if (_isRunningThumbnailScan)
            {
                return;
            }



            

            _isRunningThumbnailScan = true;
            _abortScan = false;

            if (!Directory.Exists(path))
                return;

            var dirList = scanSubdirectories ? GetSubdirectoryList(path) : new List<string>();
            dirList.Add(path);

            //int scannedFiles = dirList.TakeWhile(directory => !_abortScan).Sum(directory => PerformThumbnailScan(directory, progress));
            int totalFileCount = GetFileCount(dirList);
            int scannedFiles = await PerformThumbnailScanMultithreaded(dirList, totalFileCount, progress);

            
            _thumbnailRepository.SaveThumbnailDatabase();

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



        public bool LoadThumbnailDatabase()
        {
            try
            {



                IsLoaded = _thumbnailRepository.LoadThumbnailDatabase();

       
            }
            catch (Exception ex)
            {
                Log.Error(ex, "ThumbnailManager.LoadFromFile(string filename, string password) : " + ex.Message, ex);
            }
            finally
            {
  
            }

            return false;
        }

        public Image GetThumbnail(string fullPath)
        {
            if (ThumbnailIsCached(fullPath))
            {
                Image imgFromCache = _thumbnailRepository.GetThumbnail(fullPath);
                return imgFromCache;
                // Possibly a corrupt database
            }

             return _thumbnailRepository.CreateThumbnail(fullPath);
            
        }

        

        public int GetNumberOfCachedThumbnails()
        {
            return _thumbnailRepository.GetFileCacheCount();
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

        private async Task<int> PerformThumbnailMultiThreadScan(IEnumerable<string> dirList, int totalNumberOfFiles, IProgress<ThumbnailScanProgress> progress)
        {
            Queue<string> dirQueue = new Queue<string>(dirList);
            ConcurrentQueue<ThumbnailEntry> scannedThumbnailEntries = new ConcurrentQueue<ThumbnailEntry>();

            int threads = Environment.ProcessorCount * 2;
            int filesProcessed = 0;

            /*
             * Directory list contains main dir and all sub dirs
             * So work must be divided into two segments but only one dedicated thread can list files per directory
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

                    filesProcessed += filenames.Count;
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
                                    bool result = _thumbnailRepository.CreateThumbnail(filenameQueue.Dequeue()); // (), scannedThumbnailEntries); //     GetThumbnailEntry();
                                    if (result)
                                    {
                                        scannedThumbnailEntries.TryDequeue()
                                    }
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
                        progress?.Report(new ThumbnailScanProgress { TotalAmountOfFiles = totalNumberOfFiles, ScannedFiles = filesProcessed, IsComplete = false });
                    }

                }

                ProcessThumbnailData(scannedThumbnailEntries);

                return filesProcessed;
            });
        }

        private void ProcessThumbnailData(ConcurrentQueue<ThumbnailEntry> thumbnailDatas)
        {
            while (thumbnailDatas.Count > 0)
            {
                if (thumbnailDatas.TryDequeue(out var data))
                {
                    if (!_thumbnailRepository.ContainsCachedThumbnail(data.FullPath))
                    {
                        _thumbnailRepository.AddThumbnailItem(data);
                    }
                }
            }
        }
        

        //private void GetThumbnailEntry(string fullPath, ConcurrentQueue<ThumbnailEntry> thumbnailDatas)
        //{
        //    if (string.IsNullOrEmpty(fullPath))
        //    {
        //        return;
        //    }

        //    string fileName = GeneralConverters.GetFileNameFromPath(fullPath);
        //    string directory = GeneralConverters.GetDirectoryNameFromPath(fullPath);
        //    var fileInfo = new FileInfo(fullPath);


        //    var thumbnail = new ThumbnailEntryModel
        //    {
        //        Date = DateTime.Now,
        //        SourceImageDate = fileInfo.LastWriteTime,
        //        FileName = fileName,
        //        Directory = directory,
        //        SourceImageLength = fileInfo.Length,
        //    };


        //    var thumbnailData = new ThumbnailEntry {ThumbnailEntryModel = thumbnail};
        //    thumbnailData.LoadImage(ImageProcessHelper.CreateThumbnailImage);

        //    thumbnailDatas.Enqueue(thumbnailData);
        //}

        //private void SaveThumbnailData(ThumbnailEntry thumbnailDataModel)
        //{
        //    var thumbnail = thumbnailDataModel.ThumbnailEntryModel;

        //    FileEntry fileEntry = _fileManager.WriteImage(thumbnailDataModel.RawImage);
        //    if (fileEntry == null)
        //    {
        //        return;
        //    }

        //    thumbnail.FilePosition = fileEntry.Position;
        //    thumbnail.Length = fileEntry.Length;

        //    _thumbnailDatabaseModel.ThumbnailEntries.Add(thumbnail);
        //    _fileDictionary.Add(Path.Combine(thumbnail.Directory, thumbnail.FileName), thumbnail);
        //    IsModified = true;
        //}

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
                    img =_thumbnailRepository.CreateThumbnail(fullPath);
                }
                catch (Exception exception)
                {
                    Log.Error(exception, "Error loading file: " + fullPath);
                }

                if (img == null)
                    continue;

                var fileInfo = new FileInfo(fullPath);

                var thumbnail = new ThumbnailEntryModel
                {
                    Date = DateTime.Now,
                    SourceImageDate = fileInfo.LastWriteTime,
                    FileName = fileName,
                    Directory = path,
                    SourceImageLength = fileInfo.Length,
                };

                

                
                
                IsModified = true;

                // Update progress
                scannedFiles++;
                progress?.Report(new ThumbnailScanProgress { TotalAmountOfFiles = filesToScan, ScannedFiles = scannedFiles });
            }

            return scannedFiles;
        }

        

        private bool ThumbnailIsCached(string filename)
        {
            return _thumbnailRepository.ContainsCachedThumbnail(filename);
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

        private RawImage LoadImageFromDatabase(string filename)
        {
            ThumbnailEntryModel thumbnail = _fileDictionary[filename];
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

        private void AddImageToCache(RawImage img, string path, string fileName)
        {
            _thumbnailRepository.
        }

        public long GetThumbnailDbFileSize()
        {
            return _thumbnailRepository.GetFileCacheSize();
        }

        public bool RemoveAllMissingFilesAndRecreateDb()
        {
            try
            {
                return _thumbnailRepository.RemoveAllEntriesNotLocatedOnDisk();
            }
            finally
            {
            
            }
        }

        /// <summary>
        ///     Reduces the size of the cach. Prioritize removing the smallest original files size images first since they are
        ///     easiest to proces yet take up equal amount of
        ///     storage as the large files.
        /// </summary>
        /// <param name="maxFileSize">Maximum size of the file.</param>
        /// <returns></returns>
        public bool ReduceCacheSize(long maxFileSize)
        {
            return _thumbnailRepository.ReduceCacheSize(maxFileSize);
        }

        public bool ClearDatabase()
        {
            if (_isRunningThumbnailScan)
            {
                return false;
            }

            try
            {
            
            }
            catch (Exception exception)
            {
                Log.Error(exception, "Failed to clear database");
                return false;
            }
            finally
            {
            
            }

            return true;
        }
    }
}