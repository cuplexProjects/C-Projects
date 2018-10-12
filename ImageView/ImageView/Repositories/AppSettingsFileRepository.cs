using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Anotar.Serilog;
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
        private static int LoadSettingsThreadCount = 0;

        public AppSettingsFileRepository()
        {
            LoadSettings();
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

        public void LoadSettings()
        {
            if (LoadSettingsThreadCount > 1)
            {
                return;
            }

            try
            {
                LoadSettingsAsync().GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                LogTo.Error(ex, "Exception when loading settings");
            }
        }

        public async Task<bool> LoadSettingsAsync()
        {
            Interlocked.Increment(ref LoadSettingsThreadCount);
            if (LoadSettingsThreadCount > 1)
            {
                return false;
            }

            bool loadsuccessful = false;
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

                loadsuccessful = true;
            }
            catch (Exception exception)
            {
                LogTo.Error(exception, "Exception in AppSettingsFileRepository LoadSettingsAsync {Message}", exception.Message);
                loadsuccessful = false;
            }
            finally
            {
                Interlocked.Decrement(ref LoadSettingsThreadCount);
                if (_appSettings == null)
                {
                    _appSettings = AppSettingsFileStoreDataModel.CreateNew();
                }

                LoadSettingsCompleted?.Invoke(this, EventArgs.Empty);
            }

            return loadsuccessful;
        }

        private string GetFullPathToSettingsFile()
        {
            return Path.Combine(ApplicationBuildConfig.UserDataPath, AppSettingsFilename);
        }
    }
}