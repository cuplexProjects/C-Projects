using System.Windows.Forms;
using ImageView.Services;
using ImageView.UserControls;
using ImageView.Utility;

namespace ImageView
{
    public partial class FormThumbnailView : Form
    {
        private int _thumbnailSize;
        public FormThumbnailView()
        {
            _thumbnailSize = ValidateThumbnailSize(ApplicationSettingsService.Instance.Settings.ThumbnailSize);
            InitializeComponent();
        }

        private void FormThumbnailView_Load(object sender, System.EventArgs e)
        {

        }

        private void btnGenerate_Click(object sender, System.EventArgs e)
        {

        }

        private void btnSettings_Click(object sender, System.EventArgs e)
        {
            Form frmSettings = FormFactory.CreateSettingsForm(new ThumbnailSettings());
            if (frmSettings.ShowDialog(this) == DialogResult.OK)
            {

            }
        }

        private int ValidateThumbnailSize(int size)
        {
            const int defVal = 256;
            const int minVal = 64;
            const int maxVal = 512;

            if (size < minVal || size > maxVal)
                return defVal;

            int index = minVal;

            while (index < maxVal)
            {
                if (size - index == 0)
                    return size;

                index <<= 1;
            }

            return defVal;
        }
    }
}