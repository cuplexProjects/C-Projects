using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using ImageView.DataContracts;
using ImageView.Events;

namespace ImageView.Models.Implementation
{
    public class BookmarkFolderImplementation : BookmarkFolderBase
    {
        private BookmarkContainerImplementation BookmarkContainer;
        private readonly MapperConfiguration mapperConfiguration;
        private BookmarkFolderImplementation()
        {
            
            BookmarkFolderId = Guid.NewGuid().ToString();
            BookmarkFolders = new List<BookmarkFolder>();
            Bookmarks = new List<Bookmark>();
            OnBookmarksUpdate += BookmarkFolderImplementation_OnBookmarksUpdate;

            mapperConfiguration= new MapperConfiguration(configuration => configuration.CreateMap<BookmarkFolderImplementation,BookmarkFolder>());
        
        }



        public static BookmarkFolder CreateRootBookmarkFolder(BookmarkUpdatedEventHandler updatedEventHandler, BookmarkContainerImplementation bookmarkContainer)
        {
            var bookmarkFolder = new BookmarkFolderImplementation
            {
                Name = "Root",
                SortOrder = 1,
                ParentFolder = null,
                BookmarkContainer = bookmarkContainer
            };
            bookmarkFolder.OnBookmarksUpdate += updatedEventHandler;


            return bookmarkFolder;
        }


        private void BookmarkFolderImplementation_OnBookmarksUpdate(object sender, BookmarkUpdatedEventArgs e)
        {
            var parentFolder = ParentFolder as BookmarkFolderBase;
            parentFolder?.BookmarkUpdated(sender, e);

            BookmarkContainer?.SetDataUpdated();
        }

        private void ReindexSortOrder(bool ReIndexFolders, bool ReIndexBookmarks)
        {
            if (ReIndexFolders)
            {
                var folderList = BookmarkFolders.OrderBy(f => f.SortOrder).ToList();
                for (int i = 0; i < folderList.Count; i++)
                {
                    folderList[i].SortOrder = i + 1;
                }

                folderList.Sort();
            }

            if (ReIndexBookmarks)
            {
                var bookmarkList = Bookmarks.OrderBy(f => f.SortOrder).ToList();
                for (int i = 0; i < bookmarkList.Count; i++)
                {
                    bookmarkList[i].SortOrder = i + 1;
                }

                bookmarkList.Sort();
            }
        }

        public override BookmarkFolderImplementation AddBookmarkFolder(string folderName)
        {
            var folder = new BookmarkFolderImplementation
            {
                Name = folderName,
                ParentFolder = this,
                SortOrder = BookmarkFolders.Count
            };

            BookmarkFolders.Add(folder);
            BookmarkUpdated(new BookmarkUpdatedEventArgs(BookmarkActions.CreatedBookmarkFolder, typeof (BookmarkFolder)));
            return folder;
        }

        public override BookmarkFolderImplementation InsertBookmarkFolder(string folderName, int sortOrder)
        {
            var folder = new BookmarkFolderImplementation
            {
                Name = folderName,
                ParentFolder = this,
                SortOrder = BookmarkFolders.Count
            };

            if (sortOrder < Bookmarks.Count && sortOrder >= 0)
            {
                folder.SortOrder = sortOrder;
                var postItems = BookmarkFolders.Where(b => b.SortOrder >= sortOrder).OrderBy(b => b.SortOrder).ToList();
                foreach (BookmarkFolder item in postItems)
                {
                    item.SortOrder = item.SortOrder + 1;
                }
            }

            BookmarkFolders.Add(folder);
            BookmarkUpdated(new BookmarkUpdatedEventArgs(BookmarkActions.CreatedBookmarkFolder, typeof (BookmarkFolder)));
            return folder;
        }

        public override void AddBookmark(string boookmarkName, ImageReferenceElement imgRef)
        {
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
                SortOrder = BookmarkFolders.Count
            };

            Bookmarks.Add(bookmark);
            BookmarkUpdated(new BookmarkUpdatedEventArgs(BookmarkActions.CreatedBookmark, typeof (Bookmark)));
        }

        public override void InsertBookmark(string boookmarkName, ImageReferenceElement imgRef, int sortOrder)
        {
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
                SortOrder = BookmarkFolders.Count
            };

            if (sortOrder < Bookmarks.Count && sortOrder >= 0)
            {
                bookmark.SortOrder = sortOrder;
                var postItems = Bookmarks.Where(b => b.SortOrder >= sortOrder).OrderBy(b => b.SortOrder).ToList();
                foreach (Bookmark item in postItems)
                {
                    item.SortOrder = item.SortOrder + 1;
                }
            }

            Bookmarks.Add(bookmark);
            BookmarkUpdated(new BookmarkUpdatedEventArgs(BookmarkActions.CreatedBookmark, typeof (Bookmark)));
        }

        public override bool DeleteBookmark(Bookmark bookmark)
        {
            bool success = Bookmarks.Remove(bookmark);
            if (success)
            {
                ReindexSortOrder(false, true);
            }
            return success;
        }

        public override bool DeleteBookmarkByFilename(string fileName)
        {
            Bookmark bookmarkToDelete = null;
            foreach (Bookmark bookmark in Bookmarks)
            {
                if (bookmark.FileName == fileName)
                {
                    bookmarkToDelete = bookmark;
                    break;
                }
            }

            if (bookmarkToDelete != null)
            {
                Bookmarks.Remove(bookmarkToDelete);
                ReindexSortOrder(false, true);
                return true;
            }

            return false;
        }


        public override bool DeleteBookmarkFolder(BookmarkFolder folder)
        {
            bool success = BookmarkFolders.Remove(folder);
            if (success)
            {
                ReindexSortOrder(false, true);
            }
            return success;
        }

        public override bool DeleteBookmarkFolderById(string folderId)
        {
            BookmarkFolder folderToDelete = null;

            foreach (BookmarkFolder folder in BookmarkFolders)
            {
                if (folder.BookmarkFolderId == folderId)
                {
                    folderToDelete = folder;
                    break;
                }
            }

            if (folderToDelete != null)
            {
                BookmarkFolders.Remove(folderToDelete);
                ReindexSortOrder(true, false);
                return true;
            }

            return false;
        }
    }
}