using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GeneralToolkitLib.Log;
using GeneralToolkitLib.Storage;
using GeneralToolkitLib.Storage.Models;
using SecureMemo.DataModels;
using SecureMemo.Services;
using SecureMemo.Utility;

namespace SecureMemo.Storage
{
    public class MemoStorageService : IDisposable
    {
        private const string databaeFileName = "MemoDatabase.dat";
        private const string confSaltVal = "l73hgwiHLwscWqHQUT7vwJSTX58K0XWZlecm77NbzmqbsF60LOEeftqSdeSvL6cB";
        private const string confSaltVal2 = "BxV0CQsjr6f7MbiXTqdpHN4bjyhqUX9Yd79zA2vRZLGPQj0qdTGlTwBFiK7eiFqc";
        private static MemoStorageService _instance;
        private readonly string databaseFilePath;

        private MemoStorageService()
        {
            databaseFilePath = ConfigSpecificSettings.GetSettingsFolderPath(false);
        }

        public static MemoStorageService Instance => _instance ?? (_instance = new MemoStorageService());

        public void Dispose()
        {
            _instance = null;
        }

        private string GetFullPathToDatabaseFile()
        {
            return databaseFilePath + "\\" + databaeFileName;
        }

        private string GetFullPathToSharedDatabaseFile()
        {
            return AppSettingsService.Instance.Settings.SyncFolderPath + "\\" + databaeFileName;
        }

        private string GetFullPathToSharedEcryptedConfigFile()
        {
            return AppSettingsService.Instance.Settings.SyncFolderPath + "\\" + "ApplicationSettings.dat";
        }

        public bool DatabaseExists()
        {
            return File.Exists(GetFullPathToDatabaseFile());
        }

        public TabPageDataCollection LoadTabPageCollection(string password)
        {
            TabPageDataCollection tabPageDataCollection = null;
            try
            {
                var settings = new StorageManagerSettings(true, Environment.ProcessorCount, true, password);
                var storageManager = new StorageManager(settings);
                tabPageDataCollection = storageManager.DeserializeObjectFromFile<TabPageDataCollection>(GetFullPathToDatabaseFile(), null);
            }
            catch (Exception ex)
            {
                LogWriter.LogError("Error loading database", ex);
            }
            return tabPageDataCollection;
        }

        public bool SaveTabPageCollection(TabPageDataCollection tabPageDataCollection, string password, bool saveToSharedFolder = false)
        {
            bool success = false;
            try
            {
                var settings = new StorageManagerSettings(true, Environment.ProcessorCount, true, password);
                var storageManager = new StorageManager(settings);

                if (saveToSharedFolder)
                {
                    string encodedConfigFilePath = GetFullPathToSharedDatabaseFile();
                    if (File.Exists(encodedConfigFilePath))
                        File.Delete(encodedConfigFilePath);

                    settings.Password = confSaltVal + settings.Password + confSaltVal2;
                    File.Copy(GetFullPathToDatabaseFile(), encodedConfigFilePath);
                    success = storageManager.SerializeObjectToFile(AppSettingsService.Instance.Settings, GetFullPathToSharedEcryptedConfigFile(), null);
                }
                else
                    success = storageManager.SerializeObjectToFile(tabPageDataCollection, GetFullPathToDatabaseFile(), null);
            }
            catch (Exception ex)
            {
                LogWriter.LogError("Error saving database", ex);
            }

            return success;
        }

        public IEnumerable<BackupFileInfo> GetBackupFiles()
        {
            string backupPath = databaseFilePath + "\\backup\\";
            var dirInfo = new DirectoryInfo(backupPath);

            if (!dirInfo.Exists)
                return null;

            var backupFiles = dirInfo.GetFiles("*MemoDatabase.dat");
            return backupFiles.Select(backupFile => new BackupFileInfo {Name = backupFile.Name, CreatedDate = backupFile.CreationTime, FullName = backupFile.FullName}).ToList();
        }

        public void MakeBackup()
        {
            if (!Directory.Exists(databaseFilePath))
                throw new Exception("No database found, create a new database first.");

            string backupPath = databaseFilePath + "\\backup\\";

            if (!Directory.Exists(backupPath))
                Directory.CreateDirectory(backupPath);

            File.Copy(GetFullPathToDatabaseFile(), backupPath + DateTime.Now.ToString("yyyyMMdd_HH.mm.ss_") + databaeFileName);
        }

        public void RestoreBackup(BackupFileInfo backupFileInfo)
        {
            if (!File.Exists(backupFileInfo.FullName))
                throw new Exception("Backupfile with name: " + backupFileInfo.Name + " does not exist!");

            string pathToDatabaseFile = GetFullPathToDatabaseFile();

            if (File.Exists(pathToDatabaseFile))
                File.Delete(pathToDatabaseFile);

            File.Copy(backupFileInfo.FullName, pathToDatabaseFile);
            File.Delete(backupFileInfo.FullName);
        }

        public RestoreSyncDataResult RestoreBackupFromSyncFolder(string password)
        {
            var restoreSyncDataResult = new RestoreSyncDataResult();
            string dbFilePath = GetFullPathToDatabaseFile();
            try
            {
                if (!File.Exists(GetFullPathToSharedDatabaseFile()))
                    restoreSyncDataResult.ErrorCode = RestoreSyncDataErrorCodes.MemoDatabaseFileNotFound;
                else if (!File.Exists(GetFullPathToSharedEcryptedConfigFile()))
                    restoreSyncDataResult.ErrorCode = restoreSyncDataResult.ErrorCode | RestoreSyncDataErrorCodes.ApplicationSettingsFileNotFound;
                else
                {
                    var settings = new StorageManagerSettings(true, Environment.ProcessorCount, true, confSaltVal + password + confSaltVal2);
                    var storageManager = new StorageManager(settings);

                    var secureMemoAppSettings = storageManager.DeserializeObjectFromFile<SecureMemoAppSettings>(GetFullPathToSharedEcryptedConfigFile(), null);

                    if (string.IsNullOrWhiteSpace(secureMemoAppSettings.ApplicationSaltValue) || string.IsNullOrWhiteSpace(secureMemoAppSettings.PasswordDerivedString))
                    {
                        restoreSyncDataResult.ErrorCode = restoreSyncDataResult.ErrorCode | RestoreSyncDataErrorCodes.ApplicationSettingsFileParseError;
                        restoreSyncDataResult.ErrorText = "Invalid password";
                    }
                    else
                    {
                        AppSettingsService.Instance.Settings.ApplicationSaltValue = secureMemoAppSettings.ApplicationSaltValue;
                        AppSettingsService.Instance.Settings.PasswordDerivedString = secureMemoAppSettings.PasswordDerivedString;
                        AppSettingsService.Instance.Settings.FontSettings = secureMemoAppSettings.FontSettings;
                        AppSettingsService.Instance.Settings.UseSharedSyncFolder = secureMemoAppSettings.UseSharedSyncFolder;
                        AppSettingsService.Instance.Settings.DefaultEmptyTabPages = secureMemoAppSettings.DefaultEmptyTabPages;
                        AppSettingsService.Instance.Settings.MainWindowHeight = secureMemoAppSettings.MainWindowHeight;
                        AppSettingsService.Instance.Settings.MainWindowWith = secureMemoAppSettings.MainWindowWith;
                        AppSettingsService.Instance.SaveSettings();
                        AppSettingsService.Instance.LoadSettings();

                        if (File.Exists(dbFilePath))
                            File.Delete(dbFilePath);

                        File.Copy(GetFullPathToSharedDatabaseFile(), dbFilePath);
                        restoreSyncDataResult.Successful = true;
                    }
                }
            }
            catch (Exception ex)
            {
                LogWriter.LogError("Error when calling RestoreBackupFromSyncFolder()", ex);
                restoreSyncDataResult.ErrorText = ex.Message;
            }

            return restoreSyncDataResult;
        }
    }
}