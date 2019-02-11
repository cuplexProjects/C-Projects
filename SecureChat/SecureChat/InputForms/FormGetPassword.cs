using System;
using System.Windows.Forms;
using GeneralToolkitLib.Converters;

namespace SecureChat.InputForms
{
    public partial class FormGetPassword : Form
    {
        public bool UseDerivedPassword { get; set; }
        public string PasswordDerivedString { get; set; }
        public string PasswordString { get; private set; }
        public bool PasswordVerified { get; private set; }

        public FormGetPassword()
        {
            this.InitializeComponent();
            this.PasswordVerified = false;
            this.UseDerivedPassword = false;
        }

        private void FormGetPassword_Shown(object sender, EventArgs e)
        {
            this.txtPassword.Focus();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.HandleOkClick();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void FormGetPassword_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Escape)
            {
                this.DialogResult= DialogResult.Cancel;
                this.Close();
            }

            if(e.KeyCode == Keys.Enter)
                this.HandleOkClick();
        }

        private void HandleOkClick()
        {
            if (this.UseDerivedPassword && this.PasswordDerivedString != GeneralConverters.GeneratePasswordDerivedString(this.txtPassword.Text))
            {
                MessageBox.Show(this, "Invalid password");
                return;
            }
            this.PasswordString = this.txtPassword.Text;
            this.PasswordVerified = true;
            DialogResult = DialogResult.OK;
            this.Close();
        }

        private void txtPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                HandleOkClick();
            }
        }
    }
}
