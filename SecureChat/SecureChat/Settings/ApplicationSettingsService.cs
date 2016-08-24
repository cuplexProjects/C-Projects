using System;
using System.IO;
using System.Reflection;
using GeneralToolkitLib.Converters;
using GeneralToolkitLib.Log;
using GeneralToolkitLib.Storage;
using SecureChat.DataModels;

namespace SecureChat.Settings
{
    public class ApplicationSettingsService : IDisposable
    {
        private static ApplicationSettingsService _instance;
        private GlobalAppSettings _appSettings;
        private string settingsFilePath;
        private string settingsDirectoryPath;

        //Not secure but not relevant since global settings contains no sensitive data
        private const string APP_SERIALIZATION_KEY = "mdo0n2OCGAHSUKa0QbGgWohALMLwsyT4x/z7YqJfX/k/IZRtO2LUxeOkCcHHHzKEgeQ6y3MyMn6toMDtMDWj0jM8L1WaIbKfDKExIknIZM+t/xNZcYLY327giBBheoiJ";

        private ApplicationSettingsService()
        {
            _appSettings = new GlobalAppSettings();
            Initialize();
        }


        private void Initialize()
        {
            settingsFilePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\" + Assembly.GetExecutingAssembly().GetName().Name + "\\SecureChatAppSettings.dat";
            settingsDirectoryPath = GeneralConverters.GetDirectoryNameFromPath(settingsFilePath);
            try
            {
                if(!Directory.Exists(settingsDirectoryPath))
                    Directory.CreateDirectory(settingsDirectoryPath);
            }
            catch(Exception ex)
            {
                LogWriter.WriteLog(ex.Message);
            }
        }

        public void SaveSettings()
        {
            try
            {
                StorageManager.SerializeObjectToFile(_appSettings, settingsFilePath, APP_SERIALIZATION_KEY);
            }
            catch(Exception ex)
            {
                LogWriter.WriteLog(ex.Message);
            }
        }

        public void LoadSettings()
        {
            try
            {
                _appSettings = StorageManager.DeserializeObjectFromFile<GlobalAppSettings>(this.settingsFilePath, APP_SERIALIZATION_KEY) ?? new GlobalAppSettings();
            }
            catch(Exception ex)
            {
                LogWriter.WriteLog(ex.Message);
            }
        }

        public GlobalAppSettings AppSettings
        {
            get { return _appSettings; }
        }

        public static ApplicationSettingsService SettingsService
        {
            get { return _instance ?? (_instance = new ApplicationSettingsService()); }
        }

        public void Dispose()
        {
            _instance = null;
        }
    }
}