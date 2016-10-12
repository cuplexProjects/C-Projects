using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using GeneralToolkitLib.Converters;
using GeneralToolkitLib.Log;
using GeneralToolkitLib.WindowsApi;
using ImageView.DataContracts;
using ImageView.Events;
using ImageView.Models;
using ImageView.Properties;
using ImageView.Services;
using ImageView.Utility;

namespace ImageView
{
    public partial class FormMain : Form
    {
        private readonly FormState _formState = new FormState();
        private readonly List<FormImageView> _imageViewFormList;
        private readonly PictureBox _pictureBoxAnimation = new PictureBox();
        private readonly string _windowTitle;
        private ImageViewApplicationSettings.ChangeImageAnimation _changeImageAnimation;
        private bool _dataReady;
        private FormBookmarks _formBookmarks;
        private FormRestartWithAdminPrivileges _formRestartWithAdminPrivileges;
        private FormWindows _formWindows;
        private bool _fullScreen;
        private ImageReferenceCollection _imageReferenceCollection;
        private bool _imageTransitionRunning;
        private int _imageViewFormIdCnt = 1;
        private bool _winKeyDown;

        public FormMain()
        {
            InitializeComponent();
            _imageViewFormList = new List<FormImageView>();
            _windowTitle = "Random image viewer - " + Application.ProductVersion;
        }


        private bool ImageSourceDataAvailable => _dataReady && ImageLoaderService.Instance.ImageReferenceList != null;

        private void FormMain_Load(object sender, EventArgs e)
        {
            if (DesignMode)
                return;

            DoubleBuffered = true;
            ApplicationSettingsService.Instance.OnSettingsChanged += Instance_OnSettingsChanged;
            ApplicationSettingsService.Instance.OnRegistryAccessDenied += Instance_OnRegistryAccessDenied;
            ImageLoaderService.Instance.OnImportComplete += Instance_OnImportComplete;
            ImageLoaderService.Instance.OnImageWasDeleted += Instance_OnImageWasDeleted;

            if (!ApplicationSettingsService.Instance.LoadSettings())
            {
                MessageBox.Show(Resources.Unable_To_Access_application_settings_in_registry,
                    Resources.Faild_to_load_settings, MessageBoxButtons.OK, MessageBoxIcon.Error);
                _formRestartWithAdminPrivileges = new FormRestartWithAdminPrivileges();
                if (_formRestartWithAdminPrivileges.ShowDialog(this) == DialogResult.OK)
                    return;
            }

            SyncUserControlStateWithAppSettings();

            if (ApplicationSettingsService.Instance.Settings.UseSavedMainFormPosition)
            {
                if (ApplicationSettingsService.Instance.Settings.MainFormSize.Width > 0 &&
                    ApplicationSettingsService.Instance.Settings.MainFormSize.Height > 0)
                {
                    Width = ApplicationSettingsService.Instance.Settings.MainFormSize.Width;
                    Height = ApplicationSettingsService.Instance.Settings.MainFormSize.Height;
                }

                if (ApplicationSettingsService.Instance.Settings.MainFormPosition.X >= 0 &&
                    ApplicationSettingsService.Instance.Settings.MainFormPosition.Y <
                    Screen.PrimaryScreen.WorkingArea.Bottom)
                {
                    Left = ApplicationSettingsService.Instance.Settings.MainFormPosition.X;
                    Top = ApplicationSettingsService.Instance.Settings.MainFormPosition.Y;
                }
            }

            _changeImageAnimation = ApplicationSettingsService.Instance.Settings.NextImageAnimation;
            timerSlideShow.Interval = ApplicationSettingsService.Instance.Settings.SlideshowInterval;


            addBookmarkToolStripMenuItem.Enabled = false;
            Text = _windowTitle;
            _pictureBoxAnimation.LoadCompleted += pictureBoxAnimation_LoadCompleted;
        }

        private void Instance_OnRegistryAccessDenied(object sender, EventArgs e)
        {
        }

        private async void pictureBoxAnimation_LoadCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (_imageTransitionRunning)
                return;

            ImageViewApplicationSettings settings = ApplicationSettingsService.Instance.Settings;
            var currentImage = pictureBox1.Image.Clone() as Image;
            var nextImage = _pictureBoxAnimation.Image.Clone() as Image;
            _pictureBoxAnimation.Image = null;

            int animationTime = settings.ImageTransitionTime;
            await PerformImageTransition(currentImage, nextImage, settings.NextImageAnimation, animationTime);

            currentImage?.Dispose();

            nextImage?.Dispose();

            if (_pictureBoxAnimation.Image != null)
            {
                _pictureBoxAnimation.Image.Dispose();
                _pictureBoxAnimation.Image = null;
            }

            SyncUserControlStateWithAppSettings();
            GC.Collect();
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!ScreenSaver.ScreenSaverEnabled)
                ScreenSaver.Enable();

            timerSlideShow.Enabled = false;
            BookmarkService.Instance.SaveBookmarks();
            ApplicationSettingsService.Instance.Settings.SetMainFormPosition(Bounds);
            ApplicationSettingsService.Instance.SaveSettings();
            BookmarkService.Instance.Dispose();
        }

        private void imageViewForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            _imageViewFormList.Remove(sender as FormImageView);
        }

        private void FormMain_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape && _fullScreen)
                ToggleFullscreen();

            //if (e.KeyCode == Keys.F11)
            //{
            //    ToggleFullscreen();
            //    e.Handled = true;
            //    return;
            //}

            if (_imageTransitionRunning)
                return;

            if (_winKeyDown || !ImageSourceDataAvailable)
                return;

            if (e.KeyCode == Keys.Right || e.KeyCode == Keys.Space || e.KeyCode == Keys.Enter)
            {
                _changeImageAnimation = ImageViewApplicationSettings.ChangeImageAnimation.SlideRight;
                LoadNewImageFile(_imageReferenceCollection.GetNextImage().CompletePath);
            }
            else if (e.KeyCode == Keys.Left)
            {
                _changeImageAnimation = ImageViewApplicationSettings.ChangeImageAnimation.SlideLeft;
                LoadNewImageFile(_imageReferenceCollection.GetPreviousImage().CompletePath);
            }
        }

        private void FormMain_KeyDown(object sender, KeyEventArgs e)
        {
            _winKeyDown = e.KeyCode == Keys.LWin || e.KeyCode == Keys.RWin;
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (_imageTransitionRunning)
                return;

            if (!ImageSourceDataAvailable) return;
            ImageReferenceElement imgRef;

            //Go Forward
            if (e.Button == MouseButtons.Left)
            {
                _changeImageAnimation = ImageViewApplicationSettings.ChangeImageAnimation.SlideLeft;
                imgRef = _imageReferenceCollection.GetNextImage();
            }
            else //Go backward
            {
                _changeImageAnimation = ImageViewApplicationSettings.ChangeImageAnimation.SlideRight;
                imgRef = _imageReferenceCollection.GetPreviousImage();
            }

            LoadNewImageFile(imgRef.CompletePath);
        }

        private void timerSlideShow_Tick(object sender, EventArgs e)
        {
            if (_imageTransitionRunning)
                return;

            timerSlideShow.Interval = ApplicationSettingsService.Instance.Settings.SlideshowInterval;
            LoadNewImageFile(_imageReferenceCollection.GetNextImage().CompletePath);
        }

        private void Instance_OnImageWasDeleted(object sender, EventArgs e)
        {
            Invoke(new NativeThreadFunctin(SetImageReferenceCollection));
        }

        private void Instance_OnImportComplete(object sender, ProgressEventArgs e)
        {
            Invoke(new NativeThreadFunctin(HandleImportDataComplete));
        }

        private void formSetSlideshowInterval_OnIntervalChanged(object sender, IntervalEventArgs e)
        {
            timerSlideShow.Interval = e.Interval*1000;
            ApplicationSettingsService.Instance.Settings.SlideshowInterval = timerSlideShow.Interval;
        }

        private void HandleImportDataComplete()
        {
            addBookmarkToolStripMenuItem.Enabled = true;
            SetImageReferenceCollection();
        }

        private void Instance_OnSettingsChanged(object sender, EventArgs e)
        {
            SyncUserControlStateWithAppSettings();
        }

        private void SyncUserControlStateWithAppSettings()
        {
            ImageViewApplicationSettings settings = ApplicationSettingsService.Instance.Settings;

            if (TopMost != settings.AlwaysOntop)
                TopMost = settings.AlwaysOntop;

            topMostToolStripMenuItem.Checked = settings.AlwaysOntop;

            _changeImageAnimation = settings.NextImageAnimation;
            autoLoadPreviousFolderToolStripMenuItem.Enabled = settings.EnableAutoLoadFunctionFromMenu &&
                                                              !string.IsNullOrWhiteSpace(settings.LastFolderLocation);
        }

        private void LoadNewImageFile(string imagePath)
        {
            try
            {
                if (ApplicationSettingsService.Instance.Settings.NextImageAnimation ==
                    ImageViewApplicationSettings.ChangeImageAnimation.None)
                    _changeImageAnimation = ImageViewApplicationSettings.ChangeImageAnimation.None;

                _pictureBoxAnimation.ImageLocation = null;

                if (pictureBox1.Image != null &&
                    (_changeImageAnimation != ImageViewApplicationSettings.ChangeImageAnimation.None))
                {
                    //_pictureBoxAnimation.ImageLocation = imagePath;
                    //_pictureBoxAnimation.LoadAsync();
                    _pictureBoxAnimation.Image = ImageCacheService.Instance.GetImage(imagePath);
                    _pictureBoxAnimation.Refresh();
                }
                else
                {
                    //pictureBox1.ImageLocation = imagePath;
                    //pictureBox1.Load();
                    pictureBox1.Image = ImageCacheService.Instance.GetImage(imagePath);
                    pictureBox1.Refresh();
                }

                Text = _windowTitle + " | " + GeneralConverters.GetFileNameFromPath(imagePath);
            }
            catch (Exception ex)
            {
                LogWriter.LogError(
                    $"FormMain.LoadNewImageFile(string imagePath) Error when trying to load file: {imagePath} : {ex.Message}",
                    ex);
            }
        }

        private void SetImageReferenceCollection()
        {
            bool randomizeImageCollection = ApplicationSettingsService.Instance.Settings.AutoRandomizeCollection;
            if (!ImageLoaderService.Instance.IsRunningImport && ImageLoaderService.Instance.ImageReferenceList != null)
            {
                _imageReferenceCollection =
                    ImageLoaderService.Instance.GenerateImageReferenceCollection(randomizeImageCollection);
                if (ImageLoaderService.Instance.ImageReferenceList.Count > 0)
                    _dataReady = true;
            }
        }

        private async Task PerformImageTransition(Image currentImage, Image nextImage,
            ImageViewApplicationSettings.ChangeImageAnimation animation, int animationTime)
        {
            const int sleepTime = 1;
            _imageTransitionRunning = true;
            await Task.Run(() =>
            {
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                while (stopwatch.ElapsedMilliseconds <= animationTime)
                {
                    long elapsedTime = stopwatch.ElapsedMilliseconds;

                    float factor = stopwatch.ElapsedMilliseconds/(float) animationTime;
                    Image transitionImage;
                    switch (animation)
                    {
                        case ImageViewApplicationSettings.ChangeImageAnimation.SlideLeft:
                            transitionImage = ImageTransform.OffsetImagesHorizontal(currentImage, nextImage,
                                new Size(pictureBox1.Width, pictureBox1.Height), factor, false);
                            break;
                        case ImageViewApplicationSettings.ChangeImageAnimation.SlideRight:
                            transitionImage = ImageTransform.OffsetImagesHorizontal(nextImage, currentImage,
                                new Size(pictureBox1.Width, pictureBox1.Height), factor, true);
                            break;
                        case ImageViewApplicationSettings.ChangeImageAnimation.SlideDown:
                            transitionImage = ImageTransform.OffsetImagesVertical(nextImage, currentImage,
                                new Size(nextImage.Width, nextImage.Height), factor, true);
                            break;
                        case ImageViewApplicationSettings.ChangeImageAnimation.SlideUp:
                            transitionImage = ImageTransform.OffsetImagesVertical(currentImage, nextImage,
                                new Size(Math.Max(nextImage.Width, currentImage.Width), nextImage.Height), factor, false);
                            break;
                        case ImageViewApplicationSettings.ChangeImageAnimation.FadeIn:
                            int width = nextImage.Width;
                            int height = nextImage.Height;
                            var nextImageBitmap = new Bitmap(nextImage, new Size(width, height));
                            transitionImage = ImageTransform.SetImageOpacity(nextImageBitmap, factor);
                            break;
                        default:
                            return;
                    }

                    if (!Visible)
                        return;
                    try
                    {
                        pictureBox1.Image = transitionImage.Clone() as Image;
                    }
                    catch (Exception ex)
                    {
                        LogWriter.LogError(Resources.Failed_to_set_transition_image_over_current_image_, ex);
                        MessageBox.Show(Resources.Failed_to_set_transition_image_over_current_image_,
                            Resources.Error_loading_new_image, MessageBoxButtons.OK, MessageBoxIcon.Error);

                        _imageTransitionRunning = false;
                        return;
                    }
                    finally
                    {
                        transitionImage.Dispose();
                    }

                    elapsedTime = stopwatch.ElapsedMilliseconds - elapsedTime;

                    if (sleepTime - elapsedTime > 0)
                        Thread.Sleep(Convert.ToInt32(sleepTime - elapsedTime));
                }
                stopwatch.Stop();
                LogWriter.LogMessage("Image transition finished after " + stopwatch.ElapsedMilliseconds + " ms",
                    LogWriter.LogLevel.Trace);
                Invoke(new EventHandler(OnImageLoadComplete), this, new EventArgs());
            });

            pictureBox1.Image = nextImage.Clone() as Image;
            _imageTransitionRunning = false;
        }

        private void FormMain_Activated(object sender, EventArgs e)
        {
            _formWindows?.RestoreFocusToMainForm();
        }

        private void ToggleFullscreen()
        {
            if (_fullScreen)
            {
                _formState.Restore(this);
                menuStrip1.Visible = true;
                BackColor = Color.WhiteSmoke;
            }
            else
            {
                _formState.Save(this);
                _formState.Maximize(this);
                menuStrip1.Visible = false;

                BackColor = Color.Black;
            }
            _fullScreen = !_fullScreen;
        }


        private void autoLoadPreviousFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var formLoad = new FormLoad();
            formLoad.SetBasePath(ApplicationSettingsService.Instance.Settings.LastFolderLocation);
            formLoad.ShowDialog(this);
        }

        private void openThumbnailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var formThumbnailView = new FormThumbnailView();
            formThumbnailView.ShowDialog(this);
        }

        private void topMostToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ApplicationSettingsService.Instance.Settings.AlwaysOntop =
                !ApplicationSettingsService.Instance.Settings.AlwaysOntop;
            ApplicationSettingsService.Instance.SaveSettings();
            SyncUserControlStateWithAppSettings();
        }

        private void showFullscreenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToggleFullscreen();
        }

        private void setImageScalingModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var formImageSizeModes = new FormImageSizeModes();

            if (formImageSizeModes.ShowDialog(this) == DialogResult.OK)
            {
                _pictureBoxAnimation.SizeMode = formImageSizeModes.ImageSizeMode;
            }
        }

        private void checkForUpdatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }


        private delegate void NativeThreadFunctin();

        #region Main Menu Functions

        private void openFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var fileBrowser = new FileBrowser();
            fileBrowser.ShowDialog(this);
        }

        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!ImageSourceDataAvailable) return;
            SyncUserControlStateWithAppSettings();
            timerSlideShow.Interval = 1;
            timerSlideShow.Start();

            //Disable screensaver
            ScreenSaver.Disable();
        }

        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timerSlideShow.Enabled = false;

            //Enable screensaver
            ScreenSaver.Disable();
        }

        private void setIntervalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var formSetSlideshowInterval = new FormSetSlideshowInterval(timerSlideShow.Interval/1000);
            formSetSlideshowInterval.OnIntervalChanged += formSetSlideshowInterval_OnIntervalChanged;
            formSetSlideshowInterval.ShowDialog(this);
        }


        private void openSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var formSettings = new FormSettings();
            formSettings.ShowDialog(this);

            foreach (FormImageView imageView in _imageViewFormList)
            {
                imageView.ReloadSettings();
            }
        }

        private void randomizeCollectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetImageReferenceCollection();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new AboutBox1().ShowDialog(this);
        }

        private void copyFilepathToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ImageSourceDataAvailable)
            {
                Clipboard.Clear();
                Clipboard.SetText(_imageReferenceCollection.CurrentImage.CompletePath);
            }
        }

        private void copyFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
                Clipboard.SetImage(pictureBox1.Image);
        }

        private void addBookmarkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (BookmarkService.Instance.BookmarkManager == null)
            {
                MessageBox.Show(Resources.Please_unlock_bookmarks_first);
                return;
            }

            if (ImageSourceDataAvailable)
            {
                var starupPosition = new Point(Location.X, Location.Y);
                starupPosition.X += addBookmarkToolStripMenuItem.Width;
                starupPosition.Y += addBookmarkToolStripMenuItem.Height + (Height - ClientSize.Height);

                var formAddBookmark = new FormAddBookmark(starupPosition, _imageReferenceCollection.CurrentImage);
                formAddBookmark.ShowDialog(this);
            }
        }

        private void openBookmarksToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            if (_formBookmarks == null || _formBookmarks.IsDisposed)
            {
                _formBookmarks = new FormBookmarks();
                _formBookmarks.Show();
            }
        }

        private void newWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var imageViewForm = new FormImageView(_imageViewFormIdCnt++);
            _imageViewFormList.Add(imageViewForm);
            imageViewForm.FormClosed += imageViewForm_FormClosed;
            imageViewForm.Show();

            if (_formWindows != null && !_formWindows.IsDisposed)
                _formWindows.Subscribe(imageViewForm);

            Focus();
        }

        private void windowsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (_formWindows == null || _formWindows.IsDisposed)
            {
                _formWindows = new FormWindows();
                _formWindows.SubscribeToList(_imageViewFormList);
            }

            if (!_formWindows.Visible)
                _formWindows.Show(this);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void autoArrangeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int index = 0;
            int windowsPerScreen = 2;

            int widthOffset = ApplicationSettingsService.Instance.Settings.ScreenWidthOffset;
            int minXOffset = ApplicationSettingsService.Instance.Settings.ScreenMinXOffset;

            foreach (Screen screen in Screen.AllScreens.OrderBy(s => s.Bounds.X))
            {
                if (screen.Primary) continue;
                for (int i = 0; i < windowsPerScreen; i++)
                {
                    if (index >= _imageViewFormList.Count)
                        break;

                    FormImageView imageWindow = _imageViewFormList[index++];
                    if (imageWindow == null || imageWindow.IsDisposed) continue;
                    imageWindow.Margin = new Padding(0);

                    int screenWidth = screen.Bounds.Width + widthOffset;
                    int screenMinx = screen.WorkingArea.X - minXOffset;


                    if (i == 0)
                        imageWindow.SetDesktopBounds(screenMinx, screen.WorkingArea.Y, screenWidth/2,
                            screen.WorkingArea.Height);
                    else
                        imageWindow.SetDesktopBounds(screenMinx + screen.WorkingArea.Width/2, screen.WorkingArea.Y,
                            screenWidth/2, screen.WorkingArea.Height);


                    imageWindow.WindowState = FormWindowState.Normal;
                    imageWindow.ResetZoomAndRepaint();
                    imageWindow.Show();
                    imageWindow.Focus();
                }
            }

            Show();
            Focus();
        }

        private void showAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (FormImageView imageWindow in _imageViewFormList)
            {
                if (imageWindow == null || imageWindow.IsDisposed) continue;
                imageWindow.Show();
                imageWindow.Focus();
            }

            Show();
            Focus();
        }

        private void hideAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (FormImageView imageWindow in _imageViewFormList)
            {
                if (imageWindow != null && !imageWindow.IsDisposed)
                    imageWindow.Hide();
            }
        }

        private void closeAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_imageViewFormList.Count == 0) return;
            if (
                MessageBox.Show(this, Resources.Are_you_sure_you_want_to_close_all_windows_,
                    Resources.Close_all_windows_, MessageBoxButtons.OKCancel, MessageBoxIcon.Question) ==
                DialogResult.OK)
            {
                var imageWindowQueue = new Queue<FormImageView>(_imageViewFormList);
                while (imageWindowQueue.Count > 0)
                {
                    FormImageView imageWindow = imageWindowQueue.Dequeue();
                    imageWindow.Close();
                }
            }
        }

        private void OnImageLoadComplete(object sender, EventArgs e)
        {
            if (!timerSlideShow.Enabled)
                return;

            if (_formWindows == null)
            {
                _formWindows = new FormWindows();
                _formWindows.SubscribeToList(_imageViewFormList);
            }

            _formWindows.RestoreFocusOnPreviouslyActiveImageForm();
        }

        #endregion
    }
}