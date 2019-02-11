using System;
using System.ServiceProcess;
using System.Text;
using System.Windows.Forms;
using SecureChat.Common;
using SecureChat.InternalServices.ApplicationSettingsServiceImplementation;
using SecureChatServer.Config;

namespace SecureChatServer
{
    public partial class InstallerForm : Form
    {
        private readonly ApplicationSettingsService _applicationSettingsService;
        private readonly StringBuilder _sbLog;

        public InstallerForm()
        {
            InitializeComponent();
            _sbLog = new StringBuilder();
            _applicationSettingsService = new ApplicationSettingsService();
        }

        #region Form Action Events

        private void InstallerForm_Load(object sender, EventArgs e)
        {
            InitializeFormControlsData();
            WriteStatusLabelText(Enums.ServerStatusFlags.Initializing);
            this.TopMost = true;
        }

        private void btnShowSettings_Click(object sender, EventArgs e)
        {
            SettingsForm settingsForm = new SettingsForm();
            if (settingsForm.ShowDialog(this) == DialogResult.OK && _applicationSettingsService.HasValidSettings())
            {
                WriteLogdataLine("Configuration saved!");
                this.WriteStatusLabelText(Enums.ServerStatusFlags.Configured);
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {

        }

        private void btnStop_Click(object sender, EventArgs e)
        {

        }

        private void btnInstall_Click(object sender, EventArgs e)
        {

        }

        private void btnUninstall_Click(object sender, EventArgs e)
        {

        }

        #endregion

        public void InitializeInstallationStatus()
        {
            comboBoxServiceName.Items.Clear();
            try
            {
                string assemblyPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
                foreach (string installedService in ServiceHelper.GetServicesWithPath(assemblyPath))
                {
                    comboBoxServiceName.Items.Add(installedService);
                }

                if(comboBoxServiceName.Items.Count > 0)
                {
                    comboBoxServiceName.SelectedIndex = 0;
                    lblStatus.Text = "Installed";

                    // Service is installed check if it is running
                    ConfigureServiceStatus(comboBoxServiceName.Items[0].ToString());
                }
                else
                {
                    comboBoxServiceName.Text = Constants.SERVICE_DEFAULT_NAME;
                    btnStart.Enabled = false;
                    btnStop.Enabled = false;
                    lblStatus.Text = "Not installed";

                    btnInstall.Enabled = false;
                    CheckDbConnection();
                }

                this.btnUninstall.Enabled = comboBoxServiceName.Items.Count > 0;
                this.btnInstall.Enabled = comboBoxServiceName.Items.Count == 0;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message + "\r\r" + ex.StackTrace);
            }
        }

        private async void CheckDbConnection()
        {
            WriteStatusLabelText(Enums.ServerStatusFlags.TryingToConnectToDatabase);
            bool canConnect = await _applicationSettingsService.CanConnectToDatabase();
            if(canConnect)
            {
                WriteLogdataLine("Connection to database was successful");
                WriteStatusLabelText(Enums.ServerStatusFlags.DbConnectionSuccessful);
                WriteLogdataLine("Checking server configuration");

                bool configExists = await _applicationSettingsService.HasValidSettingsAsync();
                if(configExists)
                {
                    btnInstall.Enabled = true;
                    WriteLogdataLine("Valid server settings found");
                    WriteStatusLabelText(Enums.ServerStatusFlags.Configured);
                }
                else
                {
                    WriteStatusLabelText(Enums.ServerStatusFlags.NotConfigured);
                    WriteLogdataLine("No valid server settings found, save settings before running service");
                }
            }
            else
            {
                WriteLogdataLine("Connection to database failed!");
                WriteStatusLabelText(Enums.ServerStatusFlags.DbConnectionFailed);
                
            }
        }

        private void WriteStatusLabelText(Enums.ServerStatusFlags serverState)
        {
            switch (serverState)
            {
                case Enums.ServerStatusFlags.Initializing:
                    lblStatus.Text = "Initializing";
                    break;
                case Enums.ServerStatusFlags.DbConnectionSuccessful:
                    lblStatus.Text = "Db Connection Ok";
                    break;
                case Enums.ServerStatusFlags.TryingToConnectToDatabase:
                    lblStatus.Text = "Trying to connect to database";
                    break;
                case Enums.ServerStatusFlags.Configured:
                    lblStatus.Text = "Configured";
                    break;
                case Enums.ServerStatusFlags.NotConfigured:
                    lblStatus.Text = "No Configuration";
                    break;
                case Enums.ServerStatusFlags.DbConnectionFailed:
                    lblStatus.Text = "Db Connection Error";
                    break;
                default:
                    throw new ArgumentOutOfRangeException("serverState");
            }
        }

        private void WriteLogdataLine(string text)
        {
            _sbLog.AppendLine(" * " + text);
            txtLog.Text = _sbLog.ToString().Trim();
        }

        private void ConfigureServiceStatus(string serviceName, bool setInstall = false, bool setUninstall = false)
        {
            if(!setUninstall)
            {
                if(ServiceHelper.GetServiceStatus(serviceName) == ServiceControllerStatus.Running)
                {
                    btnStart.Enabled = false;
                    btnStop.Enabled = true;
                }
                else
                {
                    btnStart.Enabled = true;
                    btnStop.Enabled = false;
                }
            }

            if(setInstall)
            {
                btnInstall.Enabled = false;
                btnUninstall.Enabled = true;
            }

            if(setUninstall)
            {
                btnInstall.Enabled = true;
                btnUninstall.Enabled = false;

                btnStart.Enabled = false;
                btnStop.Enabled = false;
            }
        }

        private void InitializeFormControlsData()
        {
            comboBoxStartupMode.Items.Add("Automatic");
            comboBoxStartupMode.Items.Add("Manual");
            comboBoxStartupMode.Items.Add("Disabled");
            comboBoxStartupMode.SelectedIndex = 0;
            InitializeInstallationStatus();

            btnInstall.Enabled = false;
        }
    }
}
