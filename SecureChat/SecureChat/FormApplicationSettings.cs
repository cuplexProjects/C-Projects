using System;
using System.Windows.Forms;
using GeneralToolkitLib.Encryption;
using SecureChat.CustomControls;
using SecureChat.Settings;

namespace SecureChat
{
    public partial class FormApplicationSettings : Form
    {

        public FormApplicationSettings()
        {
            InitializeComponent();
        }

        private void FormSettings_Load(object sender, EventArgs e)
        {
            networkStatusControl1.BackColor = System.Drawing.SystemPens.ButtonFace.Color;
            SetConnectionStatus(NetworkStatusControl.ConnectionStatus.Pending);
            Initialize();
        }

        private void Initialize()
        {
            if(!string.IsNullOrEmpty(ApplicationSettingsService.SettingsService.AppSettings.APIHostName))
                txtAPIUrl.Text = ApplicationSettingsService.SettingsService.AppSettings.APIHostName;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            ApplicationSettingsService.SettingsService.AppSettings.APIHostName = txtAPIUrl.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void SetConnectionStatus(NetworkStatusControl.ConnectionStatus connStatus)
        {
            networkStatusControl1.SetConnectionStatus(connStatus);

            switch (connStatus)
            {
                case NetworkStatusControl.ConnectionStatus.None:
                    lblConnStatusText.Text = "";
                    break;
                case NetworkStatusControl.ConnectionStatus.Connected:
                    lblConnStatusText.Text = "Connected to API";
                    break;
                case NetworkStatusControl.ConnectionStatus.Disconnected:
                    lblConnStatusText.Text = "Unable to connect to Api";
                    break;
                case NetworkStatusControl.ConnectionStatus.Pending:
                    lblConnStatusText.Text = "Pending";
                    break;
                default:
                    throw new ArgumentOutOfRangeException("connStatus");
            }
        }
    }
}
