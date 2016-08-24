using System;
using System.Windows.Forms;
using GeneralToolkitLib.Encryption;

namespace SecureChat
{
    public partial class FormViewKeys : Form
    {
        private RSAKeySetIdentity rsaKeySet;
        public FormViewKeys()
        {
            InitializeComponent();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            const int WM_KEYDOWN = 0x100;
            var keyCode = (Keys)(msg.WParam.ToInt32() & Convert.ToInt32(Keys.KeyCode));
            if((msg.Msg == WM_KEYDOWN && keyCode == Keys.A) && (ModifierKeys == Keys.Control))
            {
                if (txtPublicKey.Focused)
                {
                    txtPublicKey.SelectAll();
                    return true;
                }
                if (this.txtCompleteKey.Focused)
                {
                    this.txtCompleteKey.SelectAll();
                    return true;
                }
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        public void SetKeyData(RSAKeySetIdentity keySet)
        {
            rsaKeySet = keySet;
        }

        private void FormViewKeys_Load(object sender, EventArgs e)
        {
            if(rsaKeySet != null)
            {
                txtPublicKey.Text = rsaKeySet.RSA_PublicKey;
                txtCompleteKey.Text= rsaKeySet.RSA_PrivateKey;
                lblGUID.Text = rsaKeySet.RSA_GUID;
            }
        }

        private void txtPublicKey_Enter(object sender, EventArgs e)
        {
            txtPublicKey.SelectAll();
        }

        private void txtCompleteKey_Enter(object sender, EventArgs e)
        {
            txtCompleteKey.SelectAll();
        }
    }
}