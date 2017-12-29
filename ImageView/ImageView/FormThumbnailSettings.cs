using System;
using System.Windows.Forms;
using GeneralToolkitLib.Converters;
using ImageView.Services;

namespace ImageView
{
    public partial class FormThumbnailSettings : Form
    {
        private readonly ThumbnailService _thumbnailService;
        public FormThumbnailSettings(ThumbnailService thumbnailService)
        {
            _thumbnailService = thumbnailService;
            InitializeComponent();
        }

        private void FormThumbnailSettings_Load(object sender, EventArgs e)
        {
            UpdateInformationLabels();
            lblInfo.Text = "";
        }

        private void UpdateInformationLabels()
        {
            lblCurrentDbSize.Text = GeneralConverters.FormatFileSizeToString(_thumbnailService.GetThumbnailDbSize());
            int thumbnailItems = _thumbnailService.GetNumberOfCachedThumbnails();
            lblCachedItems.Text = thumbnailItems > 0 ? _thumbnailService.GetNumberOfCachedThumbnails().ToString() : "n/a";
        }

        private void btnRunDefragmentJob_Click(object sender, EventArgs e)
        {
            btnRunDefragmentJob.Enabled = false;
            _thumbnailService.OptimizeDatabase();
            btnRunDefragmentJob.Enabled = true;
        }

        private void btnClearDatabase_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to completly remove the thumbnail cache?", "Confirm delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {

            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnRemoveFilesNotFound_Click(object sender, EventArgs e)
        {
            btnRemoveFilesNotFound.Enabled = false;
            var result = _thumbnailService.RemoveAllNonAccessableFilesAndSaveDb();
            btnRemoveFilesNotFound.Enabled = true;

            if (result)
            {
                lblInfo.Text = "Successfully removed missing files.";
            }
            else
            {
                lblInfo.Text = "No missing files where found.";
            }
        }

        private void btnReduceCachSize_Click(object sender, EventArgs e)
        {
            int maxSize = Convert.ToInt32(numericSize.Value);
            long truncatedSize = maxSize * 1048576;
            _thumbnailService.TruncateCacheSize(truncatedSize);

            UpdateInformationLabels();
        }
    }
}
