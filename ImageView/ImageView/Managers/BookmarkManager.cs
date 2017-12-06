using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GeneralToolkitLib.Converters;
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
        private BookmarkContainer _bookmarkContainer;
        private bool _isModified;
        private bool _isLoadedFromFile;
        public event BookmarkUpdatedEventHandler OnBookmarksUpdate;

        public BookmarkManager()
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

            RootFolder = _bookmarkContainer.RootFolder;
        }

        public BookmarkFolder RootFolder { get; private set; }

        public bool IsModified => _isModified;

        public bool LoadedFromFile => _isLoadedFromFile;

        private void BookmarkUpdated(BookmarkUpdatedEventArgs e)
        {
            _isModified = true;
            OnBookmarksUpdate?.Invoke(this, e);
        }

        public bool SaveToFile(string filename, string password)
        {
            try
            {
                //Make a copy of the original file
                if (File.Exists(filename))
                {
                    string copyFilename = GeneralConverters.GetDirectoryNameFromPath(filename, true) + "BookmarksCopy.dat";
                    if (File.Exists(copyFilename))
                    {
                        File.Delete(copyFilename);
                    }
                    File.Copy(filename, copyFilename);
                }

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
                _isLoadedFromFile = true;

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

        public bool LoadFromFileAndAppendBookmarks(string filename, string password)
        {
            try
            {
                _isLoadedFromFile = true;

                var settings = new StorageManagerSettings(false, Environment.ProcessorCount, true, password);
                var storageManager = new StorageManager(settings);
                var bookmarkContainer = storageManager.DeserializeObjectFromFile<BookmarkContainer>(filename, null);

                if (ValidateBookmarkContainer(bookmarkContainer))
                {
                    if (_bookmarkContainer == null)
                    {
                        _bookmarkContainer = bookmarkContainer;
                    }
                    else
                    {
                        RecursiveAdd(_bookmarkContainer.RootFolder, bookmarkContainer.RootFolder);
                    }

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

        private void RecursiveAdd(BookmarkFolder source, BookmarkFolder appendFrom)
        {
            foreach (var bookmark in appendFrom.Bookmarks)
            {
                if (!source.Bookmarks.Any(x => x.CompletePath == bookmark.CompletePath && x.Size == bookmark.Size))
                {
                    source.Bookmarks.Add(bookmark);
                }
            }

            foreach (var folder in appendFrom.BookmarkFolders)
            {
                if (source.BookmarkFolders.All(x => x.Name != folder.Name))
                {
                    source.BookmarkFolders.Add(folder);
                }

                if (folder.BookmarkFolders.Any())
                {
                    foreach (var subFolder in folder.BookmarkFolders)
                    {
                        var sFolder = source.BookmarkFolders.FirstOrDefault(x => x.Name == folder.Name);
                        if (sFolder != null)
                        {
                            RecursiveAdd(sFolder, subFolder);
                        }
                    }
                }
            }
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

        private void ReindexSortOrder(bool reIndexFolders, bool reIndexBookmarks)
        {
            if (reIndexFolders)
            {
                var folderList = _bookmarkContainer.RootFolder.BookmarkFolders.OrderBy(f => f.SortOrder).ToList();
                for (int i = 0; i < folderList.Count; i++)
                {
                    folderList[i].SortOrder = i + 1;
                }

                _bookmarkContainer.RootFolder.BookmarkFolders.Sort((f1, f2) => f1.SortOrder.CompareTo(f2.SortOrder));
            }

            if (reIndexBookmarks)
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
                SortOrder = index,
                Id = Guid.NewGuid().ToString(),
                BookmarkFolders = new List<BookmarkFolder>(),
                Bookmarks = new List<Bookmark>()
            };

            var postItems =
                parentFolder.BookmarkFolders.Where(b => b.SortOrder >= index).OrderBy(b => b.SortOrder).ToList();
            foreach (BookmarkFolder item in postItems)
            {
                item.SortOrder = item.SortOrder + 1;
            }

            parentFolder.BookmarkFolders.Add(folder);
            parentFolder.BookmarkFolders.Sort((f1, f2) => f1.SortOrder.CompareTo(f2.SortOrder));
            BookmarkUpdated(new BookmarkUpdatedEventArgs(BookmarkActions.CreatedBookmarkFolder, typeof(BookmarkFolder)));
            return folder;
        }

        public Bookmark AddBookmark(string parentFolderId, string boookmarkName, ImageReferenceElement imgRef)
        {
            BookmarkFolder parentFolder = GetBookmarkFolderById(_bookmarkContainer.RootFolder, parentFolderId);
            if (parentFolder == null)
                return null;

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
            return bookmark;
        }

        public Bookmark InsertBookmark(string parentFolderId, string boookmarkName, ImageReferenceElement imgRef, int index)
        {
            BookmarkFolder parentFolder = GetBookmarkFolderById(_bookmarkContainer.RootFolder, parentFolderId);
            if (parentFolder == null)
                return null;

            if (index < 0 || index > parentFolder.Bookmarks.Count)
                return null;

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
                SortOrder = index,
                ParentFolderId = parentFolder.Id
            };

            var postItems = parentFolder.Bookmarks.Where(b => b.SortOrder >= index).OrderBy(b => b.SortOrder).ToList();
            foreach (Bookmark item in postItems)
            {
                item.SortOrder = item.SortOrder + 1;
            }

            parentFolder.Bookmarks.Add(bookmark);
            parentFolder.Bookmarks.Sort((b1, b2) => b1.SortOrder.CompareTo(b2.SortOrder));
            BookmarkUpdated(new BookmarkUpdatedEventArgs(BookmarkActions.CreatedBookmark, typeof(Bookmark)));
            return bookmark;
        }

        public bool MoveBookmark(Bookmark bookmark, string destinationFolderId)
        {
            BookmarkFolder parentFolder = GetBookmarkFolderById(_bookmarkContainer.RootFolder, bookmark.ParentFolderId);
            BookmarkFolder destinationFolder = GetBookmarkFolderById(_bookmarkContainer.RootFolder, destinationFolderId);

            if (parentFolder == null | destinationFolder == null || parentFolder == destinationFolder)
                return false;

            parentFolder.Bookmarks.Remove(bookmark);
            destinationFolder.Bookmarks.Add(bookmark);
            bookmark.ParentFolderId = destinationFolder.Id;

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
                BookmarkUpdated(new BookmarkUpdatedEventArgs(BookmarkActions.DeletedBookmark, typeof(Bookmark)));
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
                BookmarkUpdated(new BookmarkUpdatedEventArgs(BookmarkActions.DeletedBookmark, typeof(Bookmark)));
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
                BookmarkUpdated(new BookmarkUpdatedEventArgs(BookmarkActions.DeletedBookmarkFolder, typeof(Bookmark)));
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
            if (result)
                BookmarkUpdated(new BookmarkUpdatedEventArgs(BookmarkActions.DeletedBookmarkFolder, typeof(Bookmark)));

            return result;
        }

        private BookmarkFolder GetBookmarkFolderById(BookmarkFolder rootFolder, string id)
        {
            BookmarkFolder subFolder = null;
            if (rootFolder.Id == id)
                return rootFolder;

            foreach (BookmarkFolder bookmarkFolder in rootFolder.BookmarkFolders)
            {
                if (bookmarkFolder.Id == id)
                {
                    return bookmarkFolder;
                }
                if (bookmarkFolder.BookmarkFolders != null && bookmarkFolder.BookmarkFolders.Count > 0)
                    subFolder = GetBookmarkFolderById(bookmarkFolder, id);

                if (subFolder != null)
                    return subFolder;
            }

            return null;
        }

        public void RemoveDuplicates()
        {
            var bookmarkFilenames = _bookmarkContainer.RootFolder.Bookmarks.Select(x => x.FileName).ToList();

            var removeList = new List<Bookmark>();

            foreach (string filename in bookmarkFilenames)
            {
                var selection = _bookmarkContainer.RootFolder.Bookmarks.Where(x => x.FileName == filename).ToList();
                if (selection.Count > 1)
                {
                    selection.RemoveAt(0);
                    removeList.AddRange(selection);
                }
            }

            foreach (var bookmark in removeList)
            {
                DeleteBookmark(bookmark);
            }
        }

        public void BookmarkDatasourceUpdated()
        {
            BookmarkUpdated(new BookmarkUpdatedEventArgs(BookmarkActions.LoadedNewDataSource, typeof(Bookmark)));
        }
    }
}