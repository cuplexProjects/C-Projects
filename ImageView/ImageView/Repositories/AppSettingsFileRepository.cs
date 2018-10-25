using System;
using System.IO;
using GeneralToolkitLib.Configuration;
using GeneralToolkitLib.Hashing;
using GeneralToolkitLib.Storage;
using GeneralToolkitLib.Storage.Models;
using ImageViewer.DataContracts;
using JetBrains.Annotations;
using Serilog;

namespace ImageViewer.Repositories
{
    [UsedImplicitly]
    public class AppSettingsFileRepository : RepositoryBase
    {
        private const string AppSettingsFilename = "localSettings.dat";
        private const string AppSettingsPassword = "F9BB2AED-BA1D-46AF-9FFE-4B69610C9BE1";

        private bool _saveInProgress;
        private bool _loadInProgress;

        public event EventHandler LoadSettingsCompleted;
        public event EventHandler SaveSettingsCompleted;
        private bool _isDirty;

        private ImageViewApplicationSettings _appSettings;



        public ImageViewApplicationSettings AppSettings
        {
            get
            {
                if (_appSettings == null)
                {
                    throw new ApplicationException("AppSettingsFileStoreDataModel AppSettings was null when accessing it. Meaning fetching settings before they are loaded");
                }
                return _appSettings;
            }
        }

        public bool IsDirty { get => _isDirty; private set => _isDirty = value; }

        public void NotifySettingsChanged()
        {
            IsDirty = true;
        }

        public bool SaveSettings()
        {
            if (!IsDirty || _saveInProgress)
            {
                return false;
            }

            return SaveSettingsInternal();
        }

        private bool SaveSettingsInternal()
        {

            bool result = false;
            _saveInProgress = true;

            try
            {
                string path = GetFullPathToSettingsFile();

                var storageManager = new StorageManager(new StorageManagerSettings(false, 1, true, SHA256.GetSHA256HashAsHexString(AppSettingsPassword)));
                result = storageManager.SerializeObjectToFile(_appSettings, path, null);
                if (result)
                {
                    IsDirty = false;
                    SaveSettingsCompleted?.Invoke(this, EventArgs.Empty);
                }

            }
            catch (Exception exception)
            {
                Log.Error(exception, "AppSettingsFileRepository SaveSettings Exception: {Message}", exception.Message);
                _saveInProgress = false;
            }
            finally
            {
                _saveInProgress = false;
            }

            return result;
        }

        private void SaveNewSettings()
        {
            try
            {
                string path = GetFullPathToSettingsFile();

                var storageManager = new StorageManager(new StorageManagerSettings(false, 1, true, SHA256.GetSHA256HashAsHexString(AppSettingsPassword)));
                storageManager.SerializeObjectToFile(_appSettings, path, null);
                IsDirty = false;
            }
            catch (Exception exception)
            {
                Log.Error(exception, "AppSettingsFileRepository SaveSettings Exception: {Message}", exception.Message);
            }
        }


        public bool LoadSettings()
        {
            if (IsDirty && !(_saveInProgress || _loadInProgress))
            {
                SaveSettings();
            }

            return LoadSettingsInternal();

        }
        private bool LoadSettingsInternal()
        {
            if (_loadInProgress)
            {
                return false;
            }

            _loadInProgress = true;
            bool unsuccessful;

            try
            {
                string path = GetFullPathToSettingsFile();
                if (!File.Exists(path))
                {
                    _appSettings = ImageViewApplicationSettings.CreateDefaultSettings();
                    SaveNewSettings();
                }
                else
                {
                    var storageManager = new StorageManager(new StorageManagerSettings(false, 1, true, SHA256.GetSHA256HashAsHexString(AppSettingsPassword)));
                    _appSettings = storageManager.DeserializeObjectFromFile<ImageViewApplicationSettings>(path, null);

                    if (_appSettings.ExtendedAppSettings.FormStateDictionary == null)
                    {
                        _appSettings.ExtendedAppSettings.InitFormDictionary();
                    }
                }

                IsDirty = false;
                unsuccessful = true;
            }
            catch (Exception exception)
            {
                Log.Error(exception, "Exception in AppSettingsFileRepository LoadSettingsAsync {Message}", exception.Message);
                unsuccessful = false;
            }
            finally
            {
                _loadInProgress = false;
                LoadSettingsCompleted?.Invoke(this, EventArgs.Empty);
            }

            return unsuccessful;
        }


        private string GetFullPathToSettingsFile()
        {
            return Path.Combine(ApplicationBuildConfig.UserDataPath, AppSettingsFilename);
        }
    }
}