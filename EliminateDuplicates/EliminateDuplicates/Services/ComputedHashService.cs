#region

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DeleteDuplicateFiles.Models;
using GeneralToolkitLib.ConfigHelper;
using GeneralToolkitLib.Converters;
using GeneralToolkitLib.Encryption;
using GeneralToolkitLib.Hashing;
using GeneralToolkitLib.Storage;
using GeneralToolkitLib.Storage.Models;
using JetBrains.Annotations;
using Serilog;

#endregion

namespace DeleteDuplicateFiles.Services
{
    [UsedImplicitly]
    public class ComputedHashService : ServiceBase, IDisposable
    {
        private const string PasswordDerivative1 =
            "IM8BSsBACKNNPDVJ7K2c2Jr6B3eCpjfpQMYu8nH1xBLGVoOwBgysEEYdE1c7hExjMGuFVp3dWNwhQk94XV3S4Pird6VjHkjRPD5jZO2Q1Eto7Cj8irbqt8p6hGwJxDUqDm8eZjK7POqySx9fU4dgzbzXAW0SMsTOh8OO57wGDcaTicCFGJpWLBQHEHFphOWFMYNDvtxI9NawiTyli1fcTlCafHbrgqPbHz7uOA0AdebdWxp44WG80FrFd9vIKghRE6qMbPh8tCn2c6isqqfKRO8BTpFzo2HaFK7cENv18OrCsPbEuR4pTTH3OXyu0zvfHcA39NmKrpkjgpvJ39z7sUJAxfwPFGfdoU368Dd2DJ3vHGgcr8FkCc9fip2FVRtd5qGFE5cycanocIAhp2xVej6yjjJbR9YwZhkEdgYtIRzka5QMFdFACd7HSXwZnvo1xXe6eCSvaaTmOEhVSZS9WbMOVO4d5VBYH9rgvMTBnqlVyNJcWiSkYyfOUeqjGXCg";

        private const string PasswordDerivative2 =
            "FOVoCjVdfGYS3ds7dKdvFBaLWwuz9bUFsBPJOicsCACOrW0XVDtUtWfALCgWCWyT2tO4xhSIK6S9xuekAtMhcr0ku9len7jNn0RBPxE7fmVsPZw0jWqmlPed81tV0F3h2lMdaoZ1HUtRNLbtN2F1kZhAIDtyfkyeCYHLJGVEI2KFOHkBB02MKk6N9G5cPe1lyN8FVoz7uas4BvHwbDWDDb6HjQI97X8bOmnWEo5wWRYBFHI8UG0ACemj8lxX7cXz3edtR02Ybx7Tc0ZLRzyCaRcTxQCXgM682oDAcMc26BUNVrdyJSwtkXlgdszQQa0OoatnN9jyrY8NxuhqAinZ7cB8jrHUz4VhJo2YdWz4PJlC0UzmVq41f6iBXJ8h9C05qxrksAfXyHACSgb3fswC3hIzve87rx24XwJxFMmUgKa13toH1eDwUfdrqSefdkaOFR1FLmYBQPTefalHXHy8LUpu0W42nEWSQ9dFU6wAx5ZaqQnlGeDvchAlqpemUWkA";


        private readonly object _getMd5HashlockObject = new object();
        private readonly object _fileHashLock = new object();
        private readonly MD5 _md5Hash = new MD5();
        private FileHashCollection _fileHashCollection;
        private string _hashtableFilename;
        private readonly ILogger _logger;
        private readonly object _saveHashValueLockObject = new object();

        public ComputedHashService(ILogger logger)
        {
            _logger = logger;
        }

        public bool SaveComplete { get; private set; }
        public bool IsReady { get; private set; }
        private bool IsDirty { get; set; }

        private FileHashCollection FileHashCollection
        {
            get
            {
                lock (_fileHashLock)
                {
                    return _fileHashCollection;
                }

            }
            set
            {
                lock (_fileHashCollection)
                {
                    _fileHashCollection = value;
                }
            }
        }


        public void Dispose()
        {
            SaveComplete = false;
            SaveDatabase();
        }

        public event RemoveDeletedHashDbItemsEventHandler OnCompletedRemovalOfDeletedFiles;

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
            if (FileHashCollection.FileHashDictionary.ContainsKey(duplicateFile.UniqueIdHashValue))
                return FileHashCollection.FileHashDictionary[duplicateFile.UniqueIdHashValue].CRC32HashValue !=
                       null;

            return false;

        }

        public bool IsHashMD5ComputedForFile(DuplicateFile duplicateFile)
        {
            if (FileHashCollection.FileHashDictionary.ContainsKey(duplicateFile.UniqueIdHashValue))
                return FileHashCollection.FileHashDictionary[duplicateFile.UniqueIdHashValue].MD5HashValue != null;

            return false;
        }

        public bool IsHashComputedForFile(DuplicateFile duplicateFile, ProgramSettings.HashAlgorithms hashAlgorithm)
        {
            return hashAlgorithm == ProgramSettings.HashAlgorithms.MD5
                ? IsHashMD5ComputedForFile(duplicateFile)
                : IsHashCRC32ComputedForFile(duplicateFile);

        }

        public string GetCRC32HashValueForFile(DuplicateFile duplicateFile)
        {
            return FileHashCollection.FileHashDictionary.ContainsKey(duplicateFile.UniqueIdHashValue) ? FileHashCollection.FileHashDictionary[duplicateFile.UniqueIdHashValue].CRC32HashValue : null;
        }

        public string GetMD5HashValueForFile(DuplicateFile duplicateFile)
        {
            return FileHashCollection.FileHashDictionary.ContainsKey(duplicateFile.UniqueIdHashValue) ? FileHashCollection.FileHashDictionary[duplicateFile.UniqueIdHashValue].MD5HashValue : null;
        }

        public string GetHashValueForFile(DuplicateFile duplicateFile, ProgramSettings.HashAlgorithms hashAlgorithm)
        {
            if (FileHashCollection.FileHashDictionary.ContainsKey(duplicateFile.UniqueIdHashValue))
            {
                var fileHash = FileHashCollection.FileHashDictionary[duplicateFile.UniqueIdHashValue];
                return hashAlgorithm == ProgramSettings.HashAlgorithms.CRC32
                    ? fileHash.CRC32HashValue
                    : fileHash.MD5HashValue;
            }

            return null;
        }

        public void SetHashValueForFile(DuplicateFile duplicateFile, ProgramSettings.HashAlgorithms hashAlgorithm)
        {
            lock (_saveHashValueLockObject)
            {
                if (FileHashCollection.FileHashDictionary.ContainsKey(duplicateFile.UniqueIdHashValue))
                {
                    var computedFileHash =
                        FileHashCollection.FileHashDictionary[duplicateFile.UniqueIdHashValue];
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
                        FullPath = duplicateFile.FullName
                    };

                    if (hashAlgorithm == ProgramSettings.HashAlgorithms.CRC32)
                        computedFileHash.CRC32HashValue = duplicateFile.HashValue;
                    else
                        computedFileHash.MD5HashValue = duplicateFile.HashValue;

                    FileHashCollection.FileHashDictionary.Add(duplicateFile.UniqueIdHashValue, computedFileHash);
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

                foreach (var computedFileHashKey in FileHashCollection.FileHashDictionary.Keys)
                {
                    var computedFileHash = FileHashCollection.FileHashDictionary[computedFileHashKey];

                    if (availableDrives.Exists(x => x == computedFileHash.DriveLetter) &&
                        !File.Exists(computedFileHash.FullPath))
                        itemsToRemoveFromCollection.Add(computedFileHashKey);
                }
                itemsRemoved = itemsToRemoveFromCollection.Count;

                foreach (var computedFileHashKey in itemsToRemoveFromCollection)
                {
                    FileHashCollection.FileHashDictionary.Remove(computedFileHashKey);
                }

                IsDirty = itemsRemoved > 0;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "ComputedHashService.RemoveDeletedFilesFromDataBaseAsync() failed due to an exception.");
            }

            IsReady = true;
            OnCompletedRemovalOfDeletedFiles?.Invoke(this, new FileHashRemovalEventArgs { ItemsRemoved = itemsRemoved });
        }

        private void LoadDatabase()
        {
            try
            {
                if (File.Exists(_hashtableFilename))
                {
                    Task.Run(() =>
                    {
                        var settings = new StorageManagerSettings(true, Environment.ProcessorCount, true, EncryptionManager.GetDerivedPassword(PasswordDerivative1, PasswordDerivative2));
                        var storageManager = new StorageManager(settings);
                        _logger.Information("Begin loading file hash database");
                        var stopwatch = new Stopwatch();
                        stopwatch.Start();
                        FileHashCollection = storageManager.DeserializeObjectFromFile<FileHashCollection>(_hashtableFilename, null);
                        string totalFileSize = GeneralConverters.FormatFileSizeToString(FileHashCollection.FileHashDictionary.Sum(x => x.Value.FileSize));
                        stopwatch.Stop();

                        _logger.Information("Completed loading file hash database after: {Elapsed}", stopwatch.Elapsed);
                        _logger.Information("Number of files in database: {Count} - Total hashed data: {totalFileSize}", FileHashCollection.FileHashDictionary.Count, totalFileSize);

                        IsReady = true;
                    });
                }
                else
                    IsReady = true;
            }
            catch (Exception ex)
            {
                FileHashCollection = new FileHashCollection();
                IsReady = true;
                _logger.Error(ex, "ComputedHashService.LoadDatabase() failed due to an exception.");
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
                _logger.Error(ex, "ComputeHashService.SaveDatabase() failed due to an exception.");
                SaveComplete = true;
            }
        }

        private void SaveDataBaseOnNewThread()
        {
            try
            {
                var settings = new StorageManagerSettings(true, Environment.ProcessorCount, true, EncryptionManager.GetDerivedPassword(PasswordDerivative1, PasswordDerivative2));
                var storageManager = new StorageManager(settings);

                _logger.Information("Saving file hash database to disk");
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                IProgress<StorageManagerProgress> storageProgress = new Progress<StorageManagerProgress>(delegate (StorageManagerProgress progress)
                {
                    _logger.Debug("{Text} - Percent completed: {ProgressPercentage}", progress.Text, progress.ProgressPercentage);
                });
                storageManager.SerializeObjectToFile(FileHashCollection, _hashtableFilename, storageProgress);
                stopwatch.Stop();

                _logger.Information("Completed saving file hash database after {Elapsed} - Number of files in database: {Count}", stopwatch.Elapsed, FileHashCollection.FileHashDictionary.Count);
                //LogWriter.LogMessage("Completed saving file hash database after " + stopwatch.Elapsed + " - Number of files in database: " + FileHashCollection.FileHashDictionary.Count, LogWriter.LogLevel.Info);

                SaveComplete = true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "SaveDataBaseOnNewThread encountered an exception");
                SaveComplete = true;
            }
        }

        public string GetFileUniqueId(DuplicateFile duplicateFile)
        {
            lock (_getMd5HashlockObject)
            {
                return _md5Hash.ComputeHashOnString(duplicateFile.FullName);
            }
        }
    }
}