using System;
using System.Windows.Forms;
using ImageView.Services;

namespace ImageView.UserControls
{
    public partial class ThumbnailSettings : UserControl
    {
        public ThumbnailSettings()
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            Form parentForm = ParentForm;
            if (parentForm == null)
                return;

            ApplicationSettingsService.Instance.Settings.ThumbnailSize = GetSelectedThumbnailSize();
            ApplicationSettingsService.Instance.Settings.MaxThumbnails = trackBarThumbnailCount.Value;
            parentForm.DialogResult = DialogResult.OK;
            parentForm.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Form parentForm = ParentForm;
            if (parentForm == null)
                return;

            parentForm.DialogResult = DialogResult.Cancel;
            parentForm.Close();
        }

        private int GetSelectedThumbnailSize()
        {
            if (rb64.Checked)
                return 64;
            if (rb128.Checked)
                return 128;
            if (rb256.Checked)
                return 256;
            if (rb512.Checked)
                return 512;
           
            return 256;
        }

        private void ThumbnailSettings_Load(object sender, EventArgs e)
        {
            int thumbnailsize = ApplicationSettingsService.Instance.Settings.ThumbnailSize;
            int maxThumbnails = ApplicationSettingsService.Instance.Settings.MaxThumbnails;

            if (thumbnailsize == 64)
                rb64.Checked = true;
            else if (thumbnailsize == 128)
                rb128.Checked = true;
            else if (thumbnailsize == 256)
                rb256.Checked = true;
            else if (thumbnailsize == 512)
                rb512.Checked = true;
            else
                rb256.Checked = true;

            if (maxThumbnails > 512)
                maxThumbnails = 512;

            if (maxThumbnails < 32)
                maxThumbnails = 32;

            trackBarThumbnailCount.Value = maxThumbnails;
            lblThumbnailCount.Text = trackBarThumbnailCount.Value.ToString();
        }

        private void trackBarThumbnailCount_ValueChanged(object sender, EventArgs e)
        {
            lblThumbnailCount.Text = trackBarThumbnailCount.Value.ToString();
        }
    }
}
