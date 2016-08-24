using System;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace PasswordGenerator
{
    public partial class frmMain : Form
    {
        private const string PASSWORD_CHAR_POOL = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        public frmMain()
        {
            InitializeComponent();
        }

        private void btnGeneratePassword_Click(object sender, EventArgs e)
        {
            int length = 0;
            int.TryParse(txtPasswordLength.Text, out length);

            if(length > 0)
            {
                StringBuilder sb = new StringBuilder();
                RandomNumberGenerator rndGenerator= RandomNumberGenerator.Create();
                byte[] buffer = new byte[length * 4];

                rndGenerator.GetBytes(buffer);

                for (int i = 0; i < length; i++)
                {
                    int poolIndex = Math.Abs(BitConverter.ToInt32(buffer, i * 4));
                    sb.Append(PASSWORD_CHAR_POOL[poolIndex % PASSWORD_CHAR_POOL.Length]);
                }

                txtGeneratedPassword.Text = sb.ToString();
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtGeneratedPassword.Text = "";
        }

        private void txtPasswordLength_Enter(object sender, EventArgs e)
        {
            txtPasswordLength.SelectAll();
        }

        private void txtPasswordLength_Click(object sender, EventArgs e)
        {
            txtPasswordLength.SelectAll();
        }
    }
}
