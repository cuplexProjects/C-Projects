using Autofac;
using GoogleDDNS.ConfigLib.Repositories;
using GoogleDDNS.ConfigLib.Services;

namespace GoogleDDNS.ConfigLib.Library.AutofacModules
{
    public class ConfigLibModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //builder.RegisterType<PasswordStorage>().SingleInstance();

            builder.RegisterAssemblyTypes(typeof(RepositoryBase).Assembly)
                   .AssignableTo<RepositoryBase>()
                   .AsSelf()
                   .AsImplementedInterfaces()
                   .SingleInstance();

            builder.RegisterAssemblyTypes(typeof(GoogleServiceBase).Assembly)
                   .AssignableTo<GoogleServiceBase>()
                   .AsSelf()
                   .AsImplementedInterfaces()
                   .SingleInstance();
        }
    }
}