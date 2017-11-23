using System;
using System.IO;
using GeneralToolkitLib.ConfigHelper;
using GeneralToolkitLib.Storage.Memory;
using GeneralToolkitLib.Utility.RandomGenerator;
using ImageView.Managers;

namespace ImageView.Services
{
    public class BookmarkService : IDisposable
    {
        private const string BookmarkFileName = "ImageViewBookmarks.dat";
        private readonly string _directory;

        private readonly PasswordStorage _passwordStorage;
        private readonly string _protectedMemoryStorageKey;
        private readonly BookmarkManager _bookmarkManager;
        private readonly ApplicationSettingsService _applicationSettingsService;

        public BookmarkService(BookmarkManager bookmarkManager, ApplicationSettingsService applicationSettingsService)
        {
            _bookmarkManager = bookmarkManager;
            _applicationSettingsService = applicationSettingsService;
            _protectedMemoryStorageKey = new SecureRandomGenerator().GetAlphaNumericString(256);
            _directory = GlobalSettings.GetUserDataDirectoryPath();

            _passwordStorage = new PasswordStorage();
            _passwordStorage.Set(_protectedMemoryStorageKey, GetDefaultPassword());

            
            //if (!ApplicationSettingsService.Instance.Settings.PasswordProtectBookmarks)
            //    OpenBookmarks(GetDefaultPassword());
        }

        public BookmarkManager BookmarkManager => _bookmarkManager;

        public void Dispose()
        {
            _passwordStorage?.Dispose();
            GC.Collect();
        }

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
                return true;
            }
            return false;
        }

        public bool SaveBookmarks()
        {
            string password = _passwordStorage.Get(_protectedMemoryStorageKey);
            return _bookmarkManager.SaveToFile(_directory + BookmarkFileName, password);
        }

        private string GetDefaultPassword()
        {
            string defaultKey = _applicationSettingsService.Settings.DefaultKey;

            if (defaultKey != null && defaultKey.Length == 256) return defaultKey;
            defaultKey = new SecureRandomGenerator().GetAlphaNumericString(256);
            _applicationSettingsService.Settings.DefaultKey = defaultKey;
            _applicationSettingsService.SaveSettings();

            return defaultKey;
        }
    }
}