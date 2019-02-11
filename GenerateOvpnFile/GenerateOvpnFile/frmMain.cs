using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using GenerateOvpnFile.FileIo;
using GenerateOvpnFile.Misc;
using GenerateOvpnFile.Models;
using GenerateOvpnFile.Settings;

namespace GenerateOvpnFile
{
    public partial class frmMain : Form
    {
        private OpenVPNConnection _openVpnConnection;
        private string _lastWorkingPath = null;
        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            _openVpnConnection = new OpenVPNConnection();
            InitComboboxes();
            LoadSettings();
            lblFileGenerarationStatus.Text = "";
            btnBrowseOutputDir.Enabled = false;
        }

        private void InitComboboxes(bool reset = false)
        {
            if (reset)
            {
                cbInterface.SelectedIndex = 0;
                cbProtocol.SelectedIndex = 0;
                cbCompression.SelectedIndex = 0;
                cbTPSAuth.SelectedIndex = 0;
                cbCipher.SelectedIndex = 0;
                return;
            }

            cbInterface.DataSource = Enum.GetNames(typeof(ConfigEnums.InterfaceType));
            cbProtocol.DataSource = Enum.GetNames(typeof(ConfigEnums.Protocol));
            cbCompression.DataSource = Enum.GetNames(typeof(ConfigEnums.Compression));
            cbTPSAuth.DataSource = Enum.GetNames(typeof(ConfigEnums.TLS_Auth));
            cbCipher.DataSource = ConfigEnums.CipherAlgorithms;
        }

        private void LoadSettings()
        {
            ConfigurationManager.Instance.LoadSettings();

            var config = ConfigurationManager.Instance.AppConfigSettings;
            txtName.Text = config.Name;
            txtHost.Text = config.Host;

            if (config.Interface != null && cbInterface.Items.Contains(config.Interface))
            {
                cbInterface.SelectedIndex = cbInterface.Items.IndexOf(config.Interface);
            }

            if (!string.IsNullOrEmpty(config.ServerPort))
            {
                txtPort.Text = config.ServerPort;
            }

            if (config.Protocol != null && cbInterface.Items.Contains(config.Protocol))
            {
                cbProtocol.SelectedIndex = cbProtocol.Items.IndexOf(config.Protocol);
            }

            if (config.Cipher != null && cbCipher.Items.Contains(config.Cipher))
            {
                cbCipher.SelectedIndex = cbCipher.Items.IndexOf(config.Cipher);
            }
        }

        private void btnBrowseCA_Click(object sender, EventArgs e)
        {
            var configSettings = ConfigurationManager.Instance.AppConfigSettings;
            openFileDialog1.FileName = "ca.crt";
            openFileDialog1.Filter = "CertificateFiles|*.crt";

            string initPath = string.IsNullOrEmpty(configSettings.CaFileName) ? null : Converters.GetDirectoryNameFromPath(configSettings.CaFileName);
            if (Directory.Exists(initPath))
            {
                openFileDialog1.InitialDirectory = initPath;
            }

            if (openFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                txtCaPath.Text = openFileDialog1.FileName;
                configSettings.CaFileName = openFileDialog1.FileName;
            }
        }

        private void btnBrowseClientCert_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = txtName.Text + ".crt";
            openFileDialog1.Filter = "CertificateFiles|*.crt";
            if (openFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                txtClientCertPath.Text = openFileDialog1.FileName;
            }
        }

        private void btnBrowseClientKey_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = txtName.Text + ".key";
            openFileDialog1.Filter = "Certificate Key Files|*.key";
            if (openFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                txtClientKey.Text = openFileDialog1.FileName;
            }

        }

        private void btnBrowseTaPath_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = "ta.key";
            openFileDialog1.Filter = "Certificate Key Files|*.key";
            if (openFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                txtTLSPath.Text = openFileDialog1.FileName;
            }
        }

        private void btnGenerateCompactFile_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateConfiguration() && SetConectionObjParameters())
                {
                    saveFileDialog1.FileName = txtName.Text + ".ovpn";
                    saveFileDialog1.Filter = "OpenVpnProfiles|*.ovpn";
                    if (saveFileDialog1.ShowDialog(this) == DialogResult.OK)
                    {
                        GenerateVPNConfig.SaveConfigFile(saveFileDialog1.FileName, _openVpnConnection);
                        lblFileGenerarationStatus.Text = "OpenVPN Config File created successfully";
                        _lastWorkingPath = Converters.GetDirectoryNameFromPath(saveFileDialog1.FileName);
                        btnBrowseOutputDir.Enabled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool SetConectionObjParameters()
        {
            _openVpnConnection = new OpenVPNConnection
            {
                ConnectionName = txtName.Text,
                Hostname = txtHost.Text,
                InterfaceType = (ConfigEnums.InterfaceType)Enum.Parse(typeof(ConfigEnums.InterfaceType), cbInterface.Text),
                Protocol = (ConfigEnums.Protocol)Enum.Parse(typeof(ConfigEnums.Protocol), cbProtocol.Text),
                TlsAuth = (ConfigEnums.TLS_Auth)Enum.Parse(typeof(ConfigEnums.TLS_Auth), cbTPSAuth.Text),
                Compression = (ConfigEnums.Compression)Enum.Parse(typeof(ConfigEnums.Compression), cbCompression.Text),
                Cipher = cbCipher.Text,
                ServerPort = int.Parse(txtPort.Text),
                CaCertFileData = ParseCertData.ReadFileData(txtCaPath.Text),
                UserCertFileData = ParseCertData.ReadFileData(txtClientCertPath.Text),
                UserCertPrivateKeyData = ParseCertData.ReadFileData(txtClientKey.Text),
                TaKeyData = ParseCertData.ReadFileData(txtTLSPath.Text)
            };

            return true;
        }

        private bool ValidateConfiguration()
        {
            if (txtName.Text.Length == 0)
                throw new Exception("Name must exist");

            if (txtHost.Text.Length == 0)
                throw new Exception("Host must exist");


            if (!FileExists(txtCaPath.Text))
                throw new Exception("Invalid CA");

            if (!FileExists(txtClientCertPath.Text))
                throw new Exception("Invalid Client certificate");

            if (!FileExists(txtClientKey.Text))
                throw new Exception("Invalid Client key");


            if (cbTPSAuth.SelectedIndex == 0 && !FileExists(txtTLSPath.Text))
                throw new Exception("Invalid TLS key");

            return true;
        }

        private bool FileExists(string path)
        {
            if (string.IsNullOrEmpty(path))
                return false;
            return File.Exists(path);
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                SaveApplicationState();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void SaveApplicationState()
        {
            var appConfig = ConfigurationManager.Instance.AppConfigSettings;

            appConfig.ServerPort = txtPort.Text;
            appConfig.Host = txtHost.Text;
            appConfig.Name = txtName.Text;
            appConfig.Interface = cbInterface.Text;
            appConfig.Protocol = cbProtocol.Text;
            appConfig.CaFileName = txtCaPath.Text;
            appConfig.Compression = cbCompression.Text;
            appConfig.ExtraHmac = cbTPSAuth.Text;
            appConfig.Cipher = cbCipher.SelectedItem.ToString();

            ConfigurationManager.Instance.SaveSettings();
        }

        private void btnBrowseOutputDir_Click(object sender, EventArgs e)
        {
            if (_lastWorkingPath != null)
            {
                try
                {
                    Process.Start(_lastWorkingPath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnRevertSettings_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure?", "Confirm", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                InitComboboxes(true);
                txtPort.Text = "1194";
                txtName.Text = "";
                txtHost.Text = "";
                txtClientCertPath.Text = "";
                txtClientKey.Text = "";
                txtTLSPath.Text = "";
            }

        }
    }
}
