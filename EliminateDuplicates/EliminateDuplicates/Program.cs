#region
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

#endregion

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
            InitializeAutofac();

            if (Environment.OSVersion.Version.Major >= 6)
                SetProcessDPIAware();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(true);
            using (var scope = Container.BeginLifetimeScope())
            {

                AppSettingsService settingsService = scope.Resolve<AppSettingsService>();
                settingsService.LoadSettings();

                // Begin startup async jobs
                //var startupService = scope.Resolve<StartupService>();
                //startupService.ScheduleAndRunStartupJobs();

                var frmMain = scope.Resolve<FrmMain>();
                Application.Run(frmMain);
                settingsService.SaveSettings();
            }

            Log.Information("Application ended.");


            //if (Environment.OSVersion.Version.Major >= 6)
            //    SetProcessDPIAware();

            ////Initialize Global Settings
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