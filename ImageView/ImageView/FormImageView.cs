using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using GeneralToolkitLib.Events;
using GeneralToolkitLib.Log;
using GeneralToolkitLib.WindowsApi;
using ImageView.Events;
using ImageView.Models;
using ImageView.Models.Interface;
using ImageView.Properties;
using ImageView.Services;

namespace ImageView
{
    public partial class FormImageView : Form, IObservable<ImageViewFormInfoBase>, ImageViewFormWindow
    {
        private const float ZOOM_MIN = 0.0095f;
        private const float SwitchImageButtonsPercentOfScreen = 0.1f;
        private readonly ImageLoaderService _imageLoaderService;
        private readonly List<IObserver<ImageViewFormInfoBase>> _observers;
        private Image _currentImage;
        private bool _dataReady;
        private ImageReferenceCollection _imageReferenceCollection;
        private int _imagesViewed;
        private ImageViewFormImageInfo _imageViewFormInfo;
        private ImageReferenceElement _imgRef;
        private int _imgx; // current offset of image
        private int _imgy;
        private Point _mouseDown;
        private bool _mouseHover;
        private MouseHoverInfo _mouseHoverInfo;
        private bool _mousepressed; // true as long as left mousebutton is pressed
        private bool _requireFocusNotification = true;
        private bool _showSwitchImgOnMouseOverWindow;
        private int _startx; // offset of image when mouse was pressed
        private int _starty;
        private bool _switchImgButtonsEnabled;
        private float _zoom = -1;

        public FormImageView(int id)
        {
            InitializeComponent();
            _imageViewFormInfo = new ImageViewFormImageInfo(this, null, 0);
            _observers = new List<IObserver<ImageViewFormInfoBase>>();
            _imageLoaderService = ImageLoaderService.Instance;
            pictureBox.Paint += pictureBox_Paint;
            FormId = id;
            _switchImgButtonsEnabled = ApplicationSettingsService.Instance.Settings.ShowSwitchImageButtons;
            if (_switchImgButtonsEnabled)
            {
                _showSwitchImgOnMouseOverWindow =
                    ApplicationSettingsService.Instance.Settings.ShowNextPrevControlsOnEnterWindow;
                _mouseHoverInfo = new MouseHoverInfo();
            }
        }

        private int FormId { get; }

        private bool ImageSourceDataAvailable
            => _dataReady && _imageLoaderService.ImageReferenceList != null && !_imageLoaderService.IsRunningImport;

        public void ResetZoomAndRepaint()
        {
            //Center Image and resize
            _imgx = 0;
            _imgy = 0;
            ResetZoom(true);
            pictureBox.Refresh();
        }

        public void ReloadSettings()
        {
            _switchImgButtonsEnabled = ApplicationSettingsService.Instance.Settings.ShowSwitchImageButtons;
            _showSwitchImgOnMouseOverWindow =
                ApplicationSettingsService.Instance.Settings.ShowNextPrevControlsOnEnterWindow;
            _mouseHoverInfo = _switchImgButtonsEnabled ? new MouseHoverInfo() : null;
        }

        public IDisposable Subscribe(IObserver<ImageViewFormInfoBase> observer)
        {
            // Check whether observer is already registered. If not, add it 
            if (!_observers.Contains(observer))
            {
                _observers.Add(observer);
                // Provide observer with existing data. 
                observer.OnNext(_imageViewFormInfo);
            }
            return new Unsubscriber<ImageViewFormInfoBase>(_observers, observer);
        }

        private void FormImageView_Load(object sender, EventArgs e)
        {
            ShowInTaskbar = ApplicationSettingsService.Instance.Settings.ShowImageViewFormsInTaskBar;
            SetImageReferenceCollection();
            if (!ImageSourceDataAvailable) return;

            _imgRef = _imageReferenceCollection.GetNextImage();
            LoadNewImageFile(_imgRef);
        }

        private void SetImageReferenceCollection()
        {
            var randomizeImageCollection = ApplicationSettingsService.Instance.Settings.AutoRandomizeCollection;
            if (!_imageLoaderService.IsRunningImport && _imageLoaderService.ImageReferenceList != null)
            {
                _imageReferenceCollection =
                    _imageLoaderService.GenerateImageReferenceCollection(randomizeImageCollection);
                _dataReady = true;
            }
        }

        private void LoadNewImageFile(ImageReferenceElement imageReference)
        {
            try
            {
                _currentImage = Image.FromFile(imageReference.CompletePath);

                _imgx = 0;
                _imgy = 0;
                ResetZoom(true);

                pictureBox.Refresh();
                Text = imageReference.FileName;

                //Notify observers
                _imagesViewed++;
                _imageViewFormInfo = new ImageViewFormImageInfo(this, imageReference.FileName, _imagesViewed);
                foreach (var observer in _observers)
                {
                    observer.OnNext(_imageViewFormInfo);
                }

                LogWriter.LogMessage("New Image loaded in ImageViewForm FormId=" + FormId, LogWriter.LogLevel.Trace);
            }
            catch (Exception ex)
            {
                LogWriter.LogError(imageReference != null
                    ? $"FormMain.LoadNewImageFile(string imagePath) Error when trying to load file: {imageReference.CompletePath} : {ex.Message}"
                    : "imgRef was null in FormImageView.LoadNewImageFile()", ex);
            }
        }

        private void FormImageView_FormClosed(object sender, FormClosedEventArgs e)
        {
            _imageViewFormInfo.FormIsClosing = true;
            foreach (var observer in _observers)
            {
                observer.OnNext(_imageViewFormInfo);
            }

            LogWriter.LogMessage("ImageView Form with id=" + FormId + " closed", LogWriter.LogLevel.Trace);
        }

        private void pictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            var mouse = e;
            if (mouse.Button == MouseButtons.Left)
            {
                if (_switchImgButtonsEnabled)
                {
                }

                if (_mousepressed) return;
                _mousepressed = true;
                _mouseDown = mouse.Location;
                _startx = _imgx;
                _starty = _imgy;
            }
        }

        private void pictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            var mouse = e;

            if (mouse.Button == MouseButtons.Left)
            {
                var mousePosNow = mouse.Location;

                var deltaX = mousePosNow.X - _mouseDown.X;
                    // the distance the mouse has been moved since mouse was pressed
                var deltaY = mousePosNow.Y - _mouseDown.Y;

                _imgx = (int) (_startx + deltaX/_zoom);
                    // calculate new offset of image based on the current zoom factor
                _imgy = (int) (_starty + deltaY/_zoom);

                pictureBox.Refresh();
            }

            if (_mouseHoverInfo == null) return;

            var buttonWidth = (int) (ClientSize.Width*SwitchImageButtonsPercentOfScreen);
            var leftButton = new Rectangle(0, 0, buttonWidth, Height);
            var rightButton = new Rectangle(Width - buttonWidth, 0, buttonWidth, Height);

            _mouseHoverInfo.OverLeftButton = false;
            _mouseHoverInfo.OverRightButton = false;
            if (leftButton.IntersectsWith(new Rectangle(mouse.Location, new Size(1, 1))))
            {
                _mouseHoverInfo.OverLeftButton = true;
            }
            else if (rightButton.IntersectsWith(new Rectangle(mouse.Location, new Size(1, 1))))
            {
                _mouseHoverInfo.OverRightButton = true;
            }

            if (_mouseHoverInfo.StateChanged)
            {
                pictureBox.Refresh();
                _mouseHoverInfo.ResetState();
            }
        }

        private void pictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            _mousepressed = false;
        }

        protected override void OnResize(EventArgs e)
        {
            ResetZoom(true);
            base.OnResize(e);
            pictureBox.Refresh();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if ((msg.Msg != WindowEvents.WM_KEYDOWN) && (msg.Msg != WindowEvents.WM_SYSKEYDOWN))
                return base.ProcessCmdKey(ref msg, keyData);

            switch (keyData)
            {
                case Keys.Right:
                    _imgx -= (int) (pictureBox.Width*0.1F/_zoom);
                    pictureBox.Refresh();
                    break;

                case Keys.Left:
                    _imgx += (int) (pictureBox.Width*0.1F/_zoom);
                    pictureBox.Refresh();
                    break;

                case Keys.Down:
                    _imgy -= (int) (pictureBox.Height*0.1F/_zoom);
                    pictureBox.Refresh();
                    break;

                case Keys.Up:
                    _imgy += (int) (pictureBox.Height*0.1F/_zoom);
                    pictureBox.Refresh();
                    break;

                case Keys.PageDown:
                    _imgy -= (int) (pictureBox.Height*0.90F/_zoom);
                    pictureBox.Refresh();
                    break;

                case Keys.PageUp:
                    _imgy += (int) (pictureBox.Height*0.90F/_zoom);
                    pictureBox.Refresh();
                    break;

                case Keys.NumPad1:
                case Keys.D1:
                case Keys.Add:
                    SetNextImage();
                    break;

                case Keys.NumPad2:
                case Keys.D2:
                case Keys.Subtract:
                    SetPreviousImage();
                    break;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void SetNextImage()
        {
            if (!ImageSourceDataAvailable)
            {
                //Try SetImageReference collection
                SetImageReferenceCollection();
                return;
            }
            _imgRef = _imageReferenceCollection.GetNextImage();
            LoadNewImageFile(_imgRef);
        }

        private void SetPreviousImage()
        {
            if (!ImageSourceDataAvailable)
            {
                //Try SetImageReference collection
                SetImageReferenceCollection();
                return;
            }
            _imgRef = _imageReferenceCollection.GetPreviousImage();
            LoadNewImageFile(_imgRef);
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            var oldzoom = _zoom;

            if (e.Delta > 0)
                _zoom += 0.1F + _zoom*.05f;

            else if (e.Delta < 0)
                _zoom = Math.Max(_zoom - 0.1F - _zoom*.05f, ZOOM_MIN);

            var mouse = e;
            var mousePosNow = mouse.Location;

            var x = mousePosNow.X - pictureBox.Location.X; // Where location of the mouse in the pictureframe
            var y = mousePosNow.Y - pictureBox.Location.Y;

            var oldimagex = (int) (x/oldzoom); // Where in the IMAGE is it now
            var oldimagey = (int) (y/oldzoom);

            var newimagex = (int) (x/_zoom); // Where in the IMAGE will it be when the new zoom i made
            var newimagey = (int) (y/_zoom);

            _imgx = newimagex - oldimagex + _imgx; // Where to move image to keep focus on one point
            _imgy = newimagey - oldimagey + _imgy;

            if (_zoom < ZOOM_MIN)
                _zoom = ZOOM_MIN;

            pictureBox.Refresh();
        }

        private void ResetZoom(bool fitEntireImage)
        {
            if (_imgx < 0)
                _imgx = 0;

            if (_imgy < 0)
                _imgy = 0;

            if (_currentImage == null) return;
            var g = CreateGraphics();
            if (fitEntireImage)
                _zoom = Math.Min(
                    (float) pictureBox.Height/_currentImage.Height*(_currentImage.VerticalResolution/g.DpiY),
                    (float) pictureBox.Width/_currentImage.Width*(_currentImage.HorizontalResolution/g.DpiX)
                    );
            else
                _zoom = pictureBox.Width/(float) _currentImage.Width*(_currentImage.HorizontalResolution/g.DpiX);
        }

        private void pictureBox_Paint(object sender, PaintEventArgs e)
        {
            if (_currentImage == null || _zoom <= 0) return;

            try
            {
                var g = e.Graphics;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.ScaleTransform(_zoom, _zoom);
                g.DrawImage(_currentImage, _imgx, _imgy);

                if (_switchImgButtonsEnabled && _mouseHover)
                {
                    if (!_showSwitchImgOnMouseOverWindow && !_mouseHoverInfo.OverAnyButton)
                        return;

                    g.ResetTransform();
                    Brush b = new SolidBrush(Color.FromArgb(128, Color.Black));
                    var buttonWidth = (int) (ClientSize.Width*SwitchImageButtonsPercentOfScreen);

                    g.FillRectangle(b, new Rectangle(0, 0, buttonWidth, ClientSize.Height));
                    g.FillRectangle(b, new Rectangle(ClientSize.Width - buttonWidth, 0, buttonWidth, ClientSize.Height));

                    var imgScale = buttonWidth/(float) Resources.Arrow_Next_icon.Size.Width;

                    imgScale = imgScale*0.75f;

                    var imgScaleInv = 1/imgScale;
                    g.ScaleTransform(imgScale, imgScale);
                    g.DrawImage(Resources.Arrow_Next_icon, (ClientSize.Width - buttonWidth)*imgScaleInv,
                        (ClientSize.Height/2f - 32*imgScale)*imgScaleInv);
                    g.DrawImage(Resources.Arrow_Back_icon, 0, (ClientSize.Height/2f - 32*imgScale)*imgScaleInv);
                    g.ResetTransform();


                    if (_mouseHoverInfo.OverAnyButton)
                    {
                        var rect = _mouseHoverInfo.OverLeftButton
                            ? new Rectangle(0, 0, buttonWidth, Height)
                            : new Rectangle(ClientSize.Width - buttonWidth - 1, 0, buttonWidth, Height);
                        Brush selectionBrush = new HatchBrush(HatchStyle.Percent50, Color.DimGray);

                        var p = new Pen(selectionBrush);
                        g.DrawRectangle(p, rect);
                        rect.Inflate(-1, -1);
                        p.Color = Color.FromArgb(128, Color.MidnightBlue);
                        g.DrawRectangle(p, rect);


                        b = new SolidBrush(Color.FromArgb(64, Color.Black));
                        g.FillRectangle(b, rect);
                    }
                }
            }
            catch (Exception ex)
            {
                LogWriter.LogError("Exception in image scale transform", ex);
            }
        }

        private void copyFilepathToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_imgRef != null)
            {
                Clipboard.Clear();
                Clipboard.SetText(_imgRef.CompletePath);
            }
        }

        private void openWithDefaultProgramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_imgRef != null)
                try
                {
                    Process.Start(_imgRef.CompletePath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
        }

        private void FormImageView_Activated(object sender, EventArgs e)
        {
            _requireFocusNotification = true;

            if (Focused && _requireFocusNotification)
            {
                var formFocusStateInfo = new ImageViewFormInfo(this);
                foreach (var observer in _observers)
                {
                    observer.OnNext(formFocusStateInfo);
                }
            }
        }

        private void FormImageView_Deactivate(object sender, EventArgs e)
        {
            if (!Focused)
            {
                _requireFocusNotification = true;
                var formFocusStateInfo = new ImageViewFormInfo(this, true);
                foreach (var observer in _observers)
                {
                    observer.OnNext(formFocusStateInfo);
                }
            }
        }


        private void pictureBox_MouseEnter(object sender, EventArgs e)
        {
            if (!_switchImgButtonsEnabled) return;
            _mouseHover = true;
            pictureBox.Refresh();
        }

        private void pictureBox_MouseLeave(object sender, EventArgs e)
        {
            if (!_switchImgButtonsEnabled) return;
            _mouseHover = false;
            pictureBox.Refresh();
        }

        private void pictureBox_MouseClick(object sender, MouseEventArgs e)
        {
            if (_mouseHoverInfo != null && _mouseHoverInfo.OverAnyButton)
            {
                if (_mouseHoverInfo.OverLeftButton)
                    SetPreviousImage();
                else
                    SetNextImage();
            }
        }

        private class MouseHoverInfo
        {
            private bool _overLeftButton;
            private bool _overRightButton;

            public bool OverLeftButton
            {
                get { return _overLeftButton; }
                set
                {
                    if (_overLeftButton != value)
                        StateChanged = true;

                    _overLeftButton = value;
                }
            }

            public bool OverRightButton
            {
                get { return _overRightButton; }
                set
                {
                    if (_overRightButton != value)
                        StateChanged = true;
                    _overRightButton = value;
                }
            }

            public bool OverAnyButton => OverLeftButton || OverRightButton;

            public bool StateChanged { get; private set; }

            public void ResetState()
            {
                StateChanged = false;
            }
        }
    }
}