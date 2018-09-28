using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using GeneralToolkitLib.Configuration;
using GeneralToolkitLib.Hashing;
using GeneralToolkitLib.Storage;
using GeneralToolkitLib.Storage.Models;
using ImageView.DataContracts;
using Serilog;

namespace ImageView.Repositories
{
    public class AppSettingsFileRepository : RepositoryBase
    {
        private const string AppSettingsFilename = "localSettings.dat";
        private const string AppSettingsPassword = "F9BB2AED-BA1D-46AF-9FFE-4B69610C9BE1";
        private static readonly object SaveSettingsFileLock = new object();

        public AppSettingsFileRepository()
        {
            Task.Factory.StartNew(LoadSettings);
        }

        private AppSettingsFileStoreDataModel _appSettings;

        public AppSettingsFileStoreDataModel AppSettings => _appSettings;

        public event EventHandler LoadSettingsCompleted;

        public event EventHandler SaveSettingsCompleted;

        public async Task SaveSettings()
        {
            try
            {
                string path = GetFullPathToSettingsFile();

                var storageManager = new StorageManager(new StorageManagerSettings(false, 1, true, SHA256.GetSHA256HashAsHexString(AppSettingsPassword)));
                await storageManager.SerializeObjectToFileAsync(_appSettings, path, null);
                SaveSettingsCompleted?.Invoke(this, EventArgs.Empty);

            }
            catch (Exception exception)
            {
                Log.Error(exception, "AppSettingsFileRepository SaveSettings Exception: {Message}", exception.Message);
            }
        }

        public async Task LoadSettings()
        {
            try
            {
                string path = GetFullPathToSettingsFile();
                if (!File.Exists(path))
                {
                    _appSettings = AppSettingsFileStoreDataModel.CreateNew();
                    await SaveSettings();
                }
                else
                {
                    var storageManager = new StorageManager(new StorageManagerSettings(false, 1, true, SHA256.GetSHA256HashAsHexString(AppSettingsPassword)));
                    _appSettings = await storageManager.DeserializeObjectFromFileAsync<AppSettingsFileStoreDataModel>(path, null);

                    if (_appSettings.FormStateDictionary == null)
                    {
                        _appSettings.InitFormDictionary();
                    }
                }
            }
            catch (Exception exception)
            {
                Log.Error(exception, "Exception in AppSettingsFileRepository LoadSettings {Message}", exception.Message);
            }
            finally
            {
                if (_appSettings == null)
                {
                    _appSettings = AppSettingsFileStoreDataModel.CreateNew();
                }

                LoadSettingsCompleted?.Invoke(this, EventArgs.Empty);
            }
        }

        private string GetFullPathToSettingsFile()
        {
            return Path.Combine(ApplicationBuildConfig.UserDataPath, AppSettingsFilename);
        }
    }
}