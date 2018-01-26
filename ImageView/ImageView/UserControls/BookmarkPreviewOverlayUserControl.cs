using System;
using System.Drawing;
using System.Windows.Forms;

namespace ImageView.UserControls
{
    public partial class BookmarkPreviewOverlayUserControl : UserControl
    {
        public int MaxHeight { get; set; }
        public int MaxWidth { get; set; }

        public BookmarkPreviewOverlayUserControl()
        {
            InitializeComponent();
        }

        public void LoadImage(string filename)
        {
            OverlayPictureBox.Image = Image.FromFile(filename);
        }

        public Size GetImageSize()
        {
            return OverlayPictureBox.Image?.Size ?? Size.Empty;
        }

        private void BookmarkPreviewOverlayUserControl_Load(object sender, EventArgs e)
        {
            if (!DesignMode)
            {
                BookmarkPreviewContentPanel.Dock = DockStyle.Fill;
            }
        }
    }
}
