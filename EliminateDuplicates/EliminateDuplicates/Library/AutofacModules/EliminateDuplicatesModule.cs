using System.Reflection;
using System.Windows.Forms;
using Autofac;
using DeleteDuplicateFiles.Managers;
using DeleteDuplicateFiles.Repositories;
using DeleteDuplicateFiles.Services;
using GeneralToolkitLib.Storage.Memory;
using JetBrains.Annotations;
using Module = Autofac.Module;

namespace DeleteDuplicateFiles.Library.AutofacModules
{
    [UsedImplicitly]
    public class EliminateDuplicatesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<PasswordStorage>().SingleInstance();

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

            //var assembly = Assembly.GetExecutingAssembly().GetTypes();
            builder.RegisterAssemblyTypes(typeof(FrmMain).Assembly)
                   .AssignableTo<FrmMain>()
                   .AsSelf()
                   .AsImplementedInterfaces()
                   .SingleInstance();

            builder.RegisterAssemblyTypes(typeof(FrmSettings).Assembly)
                   .AssignableTo<FrmSettings>()
                   .AsSelf()
                   .AsImplementedInterfaces()
                   .SingleInstance();

            var assembly = Assembly.GetExecutingAssembly();
            builder.RegisterAssemblyTypes(assembly)
                   .AssignableTo<Form>();

        }
     
    }
}
