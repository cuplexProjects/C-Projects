using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using Autofac;
using GeneralToolkitLib.Configuration;
using ImageView.Configuration;
using ImageView.Managers;
using ImageView.Services;
using ImageView.UnitTest.Properties;
using ImageView.UnitTest.TestHelper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace ImageView.UnitTest
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class ThumbnailTest
    {
        private static readonly string TestDirectory = ContainerFactory.GetTestDirectory();
        private static readonly string[] TestImages = { "testImg.jpg", "testImg2.jpg", "testImg3.jpg" };
        private static IApplicationBuildConfig _applicationBuildConfig;
        private IContainer _container;
        private ILifetimeScope _lifetimeScope;

        public ThumbnailTest()
        {

        }

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

            ApplicationBuildConfig.SetOverrideUserDataPath(TestDirectory);

            // Create test data
            Image img = Resources.testImg;
            img.Save(Path.Combine(TestDirectory, TestImages[0]));

            img = Resources.anonymus;
            img.Save(Path.Combine(TestDirectory, TestImages[1]));

            img = Resources.anonymus_small;
            img.Save(Path.Combine(TestDirectory, TestImages[2]));

            _applicationBuildConfig = Substitute.For<IApplicationBuildConfig>();
            _applicationBuildConfig.UserDataPath.Returns(TestDirectory);
            _applicationBuildConfig.DebugMode.Returns(true);
        }

        [ClassCleanup]
        public static void TestClassCleanup()
        {
            ClearTestDirectory();
        }

        [TestInitialize]
        public void ThumbnailTestInitialize()
        {
            _container = ContainerFactory.BuildContainerForThumbnailTests();
            _lifetimeScope = _container.BeginLifetimeScope();
        }

        [TestCleanup]
        public void ThumbnailTestCleanup()
        {
            _lifetimeScope.Dispose();
            _container.Dispose();
        }

        private static void ClearTestDirectory()
        {
            string[] files = Directory.GetFiles(TestDirectory);
            foreach (string file in files)
            {
                File.Delete(file);
            }
        }

        [TestMethod]
        public void ThumbnailScanDirectory()
        {
            ThumbnailService thumbnailService = GetThumbnailService();
            thumbnailService.ScanDirectory(TestDirectory, false);

            Image thumbNailImage = thumbnailService.GetThumbnail(TestDirectory + TestImages[0]);
            Assert.IsNotNull(thumbNailImage, "Thumbnail image 1 was null");

            thumbNailImage = thumbnailService.GetThumbnail(TestDirectory + TestImages[1]);
            Assert.IsNotNull(thumbNailImage, "Thumbnail image 2 was null");

            thumbNailImage = thumbnailService.GetThumbnail(TestDirectory + TestImages[2]);
            Assert.IsNotNull(thumbNailImage, "Thumbnail image 3 was null");

            thumbnailService.Dispose();
        }

        [TestMethod]
        public void ThumbnailLoadDatabase()
        {
            ThumbnailService thumbnailService = _lifetimeScope.Resolve<ThumbnailService>(); // GetThumbnailService();

            CreateThumbnailDatabase(thumbnailService);
            bool result = thumbnailService.LoadThumbnailDatabase();

            Assert.IsTrue(result, "Load thumbnail database failed");
            Assert.AreEqual(thumbnailService.GetNumberOfCachedThumbnails(), 3, "Database did not contain 3 items");

            thumbnailService.Dispose();
        }

        [TestMethod]
        public void ThumbnailOptimizeDatabaseAfterFileRemoval()
        {
            ThumbnailService thumbnailService = GetThumbnailService();

            // Verify that there are testImages.Length thumbnails created
            Assert.IsTrue(thumbnailService.GetNumberOfCachedThumbnails() == TestImages.Length, "The thumbnail cache did not contain thhe right amount of images");

            //Remove the first file
            File.Delete(TestDirectory + TestImages[0]);

            // Optimize DB
            thumbnailService.OptimizeDatabase();

            // Verify that one thumbnail was removed
            Assert.IsTrue(thumbnailService.GetNumberOfCachedThumbnails() == TestImages.Length - 1, "The thumbnail service did not remove a cached item");

            thumbnailService.Dispose();
        }

        [TestMethod]
        public void ThumbnailOptimizeDatabaseAfterFileUpdated()
        {
            ThumbnailService thumbnailService = GetThumbnailService();

            // Verify that there are testImages.Length thumbnails created
            Assert.IsTrue(thumbnailService.GetNumberOfCachedThumbnails() == TestImages.Length, "The thumbnail cache did not contain thhe right amount of images");

            //Modify the first file
            FileStream fs = File.OpenWrite(TestDirectory + TestImages[0]);
            byte[] buffer = new byte[1];
            buffer[0] = 0xff;
            fs.Write(buffer, 0, 1);
            fs.Flush(true);
            fs.Close();

            // Optimize DB
            thumbnailService.OptimizeDatabase();

            // Verify that one thumbnail was removed
            Assert.IsTrue(thumbnailService.GetNumberOfCachedThumbnails() == TestImages.Length - 1, "The thumbnail service did not remove a cached item");

            thumbnailService.Dispose();
        }

        private bool CreateThumbnailDatabase(ThumbnailService thumbnailService)
        {
            return CreateThumbnailDatabaseAsync(thumbnailService).Result;
        }

        private async Task<bool> CreateThumbnailDatabaseAsync(ThumbnailService thumbnailService)
        {
            var scanTask = Task.Factory.StartNew(() =>
            {
                thumbnailService.ScanDirectory(TestDirectory, false);
                return thumbnailService.SaveThumbnailDatabase();


            });

            return await scanTask;
        }


        private ThumbnailService GetThumbnailService()
        {
            //FileManager fileManager = new FileManager(Path.Combine( TestDirectory , "thumbs.ibd"));
            //ThumbnailManager thumbnailManager = new ThumbnailManager(fileManager);
            //ThumbnailService thumbnailService = new ThumbnailService(thumbnailManager);

            var thumbnailService = _lifetimeScope.Resolve<ThumbnailService>();
            //thumbnailService.BasePath.Returns(_applicationBuildConfig.UserDataPath);
            //thumbnailService.ScanDirectory(TestDirectory, false);
            return thumbnailService;
        }
    }
}
