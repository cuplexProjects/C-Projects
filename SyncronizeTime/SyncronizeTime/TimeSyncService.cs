using System.Collections;
using System.Configuration.Install;
using System.Reflection;
using System.ServiceProcess;
using SynchronizeTime.Common;
using SynchronizeTime.Core;
using SynchronizeTime.ServiceConfig;
using SynchronizeTime.WinAPI;

namespace SynchronizeTime
{
    partial class TimeSyncService : ServiceBase
    {
        private readonly TimeSyncServiceCore timeSyncServiceCore;
        public TimeSyncService()
        {
            timeSyncServiceCore = new TimeSyncServiceCore();
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            LogWriter.WriteLog("Service started");
            if(timeSyncServiceCore.Initialize())
            {
                // Request PROCESS ALL ACCESS
                Security.DisableRemoteShutdown();
                timeSyncServiceCore.Start();
            }
            else
                LogWriter.WriteLog("Service initialization failed, canceling service launch");
        }

        protected override void OnStop()
        {
            LogWriter.WriteLog("Service stoped");
            timeSyncServiceCore.Stop();
        }

        internal static void RunInstaller(bool install, string serviceName)
        {
            TransactedInstaller tinstaller = new TransactedInstaller();
            SynchronizeTimeServiceInstaller cfinstaller = new SynchronizeTimeServiceInstaller(serviceName);
            tinstaller.Installers.Add(cfinstaller);
            tinstaller.Context = new InstallContext("", new[] {string.Format("/assemblypath={0}", Assembly.GetExecutingAssembly().Location)});
            
            if(install)
            {
                tinstaller.Install(new Hashtable());
            }
            else
            {
                tinstaller.Uninstall(null);
            }
        }

    }
}
