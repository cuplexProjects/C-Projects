using System;
using System.Reflection;
using System.ServiceProcess;
using System.Windows.Forms;
using GeneralToolkitLib.ConfigHelper;
using GeneralToolkitLib.Log;

namespace LoopiaDNSTools
{
    internal static class Program
    {
        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        private static void Main()
        {
            if (Environment.UserInteractive)
            {
                GlobalSettings.Initialize(Assembly.GetExecutingAssembly().GetName().Name, false);
                LogWriter.SetMinimumLogLevel(LogWriter.LogLevel.Debug);
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                var installerForm = new InstallerForm();
                Application.Run(installerForm);
                return;
            }

            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new DomainUpdateService()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}