using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using GeneralToolkitLib.Encryption;

namespace RSAKeyGenerator
{
    public partial class frmMain : Form
    {
        private readonly RSA_AsymetricEncryption _rsaAsymetricEncryption;
        private RSAKeySetIdentity rsaKeySetIdentity;
        private bool isRunning = false;

        public frmMain()
        {
            InitializeComponent();
            _rsaAsymetricEncryption = new RSA_AsymetricEncryption();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var keySizeList = Enum.GetNames(typeof(RSA_AsymetricEncryption.RSAKeySize));
            var keyValues = Enum.GetValues(typeof(RSA_AsymetricEncryption.RSAKeySize));

            for (int i = 0; i < keySizeList.Length; i++)
            {
                string strKeySize = keySizeList[i];
                int keySize = (int)keyValues.GetValue(i);
                cmbKeySize.Items.Add(new RSAKeySizeListItem(strKeySize, keySize));
            }
            cmbKeySize.SelectedIndex = 0;
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            btnGenerate.Enabled = false;
            RSAKeySizeListItem selectedItem = cmbKeySize.SelectedItem as RSAKeySizeListItem;
            RSA_AsymetricEncryption.RSAKeySize keySize = (RSA_AsymetricEncryption.RSAKeySize)Enum.Parse(typeof(RSA_AsymetricEncryption.RSAKeySize), selectedItem.Value.ToString());

            ClearKeys();

            var t = new Task(() => this.GenerateRSAKeyPair(keySize));
            t.Start();
            t.GetAwaiter().OnCompleted(UpdateFormControlsWhenOperationComplete);
        }

        private void ClearKeys()
        {
            txtPrivateKey.Text = "";
            txtPublicKey.Text = "";
        }

        private void UpdateFormControlsWhenOperationComplete()
        {
            txtPrivateKey.Text = rsaKeySetIdentity.RSA_PrivateKey;
            txtPublicKey.Text = rsaKeySetIdentity.RSA_PublicKey;
            btnGenerate.Enabled = true;
        }

        private void GenerateRSAKeyPair(RSA_AsymetricEncryption.RSAKeySize keySize)
        {
            isRunning = true;
            rsaKeySetIdentity = _rsaAsymetricEncryption.GenerateRSAKeyPair(keySize);
            isRunning = false;
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(!this.isRunning || (e.CloseReason != CloseReason.FormOwnerClosing && e.CloseReason != CloseReason.UserClosing)) return;
            if(
                MessageBox.Show(this,
                    "Key generation is still running, are you sure you want to close?",
                    "Exit before key file generation completed?",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Information) != DialogResult.OK)
                e.Cancel = true;
        }
    }
}
