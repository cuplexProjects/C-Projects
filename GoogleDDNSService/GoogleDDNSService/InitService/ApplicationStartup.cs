using System.Reflection;
using Autofac;

namespace GoogleDDNSService.InitService
{
    public static class ApplicationStartup
    {
        public static IContainer InitializeSystem()
        {
            var container = CreateContainer();
            return container;
        }

        private static IContainer CreateContainer()
        {
            var builder = new ContainerBuilder();
            var thisAssembly = Assembly.GetExecutingAssembly();
            AssemblyName[] assemblyRefs = thisAssembly.GetReferencedAssemblies();

            foreach (var assemblName in assemblyRefs) builder.RegisterAssemblyModules(Assembly.Load(assemblName));

            builder.RegisterAssemblyModules(thisAssembly);
            var container = builder.Build();

            return container;
        }
    }
}