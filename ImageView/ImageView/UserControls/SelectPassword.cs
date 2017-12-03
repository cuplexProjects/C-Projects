using System;
using System.Windows.Forms;

namespace ImageView.UserControls
{
    public partial class SelectPassword : UserControl
    {
        public string SelectedPassword { get; set; }
        private const int MinLength = 8;
        public SelectPassword()
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (!VerifyPasswords())
            {
                return;
            }

            SelectedPassword = txtPassword.Text;
            var parentForm = this.ParentForm;
            if (parentForm != null)
            {
                parentForm.DialogResult = DialogResult.OK;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            var parentForm = this.ParentForm;
            if (parentForm != null)
            {
                parentForm.DialogResult = DialogResult.Cancel;
            }
        }

        private bool VerifyPasswords()
        {
            if (txtPassword.Text.Length < MinLength)
            {
                lblStatus.Text = $"Password length must be atleast {MinLength}";
                return false;
            }

            if (txtPassword.Text != txtPasswordConfirm.Text)
            {
                lblStatus.Text = "Passwords dident match";
                return false;
            }

            lblStatus.Text = "";
            return true;
        }
    }
}
