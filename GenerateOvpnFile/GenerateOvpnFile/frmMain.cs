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
        private OpenVPNConnection openVpnConnection;
        private string lastWorkingPath = null;
        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            openVpnConnection = new OpenVPNConnection();
            InitComboboxes();
            ConfigurationManager.Instance.LoadSettings();
            txtHost.Text = ConfigurationManager.Instance.AppConfigSettings.Host;
            if(!string.IsNullOrEmpty(ConfigurationManager.Instance.AppConfigSettings.Name))
                txtName.Text = ConfigurationManager.Instance.AppConfigSettings.Name;
            lblFileGenerarationStatus.Text = "";
            btnBrowseOutputDir.Enabled = false;
        }

        private void InitComboboxes()
        {
            cbInterface.DataSource = Enum.GetNames(typeof(ConfigEnums.InterfaceType));
            cbProtocol.DataSource = Enum.GetNames(typeof(ConfigEnums.Protocol));
            cbCompression.DataSource = Enum.GetNames(typeof(ConfigEnums.Compression));
            cbTPSAuth.DataSource = Enum.GetNames(typeof(ConfigEnums.TLS_Auth));
            cbCipher.DataSource = ConfigEnums.CipherAlgorithms;


        }

        private void btnBrowseCA_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = "ca.crt";
            openFileDialog1.Filter = "CertificateFiles|*.crt";
            if(openFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                txtCaPath.Text = openFileDialog1.FileName;
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
                    if(saveFileDialog1.ShowDialog(this) == DialogResult.OK)
                    {
                        GenerateVPNConfig.SaveConfigFile(saveFileDialog1.FileName, openVpnConnection);
                        lblFileGenerarationStatus.Text = "OpenVPN Config File created successfully";
                        lastWorkingPath = Converters.GetDirectoryNameFromPath(saveFileDialog1.FileName);
                        btnBrowseOutputDir.Enabled = true;
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool SetConectionObjParameters()
        {
            openVpnConnection = new OpenVPNConnection();
            this.openVpnConnection.ConnectionName = this.txtName.Text;
            this.openVpnConnection.Hostname = this.txtHost.Text;
            this.openVpnConnection.InterfaceType = (ConfigEnums.InterfaceType)Enum.Parse(typeof(ConfigEnums.InterfaceType), this.cbInterface.Text);
            this.openVpnConnection.Protocol = (ConfigEnums.Protocol)Enum.Parse(typeof(ConfigEnums.Protocol), this.cbProtocol.Text);
            this.openVpnConnection.TlsAuth = (ConfigEnums.TLS_Auth)Enum.Parse(typeof(ConfigEnums.TLS_Auth), this.cbTPSAuth.Text);
            this.openVpnConnection.Compression = (ConfigEnums.Compression)Enum.Parse(typeof(ConfigEnums.Compression), this.cbCompression.Text);
            this.openVpnConnection.Cipher = this.cbCipher.Text;
            this.openVpnConnection.ServerPort = int.Parse(this.txtPort.Text);
            this.openVpnConnection.CACert_FileData = ParseCertData.ReadFileData(this.txtCaPath.Text);
            this.openVpnConnection.UserCert_FileData = ParseCertData.ReadFileData(this.txtClientCertPath.Text);
            this.openVpnConnection.UserCertPrivateKeyData = ParseCertData.ReadFileData(this.txtClientKey.Text);
            this.openVpnConnection.TaKeyData = ParseCertData.ReadFileData(this.txtTLSPath.Text);

            return true;
        }

        private bool ValidateConfiguration()
        {
            if(txtName.Text.Length == 0)
                throw new Exception("Name must exist");

            if (txtHost.Text.Length == 0)
                throw new Exception("Host must exist");


            if (!FileExists(txtCaPath.Text))
                throw new Exception("Invalid CA");

            if (!FileExists(txtClientCertPath.Text))
                throw new Exception("Invalid Client certificate");

            if (!FileExists(txtClientKey.Text))
                throw new Exception("Invalid Client key");


            if(cbTPSAuth.SelectedIndex == 0 && !FileExists(txtTLSPath.Text))
                throw new Exception("Invalid TLS key");

            return true;
        }

        private bool FileExists(string path)
        {
            if(string.IsNullOrEmpty(path))
                return false;
            return File.Exists(path);
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                ConfigurationManager.Instance.AppConfigSettings.Host = txtHost.Text;
                ConfigurationManager.Instance.AppConfigSettings.Name = txtName.Text;
                ConfigurationManager.Instance.SaveSettings();
            }
            catch(Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private void btnBrowseOutputDir_Click(object sender, EventArgs e)
        {
            if(lastWorkingPath != null)
            {
                try
                {
                    Process.Start(lastWorkingPath);
                }
                catch(Exception ex)
                {
                    MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
