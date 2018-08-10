using System.IO;
using Autofac;
using GeneralToolkitLib.Storage.Memory;
using ImageView.Managers;
using ImageView.Services;
using ImageView.UnitTest.TestHelper;
using Module = Autofac.Module;

namespace ImageView.UnitTest.AutofacModules
{
    public class ImageViewTestModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<PasswordStorage>().SingleInstance();
            var fileManagerCompleteFilepath = Path.Combine(ContainerFactory.GetTestDirectory(), "thumbs.ibd");

            builder.RegisterType<FileManager>().SingleInstance().OnPreparing(args => { args.Parameters = new[] { TypedParameter.From(fileManagerCompleteFilepath) }; });

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


        }
    }
}