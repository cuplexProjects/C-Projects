using System.Collections.Generic;
using System.IO;
using Autofac;
using Autofac.Core;
using AutoMapper;
using GeneralToolkitLib.Storage.Memory;
using GeneralToolkitLib.Storage.Registry;
using ImageViewer.Managers;
using ImageViewer.Repositories;
using ImageViewer.Services;
using ImageViewer.Storage;
using ImageViewer.UnitTests.TestHelper;
using Module = Autofac.Module;

namespace ImageViewer.UnitTests.AutofacModules
{
    public class ImageViewTestModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<PasswordStorage>().SingleInstance();
            var fileManagerCompleteFilepath = Path.Combine(ContainerFactory.GetTestDirectory(), "thumbs.ibd");

            builder.RegisterType<FileManager>().InstancePerLifetimeScope().OnPreparing(args => { args.Parameters = new[] { TypedParameter.From(fileManagerCompleteFilepath) }; });
            builder.RegisterType<PasswordStorage>().SingleInstance();


            builder.RegisterType<LocalStorageRegistryAccess>()
                .As<IRegistryAccess>()
                .InstancePerLifetimeScope()
                .WithParameters(new List<Parameter>()
                {
                    new NamedParameter("companyName", "Cuplex"),
                    new NamedParameter("productName","UnitTest Runner")
                });

            builder.RegisterType<ApplicationSettingsService>()
                .As<ServiceBase>()
                .InstancePerLifetimeScope()
                .WithParameters(new List<Parameter>()
                {
                    new NamedParameter("registryAccess", new LocalStorageRegistryAccess("Cuplex","Unit Test Run "))
                });


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

            builder.RegisterAssemblyTypes(typeof(RepositoryBase).Assembly)
                .AssignableTo<RepositoryBase>()
                .AsSelf()
                .AsImplementedInterfaces();

            builder.Register(context => context.Resolve<MapperConfiguration>()
                    .CreateMapper())
                .As<IMapper>()
                .AutoActivate()
                .SingleInstance();

            builder.Register(Configure)
                .AutoActivate()
                .AsSelf()
                .AsImplementedInterfaces()
                .SingleInstance();
        }

        private static MapperConfiguration Configure(IComponentContext context)
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                var innerContext = context.Resolve<IComponentContext>();
                cfg.ConstructServicesUsing(innerContext.Resolve);

                foreach (var profile in context.Resolve<IEnumerable<Profile>>())
                {
                    cfg.AddProfile(profile);
                }
            });

            return configuration;
        }
    }
}