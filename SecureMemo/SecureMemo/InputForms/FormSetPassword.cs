using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SecureMemo.InputForms
{
    public partial class FormSetPassword : Form
    {
        private readonly Regex passwordPattern;
        private string errorMessage;

        public FormSetPassword()
        {
            InitializeComponent();
            passwordPattern = new Regex(@"^(?!.*\s)(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$");
            VerifiedPassword = null;
        }

        public string VerifiedPassword { get; set; }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            TryToSetPassword();
        }

        private bool ValidatePasswords()
        {
            errorMessage = null;
            if (txtPassword1.Text != txtPassword2.Text)
                errorMessage = "Passwords did not match!";
            else if (txtPassword1.Text.Length < 8)
                errorMessage = "Password needs to bee atleast 8 characters long";
            else if (!passwordPattern.IsMatch(txtPassword1.Text))
                errorMessage = "Password did not mach the required complexity or did contain illegal characters like whitespaces.";

            return errorMessage == null;
        }

        private void txtPassword1_Enter(object sender, EventArgs e)
        {
            txtPassword1.SelectAll();
        }

        private void txtPassword2_Enter(object sender, EventArgs e)
        {
            txtPassword2.SelectAll();
        }

        private void txtPasswordFields_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && txtPassword1.Text.Length > 0 && txtPassword2.Text.Length > 0)
                e.Handled = true;
        }

        private void txtPasswordFields_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && txtPassword1.Text.Length > 0 && txtPassword2.Text.Length > 0)
            {
                e.Handled = true;
                TryToSetPassword();
            }
        }

        private void TryToSetPassword()
        {
            if (!ValidatePasswords())
                MessageBox.Show(errorMessage);
            else
            {
                VerifiedPassword = txtPassword1.Text;
                DialogResult = DialogResult.OK;
                Close();
            }
        }
    }
}