using Autofac;
using FileSystemCore.Managers;
using FileSystemCore.Repositories;
using FileSystemCore.Services;
using FileSystemRules.DataContext;
using Hangfire.Annotations;
using Module = Autofac.Module;

namespace FileSystemCore.Library.AutofacModules
{
    [UsedImplicitly]
    public class DirectoryDocModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            

            builder.RegisterAssemblyTypes(typeof(ManagerBase).Assembly)
                   .AssignableTo<ManagerBase>()
                   .AsSelf()
                   .AsImplementedInterfaces()
                   .SingleInstance();

            builder.RegisterAssemblyTypes(typeof(ServiceBase).Assembly)
                   .AssignableTo<ServiceBase>()
                   .AsSelf()
                   .AsImplementedInterfaces()
                   .SingleInstance();

            builder.RegisterAssemblyTypes(typeof(RepositoryBase).Assembly)
                   .AssignableTo<RepositoryBase>()
                   .AsSelf()
                   .AsImplementedInterfaces()
                   .SingleInstance();
            

            builder.RegisterAssemblyTypes(typeof(DataContextBase).Assembly)
                   .AssignableTo<DataContextBase>()
                   .AsSelf()
                   .AsImplementedInterfaces()
                   .InstancePerLifetimeScope();
        }
     
    }
}
