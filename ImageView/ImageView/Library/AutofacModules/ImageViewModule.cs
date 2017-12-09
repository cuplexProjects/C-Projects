using System.Reflection;
using System.Windows.Forms;
using Autofac;
using GeneralToolkitLib.Storage.Memory;
using ImageView.Managers;
using ImageView.Services;
using Module = Autofac.Module;

namespace ImageView.Library.AutofacModules
{
    public class ImageViewModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<PasswordStorage>().SingleInstance();

            builder.RegisterAssemblyTypes(typeof(ManagerBase).Assembly)
                .AssignableTo<ManagerBase>()
                .AsSelf()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(typeof(ServiceBase).Assembly)
                .AssignableTo<ServiceBase>()
                .AsSelf()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            var assembly = Assembly.GetExecutingAssembly();
            builder.RegisterAssemblyTypes(assembly)
                .AssignableTo<Form>();
        }
    }
}
