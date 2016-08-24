using System;
using System.ServiceProcess;
using System.Threading;
using System.Windows.Forms;
using SynchronizeTime.Common;

namespace SynchronizeTime
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        private static Mutex singleInstanceMutex;
        static void Main()
        {
            if(Environment.UserInteractive)
            {
                RunWinForm();
            }
            else
            {
                if(IsSingleInstance())
                {
                    // Service launch
                    ServiceBase[] ServicesToRun = {new TimeSyncService()};
                    ServiceBase.Run(ServicesToRun);

                    singleInstanceMutex.ReleaseMutex();
                    singleInstanceMutex = null;
                }
                else
                {
                    LogWriter.WriteLog("Service launch canceled because another instance is currently running");
                }
            }
        }

        [STAThread]
        static void RunWinForm()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMain());
        }

        static bool IsSingleInstance()
        {
            try
            {
                Mutex.OpenExisting("SynchronizeTimeService");
            }
            catch
            {
                singleInstanceMutex = new Mutex(true, "SynchronizeTimeService");
                return true;
            }
            return false;
        }
    }
}