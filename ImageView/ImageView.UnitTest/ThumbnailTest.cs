using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.IO;
using ImageView.Services;
using ImageView.UnitTest.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ImageView.UnitTest
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class ThumbnailTest
    {
        private const string TestDirectory = @"c:\temp\thumbnailTest\";
        private static readonly string[] testImages = { "testImg.jpg", "testImg2.jpg", "testImg3.jpg" };

        [ClassInitialize]
        public static void TestClassInit(TestContext testContext)
        {
            // Create test directory if it does not exist

            if (!Directory.Exists(TestDirectory))
            {
                Directory.CreateDirectory(TestDirectory);
            }
            else
            {
                ClearTestDirectory();
            }

            // Create test data
            Image img = Resources.testImg;
            img.Save(TestDirectory + testImages[0]);

            img = Resources.anonymus;
            img.Save(TestDirectory + testImages[1]);

            img = Resources.anonymus_small;
            img.Save(TestDirectory + testImages[2]);

        }

        [ClassCleanup]
        public static void TestClassCleanup()
        {
            ClearTestDirectory();
        }

        [TestInitialize]
        public void ThumbnailTestInitialize()
        {

        }

        [TestCleanup]
        public void ThumbnailTestCleanup()
        {

        }

        private static void ClearTestDirectory()
        {
            string[] Files = Directory.GetFiles(TestDirectory);
            foreach (string file in Files)
            {
                File.Delete(file);
            }
        }

        [TestMethod]
        public void ThumbnailScanDirectory()
        {
            ThumbnailService thumbnailService = new ThumbnailService(TestDirectory);
            thumbnailService.ScanDirectory(TestDirectory);

            Image thumbNailImage = thumbnailService.GetThumbnail(TestDirectory + testImages[0]);
            Assert.IsNotNull(thumbNailImage,"Thumbnail image 1 was null");

            thumbNailImage = thumbnailService.GetThumbnail(TestDirectory + testImages[1]);
            Assert.IsNotNull(thumbNailImage, "Thumbnail image 2 was null");

            thumbNailImage = thumbnailService.GetThumbnail(TestDirectory + testImages[2]);
            Assert.IsNotNull(thumbNailImage, "Thumbnail image 3 was null");

            thumbnailService.Dispose();
        }

    }
}
