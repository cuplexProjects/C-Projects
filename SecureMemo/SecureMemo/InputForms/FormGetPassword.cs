using System;
using System.Windows.Forms;
using GeneralToolkitLib.Converters;

namespace SecureMemo.InputForms
{
    public partial class FormGetPassword : Form
    {
        public FormGetPassword()
        {
            PasswordSalt = "";
            InitializeComponent();
            PasswordVerified = false;
            UsePasswordValidation = true;
        }

        public string PasswordDerivedString { get; set; }
        public string PasswordSalt { get; set; }
        public string PasswordString { get; private set; }
        public bool PasswordVerified { get; private set; }
        public bool UsePasswordValidation { get; set; }

        private void FormGetPassword_Shown(object sender, EventArgs e)
        {
            txtPassword.Focus();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            HandleOkClick();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            HandleCancelClick();
        }

        private void HandleOkClick()
        {
            if (UsePasswordValidation && PasswordDerivedString != GeneralConverters.GeneratePasswordDerivedString(PasswordSalt + txtPassword.Text + PasswordSalt))
            {
                MessageBox.Show(this, "Invalid password");
                return;
            }
            PasswordString = txtPassword.Text;
            PasswordVerified = true;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void txtPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char) Keys.Enter)
            {
                e.Handled = true;
                HandleOkClick();
            }
        }

        private void txtPassword_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == (char) Keys.Escape)
                HandleCancelClick();
        }

        private void HandleCancelClick()
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}