using System.IO;
using System.Reflection;
using Autofac;
using GeneralToolkitLib.ConfigHelper;
using GeneralToolkitLib.Configuration;
using SecureMemo.Services;
using SecureMemo.Storage;
using SecureMemo.Utility;

namespace SecureMemo.Configuration
{
    public static class AutofacConfig
    {
        public static IContainer CreateContainer()
        {
            string settingsFolderPath = ApplicationBuildConfig.UserDataPath;
            string iniConfigFilePath = Path.Combine(ApplicationBuildConfig.UserDataPath, "ApplicationSettings.ini");
            var appSettings = new AppSettingsService(ConfigHelper.GetDefaultSettings(), new IniConfigFileManager(), iniConfigFilePath);
            var memoStorageService = new MemoStorageService(appSettings, settingsFolderPath);

            // Create autofac container
            var builder = new ContainerBuilder();
            builder.RegisterInstance(appSettings).As<AppSettingsService>();
            builder.RegisterInstance(memoStorageService).As<MemoStorageService>();

            var generalToolKitAssembly = AssemblyHelper.GetAssembly();
            if (generalToolKitAssembly != null)
            {
                builder.RegisterAssemblyModules(generalToolKitAssembly);
            }

            builder.RegisterAssemblyModules(Assembly.GetCallingAssembly());
            var container = builder.Build();

            return container;
        }
    }
}
