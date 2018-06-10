using System;
using System.Windows.Forms;
using GeneralToolkitLib.Configuration;
using GeneralToolkitLib.Logging;
using Serilog.Events;

namespace WiFiPasswordGenerator
{
    internal static class Program
    {
        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            InitSerilog();
            Application.Run(new MainForm());
        }

        private static void InitSerilog()
        {
            var settings = ApplicationBuildConfig.DebugMode ? new SerilogAutoConfig.LogSettings(ApplicationBuildConfig.UserDataPath, LogEventLevel.Verbose) : new SerilogAutoConfig.LogSettings(ApplicationBuildConfig.UserDataPath, LogEventLevel.Warning);
            var config = new SerilogAutoConfig(settings);
            config.InitializeLogger();
        }
    }
}