using System;
using System.Drawing;
using System.Windows.Forms;
using GeneralToolkitLib.Converters;
using ImageView.DataContracts;
using ImageView.InputForms;
using ImageView.Services;
using ImageView.Utility;

namespace ImageView
{
    public partial class FormSettings : Form
    {
        private const int TrackbarDivider = 1048576;
        private readonly ApplicationSettingsService _applicationSettingsService;
        private readonly BookmarkService _bookmarkService;
        private readonly ImageCacheService _imageCacheService;
        private long _selectedCacheSize;

        public FormSettings(BookmarkService bookmarkService, ApplicationSettingsService applicationSettingsService, ImageCacheService imageCacheService)
        {
            _bookmarkService = bookmarkService;
            _applicationSettingsService = applicationSettingsService;
            _imageCacheService = imageCacheService;
            _selectedCacheSize = _imageCacheService.CacheSize;
            InitializeComponent();
        }

        private void FormSettings_Load(object sender, EventArgs e)
        {
            ImageViewApplicationSettings settings = _applicationSettingsService.Settings;
            chkAutoRandomize.Checked = settings.AutoRandomizeCollection;
            chkPasswordProtectBookmarks.Checked = settings.PasswordProtectBookmarks;
            chkShowSwitchImgButtons.Checked = settings.ShowSwitchImageButtons;
            chkEnableAutoload.Checked = settings.EnableAutoLoadFunctionFromMenu;
            chkConfirmExit.Checked = settings.ConfirmApplicationShutdown;

            if (settings.ShowNextPrevControlsOnEnterWindow)
            {
                rbOverWindow.Checked = true;
                rbOverControlArea.Checked = false;
            }
            else
            {
                rbOverWindow.Checked = false;
                rbOverControlArea.Checked = true;
            }

            rbImgTransformNone.Checked = false;
            switch (settings.NextImageAnimation)
            {
                case ImageViewApplicationSettings.ChangeImageAnimation.None:
                    rbImgTransformNone.Checked = true;
                    break;
                //case ImageViewApplicationSettings.ChangeImageAnimation.SlideLeft:
                //    rbImgTransformSlideLeft.Checked = true;
                //    break;
                //case ImageViewApplicationSettings.ChangeImageAnimation.SlideRight:
                //    rbImgTransformSlideRight.Checked = true;
                //    break;
                //case ImageViewApplicationSettings.ChangeImageAnimation.SlideDown:
                //    rbImgTransformSlideDown.Checked = true;
                //    break;
                //case ImageViewApplicationSettings.ChangeImageAnimation.SlideUp:
                //    rbImgTransformSlideUp.Checked = true;
                //  break;
                case ImageViewApplicationSettings.ChangeImageAnimation.FadeIn:
                    rbImgTransformFadeIn.Checked = true;
                    break;
                default:
                    rbImgTransformNone.Checked = true;
                    break;
            }

            if (settings.ImageTransitionTime < 10 || settings.ImageTransitionTime > 5000)
            {
                settings.ImageTransitionTime = 1000;
            }

            numericScreenMinOffset.Value = settings.ScreenMinXOffset;
            numericScreenWidthOffset.Value = settings.ScreenWidthOffset;

            trackBarFadeTime.Value = settings.ImageTransitionTime;
            lblFadeTime.Text = trackBarFadeTime.Value + " ms";

            // Colors
            backgroundColorDropdownList.DataSource = UIHelper.GetSelectableBackgroundColors();
            if (backgroundColorDropdownList.Items.Count > 0)
                backgroundColorDropdownList.SelectedIndex = 0;

            UpdateCacheStats();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            ImageViewApplicationSettings settings = _applicationSettingsService.Settings;
            settings.AutoRandomizeCollection = chkAutoRandomize.Checked;

            if (rbImgTransformNone.Checked)
                settings.NextImageAnimation = ImageViewApplicationSettings.ChangeImageAnimation.None;

            //if (rbImgTransformSlideLeft.Checked)
            //    settings.NextImageAnimation = ImageViewApplicationSettings.ChangeImageAnimation.SlideLeft;

            //if (rbImgTransformSlideRight.Checked)
            //    settings.NextImageAnimation = ImageViewApplicationSettings.ChangeImageAnimation.SlideRight;

            //if (rbImgTransformSlideDown.Checked)
            //    settings.NextImageAnimation = ImageViewApplicationSettings.ChangeImageAnimation.SlideDown;

            //if (rbImgTransformSlideUp.Checked)
            //    settings.NextImageAnimation = ImageViewApplicationSettings.ChangeImageAnimation.SlideUp;

            _applicationSettingsService.Settings.ShowSwitchImageButtons = chkShowSwitchImgButtons.Checked;
            _applicationSettingsService.Settings.EnableAutoLoadFunctionFromMenu = chkEnableAutoload.Checked;
            _applicationSettingsService.Settings.ShowNextPrevControlsOnEnterWindow = rbOverWindow.Checked;
            _applicationSettingsService.Settings.ConfirmApplicationShutdown = chkConfirmExit.Checked;

            if (rbImgTransformFadeIn.Checked)
                settings.NextImageAnimation = ImageViewApplicationSettings.ChangeImageAnimation.FadeIn;

            settings.ImageTransitionTime = trackBarFadeTime.Value;
            settings.ScreenMinXOffset = Convert.ToInt32(numericScreenMinOffset.Value);
            settings.ScreenWidthOffset = Convert.ToInt32(numericScreenWidthOffset.Value);

            _applicationSettingsService.Settings.ImageCacheSize = _selectedCacheSize;
            _imageCacheService.CacheSize = _selectedCacheSize;

            _applicationSettingsService.SaveSettings();
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void chkPasswordProtectBookmarks_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void chkPasswordProtectBookmarks_MouseUp(object sender, MouseEventArgs e)
        {
            if (chkPasswordProtectBookmarks.Checked)
                using (var formSetPassword = new FormSetPassword())
                {
                    if (formSetPassword.ShowDialog(this) == DialogResult.OK)
                    {
                        if (formSetPassword.VerifiedPassword == null)
                        {
                            chkPasswordProtectBookmarks.Checked = false;
                            return;
                        }

                        _applicationSettingsService.Settings.EnablePasswordProtectBookmarks(formSetPassword.VerifiedPassword);
                        _applicationSettingsService.SaveSettings();
                        _bookmarkService.SaveBookmarks();
                    }
                    else
                        chkPasswordProtectBookmarks.Checked = false;
                }
            else
            {
                using (
                    var formgetPassword = new FormGetPassword
                    {
                        PasswordDerivedString = _applicationSettingsService.Settings.PasswordDerivedString
                    })
                {
                    if (formgetPassword.ShowDialog() == DialogResult.OK && formgetPassword.PasswordVerified)
                    {
                        _applicationSettingsService.Settings.DisablePasswordProtectBookmarks();
                        _applicationSettingsService.SaveSettings();

                        // Check bookmark status
                        //if (BookmarkService.Instance.BookmarkManager == null)
                        //    BookmarkService.Instance.Dispose();
                    }
                    else
                        chkPasswordProtectBookmarks.Checked = true;
                }
            }
        }

        private void UpdateCacheStats()
        {
            long cacheSize = _selectedCacheSize;
            long cacheUsage = _imageCacheService.GetCacheUsage();
            const long maxSize = ImageCacheService.MaxCacheSize;
            const long minSize = ImageCacheService.MinCacheSize;

            lblCacheItems.Text = _imageCacheService.CachedItems.ToString();
            lblUsedSpace.Text = GeneralConverters.FormatFileSizeToString(cacheUsage, 2);
            lblFreeSpace.Text = GeneralConverters.FormatFileSizeToString(cacheSize - cacheUsage);
            pbarPercentUsed.Value = Convert.ToInt32((double)cacheUsage / cacheSize * 100);

            trackBarCacheSize.Minimum = Convert.ToInt32(minSize / TrackbarDivider);
            trackBarCacheSize.Maximum = Convert.ToInt32(maxSize / TrackbarDivider);
            trackBarCacheSize.Value = Convert.ToInt32(cacheSize / TrackbarDivider);
            UpdateCacheSizeLabel();
        }

        private void UpdateCacheSizeLabel()
        {
            lblCacheSize.Text = GeneralConverters.FormatFileSizeToString(trackBarCacheSize.Value * TrackbarDivider, 0);
        }

        private void trackBarFadeTime_Scroll(object sender, EventArgs e)
        {
            lblFadeTime.Text = trackBarFadeTime.Value + " ms";
        }

        private void trackBarCacheSize_Scroll(object sender, EventArgs e)
        {
            UpdateCacheSizeLabel();
            _selectedCacheSize = trackBarCacheSize.Value * TrackbarDivider;
        }
    }
}