using System;
using System.IO;
using System.Linq;
using GeneralToolkitLib.ConfigHelper;
using ImageView.DataContracts;
using ImageView.Models;
using ImageView.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ImageView.UnitTest
{
    [TestClass]
    public class BookmarkStorageTest
    {
        private static ImageReferenceElement _imageReference;

        [ClassInitialize()]
        public static void BookmarkStorageInitialize(TestContext testContext)
        {
            GlobalSettings.UnitTestInitialize("c:\\temp\\");
            _imageReference = new ImageReferenceElement
            {
                Directory = "c:\\temp\\",
                FileName = "testImage.jpg",
                CreationTime = DateTime.Now,
                Size = 1024,
                LastAccessTime = DateTime.Now.Date,
                LastWriteTime = DateTime.Now.Date
            };
            _imageReference.CompletePath = _imageReference.Directory + _imageReference.FileName;

        }
        
        [ClassCleanup()]
        public static void BookmarkStorageCleanup()
        {
            BookmarkService.ClearInstance();
            var files= Directory.GetFiles("c:\\temp", "*.dat");
            foreach (string filename in files)
            {
                File.Delete(filename);
            }
        }

        [TestMethod]
        public void AddBookmarkAndReloadFromFile()
        {
            BookmarkFolder rootFolder = BookmarkService.Instance.BookmarkManager.RootFolder;
            Assert.AreEqual("Root", rootFolder.Name, "Invaild Root folder name");

            BookmarkService.Instance.BookmarkManager.AddBookmark(rootFolder.Id, "TestImageBookmark", _imageReference);
            bool saveSuccessful = BookmarkService.Instance.SaveBookmarks();
            Assert.IsTrue(saveSuccessful, "Saving bookmarks data file failed!");
            BookmarkService.ClearInstance();


            var bookmark = BookmarkService.Instance.BookmarkManager.RootFolder.Bookmarks.ToList().FirstOrDefault();
            bool areEqual = CompareBookmarkToImgRef(bookmark, _imageReference);

            Assert.IsTrue(areEqual,"The loaded bookmark was not identical to the saved bookmark");
        }

        [TestMethod]
        public void AddBookmarkFolder()
        {
            BookmarkFolder rootFolder = BookmarkService.Instance.BookmarkManager.RootFolder;
            var folder = BookmarkService.Instance.BookmarkManager.AddBookmarkFolder(rootFolder.Id, "TestFolder");
            Assert.IsNotNull(folder, "Failed to add Folder");

            Assert.IsNotNull(folder.Id, "Folder id was null!");
            Assert.IsNotNull(folder.ParentFolderId, "Parent Folder id was null!");
            Assert.IsNotNull(folder.BookmarkFolders, "BookmarkFolder list was null!");
            Assert.IsNotNull(folder.Bookmarks, "Bookmarklist was null!");
            Assert.AreEqual(BookmarkService.Instance.BookmarkManager.RootFolder.BookmarkFolders.FirstOrDefault(),folder,"Added folder was not equal to item in collection");
        }

        [TestMethod]
        public void InsertBookmarkFolder()
        {
            BookmarkFolder rootFolder = BookmarkService.Instance.BookmarkManager.RootFolder;
            BookmarkFolder folder = BookmarkService.Instance.BookmarkManager.AddBookmarkFolder(rootFolder.Id, "Folder1");
            Assert.IsNotNull(folder, "Failed to add Folder");
            folder = BookmarkService.Instance.BookmarkManager.AddBookmarkFolder(rootFolder.Id, "Folder3");
            Assert.IsNotNull(folder, "Failed to add Folder");

            folder = BookmarkService.Instance.BookmarkManager.InsertBookmarkFolder(rootFolder.Id, "Folder2", 1);
            Assert.IsNotNull(folder, "Failed to insert Folder");

            folder = BookmarkService.Instance.BookmarkManager.RootFolder.BookmarkFolders.SingleOrDefault(f => f.SortOrder == 1 && f.Name == "Folder2");
            Assert.IsNotNull(folder,"Could not find inserted item in collection!");

            Assert.IsNotNull(folder.BookmarkFolders,"Inserted item Folder list is null!");
            Assert.IsNotNull(folder.Bookmarks,"Inserted item Bookmark list is null!");
            Assert.IsNotNull(folder.Id,"Inserted item Id is null!");
            Assert.IsNotNull(folder.Name,"Inserted item Name is null!");
            Assert.IsNotNull(folder.ParentFolderId,"Inserted item ParentFolderId is null!");
        }

        [TestMethod]
        public void InsertBookmark()
        {
            BookmarkFolder rootFolder = BookmarkService.Instance.BookmarkManager.RootFolder;
            var bookmark1 = BookmarkService.Instance.BookmarkManager.AddBookmark(rootFolder.Id, "Bookmark1", _imageReference);
            Assert.IsNotNull(bookmark1, "Failed to add Bookmark1");
            var bookmark3 = BookmarkService.Instance.BookmarkManager.AddBookmark(rootFolder.Id, "Bookmark3", _imageReference);
            Assert.IsNotNull(bookmark3, "Failed to add Bookmark3");
            var bookmark2 = BookmarkService.Instance.BookmarkManager.InsertBookmark(rootFolder.Id, "Bookmark2", _imageReference, 1);
            Assert.IsNotNull(bookmark2, "Failed to insert Bookmark2");

            var insertedItem = BookmarkService.Instance.BookmarkManager.RootFolder.Bookmarks.SingleOrDefault(f => f.SortOrder == 1 && f.BoookmarkName == "Bookmark2");
            Assert.IsNotNull(insertedItem, "Could not find inserted item in collection!");
            Assert.IsTrue(CompareBookmarkToImgRef(insertedItem,_imageReference), "The inserted bookmark was not identical to the reference bookmark");
        }

        private bool CompareBookmarkToImgRef(Bookmark bookmark, ImageReferenceElement imageReference)
        {
            return imageReference.Size == bookmark.Size &&
                   imageReference.CompletePath == bookmark.CompletePath &&
                   imageReference.CreationTime == bookmark.CreationTime &&
                   imageReference.Directory == bookmark.Directory &&
                   imageReference.FileName == bookmark.FileName &&
                   imageReference.LastAccessTime == bookmark.LastAccessTime &&
                   imageReference.LastWriteTime == bookmark.LastWriteTime;
        }
    }
}
