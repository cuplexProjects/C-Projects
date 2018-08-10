using System.IO;
using System.Reflection;
using Autofac;
using Autofac.Core;

namespace ImageView.UnitTest.TestHelper
{
    public static class ContainerFactory
    {
        private static readonly string TestDirectory = Path.Combine(Path.GetTempPath(), "ImageViewTestdata");

        public static IContainer BuildContainerForThumbnailTests()
        {
            var thisAssembly = Assembly.GetCallingAssembly();
            var builder = new ContainerBuilder();
            builder.RegisterAssemblyModules(thisAssembly);

            var container = builder.Build();

            return container;
        }

        public static string GetTestDirectory()
        {
            return TestDirectory;
        }

    }
}