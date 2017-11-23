using Autofac;
using GeneralToolkitLib.Storage.Memory;
using ImageView.Managers;
using ImageView.Services;

namespace ImageView
{
    public static class AutofacConfig
    {
        public static IContainer CreateContainer()
        {
            var builder = new ContainerBuilder();

            var bookmarkmanager = new BookmarkManager();
            ApplicationSettingsService applicationSettingsService= new ApplicationSettingsService();
            applicationSettingsService.LoadSettings();

            builder.Register(c => bookmarkmanager).As<BookmarkManager>();
            builder.Register(c => new BookmarkService(bookmarkmanager, applicationSettingsService)).As<BookmarkService>();
            builder.Register(c => applicationSettingsService).As<ApplicationSettingsService>();


            builder.RegisterType<PasswordStorage>();
            builder.RegisterType<FormMain>();
            builder.RegisterType<FormSettings>();
            builder.RegisterType<FormAddBookmark>();
            builder.RegisterType<FormThumbnailView>();
            builder.RegisterType<FormBookmarks>();
            builder.RegisterType<ImageCacheService>();
           

            var container = builder.Build();

            return container;
        }
    }
}
