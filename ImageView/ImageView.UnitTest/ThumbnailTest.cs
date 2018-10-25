using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autofac;
using GeneralToolkitLib.Configuration;
using ImageViewer.Configuration;
using ImageViewer.Managers;
using ImageViewer.Repositories;
using ImageViewer.Services;
using ImageViewer.Storage;
using ImageViewer.UnitTests.Properties;
using ImageViewer.UnitTests.TestHelper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using NSubstitute.Proxies.CastleDynamicProxy;
using NSubstitute.Proxies.DelegateProxy;

namespace ImageViewer.UnitTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class ThumbnailTest
    {
        private static readonly string TestDirectory = ContainerFactory.GetTestDirectory();
        private static readonly string[] TestImages = { "testImg.jpg", "testImg2.jpg", "testImg3.jpg" };

        private static IApplicationBuildConfig _applicationBuildConfig;
        private ThumbnailService _thumbnailService;

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
            CreateThumbnailService();
        }

        [TestCleanup]
        public void ThumbnailTestCleanup()
        {
            _thumbnailService.Dispose();
        }

        private static void ClearTestDirectory()
        {
            var files = Directory.GetFiles(TestDirectory);
            foreach (string file in files)
            {
                File.Delete(file);
            }
        }

        [TestMethod]
        public void ThumbnailScanDirectory()
        {
            _thumbnailService.ScanDirectory(TestDirectory, false);

            var thumbNailImage = _thumbnailService.GetThumbnail(TestDirectory + TestImages[0]);
            Assert.IsNotNull(thumbNailImage, "Thumbnail image 1 was null");

            thumbNailImage = _thumbnailService.GetThumbnail(TestDirectory + TestImages[1]);
            Assert.IsNotNull(thumbNailImage, "Thumbnail image 2 was null");

            thumbNailImage = _thumbnailService.GetThumbnail(TestDirectory + TestImages[2]);
            Assert.IsNotNull(thumbNailImage, "Thumbnail image 3 was null");

            _thumbnailService.Dispose();
        }

        [TestMethod]
        public void ThumbnailLoadDatabase()
        {
            CreateThumbnailDatabase(_thumbnailService).Wait();
            bool result = _thumbnailService.LoadThumbnailDatabase();

            Assert.IsTrue(result, "Load thumbnail database failed");
            Assert.AreEqual(_thumbnailService.GetNumberOfCachedThumbnails(), 3, "Database did not contain 3 items");

            _thumbnailService.Dispose();
        }

        [TestMethod]
        public void ThumbnailOptimizeDatabaseAfterFileRemoval()
        {

            // Verify that there are testImages.Length thumbnails created
            Assert.IsTrue(_thumbnailService.GetNumberOfCachedThumbnails() == TestImages.Length, "The thumbnail cache did not contain thhe right amount of images");

            //Remove the first file
            File.Delete(TestDirectory + TestImages[0]);

            // Optimize DB
            _thumbnailService.OptimizeDatabase();

            // Verify that one thumbnail was removed
            Assert.IsTrue(_thumbnailService.GetNumberOfCachedThumbnails() == TestImages.Length - 1, "The thumbnail service did not remove a cached item");

            _thumbnailService.Dispose();
        }

        [TestMethod]
        public void ThumbnailOptimizeDatabaseAfterFileUpdated()
        {
            // Verify that there are testImages.Length thumbnails created
            Assert.IsTrue(_thumbnailService.GetNumberOfCachedThumbnails() == TestImages.Length, "The thumbnail cache did not contain thhe right amount of images");

            //Modify the first file
            var fs = File.OpenWrite(TestDirectory + TestImages[0]);
            var buffer = new byte[1];
            buffer[0] = 0xff;
            fs.Write(buffer, 0, 1);
            fs.Flush(true);
            fs.Close();

            // Optimize DB
            _thumbnailService.OptimizeDatabase();

            // Verify that one thumbnail was removed
            Assert.IsTrue(_thumbnailService.GetNumberOfCachedThumbnails() == TestImages.Length - 1, "The thumbnail service did not remove a cached item");

            _thumbnailService.Dispose();
        }

        private async Task<bool> CreateThumbnailDatabase(ThumbnailService thumbnailService)
        {
            return await CreateThumbnailDatabaseAsync(thumbnailService);
        }

        private async Task<bool> CreateThumbnailDatabaseAsync(ThumbnailService thumbnailService)
        {
            var scanTask = Task.Factory.StartNew(() =>
            {
                _thumbnailService.ScanDirectory(TestDirectory, false);
                return _thumbnailService.SaveThumbnailDatabase();


            });

            return await scanTask;
        }


        private void CreateThumbnailService()
        {
            var appSettingsFileRepository = new AppSettingsFileRepository();
            appSettingsFileRepository.LoadSettings();
            var applicationSettingsService = ApplicationSettingsService.CreateService(appSettingsFileRepository, new LocalStorageRegistryAccess(ContainerFactory.CompanyName, ContainerFactory.ProductName));
            applicationSettingsService.LoadSettings();



            //applicationSettingsService.CompanyName.Returns(ContainerFactory.CompanyName);
            //applicationSettingsService.ProductName.Returns(ContainerFactory.ProductName);
            //applicationSettingsService.LoadSettings();
            
            var fileManager = new FileManager(Path.Combine(TestDirectory, ContainerFactory.ThumbnailIndexFilename));
            var thumbnailManager = new ThumbnailManager(fileManager);
            _thumbnailService = new ThumbnailService(thumbnailManager);


            _thumbnailService.BasePath.Returns(_applicationBuildConfig.UserDataPath);
            _thumbnailService.ScanDirectory(TestDirectory, false);

        }
    }
}
