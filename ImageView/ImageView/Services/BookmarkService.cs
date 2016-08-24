using System;
using System.Text;
using GeneralToolkitLib.ConfigHelper;
using GeneralToolkitLib.Storage.Memory;
using GeneralToolkitLib.Utility.RandomGenerator;
using ImageView.DataContracts;
using ImageView.Events;
using ImageView.Models.Implementation;

namespace ImageView.Services
{
    public class BookmarkService : IDisposable
    {
        private const string BookmarkFileName = "ImageViewBookmarks.dat";

        private static readonly byte[] ApplicationDefinedPassword =
        {
            81, 52, 110, 102, 82, 104, 78, 122, 121, 114, 115, 112, 103, 120, 56, 74,
            49, 108, 86, 49, 85, 118, 90, 105, 65, 68, 81, 89, 85, 50, 53, 52,
            74, 111, 104, 66, 66, 76, 67, 78, 48, 53, 78, 98, 87, 100, 56, 78,
            116, 77, 122, 78, 84, 77, 112, 85, 79, 69, 67, 119, 117, 71, 54, 120
        };

        private static BookmarkService _instance;
        private readonly string _directory;

        private readonly PasswordStorage _passwordStorage;
        private readonly string _protectedMemoryStorageKey;

        private string BookmarkFileStaticPassword
        {
            get
            {
                var encoder = new UTF8Encoding(true);
                return encoder.GetString(ApplicationDefinedPassword, 0, ApplicationDefinedPassword.Length);
            }
        }

        public BookmarkContainerImplementation BookmarksContainer { get; protected set; }


        public static BookmarkService Instance => _instance ?? (_instance = new BookmarkService());


        private BookmarkService()
        {
            _protectedMemoryStorageKey = new SecureRandomGenerator().GetAlphaNumericString(256);
            _directory = GlobalSettings.GetUserDataDirectoryPath();

            _passwordStorage = new PasswordStorage();
            _passwordStorage.Set(_protectedMemoryStorageKey,BookmarkFileStaticPassword);


            if (ApplicationSettingsService.Instance.Settings.PasswordProtectBookmarks)
                BookmarksContainer = null;

            else if (!OpenBookmarkFile(BookmarkFileStaticPassword))
            {
                BookmarksContainer = BookmarkContainerImplementation.CreateRootContainer(BookmarkStructureUpdatedEvent);
                OnBookmarksUpdate?.Invoke(this, new BookmarkUpdatedEventArgs(BookmarkActions.CreatedBookmarkFolder, typeof (BookmarkFolder)));
            }
        }

        public void Dispose()
        {
            _instance = null;
            _passwordStorage?.Dispose();
            GC.Collect();
        }

        public event BookmarkUpdatedEventHandler OnBookmarksUpdate;

        private void BookmarkStructureUpdatedEvent(object sender, BookmarkUpdatedEventArgs bookmarkUpdatedEventArgs)
        {
            OnBookmarksUpdate?.Invoke(sender, bookmarkUpdatedEventArgs);
        }


        public bool OpenBookmarkFile(string password)
        {
            string fileName = _directory + BookmarkFileName;

            BookmarkContainer bookmarkContainer = BookmarkContainerBase.LoadBookmarkFile(password, fileName);

            if (bookmarkContainer?.ContainerId == null)
                return false;



            BookmarksContainer = bookmarkContainer as BookmarkContainerImplementation;

            return true;
        }

        private void ValidateTreeStructure(BookmarkFolder rootFolder)
        {
            foreach (BookmarkFolder childNode in rootFolder.BookmarkFolders)
            {
                if (childNode.ParentFolder == null)
                    childNode.ParentFolder = rootFolder;

                ValidateTreeStructure(childNode);
            }
        }

        //Save bookmarks if any chnages has been made
        public bool SaveBookmarkFile()
        {
            if (BookmarksContainer!=null && BookmarksContainer.IsDirty)
            {
                string fileName = _directory + BookmarkFileName;

                BookmarkFolder rootFolder = AutoMapper.Mapper.Map<BookmarkFolderImplementation, BookmarkFolder>(BookmarksContainer.RootFolder as BookmarkFolderImplementation);
                BookmarkContainer container = new BookmarkContainer(rootFolder, BookmarksContainer.ContainerId, BookmarksContainer.LastUpdate);
                bool status = BookmarksContainer.SaveBookmarkFile(container, _passwordStorage.Get(_protectedMemoryStorageKey), fileName);
                return status;
            }

            return false;
        }
    }
}