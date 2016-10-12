using System;
using System.IO;
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
                CreationTime = DateTime.Now
            };
            _imageReference.CompletePath = _imageReference.Directory + _imageReference.FileName;

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

        [TestMethod]
        public void AddBookmark()
        {
            BookmarkFolder rootFolder = BookmarkService.Instance.BookmarkManager.RootFolder;
            Assert.AreEqual("Root",rootFolder.Name,"Invaild Root folder name");

            BookmarkService.Instance.BookmarkManager.AddBookmark(rootFolder.Id,"TestImageBookmark", _imageReference);
            bool saveSuccessful = BookmarkService.Instance.SaveBookmarks();
            Assert.IsTrue(saveSuccessful, "Saving bookmarks data file failed!");


        }
    }
    
}
