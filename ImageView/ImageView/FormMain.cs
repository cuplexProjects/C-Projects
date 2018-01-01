﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autofac;
using GeneralToolkitLib.Converters;
using GeneralToolkitLib.WindowsApi;
using ImageView.DataContracts;
using ImageView.Events;
using ImageView.Models;
using ImageView.Properties;
using ImageView.Services;
using ImageView.UserControls;
using ImageView.Utility;
using Serilog;

namespace ImageView
{
    public partial class FormMain : Form
    {
        private readonly ApplicationSettingsService _applicationSettingsService;
        private readonly BookmarkService _bookmarkService;
        private readonly FormAddBookmark _formAddBookmark;
        private readonly FormSettings _formSettings;
        private readonly FormState _formState = new FormState();
        private readonly ImageCacheService _imageCacheService;
        private readonly ImageLoaderService _imageLoaderService;
        private readonly List<FormImageView> _imageViewFormList;
        private readonly PictureBox _pictureBoxAnimation = new PictureBox();
        private readonly ILifetimeScope _scope;
        private readonly string _windowTitle;
        private ImageViewApplicationSettings.ChangeImageAnimation _changeImageAnimation;
        private bool _dataReady;
        private FormBookmarks _formBookmarks;
        private FormRestartWithAdminPrivileges _formRestartWithAdminPrivileges;
        private FormThumbnailView _formThumbnailView;
        private FormWindows _formWindows;
        private bool _fullScreen;
        private ImageReferenceCollection _imageReferenceCollection;
        private bool _imageTransitionRunning;
        private int _imageViewFormIdCnt = 1;
        private bool _winKeyDown;


        public FormMain(FormAddBookmark formAddBookmark, BookmarkService bookmarkService, FormSettings formSettings, ApplicationSettingsService applicationSettingsService, ImageCacheService imageCacheService,
            ImageLoaderService imageLoaderService, ILifetimeScope scope)
        {
            _formAddBookmark = formAddBookmark;
            _bookmarkService = bookmarkService;
            _formSettings = formSettings;
            _applicationSettingsService = applicationSettingsService;
            _imageCacheService = imageCacheService;
            _imageLoaderService = imageLoaderService;
            _scope = scope;
            InitializeComponent();
            _imageViewFormList = new List<FormImageView>();
            _windowTitle = "Image Viewer - " + Application.ProductVersion;
        }


        private bool ImageSourceDataAvailable => _dataReady && _imageLoaderService.ImageReferenceList != null;

        private void FormMain_Load(object sender, EventArgs e)
        {
            if (DesignMode)
            {
                return;
            }

            DoubleBuffered = true;
            _applicationSettingsService.OnSettingsSaved += Instance_OnSettingsSaved;
            _applicationSettingsService.OnRegistryAccessDenied += Instance_OnRegistryAccessDenied;
            _imageLoaderService.OnImportComplete += Instance_OnImportComplete;
            _imageLoaderService.OnImageWasDeleted += Instance_OnImageWasDeleted;

            if (!_applicationSettingsService.LoadSettings())
            {
                MessageBox.Show(Resources.Unable_To_Access_application_settings_in_registry,
                    Resources.Faild_to_load_settings, MessageBoxButtons.OK, MessageBoxIcon.Error);
                _formRestartWithAdminPrivileges = new FormRestartWithAdminPrivileges();
                if (_formRestartWithAdminPrivileges.ShowDialog(this) == DialogResult.OK)
                {
                    return;
                }
            }

            SyncUserControlStateWithAppSettings();

            if (_applicationSettingsService.Settings.UseSavedMainFormPosition)
            {
                RestoreFormState.SetFormSizeAndPosition(this, _applicationSettingsService.Settings.MainFormSize,
                    _applicationSettingsService.Settings.MainFormPosition, Screen.PrimaryScreen.WorkingArea);
            }

            _changeImageAnimation = _applicationSettingsService.Settings.NextImageAnimation;
            timerSlideShow.Interval = _applicationSettingsService.Settings.SlideshowInterval;


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
            {
                return;
            }

            var settings = _applicationSettingsService.Settings;
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

        private bool AllowAplicatonExit()
        {
            if (!_applicationSettingsService.Settings.ConfirmApplicationShutdown)
            {
                return true;
            }
            var confirmExitUserControl = new ConfirmExitUserControl(_applicationSettingsService);
            var exitDialogForm = FormFactory.CreateModalSimpleDialog(confirmExitUserControl);

            return exitDialogForm.ShowDialog(this) == DialogResult.OK;
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!AllowAplicatonExit())
            {
                e.Cancel = true;
                return;
            }

            if (!ScreenSaver.ScreenSaverEnabled)
            {
                ScreenSaver.Enable();
            }

            timerSlideShow.Enabled = false;
            _bookmarkService.SaveBookmarks();
            _applicationSettingsService.Settings.SetMainFormPosition(Bounds);
            _applicationSettingsService.SaveSettings();
        }

        private void imageViewForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            _imageViewFormList.Remove(sender as FormImageView);
        }

        private void FormMain_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape && _fullScreen)
            {
                ToggleFullscreen();
            }

            //if (e.KeyCode == Keys.F11)
            //{
            //    ToggleFullscreen();
            //    e.Handled = true;
            //    return;
            //}

            if (_imageTransitionRunning)
            {
                return;
            }

            if (_winKeyDown || !ImageSourceDataAvailable)
            {
                return;
            }

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
            {
                return;
            }

            if (!ImageSourceDataAvailable)
            {
                return;
            }
            ImageReferenceElement imgRef;

            //Reset timer
            if (timerSlideShow.Enabled)
            {
                timerSlideShow.Stop();
                timerSlideShow.Start();
            }

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
            AddNextImageToCache(_imageReferenceCollection.PeekNextImage().CompletePath);
        }

        private void timerSlideShow_Tick(object sender, EventArgs e)
        {
            if (_imageTransitionRunning)
            {
                return;
            }

            timerSlideShow.Interval = _applicationSettingsService.Settings.SlideshowInterval;
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
            timerSlideShow.Interval = e.Interval * 1000;
            _applicationSettingsService.Settings.SlideshowInterval = timerSlideShow.Interval;
        }

        private void HandleImportDataComplete()
        {
            addBookmarkToolStripMenuItem.Enabled = true;
            SetImageReferenceCollection();
        }

        private void Instance_OnSettingsSaved(object sender, EventArgs e)
        {
            SyncUserControlStateWithAppSettings();
        }

        private void SyncUserControlStateWithAppSettings()
        {
            var settings = _applicationSettingsService.Settings;

            if (TopMost != settings.AlwaysOntop)
            {
                TopMost = settings.AlwaysOntop;
            }

            topMostToolStripMenuItem.Checked = settings.AlwaysOntop;

            _changeImageAnimation = settings.NextImageAnimation;
            autoLoadPreviousFolderToolStripMenuItem.Enabled = settings.EnableAutoLoadFunctionFromMenu &&
                                                              !string.IsNullOrWhiteSpace(settings.LastFolderLocation);
        }

        private void LoadNewImageFile(string imagePath)
        {
            try
            {
                pictureBox1.SizeMode = (PictureBoxSizeMode) _applicationSettingsService.Settings.PrimaryImageSizeMode;


                if (_applicationSettingsService.Settings.NextImageAnimation == ImageViewApplicationSettings.ChangeImageAnimation.None)
                {
                    _changeImageAnimation = ImageViewApplicationSettings.ChangeImageAnimation.None;
                }

                _pictureBoxAnimation.ImageLocation = null;

                if (pictureBox1.Image != null && _changeImageAnimation != ImageViewApplicationSettings.ChangeImageAnimation.None)
                {
                    _pictureBoxAnimation.Image = _imageCacheService.GetImage(imagePath);
                    _pictureBoxAnimation.Refresh();
                }
                else
                {
                    pictureBox1.Image = _imageCacheService.GetImage(imagePath);
                    pictureBox1.Refresh();
                }

                Text = _windowTitle + @" | " + GeneralConverters.GetFileNameFromPath(imagePath);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "FormMain.LoadNewImageFile(string imagePath) Error when trying to load file: {imagePath} : {Message}", imagePath, ex.Message);
            }
        }

        private void AddNextImageToCache(string imagePath)
        {
            _imageCacheService.GetImage(imagePath);
        }

        private void SetImageReferenceCollection()
        {
            bool randomizeImageCollection = _applicationSettingsService.Settings.AutoRandomizeCollection;
            if (!_imageLoaderService.IsRunningImport && _imageLoaderService.ImageReferenceList != null)
            {
                _imageReferenceCollection = _imageLoaderService.GenerateImageReferenceCollection(randomizeImageCollection);
                if (_imageLoaderService.ImageReferenceList.Count > 0)
                {
                    _dataReady = true;
                }
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

                    float factor = stopwatch.ElapsedMilliseconds / (float) animationTime;
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
                    {
                        return;
                    }
                    try
                    {
                        pictureBox1.Image = transitionImage.Clone() as Image;
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex, Resources.Failed_to_set_transition_image_over_current_image_);
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
                    {
                        Thread.Sleep(Convert.ToInt32(sleepTime - elapsedTime));
                    }
                }
                stopwatch.Stop();
                Log.Verbose("Image transition finished after " + stopwatch.ElapsedMilliseconds + " ms");
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
                Cursor.Show();
            }
            else
            {
                _formState.Save(this);
                _formState.Maximize(this);
                menuStrip1.Visible = false;

                BackColor = Color.Black;
                Cursor.Hide();
                //HideCursorInFullScreen().Start();
            }
            _fullScreen = !_fullScreen;
        }


        private void autoLoadPreviousFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var formLoad = new FormLoad(_imageLoaderService);
            formLoad.SetBasePath(_applicationSettingsService.Settings.LastFolderLocation);
            formLoad.ShowDialog(this);
        }

        private void AutoArrangeOnSingleScreen()
        {
            if (_imageViewFormList.Count == 0)
            {
                return;
            }

            var screen = Screen.PrimaryScreen;
            int widthPerScreen = screen.WorkingArea.Width / _imageViewFormList.Count;
            int offset = 0;

            foreach (var formImage in _imageViewFormList)
            {
                formImage.Width = widthPerScreen;
                formImage.Height = screen.WorkingArea.Height;
                formImage.Left = offset;
                formImage.Top = 0;
                formImage.Focus();
                offset += widthPerScreen;
                formImage.ResetZoomAndRepaint();
            }
        }

        private void AutoArrangeOnMultipleScreens()
        {
            int index = 0;
            int windowsPerScreen = 2;

            int widthOffset = _applicationSettingsService.Settings.ScreenWidthOffset;
            int minXOffset = _applicationSettingsService.Settings.ScreenMinXOffset;

            foreach (var screen in Screen.AllScreens.OrderBy(s => s.Bounds.X))
            {
                if (screen.Primary)
                {
                    continue;
                }
                for (int i = 0; i < windowsPerScreen; i++)
                {
                    if (index >= _imageViewFormList.Count)
                    {
                        break;
                    }

                    var imageWindow = _imageViewFormList[index++];
                    if (imageWindow == null || imageWindow.IsDisposed)
                    {
                        continue;
                    }
                    imageWindow.Margin = new Padding(0);

                    int screenWidth = screen.Bounds.Width + widthOffset;
                    int screenMinx = screen.WorkingArea.X - minXOffset;


                    if (i == 0)
                    {
                        imageWindow.SetDesktopBounds(screenMinx, screen.WorkingArea.Y, screenWidth / 2,
                            screen.WorkingArea.Height);
                    }
                    else
                    {
                        imageWindow.SetDesktopBounds(screenMinx + screen.WorkingArea.Width / 2, screen.WorkingArea.Y,
                            screenWidth / 2, screen.WorkingArea.Height);
                    }


                    imageWindow.WindowState = FormWindowState.Normal;
                    imageWindow.ResetZoomAndRepaint();
                    imageWindow.Show();
                    imageWindow.Focus();
                }
            }
        }

        private void FormThumbnailView_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (sender is Form form)
            {
                form.Dispose();
            }
            _formThumbnailView = null;
            GC.Collect();
        }

        private void openInDefaultApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenImageInDefaultApp();
        }

        private void OpenImageInDefaultApp()
        {
            if (ImageSourceDataAvailable)
            {
                string currentFile = _imageReferenceCollection.CurrentImage.CompletePath;
                try
                {
                    Process.Start(currentFile);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Error in MainForm open image in default app");
                }
            }
        }

        private void imageDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void pictureBox1_LoadCompleted(object sender, AsyncCompletedEventArgs e)
        {
            pictureBox1.SizeMode = (PictureBoxSizeMode) _applicationSettingsService.Settings.PrimaryImageSizeMode;
        }

        private void openInDefaultApplicationToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            OpenImageInDefaultApp();
        }

        private delegate void NativeThreadFunctin();

        #region Main Menu Functions

        private async void menuItemLoadBookmarkedImages_Click(object sender, EventArgs e)
        {
            bool result = await _imageLoaderService.RunBookmarkImageImport();

            if (result)
            {
                MessageBox.Show("Successfuly loaded all bookmarks as source images", "Import complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                Log.Warning("RunBookmarkImageImport returned false");
                MessageBox.Show("Failed to load bookmarked images as source images", "Import failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
          
        }

        private void topMostToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _applicationSettingsService.Settings.AlwaysOntop = !_applicationSettingsService.Settings.AlwaysOntop;
            _applicationSettingsService.SaveSettings();
            SyncUserControlStateWithAppSettings();
        }

        private void showFullscreenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToggleFullscreen();
        }

        private void setImageScalingModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var formImageSizeModes = new FormImageSizeModes(_applicationSettingsService);

            if (formImageSizeModes.ShowDialog(this) == DialogResult.OK)
            {
                _pictureBoxAnimation.SizeMode = formImageSizeModes.ImageSizeMode;
            }
        }

        private async void checkForUpdatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var isLatest = await IsLatestVersion();
                if (isLatest)
                {
                    MessageBox.Show("This is the latest version available", "Update check", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (MessageBox.Show("There is a newer version available, do you want to update?", "Update", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    await DownloadAndRunLatestVersionInstaller();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Exception while checking for updates");
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task<bool> IsLatestVersion()
        {
            var updateService = _scope.Resolve<UpdateService>();
            var latestVersion = await updateService.GetLatestVersion();
            var curentVersion = ApplicationVersion.Parse(Assembly.GetExecutingAssembly().GetName().Version.ToString());

            return curentVersion.CompareTo(latestVersion) >= 0;
        }

        private async Task DownloadAndRunLatestVersionInstaller()
        {
            var updateService = _scope.Resolve<UpdateService>();
            string path = await updateService.DownloadLatestVersion();
            Process.Start(path);
        }

        private void openFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var fileBrowser = new FileBrowser(_applicationSettingsService, _imageLoaderService);
            fileBrowser.ShowDialog(this);
        }

        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!ImageSourceDataAvailable)
            {
                return;
            }
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
            var formSetSlideshowInterval = new FormSetSlideshowInterval(timerSlideShow.Interval / 1000);
            formSetSlideshowInterval.OnIntervalChanged += formSetSlideshowInterval_OnIntervalChanged;
            formSetSlideshowInterval.ShowDialog(this);
        }


        private void openSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _formSettings.ShowDialog(this);

            foreach (var imageView in _imageViewFormList)
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
            {
                Clipboard.SetImage(pictureBox1.Image);
            }
        }

        private void addBookmarkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_bookmarkService.BookmarkManager == null)
            {
                MessageBox.Show(Resources.Please_unlock_bookmarks_first);
                return;
            }

            if (ImageSourceDataAvailable)
            {
                if (_fullScreen)
                {
                    Cursor.Show();
                }
                var starupPosition = new Point(Location.X, Location.Y);
                starupPosition.X += addBookmarkToolStripMenuItem.Width;
                starupPosition.Y += addBookmarkToolStripMenuItem.Height + (Height - ClientSize.Height);

                if (_imageReferenceCollection.CurrentImage != null)
                {
                    _formAddBookmark.Init(starupPosition, _imageReferenceCollection.CurrentImage);
                    _formAddBookmark.ShowDialog(this);
                    if (_fullScreen)
                    {
                        Cursor.Hide();
                    }
                }
            }
        }

        private void openBookmarksToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            if (_formBookmarks == null)
            {
                _formBookmarks = new FormBookmarks(_bookmarkService, _bookmarkService.BookmarkManager, _applicationSettingsService);
                _formBookmarks.Show();
                _formBookmarks.Focus();
                _formBookmarks.Closed += (o, args) =>
                {
                    _formBookmarks = null;
                    GC.Collect();
                };
            }
            else
            {
                _formBookmarks.Show();
                _formBookmarks.Focus();
            }
        }

        private void newWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var imageViewForm = new FormImageView(_imageViewFormIdCnt++, _formAddBookmark, _bookmarkService.BookmarkManager, _applicationSettingsService, _imageCacheService, _imageLoaderService);
            _imageViewFormList.Add(imageViewForm);
            imageViewForm.FormClosed += imageViewForm_FormClosed;
            imageViewForm.Show();

            if (_formWindows != null && !_formWindows.IsDisposed)
            {
                _formWindows.Subscribe(imageViewForm);
            }

            Focus();
        }

        private void windowsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (_formWindows == null || _formWindows.IsDisposed)
            {
                _formWindows = new FormWindows(_applicationSettingsService);
                _formWindows.SubscribeToList(_imageViewFormList);
            }

            if (!_formWindows.Visible)
            {
                _formWindows.Show(this);
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void autoArrangeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Screen.AllScreens.Length == 1)
            {
                AutoArrangeOnSingleScreen();
            }
            else
            {
                AutoArrangeOnMultipleScreens();
            }

            Show();
            Focus();
        }

        private void showAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (var imageWindow in _imageViewFormList)
            {
                if (imageWindow == null || imageWindow.IsDisposed)
                {
                    continue;
                }
                imageWindow.Show();
                imageWindow.Focus();
            }

            Show();
            Focus();
        }

        private void hideAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (var imageWindow in _imageViewFormList)
            {
                if (imageWindow != null && !imageWindow.IsDisposed)
                {
                    imageWindow.Hide();
                }
            }
        }

        private void closeAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_imageViewFormList.Count == 0)
            {
                return;
            }
            if (
                MessageBox.Show(this, Resources.Are_you_sure_you_want_to_close_all_windows_,
                    Resources.Close_all_windows_, MessageBoxButtons.OKCancel, MessageBoxIcon.Question) ==
                DialogResult.OK)
            {
                var imageWindowQueue = new Queue<FormImageView>(_imageViewFormList);
                while (imageWindowQueue.Count > 0)
                {
                    var imageWindow = imageWindowQueue.Dequeue();
                    imageWindow.Close();
                }
            }
        }

        private void OnImageLoadComplete(object sender, EventArgs e)
        {
            if (!timerSlideShow.Enabled)
            {
                return;
            }

            if (_formWindows == null)
            {
                _formWindows = new FormWindows(_applicationSettingsService);
                _formWindows.SubscribeToList(_imageViewFormList);
            }

            _formWindows.RestoreFocusOnPreviouslyActiveImageForm();
        }

        private void menuItemOpenImage_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = Resources.ImageFormatFilter;
            openFileDialog1.FileName = "";
            if (openFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                LoadNewImageFile(openFileDialog1.FileName);
                if (pictureBox1.Image != null)
                {
                    addBookmarkToolStripMenuItem.Enabled = true;
                }

                if (_imageReferenceCollection != null)
                {
                    return;
                }
                _imageReferenceCollection = new ImageReferenceCollection(new List<int>(), _imageLoaderService);
                var currentImage = _imageReferenceCollection.SetCurrentImage(openFileDialog1.FileName);
                _dataReady = true;
                if (_imageLoaderService.ImageReferenceList == null)
                {
                    _imageLoaderService.CreateFromOpenSingleImage(currentImage);
                }
            }
        }

        private void openThumbnailsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (_formThumbnailView == null)
            {
                //_formThumbnailView = new FormThumbnailView(_formAddBookmark, _applicationSettingsService, _imageCacheService,);
                _formThumbnailView = _scope.Resolve<FormThumbnailView>();
                _formThumbnailView.Show();
                _formThumbnailView.FormClosed += FormThumbnailView_FormClosed;
            }
            else
            {
                _formThumbnailView.Focus();
            }
        }

        private void thumbnailDBSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var thumbnailservice = _scope.Resolve<ThumbnailService>();
            var form = new FormThumbnailSettings(thumbnailservice);
            form.ShowDialog(this);
        }

        #endregion

 
    }
}