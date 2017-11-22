using Apps.cuplex.se.Utilities;
using Autofac;

namespace Apps.cuplex.se
{
    public static class AutofacConfig
    {
        public static IContainer CreateContainer()
        {
            var builder = new ContainerBuilder();
           
            builder.Register(c => new ApplicationListHelper()).As<ApplicationListHelper>();
            var container = builder.Build();

            return container;
        }

        public static void Initialize()
        {
            var container = AutofacConfig.CreateContainer();
            //container.BeginLifetimeScope()
        }
    }
}