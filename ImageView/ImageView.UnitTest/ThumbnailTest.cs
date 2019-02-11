using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using Autofac;
using GeneralToolkitLib.Configuration;
using ImageViewer.Configuration;
using ImageViewer.Repositories;
using ImageViewer.Services;
using ImageViewer.UnitTests.Properties;
using ImageViewer.UnitTests.TestHelper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace ImageViewer.UnitTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class ThumbnailTest
    {
        private static readonly string TestDirectory = "c:\\temp\\";
        private static readonly string[] TestImages = { "testImg.jpg", "testImg2.jpg", "testImg3.jpg" };
        private readonly IContainer _container;
        private readonly ILifetimeScope _lifetimeScope;
        private static IApplicationBuildConfig _applicationBuildConfig;


        public ThumbnailTest()
        {
            ApplicationBuildConfig.SetOverrideUserDataPath(TestDirectory);
            _container = AutofacConfig.CreateContainer();
            _lifetimeScope = _container.BeginLifetimeScope();
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

            //ApplicationBuildConfig.SetOverrideUserDataPath(TestDirectory);

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
            CreateThumbnailService(_lifetimeScope);
        }

        [TestCleanup]
        public void ThumbnailTestCleanup()
        {
            _lifetimeScope.Dispose();
            _container.Dispose();
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
            using (var scope = _lifetimeScope.BeginLifetimeScope())
            {
                var thumbnailService = scope.Resolve<ThumbnailService>();

                thumbnailService.ScanDirectory(TestDirectory, false);

                var thumbNailImage = thumbnailService.GetThumbnail(TestDirectory + TestImages[0]);
                Assert.IsNotNull(thumbNailImage, "Thumbnail image 1 was null");

                thumbNailImage = thumbnailService.GetThumbnail(TestDirectory + TestImages[1]);
                Assert.IsNotNull(thumbNailImage, "Thumbnail image 2 was null");

                thumbNailImage = thumbnailService.GetThumbnail(TestDirectory + TestImages[2]);
                Assert.IsNotNull(thumbNailImage, "Thumbnail image 3 was null");
            }

        }

        [TestMethod]
        public void ThumbnailLoadDatabase()
        {
            using (var scope = _lifetimeScope.BeginLifetimeScope())
            {
                var thumbnailService = scope.Resolve<ThumbnailService>();

                CreateThumbnailDatabase(thumbnailService);
                bool result = thumbnailService.LoadThumbnailDatabase();

                Assert.IsTrue(result, "Load thumbnail database failed");
                Assert.AreEqual(thumbnailService.GetNumberOfCachedThumbnails(), 3, "Database did not contain 3 items");
            }

        }

        [TestMethod]
        public void ThumbnailOptimizeDatabaseAfterFileRemoval()
        {
            using (var scope = _lifetimeScope.BeginLifetimeScope())
            {
                var thumbnailService = scope.Resolve<ThumbnailService>();


                // Verify that there are testImages.Length thumbnails created
                Assert.IsTrue(thumbnailService.GetNumberOfCachedThumbnails() == TestImages.Length, "The thumbnail cache did not contain thhe right amount of images");

                //Remove the first file
                File.Delete(TestDirectory + TestImages[0]);

                // Optimize DB
                thumbnailService.OptimizeDatabaseAsync().Wait();

                // Verify that one thumbnail was removed
                Assert.IsTrue(thumbnailService.GetNumberOfCachedThumbnails() == TestImages.Length - 1, "The thumbnail service did not remove a cached item");


                thumbnailService.Dispose();
            }
        }

        [TestMethod]
        public void ThumbnailOptimizeDatabaseAfterFileUpdated()
        {
            using (var scope = _lifetimeScope.BeginLifetimeScope())
            {
                var thumbnailService = scope.Resolve<ThumbnailService>();

                // Verify that there are testImages.Length thumbnails created
                Assert.IsTrue(thumbnailService.GetNumberOfCachedThumbnails() == TestImages.Length, "The thumbnail cache did not contain thhe right amount of images");

                //Modify the first file
                var fs = File.OpenWrite(TestDirectory + TestImages[0]);
                var buffer = new byte[1];
                buffer[0] = 0xff;
                fs.Write(buffer, 0, 1);
                fs.Flush(true);
                fs.Close();

                // Optimize DB
                thumbnailService.OptimizeDatabaseAsync();

                // Verify that one thumbnail was removed
                Assert.IsTrue(thumbnailService.GetNumberOfCachedThumbnails() == TestImages.Length - 1, "The thumbnail service did not remove a cached item");

            }
        }

        private bool CreateThumbnailDatabase(ThumbnailService thumbnailService)
        {
            thumbnailService.ScanDirectory(TestDirectory, false);
            return thumbnailService.SaveThumbnailDatabase();
        }


        private void CreateThumbnailService(ILifetimeScope lifetimeScope)
        {
            using (var scope = lifetimeScope.BeginLifetimeScope())
            {
                var appSettingsFileRepository = new AppSettingsFileRepository();
                var thumbnailService = scope.Resolve<ThumbnailService>();
                appSettingsFileRepository.LoadSettings();
                var applicationSettingsService = scope.Resolve<ApplicationSettingsService>();
                applicationSettingsService.LoadSettings();

                thumbnailService.BasePath.Returns(_applicationBuildConfig.UserDataPath);
                thumbnailService.ScanDirectory(TestDirectory, false);
            }
        }
    }
}
