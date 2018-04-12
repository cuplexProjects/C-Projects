using System.Reflection;
using System.Windows.Forms;
using Autofac;
using DeleteDuplicateFiles.Managers;
using DeleteDuplicateFiles.Services;
using GeneralToolkitLib.Storage.Memory;
using Module = Autofac.Module;

namespace DeleteDuplicateFiles.Library.AutofacModules
{
    public class EliminateDuplecatesModule : Module
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

            var assembly = Assembly.GetExecutingAssembly();
            builder.RegisterAssemblyTypes(assembly)
                .AssignableTo<Form>();
        }
    }
}
