using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using GeneralToolkitLib.ConfigHelper;
using GeneralToolkitLib.Log;

namespace ImageView
{
    internal static class Program
    {
        [DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();


        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
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
            Application.Run(new FormMain());
            LogWriter.LogMessage("Application ended", LogWriter.LogLevel.Info);
        }
    }
}