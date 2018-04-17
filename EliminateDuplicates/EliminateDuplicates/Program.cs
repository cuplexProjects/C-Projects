using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Autofac;
using DeleteDuplicateFiles.Configuration;
using DeleteDuplicateFiles.Services;
using DeleteDuplicateFiles.WindowsApi;
using GeneralToolkitLib.ConfigHelper;
using GeneralToolkitLib.Configuration;
using Serilog;

namespace DeleteDuplicateFiles
{
    internal static class Program
    {
        [DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();

        private static IContainer Container { get; set; }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            if (Environment.OSVersion.Version.Major >= 6)
                SetProcessDPIAware();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(true);
            bool debugMode = ApplicationBuildConfig.DebugMode;
            GlobalSettings.Initialize(Assembly.GetExecutingAssembly().GetName().Name, !debugMode);

            InitializeAutofac();

            if (debugMode)
            {
                Log.Verbose("Application started in debug build");
            }

            using (var scope = Container.BeginLifetimeScope())
            {
                AppSettingsService settingsService = scope.Resolve<AppSettingsService>();
                settingsService.LoadSettings();

                
                var frmMain = scope.Resolve<FrmMain>();
                Application.Run(frmMain);

                settingsService.SaveSettings();
            }

            Log.Verbose("Application ended.");


            //if (Environment.OSVersion.Version.Major >= 6)
            //    SetProcessDPIAware();

            ////Initialize Global SettingsModel
            //bool useAppDataFolder = !ApplicationBuildConfig.DebugMode;
            //GlobalSettings.Initialize(Assembly.GetCallingAssembly().GetName().Name, useAppDataFolder);
            //SetWindowMessageFilters.AllowDragAndDropWhenExecutionLevelIsAdministrator();

            //// Log level is configured by build type
            //LogConfiguration.InitLogConfig();
            //ComputedHashService.Instance.Init();

            //Log.Information("Application started.");
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(true);
            //Application.Run(new frmMain());
            //Log.Information("Application ended.");
        }

        private static void InitializeAutofac()
        {
            Container = AutofacConfig.CreateContainer();
        }
    }
}