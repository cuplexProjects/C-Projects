#region

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DeleteDuplicateFiles.Delegates;
using DeleteDuplicateFiles.Models;
using DeleteDuplicateFiles.Services;
using GeneralToolkitLib.Hashing;
using GeneralToolkitLib.Storage.Registry;
using GeneralToolkitLib.WindowsApi;
using Serilog;

#endregion

namespace DeleteDuplicateFiles.Managers
{
    public class DuplicateFileFinder : ManagerBase
    {
        private const int MaxEstimateRuntime = 5000;
        private const string ZeroByteHash = "ZERO";
        private readonly SearchProfileManager _searchProfileManager;
        private readonly object _lockObject = new object();
        private readonly StringBuilder _sbHexConverter;
        private CancellationTokenSource _cancellationTokenSource;
        private List<ScanFolderListItem> _directoryList;
        private string _fileExtentionFilter;
        private int _filesAddedToSearch;
        private bool _includeSubdirs;
        private bool _runSearch;
        private Stopwatch _runtimeStopwatch;
        private int _totalNumberOfFiles;
        private readonly AccessHelper _accessHelper = new AccessHelper();
        private ProgramSettings _programSettings;
        public event FileHashEventHandler OnBeginNewFileHash;
        public event FileHashEventHandler OnCompleteFileHash;
        private readonly ComputedHashService _computedHashService;


        public DuplicateFileFinder(SearchProfileManager searchProfileManager, ComputedHashService computedHashService)
        {
            _searchProfileManager = searchProfileManager;
            _computedHashService = computedHashService;
            _sbHexConverter = new StringBuilder();
            _cancellationTokenSource = new CancellationTokenSource();
            LoadSettings();
        }

        public bool IsRunning { get; private set; }

        public event SearchProgressEventHandler OnProgressUpdate;
        public event EventHandler OnSearchComplete;
        public event EventHandler OnSearchBegin;

        public void StopSearching()
        {
            _runSearch = false;
            try
            {
                _cancellationTokenSource.CancelAfter(2000);
            }
            catch (Exception exception)
            {
                Log.Error(exception, "Task cancellation exception on abort search");
            }
        }

        public async Task<List<DuplicateFile>> GetDuplicateFilesAsync(List<ScanFolderListItem> directoryList, string fileExtentionFilter, bool includeSubdirs)
        {
            if (IsRunning)
                return null;

            _directoryList = directoryList;
            _fileExtentionFilter = fileExtentionFilter;
            _includeSubdirs = includeSubdirs;
            _runSearch = true;
            _cancellationTokenSource = new CancellationTokenSource();
            return await Task.Run(new Func<List<DuplicateFile>>(GetDuplicateFilesWrapper));
        }

        private void LoadSettings()
        {
            var registryAccess = new RegistryAccess(Application.ProductName);
            _programSettings = registryAccess.ReadObjectFromRegistry<ProgramSettings>() ?? new ProgramSettings();
        }

        private List<DuplicateFile> GetDuplicateFilesWrapper()
        {
            try
            {
                return GetDuplicateFiles();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "DuplicateFileFinder.GetDuplicateFilesWrapper()");
                IsRunning = false;
                OnSearchComplete?.Invoke(this, new EventArgs());
                return null;
            }
        }

        private List<DuplicateFile> GetDuplicateFiles()
        {
            IsRunning = true;
            OnSearchBegin?.Invoke(this, new EventArgs());
            LoadSettings();

            OnProgressUpdate?.Invoke(this, new SearchProgressEventArgs
            {
                PercentComplete = 0,
                ProgressMessage = "Begining Search"
            });

            int maxLevel = _includeSubdirs ? 0 : 1;
            bool md5Hash = _programSettings.HashAlgorithm == ProgramSettings.HashAlgorithms.MD5;

            // Run Test to estimate no files
            _runtimeStopwatch = new Stopwatch();
            _runtimeStopwatch.Start();

            foreach (ScanFolderListItem scanFolderListItem in _directoryList)
            {
                _totalNumberOfFiles += GetNumberOfFiles(scanFolderListItem.FullPath);
            }

            _runtimeStopwatch.Stop();
            _runtimeStopwatch.Restart();

            _filesAddedToSearch = 0;
            var duplicateFiles = new List<DuplicateFile>();
            var allFiles = new List<DuplicateFile>();
            foreach (ScanFolderListItem scanFolderListItem in _directoryList)
            {
                var subList = GetAllFiles(scanFolderListItem.FullPath, 0, maxLevel);
                if (subList != null)
                    allFiles.AddRange(subList);
            }

            _runtimeStopwatch.Stop();

            if (allFiles.Count > 0)
            {
                OnProgressUpdate?.Invoke(this, new SearchProgressEventArgs
                {
                    PercentComplete = 0,
                    ProgressMessage = "Initial file scan complete, found: " + allFiles.Count + " files."
                });

                IComparer<DuplicateFile> comparer = new DuplicateFileComparer(_searchProfileManager, _programSettings.MasterFileSelectionMethod);
                allFiles.Sort(comparer);

                var fileGroups = allFiles.GroupBy(f => f.FileSize).Where(x => x.Count() > 1).ToList();
                int fileCount = 0;

                var taskList = new List<Task>();
                var fileHashList = new List<DuplicateFile>();

                // Begin setting hash values 
                foreach (var files in fileGroups)
                {
                    if (!_runSearch)
                        break;

                    if (files.Count() < 2)
                        continue;

                    fileHashList.Clear();
                    files.First().IsMaster = true;

                    foreach (DuplicateFile file in files)
                    {
                        if (file.FileSize == 0)
                            file.HashValue = ZeroByteHash;

                        file.HashValue = file.HashValue ?? GetCachedHashValue(file, md5Hash);
                        if (file.HashValue == null)
                            fileHashList.Add(file);

                        fileCount++;
                    }

                    foreach (var duplicateFileGroup in fileHashList.GroupBy(x => x.GetDriveLetter()))
                    {
                        taskList.Add(Task.Run(async () =>
                        {
                            try
                            {

                                foreach (DuplicateFile duplicateFile in duplicateFileGroup)
                                {
                                    IHashTransform hashTransform = CreateHashTransform(_programSettings.HashAlgorithm);
                                    duplicateFile.HashValue = await ComputeHashValueAsync(duplicateFile, hashTransform);
                                    _computedHashService.SetHashValueForFile(duplicateFile, _programSettings.HashAlgorithm);
                                    if (_cancellationTokenSource.IsCancellationRequested)
                                        break;
                                }
                            }
                            catch (OperationCanceledException ex)
                            {
                                Log.Warning(ex, "Get duplicate files was forced to canceled before the execution had time to exit it sub procedures and loops");
                            }
                        }));

                        if (_cancellationTokenSource.IsCancellationRequested)
                            break;


                        while (taskList.Count >= _programSettings.MaximumNoOfHashingThreads)
                        {
                            Task.WaitAny(taskList.ToArray(), _cancellationTokenSource.Token);
                            taskList.RemoveAll(x => x.Status == TaskStatus.RanToCompletion);
                        }
                    }

                    if (OnProgressUpdate != null)
                    {
                        int percentComplete = fileCount * 100 / allFiles.Count;
                        OnProgressUpdate.Invoke(this, new SearchProgressEventArgs
                        {
                            PercentComplete = percentComplete,
                            ProgressMessage = "Duplicates found: " + duplicateFiles.Count
                        });
                    }

                    if (_cancellationTokenSource.IsCancellationRequested)
                        break;

                    //Task.WaitAll(taskList.ToArray(), _cancellationTokenSource.Token);
                    Task compareTask = Task.Run(async () =>
                    {
                        var duplicatesList = await FindDuplicateFiles(files.ToList());
                        if (duplicatesList.Count > 0)
                            duplicateFiles.AddRange(duplicatesList);
                    });
                    compareTask.Wait(_cancellationTokenSource.Token);
                }

                Task.WaitAll(taskList.ToArray(), _cancellationTokenSource.Token);
                if (taskList.Any(t => t.Status == TaskStatus.Running))
                {
                    Log.Error("GetDuplicateFiles() taskList should not contain running tasks after main loop completed. Number of running tasks found is: {Count}", taskList.Count(t => t.Status == TaskStatus.Running));
                    throw new ApplicationException("Invalid state, there are still running hash threadson exit loop");
                }

                Task.WaitAll(taskList.ToArray(), _cancellationTokenSource.Token);
            }

            IsRunning = false;
            OnSearchComplete?.Invoke(this, new EventArgs());

            return duplicateFiles;
        }

        private async Task<List<DuplicateFile>> FindDuplicateFiles(List<DuplicateFile> duplicateFileList)
        {
            return await Task.Run(() =>
            {
                var listOfDuplicateFiles = new List<DuplicateFile>();
                foreach (var duplicateFiles in duplicateFileList.OrderBy(x => x.FileSize).GroupBy(f => f.FileSize).ToList())
                {
                    if (!_runSearch)
                        break;

                    DuplicateFile masterFile = duplicateFiles.Single(x => x.IsMaster);
                    var subList = duplicateFiles.Where(x => !x.IsMaster).ToList();

                    masterFile.DuplicateFiles = subList.Where(x => x.Equals(masterFile)).ToList();

                    if (masterFile.DuplicateFiles.Count > 0)
                        listOfDuplicateFiles.Add(masterFile);
                }


                return listOfDuplicateFiles;
            });
        }

        private IHashTransform CreateHashTransform(ProgramSettings.HashAlgorithms hashAlgorithm)
        {
            if (hashAlgorithm == ProgramSettings.HashAlgorithms.CRC32)
                return new CRC32();
            return new MD5();
        }

        private string GetCachedHashValue(DuplicateFile file, bool md5Hash)
        {
            if (md5Hash && _computedHashService.IsHashMD5ComputedForFile(file))
                return _computedHashService.GetMD5HashValueForFile(file);

            if (!md5Hash && _computedHashService.IsHashCRC32ComputedForFile(file))
                return _computedHashService.GetCRC32HashValueForFile(file);

            return null;
        }

        private async Task<string> ComputeHashValueAsync(DuplicateFile duplicateFile, IHashTransform ihashTransform)
        {


            FileStream fs = null;
            try
            {
                fs = File.OpenRead(duplicateFile.FullName);
                OnBeginNewFileHash?.Invoke(this, new FileHashEventArgs(fs.Name, fs.Length));
                var hashBytes = await Task.Run(() => ihashTransform.ComputeHash(fs), _cancellationTokenSource.Token);
                OnCompleteFileHash?.Invoke(this, new FileHashEventArgs());
                return ByteArrayToHexString(hashBytes);
            }
            catch (TaskCanceledException taskCanceledException)
            {
                Log.Verbose(taskCanceledException, "ComputeHashValueAsync thread was aborted");
                return null;
            }
            catch (Exception ex)
            {
                var inputArgs = new
                {
                    DuplicateFilePath = duplicateFile.ToString(),
                    HashTransform = ihashTransform
                };
                Log.Error(ex, "DuplicateFileFinder.ComputeHashValueAsync() encountered an exception when running with the following parameters. DuplicateFile: {DuplicateFilePath} - HashTransform: {HashTransform}", inputArgs);


                return null;
            }
            finally
            {
                fs?.Close();
            }
        }

        //private string ComputeHashValue(DuplicateFile duplicateFile)
        //{
        //    try
        //    {
        //        FileStream fs = File.OpenRead(duplicateFile.FullPath);
        //        var hashBytes = _hashTransform.ComputeHash(fs);
        //        fs.Close();
        //        return ByteArrayToHexString(hashBytes);
        //    }
        //    catch (Exception ex)
        //    {
        //        LogWriter.LogError("DupicateFileFinder.ComputeHashValue()", ex);
        //        return null;
        //    }
        //}

        private string ByteArrayToHexString(IEnumerable<byte> data)
        {
            lock (_lockObject)
            {
                _sbHexConverter.Clear();

                foreach (byte b in data)
                    _sbHexConverter.AppendFormat("{0:X2}", b);

                return _sbHexConverter.ToString();
            }
        }

        private int GetNumberOfFiles(string rootPath)
        {
            int noFiles = 0;
            string[] subDirectories;

            if (!_runSearch)
                return noFiles;

            if (_runtimeStopwatch.ElapsedMilliseconds > MaxEstimateRuntime)
                return noFiles;

            DirectoryInfo directoryInfo = new DirectoryInfo(rootPath);
            if (_programSettings.IgnoreSystemFilesAndDirectories && directoryInfo.Attributes.HasFlag(FileAttributes.System))
                return noFiles;

            if (_programSettings.IgnoreHiddenFilesAndDirectories && directoryInfo.Attributes.HasFlag(FileAttributes.Hidden))
                return noFiles;

            if (!_accessHelper.UserHasReadAccessToDirectory(directoryInfo))
                return noFiles;

            try
            {
                if (_fileExtentionFilter == null)
                    noFiles += Directory.GetFiles(rootPath).Count();
                else
                    noFiles += Directory.GetFiles(rootPath, _fileExtentionFilter).Count();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "DupicateFileFinder.GetNumberOfFiles cougnt and exception when trying to enumnerate files in directory: {rootPath}", rootPath);
                return noFiles;
            }

            try
            {
                subDirectories = Directory.GetDirectories(rootPath);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "DupicateFileFinder.GetNumberOfFiles cougnt and exception when trying to enumerate subdirectories in: {rootPath}");
                return noFiles;
            }

            noFiles += subDirectories.Sum(GetNumberOfFiles);

            return noFiles;
        }

        private List<DuplicateFile> GetAllFiles(string rootPath, int level, int maxLevel)
        {
            if (!_runSearch)
                return null;

            if (maxLevel != 0 && level >= maxLevel)
                return null;

            var duplicateFiles = new List<DuplicateFile>();
            string[] fileNames;
            string[] subDirectories;

            try
            {
                fileNames = _fileExtentionFilter == null ? Directory.GetFiles(rootPath) : Directory.GetFiles(rootPath, _fileExtentionFilter);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "DuplicateFileFinder.GetAllFiles() - Exception calling  Directory.GetFiles() for path: {rootPath}");
                return null;
            }

            try
            {
                if (_programSettings.IgnoreHiddenFilesAndDirectories || _programSettings.IgnoreSystemFilesAndDirectories)
                {
                    foreach (string fileName in fileNames)
                    {
                        var fileInfo = new FileInfo(fileName);
                        if (_programSettings.IgnoreHiddenFilesAndDirectories && fileInfo.Attributes.HasFlag(FileAttributes.Hidden))
                            continue;

                        if (_programSettings.IgnoreSystemFilesAndDirectories && fileInfo.Attributes.HasFlag(FileAttributes.System))
                            continue;

                        duplicateFiles.Add(new DuplicateFile
                        {
                            
                        });
                    }
                }
                else
                {
                    duplicateFiles.AddRange(from fileName in fileNames
                                            let fileInfo = new FileInfo(fileName)
                                            select new DuplicateFile
                                            {
                                                Name = fileInfo.Name,
                                                FullName = fileInfo.FullName,
                                                LastWriteTime = fileInfo.LastWriteTime,
                                                CreationTime = fileInfo.CreationTime,
                                                FileSize = fileInfo.Length
                                            });
                }

                _filesAddedToSearch += fileNames.Length;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "DuplicateFileFinder.GetAllFiles() - Exeption when creating FileInfo object in directory: {rootPath}", rootPath);
            }

            if (OnProgressUpdate != null && _totalNumberOfFiles > 0 && _runtimeStopwatch.ElapsedMilliseconds > 250)
            {
                if (_filesAddedToSearch > _totalNumberOfFiles)
                    _totalNumberOfFiles = _filesAddedToSearch;

                int percentComplete = _filesAddedToSearch * 100 / _totalNumberOfFiles;

                if (percentComplete > 100)
                    percentComplete = 100;

                OnProgressUpdate.Invoke(this, new SearchProgressEventArgs
                {
                    PercentComplete = percentComplete,
                    ProgressMessage = "Files found: " + _totalNumberOfFiles
                });
                _runtimeStopwatch.Restart();
            }

            try
            {
                subDirectories = Directory.GetDirectories(rootPath);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "DuplicateFileFinder.GetAllFiles() - Exception while calling Directory.GetDirectories() for path: {rootPath}", rootPath);
                return null;
            }

            foreach (string subDirectory in subDirectories)
            {
                var directoryInfo = new DirectoryInfo(subDirectory);

                if (!directoryInfo.Attributes.HasFlag(FileAttributes.Directory) || directoryInfo.Attributes.HasFlag(FileAttributes.ReparsePoint))
                    continue;

                if (_programSettings.IgnoreHiddenFilesAndDirectories && directoryInfo.Attributes.HasFlag(FileAttributes.Hidden))
                    continue;

                if (_programSettings.IgnoreSystemFilesAndDirectories &&
                    (directoryInfo.Attributes.HasFlag(FileAttributes.System) || subDirectory.Equals("c:\\windows", StringComparison.OrdinalIgnoreCase)))
                    continue;

                var duplicateFilesInSubdirectory = GetAllFiles(subDirectory, level++, maxLevel);
                if (duplicateFilesInSubdirectory != null)
                    duplicateFiles.AddRange(duplicateFilesInSubdirectory);
            }

            return duplicateFiles;
        }
    }
}