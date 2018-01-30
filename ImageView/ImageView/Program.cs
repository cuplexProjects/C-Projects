using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autofac;
using ImageView.Configuration;
using ImageView.Services;
using Serilog;

namespace ImageView
{
    internal static class Program
    {
        [DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();
        private static IContainer Container { get; set; }


        /// <summary>
        ///     The main entry point for the application.
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
                ApplicationSettingsService settingsService = scope.Resolve<ApplicationSettingsService>();
                settingsService.LoadSettings();

                // Begin startup async jobs
                var startupService = scope.Resolve<StartupService>();
                startupService.ScheduleAndRunStartupJobs();

                FormMain frmMain = scope.Resolve<FormMain>();
                var notificationService = scope.Resolve<NotificationService>();
                Application.Run(frmMain);
                
                settingsService.SaveSettings();
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