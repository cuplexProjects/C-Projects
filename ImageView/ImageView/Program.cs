using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Autofac;
using GeneralToolkitLib.ConfigHelper;
using GeneralToolkitLib.Configuration;
using ImageViewer.Configuration;
using ImageViewer.Services;
using Serilog;

namespace ImageViewer
{
    internal static class Program
    {
        [DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();
        private static IContainer Container { get; set; }


        /// <summary>
        ///     The main entryModel point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            InitializeAutofac();

            if (Environment.OSVersion.Version.Major >= 6)
                SetProcessDPIAware();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(true);
            bool debugMode = ApplicationBuildConfig.DebugMode;
            GlobalSettings.Initialize(Assembly.GetExecutingAssembly().GetName().Name, !debugMode);

            Log.Verbose("Application started");

            using (var scope = Container.BeginLifetimeScope())
            {
                // Begin startup async jobs
                var startupService = scope.Resolve<StartupService>();
                ApplicationSettingsService settingsService = scope.Resolve<ApplicationSettingsService>();

                bool readSuccessfull = settingsService.LoadSettings();
                

                startupService.ScheduleAndRunStartupJobs();
                try
                {
                    FormMain frmMain = scope.Resolve<FormMain>();
                    Application.Run(frmMain);
                }
                catch (Exception ex)
                {
                    Log.Fatal(ex, "Main program failureException: {Message}", ex.Message);
                }


                
                //settingsService.SaveSettings();
            }

            //Application.Run(new FormMain());
            Log.Information("Application ended");
        }

        private static void InitializeAutofac()
        {
            Container = AutofacConfig.CreateContainer();
        }
    }
}