﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AlterlogicWebmailKeygen
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            dtPickerExpireDate.Value = DateTime.Today.AddYears(1);
        }

        private void txtUserLimit_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        } 

        private void btnGenerate_Click(object sender, EventArgs e)
        {

        }

        private void txtUserLimit_TextChanged(object sender, EventArgs e)
        {
            if (txtUserLimit.Text.Length > 0)
                txtUserLimit.Text = txtUserLimit.Text.TrimStart('0');
        }

        private void rdUnlimited_CheckedChanged(object sender, EventArgs e)
        {
            txtUserLimit.Enabled = !rdUnlimited.Checked;
        }

        private void rdPerUser_CheckedChanged(object sender, EventArgs e)
        {
            txtUserLimit.Enabled = !rdUnlimited.Checked;
        }
    }
}
