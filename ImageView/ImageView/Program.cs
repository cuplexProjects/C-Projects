using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Autofac;
using GeneralToolkitLib.ConfigHelper;
using GeneralToolkitLib.Log;
using ImageView.Services;

//using ImageView.Configuration;

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
            //Set log path
#if DEBUG
            GlobalSettings.Initialize(Application.ProductName, false);
            LogWriter.SetMinimumLogLevel(LogWriter.LogLevel.Trace);
            LogWriter.LogMessage("Application started", LogWriter.LogLevel.Info);
#else
            GlobalSettings.Initialize(Application.ProductName, true);
            LogWriter.SetMinimumLogLevel(LogWriter.LogLevel.Warning);
#endif

            if (Environment.OSVersion.Version.Major >= 6)
                SetProcessDPIAware();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(true);
            using (var scope = Container.BeginLifetimeScope())
            {
                FormMain frmMain = scope.Resolve<FormMain>();
                Application.Run(frmMain);
                ApplicationSettingsService settingsService = scope.Resolve<ApplicationSettingsService>();
                settingsService.SaveSettings();
            }

            //Application.Run(new FormMain());
            LogWriter.LogMessage("Application ended", LogWriter.LogLevel.Info);
        }

        private static void InitializeAutofac()
        {
            Container = AutofacConfig.CreateContainer();
        }
    }
}