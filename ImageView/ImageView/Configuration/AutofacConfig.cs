using System.Reflection;
using Autofac;
using GeneralToolkitLib.ConfigHelper;

namespace ImageViewer.Configuration
{
    public static class AutofacConfig
    {
        public static IContainer CreateContainer()
        {
            var builder = new ContainerBuilder();
            var thisAssembly = Assembly.GetCallingAssembly();
            Assembly[] coreAssemlies = new Assembly[2];
            var generalToolKitAssembly = AssemblyHelper.GetAssembly();

            coreAssemlies[0] = thisAssembly;
            coreAssemlies[1] = generalToolKitAssembly;
           
            if (generalToolKitAssembly != null)
            {
                builder.RegisterAssemblyModules(generalToolKitAssembly);
            }

            //builder.RegisterAssemblyTypes(coreAssemlies);
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
