using System;
using System.IO;
using GeneralToolkitLib.Storage.Memory;
using GeneralToolkitLib.Utility.RandomGenerator;
using ImageView.Configuration;
using ImageView.Managers;
using Serilog;

namespace ImageView.Services
{
    public sealed class BookmarkService : ServiceBase, IDisposable
    {
        private const string BookmarkFileName = "ImageViewBookmarks.dat";
        private readonly PasswordStorage _passwordStorage;
        private readonly string _protectedMemoryStorageKey;
        private readonly BookmarkManager _bookmarkManager;
        private readonly ApplicationSettingsService _applicationSettingsService;
        private readonly string _directory;

        public BookmarkService(BookmarkManager bookmarkManager, ApplicationSettingsService applicationSettingsService)
        {
            _bookmarkManager = bookmarkManager;
            _applicationSettingsService = applicationSettingsService;
            _protectedMemoryStorageKey = new SecureRandomGenerator().GetAlphaNumericString(256);
            _directory = ApplicationBuildConfig.UserDataPath;

            _passwordStorage = new PasswordStorage();
            _passwordStorage.Set(_protectedMemoryStorageKey, GetDefaultPassword());
        }

        public BookmarkManager BookmarkManager => _bookmarkManager;


        public bool OpenBookmarks()
        {
            return OpenBookmarks(GetDefaultPassword());
        }

        public bool OpenBookmarks(string password)
        {
            string filename = _directory + BookmarkFileName;
            if (!File.Exists(filename))
                return false;

            bool loadSuccessful = _bookmarkManager.LoadFromFile(filename, password);
            if (loadSuccessful)
            {
                _passwordStorage.Set(_protectedMemoryStorageKey, password);
                Log.Information("Loaded bookmarks from file");
                return true;
            }
            Log.Error("Failed to load bookmarks from file");
            return false;
        }

        public bool SaveBookmarks()
        {
            string password = _passwordStorage.Get(_protectedMemoryStorageKey);
            return _bookmarkManager.SaveToFile(_directory + BookmarkFileName, password);
        }

        private string GetDefaultPassword()
        {
            try
            {
                string defaultKey = _applicationSettingsService.Settings.DefaultKey;

                if (defaultKey != null && defaultKey.Length == 256) return defaultKey;
                defaultKey = new SecureRandomGenerator().GetAlphaNumericString(256);
                _applicationSettingsService.Settings.DefaultKey = defaultKey;
                _applicationSettingsService.SaveSettings();

                return defaultKey;
            }
            catch (Exception e)
            {
                Log.Error(e, "GetDefaultPassword");
                return "CodeRed";

            }
            
        }

        public void Dispose()
        {
            _passwordStorage?.Dispose();
        }
    }
}