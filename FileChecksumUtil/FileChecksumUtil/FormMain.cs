using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using GeneralToolkitLib.Converters;
using GeneralToolkitLib.Hashing;

namespace FileChecksumUtil
{
    public partial class FormMain : Form
    {
        IHashTransform hashTransform;
        private TextBox currentHashOutputTextBox;
        private byte[] hashBytes;

        public string SelectedFile { get; set; }
        private delegate void ProgressCallback(int value);

        public FormMain()
        {
            InitializeComponent();
        }

        private void txtPath_Click(object sender, EventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox != null) textBox.SelectAll();
        }

        void fileHashUtil_ProgressChanged(object sender, HashProgress e)
        {
            //int progressVal = Convert.ToInt32((double)e.BytesHashed / (double)e.FileSize);
            //this.Invoke(new ProgressCallback(SetProgressBarValue), progressVal);
        }

        private void SetControlState(bool enableControls)
        {
            radioButtonCRC32.Enabled = enableControls;
            radioButtonMD5.Enabled = enableControls;
            radioButtonMD5.Enabled = enableControls;
            txtInputFile.Enabled = enableControls;
            btnBrowse.Enabled = enableControls;
        }

        protected void SetProgressBarValue(int value)
        {
            this.progressBarHash.Value = value;
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                SelectedFile = openFileDialog1.FileName;
                txtInputFile.Text = SelectedFile;
                ValidatePath();
            }
        }

        private bool ValidatePath()
        {
            if (!txtInputFile.Focused)
            {
                string file = txtInputFile.Text;

                if (!string.IsNullOrEmpty(file) && File.Exists(file))
                {
                    btnCalculate.Enabled = true;
                    SelectedFile = file;
                    return true;
                }
                btnCalculate.Enabled = false;
                txtInputFile.SelectAll();
            }
            return false;
        }

        private void btnCalculate_Click(object sender, EventArgs e)
        {
            hashTransform = null;
            if (txtInputFile.Text.Length == 0)
            {
                MessageBox.Show("Please select a file first.");
                return;
            }
            if (!File.Exists(txtInputFile.Text))
            {
                MessageBox.Show("File does not exist");
                return;
            }

            if (radioButtonMD5.Checked)
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

            if (hashTransform != null && !backgroundWorkerCalculateHash.IsBusy)
                ComputeHash();

            //_fileHashUtil.ProgressChanged += fileHashUtil_ProgressChanged;
        }

        private void ComputeHash()
        {
            SetControlState(false);
            backgroundWorkerCalculateHash.RunWorkerAsync();
        }

        private void txtInputFile_Validating(object sender, CancelEventArgs e)
        {
            if(txtInputFile.Text.Length == 0) return;
            e.Cancel = !ValidatePath();
        }

        private void txtInputFile_Validated(object sender, EventArgs e)
        {

        }

        private void UpdateHashTextField(object sender, EventArgs e)
        {
            SetControlState(true);
            if (hashBytes != null)
            {
                currentHashOutputTextBox.Text = GeneralConverters.ByteArrayToHexString(hashBytes);
            }
        }

        private void backgroundWorkerCalculateHash_DoWork(object sender, DoWorkEventArgs e)
        {
            FileStream fs = null;
            try
            {
                fs = File.OpenRead(txtInputFile.Text);
                hashBytes = hashTransform.ComputeHash(fs);
            }
            catch(Exception ex)
            {

            }
            finally
            {
                if(fs != null)
                    fs.Close();
            }
        }

        private void backgroundWorkerCalculateHash_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Invoke(new EventHandler(UpdateHashTextField));
        }

        private void txtInputFile_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Link;
        }

        private void txtInputFile_DragDrop(object sender, DragEventArgs e)
        {
            string[] dropedFiles = (string[])e.Data.GetData(DataFormats.FileDrop);

            if(dropedFiles.Length > 0 && File.Exists(dropedFiles[0]))
                txtInputFile.Text = dropedFiles[0];
        }
    }
}
