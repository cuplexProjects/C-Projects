#region

using System;
using System.Windows.Forms;
using DeleteDuplicateFiles.Properties;
using DeleteDuplicateFiles.WindowsApi;
using GeneralToolkitLib.ConfigHelper;
using GeneralToolkitLib.Log;

#endregion

namespace DeleteDuplicateFiles
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
#if DEBUG
            //Initialize Global Settings
            GlobalSettings.Initialize(Resources.ApplicationUserDataFolderName, false);
            LogWriter.SetMinimumLogLevel(LogWriter.LogLevel.Debug);

#else
            //Initialize Global Settings
            GlobalSettings.Initialize(Resources.ApplicationUserDataFolderName, true);
            SetWindowMessageFilters.AllowDragAndDropWhenExecutionLevelIsAdministrator();
            LogWriter.SetMinimumLogLevel(LogWriter.LogLevel.Info);
#endif
            LogWriter.LogMessage("Application started.", LogWriter.LogLevel.Info);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMain());
            LogWriter.LogMessage("Application stopped.", LogWriter.LogLevel.Info);
        }
    }
}