using System.Reflection;
using Autofac;
using GeneralToolkitLib.ConfigHelper;

namespace ImageView.Configuration
{
    public static class AutofacConfig
    {
        public static IContainer CreateContainer()
        {
            var builder = new ContainerBuilder();
            var thisAssembly = Assembly.GetCallingAssembly();


            var generalToolKitAssembly = AssemblyHelper.GetAssembly();
            if (generalToolKitAssembly != null)
            {
                builder.RegisterAssemblyModules(generalToolKitAssembly);
            }
            
            builder.RegisterAssemblyModules(thisAssembly);
            var container = builder.Build();


            return container;
        }
    }
}
