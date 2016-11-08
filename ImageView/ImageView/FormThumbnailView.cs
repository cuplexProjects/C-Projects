using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using GeneralToolkitLib.ConfigHelper;
using GeneralToolkitLib.Log;
using ImageView.Models;
using ImageView.Properties;
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
        private readonly ThumbnailService _thumbnailService;
        private ThumbnailScanDirectory _thumbnailScan;
        private string _maximizedImgFilename;
        public FormThumbnailView()
        {
            _thumbnailSize = ValidateThumbnailSize(ApplicationSettingsService.Instance.Settings.ThumbnailSize);
            _maxThumbnails = ApplicationSettingsService.Instance.Settings.MaxThumbnails;
            string dataPath = GlobalSettings.GetUserDataDirectoryPath();
            _thumbnailService = new ThumbnailService(dataPath);
            _thumbnailService.LoadThumbnailDatabase();
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

            HideMaximizedView();
            flowLayoutPanel1.Controls.Clear();

            try
            {
                await Task.Run(() =>
                {
                    _pictureBoxList = null;
                    _pictureBoxList = GenerateThumbnails();
                    if (!IsDisposed)
                        Invoke(new EventHandler(UpdatePictureBoxList));
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, Resources.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogWriter.LogError("Error in generate thumbnails", ex);
            }
        }

        private void UpdatePictureBoxList(object sender,EventArgs e)
        {
            if (_pictureBoxList == null) return;
            flowLayoutPanel1.Controls.AddRange(_pictureBoxList.ToArray());
            GC.Collect();
        }

        private List<Control> GenerateThumbnails()
        {
            var pictureBoxes = new List<Control>();
            bool randomizeImageCollection = ApplicationSettingsService.Instance.Settings.AutoRandomizeCollection;
            var imgLoaderService = ImageLoaderService.Instance;
            var imgRefList = imgLoaderService.GenerateThumbnailList(randomizeImageCollection);
            int items = 0;
            foreach (ImageReferenceElement element in imgRefList)
            {
                PictureBox pictureBox = new PictureBox
                {
                    Image = _thumbnailService.GetThumbnail(element.CompletePath),
                    Width = _thumbnailSize,
                    Height = _thumbnailSize,
                    BorderStyle = BorderStyle.FixedSingle,
                    SizeMode = PictureBoxSizeMode.Zoom,
                    BackColor = Color.White,
                    Tag = element.CompletePath
                };

                pictureBox.MouseClick += PictureBox_MouseClick;
                pictureBoxes.Add(pictureBox);

                items++;
                if (items > _maxThumbnails)
                    return pictureBoxes;
            }
            return pictureBoxes;
           
        }

        private void PictureBox_MouseClick(object sender, MouseEventArgs e)
        {
            PictureBox pictureBox = sender as PictureBox;

            if (pictureBox == null || e.Button != MouseButtons.Left)
                return;

            string filename = pictureBox.Tag as string;
            if (filename != null)
            {
                Image fullScaleIMage = Image.FromFile(filename);
                _maximizedImgFilename = filename;
                picBoxMaximized.Image = fullScaleIMage;
            }

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

        private void HideMaximizedView()
        {
            picBoxMaximized.Visible = false;
            flowLayoutPanel1.Visible = true;
        }

        private void btnScanDirectory_Click(object sender, EventArgs e)
        {
            _thumbnailService.SaveThumbnailDatabase();
            _thumbnailScan = new ThumbnailScanDirectory(_thumbnailService);
            Form frmDirectoryScan = FormFactory.CreateModalForm(_thumbnailScan);
            frmDirectoryScan.FormClosed += FrmDirectoryScan_FormClosed;

            frmDirectoryScan.ShowDialog(this);
            _thumbnailService.LoadThumbnailDatabase();
            GC.Collect();
        }

        private void FrmDirectoryScan_FormClosed(object sender, FormClosedEventArgs e)
        {
            _thumbnailScan?.OnFormClosed();
            _thumbnailScan = null;
        }

        private void FormThumbnailView_FormClosing(object sender, FormClosingEventArgs e)
        {
            Task.Run(() =>
            {
                _thumbnailService.SaveThumbnailDatabase();
                _thumbnailService.Dispose();
                GC.Collect();
            }); 
        }

        private void picBoxMaximized_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                HideMaximizedView();
            }
            else if (e.Button == MouseButtons.Right)
            {
                Point menuPos = e.Location;
                contextMenuFullSizeImg.Show(picBoxMaximized, menuPos);
            }
        }

        private void menuItemOpenInDefApp_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(_maximizedImgFilename);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Resources.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void menuItemBookmark_Click(object sender, EventArgs e)
        {
            FileInfo fi = new FileInfo(_maximizedImgFilename);
            ImageReferenceElement imgRef = new ImageReferenceElement
            {
                CompletePath = _maximizedImgFilename,
                Size = fi.Length,
                CreationTime = fi.CreationTime,
                LastAccessTime = fi.LastAccessTime,
                LastWriteTime = fi.LastWriteTime,
                FileName = fi.Name,
                Directory = fi.DirectoryName
            };

            FormAddBookmark addBookmark = new FormAddBookmark(contextMenuFullSizeImg.Location, imgRef);
            addBookmark.ShowDialog(this);
        }

        private void menuItemCopyPath_Click(object sender, EventArgs e)
        {
            Clipboard.Clear();
            Clipboard.SetText(_maximizedImgFilename);
        }
    }
}