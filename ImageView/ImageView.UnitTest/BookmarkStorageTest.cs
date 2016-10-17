using System;
using System.IO;
using System.Linq;
using GeneralToolkitLib.ConfigHelper;
using ImageView.DataContracts;
using ImageView.Managers;
using ImageView.Models;
using ImageView.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ImageView.UnitTest
{
    [TestClass]
    public class BookmarkStorageTest
    {
        private static ImageReferenceElement _imageReference;
        private BookmarkService bookmarkService;

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

        // Use TestInitialize to run code before running each test 
        [TestInitialize()]
        public void MyTestInitialize()
        {
            bookmarkService = new BookmarkService();
            BookmarkManager bookmarkManager = bookmarkService.BookmarkManager;
            Assert.IsFalse(bookmarkManager.IsModified, "BookmarkManager can not be modified before test begins");
            Assert.IsTrue(bookmarkManager.RootFolder.Bookmarks.Count==0, "Test must start with empty bookmark list");
            Assert.IsTrue(bookmarkManager.RootFolder.BookmarkFolders.Count == 0, "Test must start with empty bookmark folder list");
        }

        [ClassCleanup()]
        public static void BookmarkStorageCleanup()
        {
            var files= Directory.GetFiles("c:\\temp", "*.dat");
            foreach (string filename in files)
            {
                File.Delete(filename);
            }
        }

        // Use TestCleanup to run code after each test has run
        [TestCleanup()]
        public void MyTestCleanup()
        {
           
        }

        [TestMethod]
        public void AddBookmarkAndReloadFromFile()
        {
            BookmarkManager bookmarkManager = bookmarkService.BookmarkManager;
            BookmarkFolder rootFolder = bookmarkManager.RootFolder;
            Assert.AreEqual("Root", rootFolder.Name, "Invaild Root folder name");

            bookmarkManager.AddBookmark(rootFolder.Id, "TestImageBookmark", _imageReference);
            bool saveSuccessful = bookmarkService.SaveBookmarks();
            Assert.IsTrue(saveSuccessful, "Saving bookmarks data file failed!");
            bookmarkService = new BookmarkService();
            bookmarkService.OpenBookmarks();
            bookmarkManager = bookmarkService.BookmarkManager;

            var bookmark = bookmarkManager.RootFolder.Bookmarks.ToList().FirstOrDefault();
            bool areEqual = CompareBookmarkToImgRef(bookmark, _imageReference);

            Assert.IsTrue(areEqual,"The loaded bookmark was not identical to the saved bookmark");
        }

        [TestMethod]
        public void AddBookmarkFolder()
        {
            BookmarkManager bookmarkManager = bookmarkService.BookmarkManager;
            BookmarkFolder rootFolder = bookmarkManager.RootFolder;
            var folder = bookmarkManager.AddBookmarkFolder(rootFolder.Id, "TestFolder");
            Assert.IsNotNull(folder, "Failed to add Folder");

            Assert.IsNotNull(folder.Id, "Folder id was null!");
            Assert.IsNotNull(folder.ParentFolderId, "Parent Folder id was null!");
            Assert.IsNotNull(folder.BookmarkFolders, "BookmarkFolder list was null!");
            Assert.IsNotNull(folder.Bookmarks, "Bookmarklist was null!");
            Assert.AreEqual(bookmarkManager.RootFolder.BookmarkFolders.FirstOrDefault(),folder,"Added folder was not equal to item in collection");
        }

        [TestMethod]
        public void InsertBookmarkFolder()
        {
            BookmarkManager bookmarkManager = bookmarkService.BookmarkManager;
            BookmarkFolder rootFolder = bookmarkManager.RootFolder;
            BookmarkFolder folder = bookmarkManager.AddBookmarkFolder(rootFolder.Id, "Folder1");
            Assert.IsNotNull(folder, "Failed to add Folder");
            folder = bookmarkManager.AddBookmarkFolder(rootFolder.Id, "Folder3");
            Assert.IsNotNull(folder, "Failed to add Folder");

            folder = bookmarkManager.InsertBookmarkFolder(rootFolder.Id, "Folder2", 1);
            Assert.IsNotNull(folder, "Failed to insert Folder");

            folder = bookmarkManager.RootFolder.BookmarkFolders.SingleOrDefault(f => f.SortOrder == 1 && f.Name == "Folder2");
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
            BookmarkManager bookmarkManager = bookmarkService.BookmarkManager;
            BookmarkFolder rootFolder = bookmarkManager.RootFolder;
            var bookmark1 = bookmarkManager.AddBookmark(rootFolder.Id, "Bookmark1", _imageReference);
            Assert.IsNotNull(bookmark1, "Failed to add Bookmark1");
            var bookmark3 = bookmarkManager.AddBookmark(rootFolder.Id, "Bookmark3", _imageReference);
            Assert.IsNotNull(bookmark3, "Failed to add Bookmark3");
            var bookmark2 = bookmarkManager.InsertBookmark(rootFolder.Id, "Bookmark2", _imageReference, 1);
            Assert.IsNotNull(bookmark2, "Failed to insert Bookmark2");

            var insertedItem = bookmarkManager.RootFolder.Bookmarks.SingleOrDefault(f => f.SortOrder == 1 && f.BoookmarkName == "Bookmark2");
            Assert.IsNotNull(insertedItem, "Could not find inserted item in collection!");
            Assert.IsTrue(CompareBookmarkToImgRef(insertedItem,_imageReference), "The inserted bookmark was not identical to the reference bookmark");
        }

        [TestMethod]
        public void TestDeleteBookmarkFolderById()
        {
            BookmarkManager bookmarkManager = bookmarkService.BookmarkManager;
            BookmarkFolder rootFolder = bookmarkManager.RootFolder;
            BookmarkFolder bookmarkFolder = bookmarkManager.AddBookmarkFolder(rootFolder.Id, "Folder1");
            string id = bookmarkFolder.Id;

            Assert.IsTrue(bookmarkManager.DeleteBookmarkFolderById(id),"Failed to delete bookmark folder!");
            Assert.IsTrue(rootFolder.BookmarkFolders.Count == 0, "Bookmark folder was not deleted!");

            bookmarkFolder = bookmarkManager.AddBookmarkFolder(rootFolder.Id, "FolderLevel1");
            BookmarkFolder bookmarkSubFolder = bookmarkManager.AddBookmarkFolder(bookmarkFolder.Id, "FolderLevel2");
            Bookmark bookmark = bookmarkManager.AddBookmark(bookmarkSubFolder.Id, "TestBookmark", _imageReference);
            Assert.IsNotNull(bookmark,"Failed to add bookmark to subfolder");

            Assert.IsTrue(bookmarkManager.DeleteBookmarkFolderById(bookmarkFolder.Id), "Failed to delete bookmark folder!");

            Assert.IsTrue(rootFolder.BookmarkFolders.Count == 0, "Not all bookmark folders where deleted!");
        }

        [TestMethod]
        public void TestDeleteBookmarkByFilename()
        {
            BookmarkManager bookmarkManager = bookmarkService.BookmarkManager;
            BookmarkFolder rootFolder = bookmarkManager.RootFolder;
            var bookmark1 = bookmarkManager.AddBookmark(rootFolder.Id, "Bookmark1", _imageReference);
            Assert.IsTrue(bookmarkManager.DeleteBookmarkByFilename(bookmark1.ParentFolderId, bookmark1.FileName), "Failed to delete bookmark!");
            Assert.IsTrue(rootFolder.Bookmarks.Count==0, "Bookmarks should be empty!");
        }

        [TestMethod]
        public void TestIsModified()
        {
            BookmarkManager bookmarkManager = bookmarkService.BookmarkManager;
            BookmarkFolder rootFolder = bookmarkManager.RootFolder;

            Assert.IsFalse(bookmarkManager.IsModified,"BookmarkManager should not be modified");
            bookmarkManager.AddBookmark(rootFolder.Id, "Bookmark1", _imageReference);
            Assert.IsTrue(bookmarkManager.IsModified, "BookmarkManager should be modified");
            bookmarkService.SaveBookmarks();
            Assert.IsFalse(bookmarkManager.IsModified, "BookmarkManager should not be modified");
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
