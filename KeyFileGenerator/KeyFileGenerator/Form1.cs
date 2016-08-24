using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace KeyFileGenerator
{
    public partial class Form1 : Form
    {
        private static Keys[] AllowedKeys = { Keys.D0, Keys.D1, Keys.D2, Keys.D3, Keys.D4, Keys.D5, Keys.D6, Keys.D7, Keys.D8, Keys.D9,Keys.Delete,Keys.Back,
            Keys.NumPad0,Keys.NumPad1,Keys.NumPad2,Keys.NumPad3,Keys.NumPad4,Keys.NumPad5,Keys.NumPad6,Keys.NumPad7,Keys.NumPad8,Keys.NumPad9,
            Keys.Left,Keys.Right,Keys.Home,Keys.End};
        private static Regex fileSizeRegEx = new Regex("^0.+");
        public Form1()
        {
            InitializeComponent();
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            if (txtFilePath.Text.Length == 0)
            {
                MessageBox.Show("Please select a filename first");
                return;
            }

            if (txtFileSize.Text.Length == 0 || txtFileSize.Text == "0")
            {
                MessageBox.Show("Please select a valid file size");
                return;
            }

            try
            {
                Int64 bytes = int.Parse(txtFileSize.Text)*1024;
                if (rbMb.Checked)
                    bytes = bytes * 1024;
                if (rbGb.Checked)
                    bytes = bytes * 1048576;

                KeyFileGen.GenerateKeyFile(txtFilePath.Text, bytes);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtFileSize_KeyDown(object sender, KeyEventArgs e)
        {
            if (!AllowedKeys.Contains(e.KeyCode))
            {
                e.SuppressKeyPress = true;
                e.Handled = true;
            }
            else if ((e.KeyValue == 96 || e.KeyValue == 48) && txtFileSize.Text == "0")
            {
                e.SuppressKeyPress = true;
                e.Handled = true;
            }
        }

        private void txtFileSize_TextChanged(object sender, EventArgs e)
        {
            if (fileSizeRegEx.IsMatch(txtFileSize.Text))
            {
                txtFileSize.Text = txtFileSize.Tag as string;
                txtFileSize.SelectAll();
            }
        }

        private void txtFileSize_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            txtFileSize.Tag = txtFileSize.Text;
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "Key files (*.key)|*.key|Dat files (*.dat)|*.dat";
            if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtFilePath.Text = saveFileDialog1.FileName;
            }
        }
    }
}
