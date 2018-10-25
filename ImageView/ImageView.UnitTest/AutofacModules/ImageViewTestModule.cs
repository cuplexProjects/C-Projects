using System.IO;
using System.Reflection;
using Autofac;
using Autofac.Core;
using GeneralToolkitLib.Storage.Memory;
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

            builder.RegisterType<FileManager>().SingleInstance().OnPreparing(args => { args.Parameters = new[] { TypedParameter.From(fileManagerCompleteFilepath) }; });
            builder.RegisterType<PasswordStorage>().SingleInstance();

            builder.RegisterType<LocalStorageRegistryAccess>()
                .SingleInstance()
                .AsSelf()
                .OnPreparing(args => { args.Parameters = new[] { new NamedParameter("companyName", "cuplex"), new NamedParameter("productName", "ImageViwerUnitTest") }; });


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


        }
    }
}