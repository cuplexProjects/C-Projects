using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using GeneralToolkitLib.Converters;
using GeneralToolkitLib.Hashing;

namespace SearchForDuplicates
{
    public partial class FormChecksum : Form
    {
        IHashTransform hashTransform;
        private TextBox currentHashOutputTextBox;
        private byte[] hashBytes;

        public FormChecksum()
        {
            InitializeComponent();
        }

        private void frmChecksum_Load(object sender, EventArgs e)
        {

        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            if(openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtInputFile.Text = openFileDialog1.FileName;
            }
        }

        private void btnCalculate_Click(object sender, EventArgs e)
        {
            hashTransform = null;
            if(txtInputFile.Text.Length == 0)
            {
                MessageBox.Show("Please select a file first.");
                return;
            }
            if(!File.Exists(txtInputFile.Text))
            {
                MessageBox.Show("File does not exist");
                return;
            }

            if(radioButtonMD5.Checked)
            {
                hashTransform = new MD5();
                currentHashOutputTextBox = txtMD5;
            }
            else if (radioButtonCRC32.Checked)
            {
                hashTransform = new CRC32();
                currentHashOutputTextBox = txtCRC32;
            }
            else if (radioButtonSha256.Checked)
            {
                hashTransform = new SHA256();
                currentHashOutputTextBox = txtSHA256;
            }

            if(hashTransform != null)
                ComputeHash();
        }

        private void ComputeHash()
        {
            backgroundWorkerCalculateHash.RunWorkerAsync();
        }

        private void UpdateHashTextField(object sender, EventArgs e)
        {
            if(hashBytes != null)
            {
                currentHashOutputTextBox.Text = GeneralConverters.ByteArrayToHexString(hashBytes);
            }
        }

        private void backgroundWorkerCalculateHash_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            FileStream fs = null;
            try
            {
                fs = File.OpenRead(txtInputFile.Text);
                hashBytes = hashTransform.ComputeHash(fs);
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
        }

        private void backgroundWorkerCalculateHash_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {

        }

        private void backgroundWorkerCalculateHash_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            this.Invoke(new EventHandler(UpdateHashTextField));
        }

    }
}