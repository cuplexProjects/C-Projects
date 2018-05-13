using System.Reflection;
using Autofac;

namespace FileSystemCore.Configuration
{
    public static class AutofacConfig
    {
        public static IContainer CreateContainer()
        {
            var builder = new ContainerBuilder();
            var thisAssembly = Assembly.GetExecutingAssembly();


            var directoryDocAssembly = Assembly.GetExecutingAssembly();
            builder.RegisterAssemblyModules(directoryDocAssembly);

            builder.RegisterAssemblyModules(thisAssembly);

            var container = builder.Build();
            return container;
        }
    }
}
