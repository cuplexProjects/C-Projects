using System;
using System.Windows.Forms;

namespace LoopiaDNSTools
{
    public partial class SettingsForm : Form
    {
        private readonly ApplicationSettingsService _applicationSettingsService;
        private IServerConfiguration _serverConfiguration;
        private bool _validConfigToSave;

        public SettingsForm()
        {
            InitializeComponent();
            _applicationSettingsService = new ApplicationSettingsService();
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            grpBoxServerSettings.Enabled = LoadSavedConfigurationValues();
            btnSave.Enabled = false;
            _validConfigToSave = false;
            btnGenerateBasicData.Enabled = false;
            LoadSavedConfigurationValues();
        }

        private void btnGenerateBasicData_Click(object sender, EventArgs e)
        {
            RandomGenerator rndGenerator = new RandomGenerator();
            _serverConfiguration.ServerGlobalSalt = rndGenerator.GenerateRandomHexString(256);
            _serverConfiguration.ServerInstanceGuid = Guid.NewGuid().ToString();
            btnSave.Enabled = true;
            btnGenerateBasicData.Enabled = false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                _serverConfiguration.ServerName = txtServerName.Text;
                _serverConfiguration.WebservicePort = int.Parse(txtServerPort.Text);
                _serverConfiguration.SaveUserProfileData = chkSaveUserProfileData.Checked;
                _serverConfiguration.UserDefaultHidden = chkDefaultUserHidden.Checked;
                _applicationSettingsService.SaveServerConfiguration(_serverConfiguration);

                this.DialogResult = DialogResult.OK;
                Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }

        private bool LoadSavedConfigurationValues()
        {
            if(_applicationSettingsService.HasValidSettings())
            {
                _serverConfiguration = _applicationSettingsService.GetServerConfiguration();
                txtServerName.Text = _serverConfiguration.ServerName;
                txtServerPort.Text = _serverConfiguration.WebservicePort.ToString();

                chkDefaultUserHidden.Checked = _serverConfiguration.UserDefaultHidden;
                chkSaveUserProfileData.Checked = _serverConfiguration.SaveUserProfileData;

                _validConfigToSave = !(string.IsNullOrWhiteSpace(_serverConfiguration.ServerGlobalSalt) || string.IsNullOrWhiteSpace(_serverConfiguration.ServerInstanceGuid));
                btnSave.Enabled = _validConfigToSave;
            }
            else
            {
                _serverConfiguration = new ServerConfiguration();
                btnGenerateBasicData.Enabled = true;
            }

            return true;
        }
    }
}
