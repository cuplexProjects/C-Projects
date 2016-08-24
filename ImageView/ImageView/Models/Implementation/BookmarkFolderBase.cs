using System;
using ImageView.DataContracts;
using ImageView.Events;

namespace ImageView.Models.Implementation
{
    public abstract class BookmarkFolderBase : BookmarkFolder
    {
        public abstract BookmarkFolderImplementation AddBookmarkFolder(string folderName);
        public abstract BookmarkFolderImplementation InsertBookmarkFolder(string folderName, int sortorder);

        public abstract void AddBookmark(string boookmarkName, ImageReferenceElement imgRef);
        public abstract void InsertBookmark(string boookmarkName, ImageReferenceElement imgRef, int sortOrder);

        public abstract bool DeleteBookmark(Bookmark bookmark);
        public abstract bool DeleteBookmarkByFilename(string fileName);


        public abstract bool DeleteBookmarkFolder(BookmarkFolder folder);
        public abstract bool DeleteBookmarkFolderById(string folderId);

        public event BookmarkUpdatedEventHandler OnBookmarksUpdate;

        protected virtual void BookmarkUpdated(BookmarkUpdatedEventArgs eventArgs)
        {
            OnBookmarksUpdate?.Invoke(this, eventArgs);
        }

        public virtual void BookmarkUpdated(object sender, BookmarkUpdatedEventArgs eventArgs)
        {
            OnBookmarksUpdate?.Invoke(sender, eventArgs);
        }

        protected BookmarkFolder GetBookmarkFolderById(BookmarkFolder rootFolder, string id)
        {
            BookmarkFolder retFolder = null;
            foreach (BookmarkFolder folder in rootFolder.BookmarkFolders)
            {
                if (folder.BookmarkFolderId == id)
                {
                    retFolder = folder;
                    break;
                }


                if (folder.BookmarkFolders.Count <= 0) continue;
                retFolder = GetBookmarkFolderById(folder, id);
                if (retFolder != null)
                    break;
            }

            return retFolder;
        }
    }
}