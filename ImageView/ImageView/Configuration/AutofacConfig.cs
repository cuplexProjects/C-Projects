using System;
using System.CodeDom;
using System.Linq;
using System.Reflection;
using Autofac;
using GeneralToolkitLib.ConfigHelper;
using GeneralToolkitLib.Storage.Memory;
using ImageView.Managers;
using ImageView.Services;

namespace ImageView.Configuration
{
    public static class AutofacConfig
    {
        public static IContainer CreateContainer()
        {
            var builder = new ContainerBuilder();
            var thisAssembly = Assembly.GetCallingAssembly();


            var generalToolKitAssembly = AssemblyHelper.GetAssembly();
            if (generalToolKitAssembly != null)
            {
                builder.RegisterAssemblyModules(generalToolKitAssembly);
            }
            
            builder.RegisterAssemblyModules(thisAssembly);

            var container = builder.Build();
            


            //var bookmarkmanager = new BookmarkManager();
            //ApplicationSettingsService applicationSettingsService= new ApplicationSettingsService();
            //applicationSettingsService.LoadSettings();
            //builder.Register(c => applicationSettingsService).As<ApplicationSettingsService>();
            //builder.Register(c => bookmarkmanager).As<BookmarkManager>();
            //builder.Register(c => new BookmarkService(bookmarkmanager, applicationSettingsService)).As<BookmarkService>();

            //builder.RegisterType<PasswordStorage>().SingleInstance();
            //builder.RegisterType<FormMain>();
            //builder.RegisterType<FormSettings>();
            //builder.RegisterType<FormAddBookmark>();
            //builder.RegisterType<FormThumbnailView>();
            //builder.RegisterType<FormBookmarks>();
            //builder.RegisterType<ImageCacheService>().SingleInstance();




            return container;
        }
    }
}
