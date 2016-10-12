using System;
using System.IO;
using GeneralToolkitLib.ConfigHelper;
using GeneralToolkitLib.Storage.Memory;
using GeneralToolkitLib.Utility.RandomGenerator;
using ImageView.Events;
using ImageView.Managers;

namespace ImageView.Services
{
    public class BookmarkService : IDisposable
    {
        private const string BookmarkFileName = "ImageViewBookmarks.dat";
        private static BookmarkService _instance;
        private readonly string _directory;

        private readonly PasswordStorage _passwordStorage;
        private readonly string _protectedMemoryStorageKey;

        private BookmarkService()
        {
            _protectedMemoryStorageKey = new SecureRandomGenerator().GetAlphaNumericString(256);
            _directory = GlobalSettings.GetUserDataDirectoryPath();

            _passwordStorage = new PasswordStorage();
            _passwordStorage.Set(_protectedMemoryStorageKey, GetDefaultPassword());

            BookmarkManager = BookmarkManager.CreateNew(BookmarkStructureUpdatedEvent);

            if (!ApplicationSettingsService.Instance.Settings.PasswordProtectBookmarks)
                OpenBookmarks(GetDefaultPassword());
        }

        public BookmarkManager BookmarkManager { get; private set; }

        public static BookmarkService Instance => _instance ?? (_instance = new BookmarkService());

        public void Dispose()
        {
            _instance = null;
            BookmarkManager = null;
            _passwordStorage?.Dispose();
            GC.Collect();
        }

        public event BookmarkUpdatedEventHandler OnBookmarksUpdate;

        private void BookmarkStructureUpdatedEvent(object sender, BookmarkUpdatedEventArgs bookmarkUpdatedEventArgs)
        {
            OnBookmarksUpdate?.Invoke(sender, bookmarkUpdatedEventArgs);
        }

        public bool OpenBookmarks(string password)
        {
            string filename = _directory + BookmarkFileName;
            if (!File.Exists(filename))
                return false;

            bool loadSuccessful = BookmarkManager.LoadFromFile(filename, password);
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
            return BookmarkManager.SaveToFile(_directory + BookmarkFileName, password);
        }

        private string GetDefaultPassword()
        {
            string defaultKey = ApplicationSettingsService.Instance.Settings.DefaultKey;

            if (defaultKey != null && defaultKey.Length == 256) return defaultKey;
            defaultKey = new SecureRandomGenerator().GetAlphaNumericString(256);
            ApplicationSettingsService.Instance.Settings.DefaultKey = defaultKey;
            ApplicationSettingsService.Instance.SaveSettings();

            return defaultKey;
        }
    }
}