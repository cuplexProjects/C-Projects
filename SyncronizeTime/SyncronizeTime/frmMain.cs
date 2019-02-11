using System;
using System.Linq;
using System.ServiceProcess;
using System.Windows.Forms;
using SynchronizeTime.Core;
using SynchronizeTime.ServiceConfig;

namespace SynchronizeTime
{
    public partial class frmMain : Form
    {
        private const int SERVICE_START_STOP_TIMEOUT = 10000;
        private ServiceConfiguration serviceConfig;
        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            try
            {
                this.LoadConfigData();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Failed to load configuration data - " + ex.Message);
            }
        }

        private void LoadConfigData()
        {
            serviceConfig = ServiceConfiguration.LoadConfiguration();
            lstTimeServers.DataSource = serviceConfig.TimeServerList;
            TimeSpan syncTimeSpan = new TimeSpan(0, 0, 0, serviceConfig.SyncInterval);
            txtSyncInterval.Text = syncTimeSpan.ToString();


            string assemblyPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string[] activeServiceInstances = ServiceHelper.GetServicesWithPath(assemblyPath);

            if(activeServiceInstances.Length == 0)
            {
                SetStatusText(false, false);
                SetInstallAvailable(true);
                btnStart.Enabled = false;
                btnStop.Enabled = false;
            }
            else
            {
                ServiceControllerStatus serviceStatus = ServiceHelper.GetServiceStatus(serviceConfig.ServiceName);
                SetStartAvailable(serviceStatus != ServiceControllerStatus.Running);
                SetStatusText(true, serviceStatus == ServiceControllerStatus.Running);

                if(serviceStatus == ServiceControllerStatus.Running)
                    DisableInstallUninstall();
                else
                    SetInstallAvailable(false);
            }
        }

        private void SetStatusText(bool installed, bool running)
        {
            if(installed)
            {
                this.txtStatus.Text = running ? "Service is installed and running" : "Service is installed but not running";    
            }
            else
                txtStatus.Text = "Service is not installed";
        }

        private string GetInstalledServiceName()
        {
            string assemblyPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            return ServiceHelper.GetServicesWithPath(assemblyPath).FirstOrDefault();
        }

        private void SetStartAvailable(bool available)
        {
            btnStart.Enabled = available;
            btnStop.Enabled = !available;
        }

        private void SetInstallAvailable(bool available)
        {
            btnInstallService.Enabled = available;
            btnUninstallService.Enabled = !available;
        }

        private void DisableStartStop()
        {
            btnStart.Enabled = false;
            btnStop.Enabled = false;
        }

        private void DisableInstallUninstall()
        {
            btnInstallService.Enabled = false;
            btnUninstallService.Enabled = false;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                string serviceName = GetInstalledServiceName();
                ServiceController service = new ServiceController(serviceName);
                TimeSpan timeout = TimeSpan.FromMilliseconds(SERVICE_START_STOP_TIMEOUT);

                service.Start();
                service.WaitForStatus(ServiceControllerStatus.Running, timeout);
                SetStartAvailable(false);
                DisableInstallUninstall();
                SetStatusText(true, true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\r\r" + ex.StackTrace);
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            try
            {
                string serviceName = GetInstalledServiceName();
                ServiceController service = new ServiceController(serviceName);
                TimeSpan timeout = TimeSpan.FromMilliseconds(SERVICE_START_STOP_TIMEOUT);

                service.Stop();
                service.WaitForStatus(ServiceControllerStatus.Stopped, timeout);
                SetStartAvailable(true);
                SetInstallAvailable(false);
                SetStatusText(true, false);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\r\r" + ex.StackTrace);
            }
        }

        private void btnInstallService_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                TimeSyncService.RunInstaller(true, serviceConfig.ServiceName);
                GetInstalledServiceName();
                SetInstallAvailable(false);
                this.SetStartAvailable(true);
                SetStatusText(true, false);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\r\r" + ex.StackTrace);
            }
            finally
            {
                this.Cursor = this.DefaultCursor;
            }
        }

        private void btnUninstallService_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                string serviceName = GetInstalledServiceName();
                TimeSyncService.RunInstaller(false, serviceName);
                SetInstallAvailable(true);
                this.DisableStartStop();
                SetStatusText(false, false);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\r\r" + ex.StackTrace);
            }
            finally
            {
                this.Cursor = this.DefaultCursor;
            }
        }
    }
}
