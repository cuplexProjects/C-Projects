using System;
using System.Windows.Forms;
using GeneralToolkitLib.ConfigHelper;
using ImageView.Models;
using ImageView.Services;

namespace ImageView.UserControls
{
    public partial class ThumbnailScanDirectory : UserControl
    {
        private readonly ThumbnailService _thumbnailService;
        private bool _directorySelected;
        private bool _scaningDirectory;

        public ThumbnailScanDirectory()
        {
            InitializeComponent();
            string dataPath = GlobalSettings.GetUserDataDirectoryPath();
            _thumbnailService = new ThumbnailService(dataPath);
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog(this) == DialogResult.OK)
            {
                txtFolderPath.Text = folderBrowserDialog1.SelectedPath;
                _directorySelected = true;
            }
        }

        private void btnScan_Click(object sender, EventArgs e)
        {
            _scaningDirectory = true;
            var progress = new Progress<ThumbnailScanProgress>(Handler);
           
            _thumbnailService.ScanDirectoryAsync(txtFolderPath.Text, progress);
            UpdateButtonState();
        }

        private void Handler(ThumbnailScanProgress thumbnailScanProgress)
        {
            progressBar.Value = thumbnailScanProgress.PercentComplete;
            if (thumbnailScanProgress.IsComplete)
            {
                progressBar.Value = progressBar.Maximum;
                _scaningDirectory = false;
                UpdateButtonState();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            CancelScan();
            UpdateButtonState();
        }

        private void UpdateButtonState()
        {
            btnScan.Enabled = _directorySelected;

            if (_scaningDirectory)
            {
                btnScan.Enabled = false;
                btnCancel.Enabled = true;
                btnBrowse.Enabled = false;
            }
            else
            {
                btnScan.Enabled = true;
                btnCancel.Enabled = false;
                btnBrowse.Enabled = true;
            }
        }

        private void ThumbnailScanDirectory_Load(object sender, EventArgs e)
        {
            UpdateButtonState();
        }

        private void CancelScan()
        {
            _thumbnailService.StopThumbnailScan();
        }

        public void OnFormClosed()
        {
            if (_scaningDirectory)
                CancelScan();
        }
    }
}