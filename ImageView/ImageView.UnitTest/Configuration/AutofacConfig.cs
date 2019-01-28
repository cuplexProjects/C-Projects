using System.Reflection;
using Autofac;
using GeneralToolkitLib.ConfigHelper;

namespace ImageViewer.UnitTests.Configuration
{
    public static class AutofacConfig
    {
        public static IContainer CreateContainer()
        {
            var builder = new ContainerBuilder();
            var thisAssembly = Assembly.GetExecutingAssembly();
            builder.RegisterAssemblyModules(thisAssembly);
            var container = builder.Build();
            return container;
        }

        public static Assembly GetAssembly()
        {
            return Assembly.GetExecutingAssembly();
        }
    }
}
