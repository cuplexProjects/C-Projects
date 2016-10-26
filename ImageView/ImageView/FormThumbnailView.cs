using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using ImageView.Models;
using ImageView.Services;
using ImageView.UserControls;
using ImageView.Utility;

namespace ImageView
{
    public partial class FormThumbnailView : Form
    {
        private int _thumbnailSize;
        private int _maxThumbnails;
        private List<Control> _pictureBoxList;
        public FormThumbnailView()
        {
            _thumbnailSize = ValidateThumbnailSize(ApplicationSettingsService.Instance.Settings.ThumbnailSize);
            _maxThumbnails = ApplicationSettingsService.Instance.Settings.MaxThumbnails;
            InitializeComponent();
        }

        private void FormThumbnailView_Load(object sender, EventArgs e)
        {
            if (DesignMode)
                return;

            SetStyle(ControlStyles.DoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            UpdateStyles();
        }

        private async void btnGenerate_Click(object sender, EventArgs e)
        {
            if (ImageLoaderService.Instance.ImageReferenceList == null)
                return;

            flowLayoutPanel1.Controls.Clear();
           
            await Task.Run(() =>
            {
                _pictureBoxList = GenerateThumbnails();
                Invoke(new EventHandler(UpdatePictureBoxList));
            });
        }

        private void UpdatePictureBoxList(object sender,EventArgs e)
        {
            if (_pictureBoxList == null) return;
            flowLayoutPanel1.Controls.AddRange(_pictureBoxList.ToArray());
        }

        private List<Control> GenerateThumbnails()
        {
            List<Control> pictureBoxes = new List<Control>();
            bool randomizeImageCollection = ApplicationSettingsService.Instance.Settings.AutoRandomizeCollection;
            var imgLoaderService = ImageLoaderService.Instance;
            var imgRefList = imgLoaderService.GenerateThumbnailList(randomizeImageCollection);
            int items = 0;
            foreach (ImageReferenceElement element in imgRefList)
            {
                PictureBox pictureBox = new PictureBox();
                pictureBox.Load(element.CompletePath);
                pictureBox.Width = _thumbnailSize;
                pictureBox.Height = _thumbnailSize;
                pictureBox.BorderStyle = BorderStyle.FixedSingle;
                pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                pictureBox.BackColor = Color.White;
                pictureBox.Click += PictureBox_Click;
                pictureBoxes.Add(pictureBox);

                items++;
                if (items > _maxThumbnails)
                    return pictureBoxes;
            }
            return pictureBoxes;
           
        }

        private void PictureBox_Click(object sender, EventArgs e)
        {
            PictureBox pictureBox = sender as PictureBox;

            if (pictureBox == null)
                return;

            picBoxMaximized.Image = pictureBox.Image;

            picBoxMaximized.Visible = true;
            flowLayoutPanel1.Visible = false;
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            Form frmSettings = FormFactory.CreateSettingsForm(new ThumbnailSettings());
            if (frmSettings.ShowDialog(this) == DialogResult.OK)
            {
                _maxThumbnails = ApplicationSettingsService.Instance.Settings.MaxThumbnails;
                _thumbnailSize = ValidateThumbnailSize(ApplicationSettingsService.Instance.Settings.ThumbnailSize);
                ApplicationSettingsService.Instance.SaveSettings();
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

        private void picBoxMaximized_Click(object sender, EventArgs e)
        {
            picBoxMaximized.Visible = false;
            flowLayoutPanel1.Visible = true;
        }
    }
}