using System.Reflection;
using Autofac;
using GoogleDDNS.ConfigLib.Services;
using GoogleDDNSService.Managers;
using GoogleDDNSService.PropagationModules;
using Module = Autofac.Module;

namespace GoogleDDNSService.Library.AutofacModules
{
    public class DDNSServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //builder.RegisterType<PasswordStorage>().SingleInstance();

            builder.RegisterAssemblyTypes(typeof(ManagerBase).Assembly)
                   .AssignableTo<ManagerBase>()
                   .AsSelf()
                   .AsImplementedInterfaces()
                   .SingleInstance();

            builder.RegisterAssemblyTypes(typeof(PropagationModuleBase).Assembly)
                   .AssignableTo<PropagationModuleBase>()
                   .AsSelf()
                   .AsImplementedInterfaces()
                   .SingleInstance();


            var assembly = Assembly.GetExecutingAssembly();
            builder.RegisterAssemblyTypes(assembly)
                   .AssignableTo<DDNSService>()
                   .AsSelf()
                   .AsImplementedInterfaces()
                   .SingleInstance();
        }
    }
}