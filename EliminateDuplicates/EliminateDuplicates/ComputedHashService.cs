#region

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DeleteDuplicateFiles.Delegates;
using DeleteDuplicateFiles.Models;
using GeneralToolkitLib.ConfigHelper;
using GeneralToolkitLib.Converters;
using GeneralToolkitLib.Encryption;
using GeneralToolkitLib.Hashing;
using GeneralToolkitLib.Log;
using GeneralToolkitLib.Storage;
using GeneralToolkitLib.Storage.Models;

#endregion

namespace DeleteDuplicateFiles
{
    public class ComputedHashService : IDisposable
    {
        private const string PasswordDerivative1 =
            "IM8BSsBACKNNPDVJ7K2c2Jr6B3eCpjfpQMYu8nH1xBLGVoOwBgysEEYdE1c7hExjMGuFVp3dWNwhQk94XV3S4Pird6VjHkjRPD5jZO2Q1Eto7Cj8irbqt8p6hGwJxDUqDm8eZjK7POqySx9fU4dgzbzXAW0SMsTOh8OO57wGDcaTicCFGJpWLBQHEHFphOWFMYNDvtxI9NawiTyli1fcTlCafHbrgqPbHz7uOA0AdebdWxp44WG80FrFd9vIKghRE6qMbPh8tCn2c6isqqfKRO8BTpFzo2HaFK7cENv18OrCsPbEuR4pTTH3OXyu0zvfHcA39NmKrpkjgpvJ39z7sUJAxfwPFGfdoU368Dd2DJ3vHGgcr8FkCc9fip2FVRtd5qGFE5cycanocIAhp2xVej6yjjJbR9YwZhkEdgYtIRzka5QMFdFACd7HSXwZnvo1xXe6eCSvaaTmOEhVSZS9WbMOVO4d5VBYH9rgvMTBnqlVyNJcWiSkYyfOUeqjGXCg";

        private const string PasswordDerivative2 =
            "FOVoCjVdfGYS3ds7dKdvFBaLWwuz9bUFsBPJOicsCACOrW0XVDtUtWfALCgWCWyT2tO4xhSIK6S9xuekAtMhcr0ku9len7jNn0RBPxE7fmVsPZw0jWqmlPed81tV0F3h2lMdaoZ1HUtRNLbtN2F1kZhAIDtyfkyeCYHLJGVEI2KFOHkBB02MKk6N9G5cPe1lyN8FVoz7uas4BvHwbDWDDb6HjQI97X8bOmnWEo5wWRYBFHI8UG0ACemj8lxX7cXz3edtR02Ybx7Tc0ZLRzyCaRcTxQCXgM682oDAcMc26BUNVrdyJSwtkXlgdszQQa0OoatnN9jyrY8NxuhqAinZ7cB8jrHUz4VhJo2YdWz4PJlC0UzmVq41f6iBXJ8h9C05qxrksAfXyHACSgb3fswC3hIzve87rx24XwJxFMmUgKa13toH1eDwUfdrqSefdkaOFR1FLmYBQPTefalHXHy8LUpu0W42nEWSQ9dFU6wAx5ZaqQnlGeDvchAlqpemUWkA";

        private static ComputedHashService _instance;
        private static readonly object InstanceLockObj = new object();
        private readonly object _getMd5HashlockObject = new object();
        private readonly object _lockObject = new object();
        private readonly MD5 _md5Hash = new MD5();
        private FileHashCollection _fileHashCollection;
        private string _hashtableFilename;

        private ComputedHashService()
        {
        }

        public bool SaveComplete { get; private set; }
        public bool IsReady { get; private set; }
        private bool IsDirty { get; set; }

        public static ComputedHashService Instance
        {
            get
            {
                lock (InstanceLockObj)
                {
                    if (_instance == null)
                    {
                        _instance = new ComputedHashService();
                    }
                }

                return _instance;
            }
        }

        public void Dispose()
        {
            SaveComplete = false;
            SaveDatabase();
            _instance = null;
        }

        public event RemoveDeletedHashDBItemsEventHandler OnCompletedRemovalOfDeletedFiles;

        public void Init()
        {
            _hashtableFilename = GlobalSettings.GetHashtableFilename();
            _fileHashCollection = new FileHashCollection();
            IsDirty = false;
            IsReady = false;
            LoadDatabase();
        }

        public bool IsHashCRC32ComputedForFile(DuplicateFile duplicateFile)
        {
            lock (_lockObject)
            {
                if (_fileHashCollection.FileHashDictionary.ContainsKey(duplicateFile.UniqueIdHashValue))
                    return _fileHashCollection.FileHashDictionary[duplicateFile.UniqueIdHashValue].CRC32HashValue !=
                           null;

                return false;
            }
        }

        public bool IsHashMD5ComputedForFile(DuplicateFile duplicateFile)
        {
            lock (_lockObject)
            {
                if (_fileHashCollection.FileHashDictionary.ContainsKey(duplicateFile.UniqueIdHashValue))
                    return _fileHashCollection.FileHashDictionary[duplicateFile.UniqueIdHashValue].MD5HashValue != null;

                return false;
            }
        }

        public bool IsHashComputedForFile(DuplicateFile duplicateFile, ProgramSettings.HashAlgorithms hashAlgorithm)
        {
            lock (_lockObject)
            {
                return hashAlgorithm == ProgramSettings.HashAlgorithms.MD5
                    ? IsHashMD5ComputedForFile(duplicateFile)
                    : IsHashCRC32ComputedForFile(duplicateFile);
            }
        }

        public string GetCRC32HashValueForFile(DuplicateFile duplicateFile)
        {
            lock (_lockObject)
            {
                if (_fileHashCollection.FileHashDictionary.ContainsKey(duplicateFile.UniqueIdHashValue))
                    return _fileHashCollection.FileHashDictionary[duplicateFile.UniqueIdHashValue].CRC32HashValue;

                return null;
            }
        }

        public string GetMD5HashValueForFile(DuplicateFile duplicateFile)
        {
            lock (_lockObject)
            {
                if (_fileHashCollection.FileHashDictionary.ContainsKey(duplicateFile.UniqueIdHashValue))
                    return _fileHashCollection.FileHashDictionary[duplicateFile.UniqueIdHashValue].MD5HashValue;

                return null;
            }
        }

        public string GetHashValueForFile(DuplicateFile duplicateFile, ProgramSettings.HashAlgorithms hashAlgorithm)
        {
            lock (_lockObject)
            {
                if (_fileHashCollection.FileHashDictionary.ContainsKey(duplicateFile.UniqueIdHashValue))
                {
                    var fileHash = _fileHashCollection.FileHashDictionary[duplicateFile.UniqueIdHashValue];
                    return hashAlgorithm == ProgramSettings.HashAlgorithms.CRC32
                        ? fileHash.CRC32HashValue
                        : fileHash.MD5HashValue;
                }

                return null;
            }
        }

        public void SetHashValueForFile(DuplicateFile duplicateFile, ProgramSettings.HashAlgorithms hashAlgorithm)
        {
            lock (_lockObject)
            {
                if (_fileHashCollection.FileHashDictionary.ContainsKey(duplicateFile.UniqueIdHashValue))
                {
                    var computedFileHash =
                        _fileHashCollection.FileHashDictionary[duplicateFile.UniqueIdHashValue];
                    if (hashAlgorithm == ProgramSettings.HashAlgorithms.CRC32)
                        computedFileHash.CRC32HashValue = duplicateFile.HashValue;
                    else
                        computedFileHash.MD5HashValue = duplicateFile.HashValue;
                }
                else
                {
                    var computedFileHash = new ComputedFileHash
                    {
                        LastWriteTime = duplicateFile.LastWriteTime,
                        CreationTime = duplicateFile.CreationTime,
                        FileSize = duplicateFile.FileSize,
                        FullPath = duplicateFile.FullPath
                    };

                    if (hashAlgorithm == ProgramSettings.HashAlgorithms.CRC32)
                        computedFileHash.CRC32HashValue = duplicateFile.HashValue;
                    else
                        computedFileHash.MD5HashValue = duplicateFile.HashValue;

                    _fileHashCollection.FileHashDictionary.Add(duplicateFile.UniqueIdHashValue, computedFileHash);
                }
                IsDirty = true;
            }
        }

        public void RemoveDeletedFilesFromDataBase()
        {
            if (IsReady)
                Task.Run(() => RemoveDeletedFilesFromDataBaseAsync());
        }

        private void RemoveDeletedFilesFromDataBaseAsync()
        {
            IsReady = false;
            var itemsRemoved = 0;
            try
            {
                var driveInfos = DriveInfo.GetDrives();
                var availableDrives = driveInfos.Select(x => x.Name).ToList();
                var itemsToRemoveFromCollection = new List<string>();

                foreach (var computedFileHashKey in _fileHashCollection.FileHashDictionary.Keys)
                {
                    var computedFileHash = _fileHashCollection.FileHashDictionary[computedFileHashKey];

                    if (availableDrives.Exists(x => x == computedFileHash.DriveLetter) &&
                        !File.Exists(computedFileHash.FullPath))
                        itemsToRemoveFromCollection.Add(computedFileHashKey);
                }
                itemsRemoved = itemsToRemoveFromCollection.Count;

                foreach (var computedFileHashKey in itemsToRemoveFromCollection)
                {
                    _fileHashCollection.FileHashDictionary.Remove(computedFileHashKey);
                }

                IsDirty = itemsRemoved > 0;
            }
            catch (Exception ex)
            {
                LogWriter.LogError("ComputedHashService.RemoveDeletedFilesFromDataBaseAsync()", ex);
            }

            IsReady = true;
            if (OnCompletedRemovalOfDeletedFiles != null)
                OnCompletedRemovalOfDeletedFiles.Invoke(this, new FileHashRemovalEventArgs {ItemsRemoved = itemsRemoved});
        }

        private void LoadDatabase()
        {
            try
            {
                if (File.Exists(_hashtableFilename))
                {
                    Task.Run(() =>
                    {
                        StorageManagerSettings settings = new StorageManagerSettings(true, Environment.ProcessorCount,true,EncryptionManager.GetDerivedPassword(PasswordDerivative1, PasswordDerivative2));
                        var storageManager = new StorageManager(settings);
                        LogWriter.LogMessage("Begin loading file hash database", LogWriter.LogLevel.Info);
                        Stopwatch stopwatch= new Stopwatch();
                        stopwatch.Start();
                        _fileHashCollection = storageManager.DeserializeObjectFromFile<FileHashCollection>(_hashtableFilename, null);
                        string totalFileSize = GeneralConverters.FormatFileSizeToString(_fileHashCollection.FileHashDictionary.Sum(x => x.Value.FileSize));
                        stopwatch.Stop();
                        LogWriter.LogMessage("Completed loading file hash database after " + stopwatch.Elapsed, LogWriter.LogLevel.Info);
                        LogWriter.LogMessage(string.Format("Number of files in database: {0} - Total hashed data: {1}", _fileHashCollection.FileHashDictionary.Count, totalFileSize), LogWriter.LogLevel.Info);
                        IsReady = true;
                    });
                }
                else
                    IsReady = true;
            }
            catch (Exception ex)
            {
                _fileHashCollection = new FileHashCollection();
                IsReady = true;
                LogWriter.LogError("ComputedHashService.LoadDatabase()", ex);
            }
        }

        private void SaveDatabase()
        {
            try
            {
                if (!IsDirty) return;
                var thread = new Thread(SaveDataBaseOnNewThread);
                thread.Start();
            }
            catch (Exception ex)
            {
                LogWriter.LogError("ComputeHashService.SaveDatabase()", ex);
                SaveComplete = true;
            }
        }

        private void SaveDataBaseOnNewThread()
        {
            try
            {
                var settings = new StorageManagerSettings(true,Environment.ProcessorCount,true,EncryptionManager.GetDerivedPassword(PasswordDerivative1, PasswordDerivative2));
                var storageManager = new StorageManager(settings);

                LogWriter.LogMessage("Saving file hash database to disk", LogWriter.LogLevel.Info);
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                IProgress<StorageManagerProgress> storageProgress = new Progress<StorageManagerProgress>(delegate(StorageManagerProgress progress)
                {
                    LogWriter.LogMessage(progress.Text + " - Percent completed: " + progress.ProgressPercentage, LogWriter.LogLevel.Debug);
                });
                storageManager.SerializeObjectToFile(_fileHashCollection, _hashtableFilename, storageProgress);
                stopwatch.Stop();

                LogWriter.LogMessage("Completed saving file hash database after " + stopwatch.Elapsed + " - Number of files in database: " + _fileHashCollection.FileHashDictionary.Count, LogWriter.LogLevel.Info);

                SaveComplete = true;
            }
            catch (Exception ex)
            {
                LogWriter.LogError("ComputeHashService.SaveDataBaseOnNewThread", ex);
                SaveComplete = true;
            }
        }

        public string GetFileUniqueId(DuplicateFile duplicateFile)
        {
            lock (_getMd5HashlockObject)
            {
                return _md5Hash.ComputeHashOnString(duplicateFile.FullPath);
            }
        }
    }
}