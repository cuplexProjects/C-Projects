using Autofac;
using GeneralToolkitLib.ConfigHelper;
using GeneralToolkitLib.Storage.Memory;
using SecureMemo.Services;
using SecureMemo.Storage;
using SecureMemo.Utility;

namespace SecureMemo.Configuration
{
    public static class AutofacConfig
    {
        public static IContainer CreateContainer()
        {
            var builder = new ContainerBuilder();

            string settingsFolderPath = ConfigSpecificSettings.GetSettingsFolderPath(false);
            string iniConfigFilePath = settingsFolderPath + "\\ApplicationSettings.ini";
            var appSettings = new AppSettingsService(ConfigHelper.GetDefaultSettings(), new IniConfigFileManager(), iniConfigFilePath);
            var memoStorageService = new MemoStorageService(appSettings, settingsFolderPath);

            builder.RegisterInstance(appSettings).As<AppSettingsService>();
            builder.RegisterInstance(memoStorageService).As<MemoStorageService>();
            builder.RegisterType<PasswordStorage>();
            builder.RegisterType<FormMain>();
            builder.RegisterType<FormSettings>();
            builder.RegisterType<FormRestoreBackup>();

            var container = builder.Build();

            return container;
        }
    }
}