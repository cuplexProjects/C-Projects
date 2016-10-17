﻿using System;
using System.Collections.Generic;
using System.Linq;
using GeneralToolkitLib.Log;
using GeneralToolkitLib.Storage;
using GeneralToolkitLib.Storage.Models;
using ImageView.DataContracts;
using ImageView.Events;
using ImageView.Models;

namespace ImageView.Managers
{
    public class BookmarkManager
    {
        private readonly BookmarkUpdatedEventHandler _bookmarkUpdated;
        private BookmarkContainer _bookmarkContainer;
        private bool _isModified;

        private BookmarkManager(BookmarkUpdatedEventHandler updatedEventHandler)
        {
            _bookmarkContainer = new BookmarkContainer
            {
                ContainerId = Guid.NewGuid().ToString(),
                LastUpdate = DateTime.Now,
                RootFolder = new BookmarkFolder
                {
                    Name = "Root",
                    Id = Guid.NewGuid().ToString(),
                    Bookmarks = new List<Bookmark>(),
                    SortOrder = 0,
                    BookmarkFolders = new List<BookmarkFolder>()
                }
            };
            _bookmarkUpdated = updatedEventHandler;
            RootFolder = _bookmarkContainer.RootFolder;
        }

        public BookmarkFolder RootFolder { get; private set; }

        public static BookmarkManager CreateNew(BookmarkUpdatedEventHandler updatedEventHandler)
        {
            var bookmarkManager = new BookmarkManager(updatedEventHandler);
            return bookmarkManager;
        }

        private void BookmarkUpdated(BookmarkUpdatedEventArgs e)
        {
            _bookmarkUpdated?.Invoke(this, e);
        }

        public bool SaveToFile(string filename, string password)
        {
            try
            {
                var settings = new StorageManagerSettings(false, Environment.ProcessorCount, true, password);
                var storageManager = new StorageManager(settings);
                bool successful = storageManager.SerializeObjectToFile(_bookmarkContainer, filename, null);

                if (successful)
                {
                    _isModified = false;
                }

                return successful;
            }
            catch (Exception ex)
            {
                LogWriter.LogError("BookmarkManager.SaveToFile(string filename, string password) : " + ex.Message, ex);
                return false;
            }
        }

        public bool LoadFromFile(string filename, string password)
        {
            try
            {
                var settings = new StorageManagerSettings(false, Environment.ProcessorCount, true, password);
                var storageManager = new StorageManager(settings);
                var bookmarkContainer = storageManager.DeserializeObjectFromFile<BookmarkContainer>(filename, null);

                if (ValidateBookmarkContainer(bookmarkContainer))
                {
                    _bookmarkContainer = bookmarkContainer;
                    RootFolder = _bookmarkContainer.RootFolder;
                    _isModified = false;
                    return true;
                }
            }
            catch (Exception ex)
            {
                LogWriter.LogError("BookmarkManager.LoadFromFile(string filename, string password) : " + ex.Message, ex);
            }

            return false;
        }

        private bool ValidateBookmarkContainer(BookmarkContainer bookmarkContainer)
        {
            if (bookmarkContainer?.RootFolder == null || bookmarkContainer.ContainerId == null)
                return false;

            BookmarkFolder rootFolder = bookmarkContainer.RootFolder;
            CreateEmptyLists(rootFolder);
            CreateParentFolderRefs(rootFolder);

            return true;
        }

        private void CreateParentFolderRefs(BookmarkFolder rootFolder)
        {
            foreach (Bookmark bookmark in rootFolder.Bookmarks)
            {
                if (bookmark.ParentFolderId == null)
                    bookmark.ParentFolderId = rootFolder.Id;
            }

            if (rootFolder.BookmarkFolders != null)
            {
                foreach (BookmarkFolder folder in rootFolder.BookmarkFolders)
                {
                    CreateParentFolderRefs(folder);
                }
            }
        }

        private void CreateEmptyLists(BookmarkFolder rootFolder)
        {
            if (rootFolder.Bookmarks == null)
                rootFolder.Bookmarks = new List<Bookmark>();

            if (rootFolder.Id == null)
                rootFolder.Id = Guid.NewGuid().ToString();

            if (rootFolder.BookmarkFolders == null)
                rootFolder.BookmarkFolders = new List<BookmarkFolder>();
            else
            {
                foreach (BookmarkFolder folder in rootFolder.BookmarkFolders)
                {
                    CreateEmptyLists(folder);
                }
            }
        }

        private void ReindexSortOrder(bool ReIndexFolders, bool ReIndexBookmarks)
        {
            if (ReIndexFolders)
            {
                var folderList = _bookmarkContainer.RootFolder.BookmarkFolders.OrderBy(f => f.SortOrder).ToList();
                for (int i = 0; i < folderList.Count; i++)
                {
                    folderList[i].SortOrder = i + 1;
                }

                folderList.Sort();
            }

            if (ReIndexBookmarks)
            {
                var bookmarkList = _bookmarkContainer.RootFolder.Bookmarks.OrderBy(f => f.SortOrder).ToList();
                for (int i = 0; i < bookmarkList.Count; i++)
                {
                    bookmarkList[i].SortOrder = i + 1;
                }

                _bookmarkContainer.RootFolder.Bookmarks.Sort((b1, b2) => b1.SortOrder.CompareTo(b2.SortOrder));
            }
        }

        public BookmarkFolder AddBookmarkFolder(string parentId, string folderName)
        {
            BookmarkFolder parentFolder = GetBookmarkFolderById(_bookmarkContainer.RootFolder, parentId);
            if (parentFolder == null)
                return null;

            var folder = new BookmarkFolder
            {
                Name = folderName,
                Id = Guid.NewGuid().ToString(),
                ParentFolderId = parentFolder.Id,
                SortOrder = _bookmarkContainer.RootFolder.BookmarkFolders.Count,
                BookmarkFolders = new List<BookmarkFolder>(),
                Bookmarks = new List<Bookmark>()
            };

            parentFolder.BookmarkFolders.Add(folder);
            BookmarkUpdated(new BookmarkUpdatedEventArgs(BookmarkActions.CreatedBookmarkFolder, typeof(BookmarkFolder)));
            return folder;
        }

        public BookmarkFolder InsertBookmarkFolder(string parentId, string folderName, int index)
        {
            BookmarkFolder parentFolder = GetBookmarkFolderById(_bookmarkContainer.RootFolder, parentId);
            if (parentFolder == null)
                return null;

            if (index < 0 || index > parentFolder.BookmarkFolders.Count)
                return null;

            var folder = new BookmarkFolder
            {
                Name = folderName,
                ParentFolderId = parentFolder.Id,
                SortOrder = index
            };

            var postItems =
                parentFolder.BookmarkFolders.Where(b => b.SortOrder >= index).OrderBy(b => b.SortOrder).ToList();
            foreach (BookmarkFolder item in postItems)
            {
                item.SortOrder = item.SortOrder + 1;
            }

            parentFolder.BookmarkFolders.Add(folder);
            BookmarkUpdated(new BookmarkUpdatedEventArgs(BookmarkActions.CreatedBookmarkFolder, typeof(BookmarkFolder)));
            return folder;
        }

        public bool AddBookmark(string parentFolderId, string boookmarkName, ImageReferenceElement imgRef)
        {
            BookmarkFolder parentFolder = GetBookmarkFolderById(_bookmarkContainer.RootFolder, parentFolderId);
            if (parentFolder == null)
                return false;

            var bookmark = new Bookmark
            {
                BoookmarkName = boookmarkName,
                CompletePath = imgRef.CompletePath,
                CreationTime = imgRef.CreationTime,
                Directory = imgRef.Directory,
                FileName = imgRef.FileName,
                LastAccessTime = imgRef.LastAccessTime,
                LastWriteTime = imgRef.LastWriteTime,
                Size = imgRef.Size,
                SortOrder = parentFolder.Bookmarks.Count,
                ParentFolderId = parentFolder.Id
            };

            parentFolder.Bookmarks.Add(bookmark);
            BookmarkUpdated(new BookmarkUpdatedEventArgs(BookmarkActions.CreatedBookmark, typeof(Bookmark)));
            return true;
        }

        public bool InsertBookmark(string parentFolderId, string boookmarkName, ImageReferenceElement imgRef, int index)
        {
            BookmarkFolder parentFolder = GetBookmarkFolderById(_bookmarkContainer.RootFolder, parentFolderId);
            if (parentFolder == null)
                return false;

            var bookmark = new Bookmark
            {
                BoookmarkName = boookmarkName,
                CompletePath = imgRef.CompletePath,
                CreationTime = imgRef.CreationTime,
                Directory = imgRef.Directory,
                FileName = imgRef.FileName,
                LastAccessTime = imgRef.LastAccessTime,
                LastWriteTime = imgRef.LastWriteTime,
                Size = imgRef.Size,
                SortOrder = parentFolder.Bookmarks.Count,
                ParentFolderId = parentFolder.Id
            };

            var postItems = parentFolder.Bookmarks.Where(b => b.SortOrder >= index).OrderBy(b => b.SortOrder).ToList();
            foreach (Bookmark item in postItems)
            {
                item.SortOrder = item.SortOrder + 1;
            }

            parentFolder.Bookmarks.Add(bookmark);
            BookmarkUpdated(new BookmarkUpdatedEventArgs(BookmarkActions.CreatedBookmark, typeof(Bookmark)));
            return true;
        }

        public bool DeleteBookmark(Bookmark bookmark)
        {
            BookmarkFolder parentFolder = GetBookmarkFolderById(_bookmarkContainer.RootFolder, bookmark.ParentFolderId);

            if (parentFolder == null)
                return false;

            bool success = parentFolder.Bookmarks.Remove(bookmark);
            if (success)
            {
                ReindexSortOrder(false, true);
            }
            return success;
        }

        public bool DeleteBookmarkByFilename(string parentFolderId, string fileName)
        {
            BookmarkFolder parentFolder = GetBookmarkFolderById(_bookmarkContainer.RootFolder, parentFolderId);
            if (parentFolder == null)
                return false;

            Bookmark bookmarkToDelete = null;
            foreach (Bookmark bookmark in parentFolder.Bookmarks)
            {
                if (bookmark.FileName == fileName)
                {
                    bookmarkToDelete = bookmark;
                    break;
                }
            }

            if (bookmarkToDelete != null)
            {
                parentFolder.Bookmarks.Remove(bookmarkToDelete);
                ReindexSortOrder(false, true);
                return true;
            }

            return false;
        }


        public bool DeleteBookmarkFolder(BookmarkFolder folder)
        {
            BookmarkFolder parentFolder = GetBookmarkFolderById(_bookmarkContainer.RootFolder, folder.ParentFolderId);
            if (parentFolder == null)
                return false;

            bool success = parentFolder.BookmarkFolders.Remove(folder);
            if (success)
            {
                ReindexSortOrder(false, true);
            }
            return success;
        }

        public bool DeleteBookmarkFolderById(string folderId)
        {
            BookmarkFolder bookmarkFolder = GetBookmarkFolderById(_bookmarkContainer.RootFolder, folderId);

            if (bookmarkFolder?.ParentFolderId == null)
                return false;

            BookmarkFolder parentFolder = GetBookmarkFolderById(_bookmarkContainer.RootFolder, bookmarkFolder.ParentFolderId);
            bool result = parentFolder.BookmarkFolders.Remove(bookmarkFolder);
            ReindexSortOrder(true, false);
            return result;
        }

        private BookmarkFolder GetBookmarkFolderById(BookmarkFolder rootFolder, string id)
        {
            if (rootFolder.Id == id)
                return rootFolder;

            foreach (BookmarkFolder bookmarkFolder in rootFolder.BookmarkFolders)
            {
                if (bookmarkFolder.Id == id)
                {
                    return bookmarkFolder;
                }
            }

            return null;
        }
    }
}