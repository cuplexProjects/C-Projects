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
        private const int ChangeImagePanelWidth = 50; 
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
        private bool _showSwitchImgPanel;
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
                _showSwitchImgOnMouseOverWindow = ApplicationSettingsService.Instance.Settings.ShowNextPrevControlsOnEnterWindow;
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
            bool randomizeImageCollection = ApplicationSettingsService.Instance.Settings.AutoRandomizeCollection;
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
            MouseEventArgs mouse = e;
            if (mouse.Button == MouseButtons.Left)
            {
                if (_mousepressed) return;
                _mousepressed = true;
                _mouseDown = mouse.Location;
                _startx = _imgx;
                _starty = _imgy;
            }
        }

        private void pictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            MouseEventArgs mouse = e;

            if (mouse.Button == MouseButtons.Left)
            {
                Point mousePosNow = mouse.Location;

                int deltaX = mousePosNow.X - _mouseDown.X;
                // the distance the mouse has been moved since mouse was pressed
                int deltaY = mousePosNow.Y - _mouseDown.Y;

                _imgx = (int) (_startx + deltaX/_zoom);
                // calculate new offset of image based on the current zoom factor
                _imgy = (int) (_starty + deltaY/_zoom);

                pictureBox.Refresh();
            }

            if (_mouseHoverInfo == null) return;

           
            var leftPanel = new Rectangle(0, 0, ChangeImagePanelWidth, Height);
            var rightPanel = new Rectangle(ClientSize.Width - ChangeImagePanelWidth, 0, ChangeImagePanelWidth, Height);

            _mouseHoverInfo.OverLeftPanel = false;
            _mouseHoverInfo.OverRightPanel = false;
            _mouseHoverInfo.LeftButtonPressed = mouse.Button == MouseButtons.Left;
            if (leftPanel.IntersectsWith(new Rectangle(mouse.Location, new Size(1, 1))))
            {
                _mouseHoverInfo.OverLeftPanel = true;
            }
            else if (rightPanel.IntersectsWith(new Rectangle(mouse.Location, new Size(1, 1))))
            {
                _mouseHoverInfo.OverRightPanel = true;
            }

            UpdateSwitchImgPanelState();

            if (_mouseHoverInfo.StateChanged)
            {
                pictureBox.Refresh();
                _mouseHoverInfo.ResetState();
            }
        }

        private void UpdateSwitchImgPanelState()
        {
            _showSwitchImgPanel = _switchImgButtonsEnabled && _mouseHover && !_mouseHoverInfo.LeftButtonPressed;
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
            float oldzoom = _zoom;

            if (e.Delta > 0)
                _zoom += 0.1F + _zoom*.05f;

            else if (e.Delta < 0)
                _zoom = Math.Max(_zoom - 0.1F - _zoom*.05f, ZOOM_MIN);

            MouseEventArgs mouse = e;
            Point mousePosNow = mouse.Location;

            int x = mousePosNow.X - pictureBox.Location.X; // Where location of the mouse in the pictureframe
            int y = mousePosNow.Y - pictureBox.Location.Y;

            int oldimagex = (int) (x/oldzoom); // Where in the IMAGE is it now
            int oldimagey = (int) (y/oldzoom);

            int newimagex = (int) (x/_zoom); // Where in the IMAGE will it be when the new zoom i made
            int newimagey = (int) (y/_zoom);

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
            Graphics g = CreateGraphics();
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
                Graphics g = e.Graphics;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.ScaleTransform(_zoom, _zoom);
                g.DrawImage(_currentImage, _imgx, _imgy);

                if (_showSwitchImgPanel)
                {
                    if (!_showSwitchImgOnMouseOverWindow && !_mouseHoverInfo.OverAnyButton)
                        return;

                    g.ResetTransform();
                    Brush b = new SolidBrush(Color.FromArgb(64, Color.LightGray));

                    for (int i = 0; i < 2; i++)
                    {
                        g.FillRectangle(b, new Rectangle(0, 0, ChangeImagePanelWidth, ClientSize.Height));
                        g.FillRectangle(b, new Rectangle(ClientSize.Width - ChangeImagePanelWidth, 0, ChangeImagePanelWidth, ClientSize.Height));
                        b = new SolidBrush(Color.FromArgb(128, Color.Black));
                    }

                    int imgWidth = Convert.ToInt32(Math.Min(Resources.Arrow_Back_icon.Width, ChangeImagePanelWidth)*0.8);
                    float imgScale = (float)imgWidth/Resources.Arrow_Back_icon.Width*0.7f;
                    int imgMargin = (ChangeImagePanelWidth - imgWidth)/2;
                    int imgYpos = ClientSize.Height/2 - imgWidth/2;

                    g.ScaleTransform(imgScale, imgScale);
                    g.TranslateTransform(imgMargin, 0);
                    Point leftArrowPos = new Point(0, imgYpos);
                    Point rightArrowPos=new Point(ClientSize.Width- ChangeImagePanelWidth, imgYpos);
                    
                    g.DrawImage(Resources.Arrow_Back_icon, TranslatePoint(leftArrowPos, imgScale));
                    g.DrawImage(Resources.Arrow_Next_icon, TranslatePoint(rightArrowPos, imgScale));
                    g.ResetTransform();

                    if (_mouseHoverInfo.OverAnyButton)
                    {
                        Rectangle rect = _mouseHoverInfo.OverLeftPanel
                            ? new Rectangle(0, 0, ChangeImagePanelWidth, Height)
                            : new Rectangle(ClientSize.Width - ChangeImagePanelWidth - 1, 0, ChangeImagePanelWidth, Height);
                        Brush selectionBrush = new HatchBrush(HatchStyle.Percent50, Color.DimGray);

                        var p = new Pen(selectionBrush);
                        g.DrawRectangle(p, rect);
                        rect.Inflate(-1, -1);
                        p.Color = Color.FromArgb(128, Color.MidnightBlue);
                        g.DrawRectangle(p, rect);
                        
                        b = new SolidBrush(Color.FromArgb(96, Color.Black));
                        g.FillRectangle(b, rect);
                    }
                }
            }
            catch (Exception ex)
            {
                LogWriter.LogError("Exception in image scale transform", ex);
            }
        }

        private PointF TranslatePoint(Point point, float scale)
        {
            return new PointF(point.X/scale, point.Y/scale);
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
            if (_imgRef == null) return;
            try
            {
                Process.Start(_imgRef.CompletePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Resources.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            UpdateSwitchImgPanelState();
            pictureBox.Refresh();
        }

        private void pictureBox_MouseLeave(object sender, EventArgs e)
        {
            if (!_switchImgButtonsEnabled) return;
            _mouseHover = false;
            UpdateSwitchImgPanelState();
            pictureBox.Refresh();
        }

        private void pictureBox_MouseClick(object sender, MouseEventArgs e)
        {
            if (_mouseHoverInfo != null && _mouseHoverInfo.OverAnyButton && _showSwitchImgPanel)
            {
                if (_mouseHoverInfo.OverLeftPanel)
                    SetPreviousImage();
                else
                    SetNextImage();
            }
        }

        private class MouseHoverInfo
        {
            private bool _overLeftButton;
            private bool _overRightButton;
            private bool _leftButtonPressed;

            public bool OverLeftPanel
            {
                get { return _overLeftButton; }
                set
                {
                    if (_overLeftButton != value)
                        StateChanged = true;

                    _overLeftButton = value;
                }
            }

            public bool OverRightPanel
            {
                get => _overRightButton;
                set
                {
                    if (_overRightButton != value)
                        StateChanged = true;
                    _overRightButton = value;
                }
            }

            public bool OverAnyButton => OverLeftPanel || OverRightPanel;

            public bool StateChanged { get; private set; }

            public bool LeftButtonPressed
            {
                get => _leftButtonPressed;
                set {
                    if (_leftButtonPressed != value)
                        StateChanged = true;
                    _leftButtonPressed = value;
                }
            }

            public void ResetState()
            {
                StateChanged = false;
            }
        }

        private void bookmarkImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ServiceLocator.GetBookmarkService().BookmarkManager == null)
            {
                MessageBox.Show(Resources.Please_unlock_bookmarks_first);
                return;
            }
            if (!ImageSourceDataAvailable || _imageReferenceCollection.CurrentImage == null)
            {
                return;
            }
        
            var starupPosition = Location;
            var formAddBookmark = new FormAddBookmark(starupPosition, _imageReferenceCollection.CurrentImage);
            formAddBookmark.ShowDialog(this);
        }
    }
}