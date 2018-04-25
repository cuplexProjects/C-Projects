//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;
//using System.Windows.Forms;
//using DeleteDuplicateFiles.DataModels;
//using DeleteDuplicateFiles.Delegates;
//using DeleteDuplicateFiles.Models;
//using DeleteDuplicateFiles.Services;
//using GeneralToolkitLib.Hashing;
//using GeneralToolkitLib.Storage.Registry;
//using GeneralToolkitLib.WindowsApi;
//using Serilog;

//namespace DeleteDuplicateFiles.Managers
//{
//    public class tmpDuplicateFileFinder
//    {

    
//        private const int MaxEstimateRuntime = 5000;
//        private const string ZeroByteHash = "ZERO";
//        private readonly SearchProfileManager _searchProfileManager;
//        private readonly object _lockObject = new object();
//        private readonly StringBuilder _sbHexConverter;
//        private CancellationTokenSource _cancellationTokenSource;
//        private List<ScanFolderModel> _directoryList;
//        private string _fileExtentionFilter;
//        private int _filesAddedToSearch;
//        private bool _includeSubdirs;
//        private bool _runSearch;
//        private Stopwatch _runtimeStopwatch;
//        private int _totalNumberOfFiles;
//        private readonly AccessHelper _accessHelper = new AccessHelper();
//        private ApplicationSettingsModel _applicationSettingsModel;
//        public event FileHashEventHandler OnBeginNewFileHash;
//        public event FileHashEventHandler OnCompleteFileHash;
//        private readonly ComputedHashService _computedHashService;


//        public tmpDuplicateFileFinder(SearchProfileManager searchProfileManager, ComputedHashService computedHashService)
//        {
//            _searchProfileManager = searchProfileManager;
//            _computedHashService = computedHashService;
//            _sbHexConverter = new StringBuilder();
//            _cancellationTokenSource = new CancellationTokenSource();
//            LoadSettings();
//        }

//        public bool IsRunning { get; private set; }

//        public event SearchProgressEventHandler OnProgressUpdate;
//        public event EventHandler OnSearchComplete;
//        public event EventHandler OnSearchBegin;

//        public void StopSearching()
//        {
//            _runSearch = false;
//            try
//            {
//                _cancellationTokenSource.CancelAfter(2000);
//            }
//            catch (Exception exception)
//            {
//                Log.Error(exception, "Task cancellation exception on abort search");
//            }
//        }

//        public async Task<List<DuplicateFileModel>> GetDuplicateFilesAsync(List<ScanFolderModel> directoryList, string fileExtentionFilter, bool includeSubdirs)
//        {
//            if (IsRunning)
//                return null;

//            _directoryList = directoryList;
//            _fileExtentionFilter = fileExtentionFilter;
//            _includeSubdirs = includeSubdirs;
//            _runSearch = true;
//            _cancellationTokenSource = new CancellationTokenSource();
//            return await Task.Run(new Func<List<DuplicateFileModel>>(GetDuplicateFilesWrapper));
//        }

//        private void LoadSettings()
//        {
//            var registryAccess = new RegistryAccess(Application.ProductName);
//            _applicationSettingsModel = registryAccess.ReadObjectFromRegistry<ApplicationSettingsModel>() ?? new ApplicationSettingsModel();
//        }

//        private List<DuplicateFileModel> GetDuplicateFilesWrapper()
//        {
//            try
//            {
//                return GetDuplicateFiles();
//            }
//            catch (Exception ex)
//            {
//                Log.Error(ex, "DuplicateFileFinder.GetDuplicateFilesWrapper()");
//                IsRunning = false;
//                OnSearchComplete?.Invoke(this, new EventArgs());
//                return null;
//            }
//        }

//        private List<DuplicateFileModel> GetDuplicateFiles()
//        {
//            IsRunning = true;
//            OnSearchBegin?.Invoke(this, new EventArgs());
//            LoadSettings();

//            OnProgressUpdate?.Invoke(this, new SearchProgressEventArgs
//            {
//                PercentComplete = 0,
//                ProgressMessage = "Begining Search"
//            });

//            int maxLevel = _includeSubdirs ? 0 : 1;
//            bool md5Hash = _applicationSettingsModel.HashAlgorithm == ApplicationSettingsModel.HashAlgorithms.MD5;

//            // Run Test to estimate no files
//            _runtimeStopwatch = new Stopwatch();
//            _runtimeStopwatch.Start();

//            foreach (ScanFolderModel scanFolderListItem in _directoryList)
//            {
//                _totalNumberOfFiles += GetNumberOfFiles(scanFolderListItem.FullPath);
//            }

//            _runtimeStopwatch.Stop();
//            _runtimeStopwatch.Restart();

//            _filesAddedToSearch = 0;
//            var duplicateFiles = new List<DuplicateFileModel>();
//            var allFiles = new List<DuplicateFileModel>();
//            foreach (ScanFolderModel scanFolderListItem in _directoryList)
//            {
//                var subList = GetAllFiles(scanFolderListItem.FullPath, 0, maxLevel);
//                if (subList != null)
//                    allFiles.AddRange(subList);
//            }

//            _runtimeStopwatch.Stop();

//            if (allFiles.Count > 0)
//            {
//                OnProgressUpdate?.Invoke(this, new SearchProgressEventArgs
//                {
//                    PercentComplete = 0,
//                    ProgressMessage = "Initial file scan complete, found: " + allFiles.Count + " files."
//                });

//                IComparer<DuplicateFileModel> comparer = new DuplicateFileComparer(_searchProfileManager, _applicationSettingsModel.MasterFileSelectionMethod);
//                allFiles.Sort(comparer);

//                var fileGroups = allFiles.GroupBy(f => f.FileSize).Where(x => x.Count() > 1).ToList();
//                int fileCount = 0;

//                var taskList = new List<Task>();
//                var fileHashList = new List<DuplicateFileModel>();

//                // Begin setting hash values 
//                foreach (var files in fileGroups)
//                {
//                    if (!_runSearch)
//                        break;

//                    if (files.Count() < 2)
//                        continue;

//                    fileHashList.Clear();
//                    files.First().IsMaster = true;

//                    foreach (DuplicateFileModel file in files)
//                    {
//                        if (file.FileSize == 0)
//                            file.HashValue = ZeroByteHash;

//                        file.HashValue = file.HashValue ?? GetCachedHashValue(file, md5Hash);
//                        if (file.HashValue == null)
//                            fileHashList.Add(file);

//                        fileCount++;
//                    }

//                    foreach (var duplicateFileGroup in fileHashList.GroupBy(x => x.GetDriveLetter()))
//                    {
//                        taskList.Add(Task.Factory.StartNew((async () =>
//                        {
//                            try
//                            {

//                                foreach (DuplicateFileModel duplicateFile in duplicateFileGroup)
//                                {
//                                    IHashTransform hashTransform = CreateHashTransform(_applicationSettingsModel.HashAlgorithm);
//                                    duplicateFile.HashValue = await ComputeHashValueAsync(duplicateFile, hashTransform);
//                                    _computedHashService.SetHashValueForFile(duplicateFile, _applicationSettingsModel.HashAlgorithm);
//                                    if (_cancellationTokenSource.IsCancellationRequested)
//                                        break;
//                                }
//                            }
//                            catch (OperationCanceledException ex)
//                            {
//                                Log.Warning(ex, "Get duplicate files was forced to canceled before the execution had time to exit it sub procedures and loops");
//                            }
//                        })));

//                        if (_cancellationTokenSource.IsCancellationRequested)
//                            break;


//                        while (taskList.Count >= _applicationSettingsModel.MaximumNoOfHashingThreads)
//                        {
//                            Task.WaitAny(taskList.ToArray(), _cancellationTokenSource.Token);
//                            taskList.RemoveAll(x => x.Status == TaskStatus.RanToCompletion);
//                        }
//                    }

//                    if (OnProgressUpdate != null)
//                    {
//                        int percentComplete = fileCount * 100 / allFiles.Count;
//                        OnProgressUpdate.Invoke(this, new SearchProgressEventArgs
//                        {
//                            PercentComplete = percentComplete,
//                            ProgressMessage = "Duplicates found: " + duplicateFiles.Count
//                        });
//                    }

//                    if (_cancellationTokenSource.IsCancellationRequested)
//                        break;

//                    //Task.WaitAll(taskList.ToArray(), _cancellationTokenSource.Token);
//                    Task compareTask = Task.Run(async () =>
//                    {
//                        var duplicatesList = await FindDuplicateFiles(files.ToList());
//                        if (duplicatesList.Count > 0)
//                            duplicateFiles.AddRange(duplicatesList);
//                    });
//                    compareTask.Wait(_cancellationTokenSource.Token);
//                }

//                Task.WaitAll(taskList.ToArray(), _cancellationTokenSource.Token);
//                if (taskList.Any(t => t.Status == TaskStatus.Running))
//                {
//                    Log.Error("GetDuplicateFiles() taskList should not contain running tasks after main loop completed. Number of running tasks found is: {Count}", taskList.Count(t => t.Status == TaskStatus.Running));
//                    throw new ApplicationException("Invalid state, there are still running hash threadson exit loop");
//                }

//                Task.WaitAll(taskList.ToArray(), _cancellationTokenSource.Token);
//            }

//            IsRunning = false;
//            OnSearchComplete?.Invoke(this, new EventArgs());

//            return duplicateFiles;
//        }

//        private async Task<List<DuplicateFileModel>> FindDuplicateFiles(List<DuplicateFileModel> duplicateFileList)
//        {
//            return await Task.Run(() =>
//            {
//                var listOfDuplicateFiles = new List<DuplicateFileModel>();
//                foreach (var duplicateFiles in duplicateFileList.OrderBy(x => x.FileSize).GroupBy(f => f.FileSize).ToList())
//                {
//                    if (!_runSearch)
//                        break;

//                    DuplicateFileModel masterFileModel = duplicateFiles.Single(x => x.IsMaster);
//                    var subList = duplicateFiles.Where(x => !x.IsMaster).ToList();

//                    masterFileModel.DuplicateFiles = subList.Where(x => x.Equals(masterFileModel)).ToList();

//                    if (masterFileModel.DuplicateFiles.Count > 0)
//                        listOfDuplicateFiles.Add(masterFileModel);
//                }


//                return listOfDuplicateFiles;
//            });
//        }

//        private IHashTransform CreateHashTransform(ApplicationSettingsModel.HashAlgorithms hashAlgorithm)
//        {
//            if (hashAlgorithm == ApplicationSettingsModel.HashAlgorithms.CRC32)
//                return new CRC32();
//            return new MD5();
//        }

//        private string GetCachedHashValue(DuplicateFileModel fileModel, bool md5Hash)
//        {
//            if (md5Hash && _computedHashService.IsHashMD5ComputedForFile(fileModel))
//                return _computedHashService.GetMD5HashValueForFile(fileModel);

//            if (!md5Hash && _computedHashService.IsHashCRC32ComputedForFile(fileModel))
//                return _computedHashService.GetCRC32HashValueForFile(fileModel);

//            return null;
//        }

//        private async Task<string> ComputeHashValueAsync(DuplicateFileModel duplicateFileModel, IHashTransform ihashTransform)
//        {


//            FileStream fs = null;
//            try
//            {
//                fs = File.OpenRead(duplicateFileModel.FullName);
//                OnBeginNewFileHash?.Invoke(this, new FileHashEventArgs(fs.Name, fs.Length));
//                var hashBytes = await Task.Run(() => ihashTransform.ComputeHash(fs), _cancellationTokenSource.Token);
//                OnCompleteFileHash?.Invoke(this, new FileHashEventArgs());
//                return ByteArrayToHexString(hashBytes);
//            }
//            catch (TaskCanceledException taskCanceledException)
//            {
//                Log.Verbose(taskCanceledException, "ComputeHashValueAsync thread was aborted");
//                return null;
//            }
//            catch (Exception ex)
//            {
//                var inputArgs = new
//                {
//                    DuplicateFilePath = duplicateFileModel.ToString(),
//                    HashTransform = ihashTransform
//                };
//                Log.Error(ex, "DuplicateFileFinder.ComputeHashValueAsync() encountered an exception when running with the following parameters. DuplicateFile: {DuplicateFilePath} - HashTransform: {HashTransform}", inputArgs);


//                return null;
//            }
//            finally
//            {
//                fs?.Close();
//            }
//        }

//        //private string ComputeHashValue(DuplicateFile duplicateFile)
//        //{
//        //    try
//        //    {
//        //        FileStream fs = File.OpenRead(duplicateFile.FullPath);
//        //        var hashBytes = _hashTransform.ComputeHash(fs);
//        //        fs.Close();
//        //        return ByteArrayToHexString(hashBytes);
//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        LogWriter.LogError("DupicateFileFinder.ComputeHashValue()", ex);
//        //        return null;
//        //    }
//        //}

//        private string ByteArrayToHexString(IEnumerable<byte> data)
//        {
//            lock (_lockObject)
//            {
//                _sbHexConverter.Clear();

//                foreach (byte b in data)
//                    _sbHexConverter.AppendFormat("{0:X2}", b);

//                return _sbHexConverter.ToString();
//            }
//        }

//        private int GetNumberOfFiles(string rootPath)
//        {
//            int noFiles = 0;
//            string[] subDirectories;

//            if (!_runSearch)
//                return noFiles;

//            if (_runtimeStopwatch.ElapsedMilliseconds > MaxEstimateRuntime)
//                return noFiles;

//            DirectoryInfo directoryInfo = new DirectoryInfo(rootPath);
//            if (_applicationSettingsModel.IgnoreSystemFilesAndDirectories && directoryInfo.Attributes.HasFlag(FileAttributes.System))
//                return noFiles;

//            if (_applicationSettingsModel.IgnoreHiddenFilesAndDirectories && directoryInfo.Attributes.HasFlag(FileAttributes.Hidden))
//                return noFiles;

//            if (!_accessHelper.UserHasReadAccessToDirectory(directoryInfo))
//                return noFiles;

//            try
//            {
//                if (_fileExtentionFilter == null)
//                    noFiles += Directory.GetFiles(rootPath).Count();
//                else
//                    noFiles += Directory.GetFiles(rootPath, _fileExtentionFilter).Count();
//            }
//            catch (Exception ex)
//            {
//                Log.Error(ex, "DupicateFileFinder.GetNumberOfFiles cougnt and exception when trying to enumnerate files in directory: {rootPath}", rootPath);
//                return noFiles;
//            }

//            try
//            {
//                subDirectories = Directory.GetDirectories(rootPath);
//            }
//            catch (Exception ex)
//            {
//                Log.Error(ex, "DupicateFileFinder.GetNumberOfFiles cougnt and exception when trying to enumerate subdirectories in: {rootPath}");
//                return noFiles;
//            }

//            noFiles += subDirectories.Sum(GetNumberOfFiles);

//            return noFiles;
//        }

//        private List<DuplicateFileModel> GetAllFiles(string rootPath, int level, int maxLevel)
//        {
//            if (!_runSearch)
//                return null;

//            if (maxLevel != 0 && level >= maxLevel)
//                return null;

//            var duplicateFiles = new List<DuplicateFileModel>();
//            string[] fileNames;
//            string[] subDirectories;

//            try
//            {
//                fileNames = _fileExtentionFilter == null ? Directory.GetFiles(rootPath) : Directory.GetFiles(rootPath, _fileExtentionFilter);
//            }
//            catch (Exception ex)
//            {
//                Log.Error(ex, "DuplicateFileFinder.GetAllFiles() - Exception calling  Directory.GetFiles() for path: {rootPath}");
//                return null;
//            }

//            try
//            {
//                if (_applicationSettingsModel.IgnoreHiddenFilesAndDirectories || _applicationSettingsModel.IgnoreSystemFilesAndDirectories)
//                {
//                    foreach (string fileName in fileNames)
//                    {
//                        var fileInfo = new FileInfo(fileName);
//                        if (_applicationSettingsModel.IgnoreHiddenFilesAndDirectories && fileInfo.Attributes.HasFlag(FileAttributes.Hidden))
//                            continue;

//                        if (_applicationSettingsModel.IgnoreSystemFilesAndDirectories && fileInfo.Attributes.HasFlag(FileAttributes.System))
//                            continue;

//                        duplicateFiles.Add(new DuplicateFileModel
//                        {
                            
//                        });
//                    }
//                }
//                else
//                {
//                    duplicateFiles.AddRange(from fileName in fileNames
//                                            let fileInfo = new FileInfo(fileName)
//                                            select new DuplicateFileModel
//                                            {
//                                                Name = fileInfo.Name,
//                                                FullName = fileInfo.FullName,
//                                                LastWriteTime = fileInfo.LastWriteTime,
//                                                CreationTime = fileInfo.CreationTime,
//                                                FileSize = fileInfo.Length
//                                            });
//                }

//                _filesAddedToSearch += fileNames.Length;
//            }
//            catch (Exception ex)
//            {
//                Log.Error(ex, "DuplicateFileFinder.GetAllFiles() - Exeption when creating FileInfo object in directory: {rootPath}", rootPath);
//            }

//            if (OnProgressUpdate != null && _totalNumberOfFiles > 0 && _runtimeStopwatch.ElapsedMilliseconds > 250)
//            {
//                if (_filesAddedToSearch > _totalNumberOfFiles)
//                    _totalNumberOfFiles = _filesAddedToSearch;

//                int percentComplete = _filesAddedToSearch * 100 / _totalNumberOfFiles;

//                if (percentComplete > 100)
//                    percentComplete = 100;

//                OnProgressUpdate.Invoke(this, new SearchProgressEventArgs
//                {
//                    PercentComplete = percentComplete,
//                    ProgressMessage = "Files found: " + _totalNumberOfFiles
//                });
//                _runtimeStopwatch.Restart();
//            }

//            try
//            {
//                subDirectories = Directory.GetDirectories(rootPath);
//            }
//            catch (Exception ex)
//            {
//                Log.Error(ex, "DuplicateFileFinder.GetAllFiles() - Exception while calling Directory.GetDirectories() for path: {rootPath}", rootPath);
//                return null;
//            }

//            foreach (string subDirectory in subDirectories)
//            {
//                var directoryInfo = new DirectoryInfo(subDirectory);

//                if (!directoryInfo.Attributes.HasFlag(FileAttributes.Directory) || directoryInfo.Attributes.HasFlag(FileAttributes.ReparsePoint))
//                    continue;

//                if (_applicationSettingsModel.IgnoreHiddenFilesAndDirectories && directoryInfo.Attributes.HasFlag(FileAttributes.Hidden))
//                    continue;

//                if (_applicationSettingsModel.IgnoreSystemFilesAndDirectories &&
//                    (directoryInfo.Attributes.HasFlag(FileAttributes.System) || subDirectory.Equals("c:\\windows", StringComparison.OrdinalIgnoreCase)))
//                    continue;

//                var duplicateFilesInSubdirectory = GetAllFiles(subDirectory, level++, maxLevel);
//                if (duplicateFilesInSubdirectory != null)
//                    duplicateFiles.AddRange(duplicateFilesInSubdirectory);
//            }

//            return duplicateFiles;
//        }
//    }
//}