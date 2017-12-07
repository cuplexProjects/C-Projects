﻿using System;
using System.Windows.Forms;

namespace ImageView.UserControls
{
    public partial class GetPassword : UserControl
    {
        public string SelectedPassword { get; private set; }
        public GetPassword()
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            SelectedPassword = txtPassword.Text;
            var parentForm = ParentForm;
            if (parentForm != null)
            {
                parentForm.DialogResult = DialogResult.OK;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            var parentForm = ParentForm;
            if (parentForm != null)
            {
                parentForm.DialogResult = DialogResult.Cancel;
            }
        }

        private void txtPassword_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                SelectedPassword = txtPassword.Text;
                var parentForm = ParentForm;
                if (parentForm != null)
                {
                    parentForm.DialogResult = DialogResult.OK;
                }
            }
        }
    }
}