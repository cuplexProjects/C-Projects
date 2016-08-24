using System;
using System.Windows.Forms;
using ImageView.DataContracts;
using ImageView.InputForms;
using ImageView.Properties;
using ImageView.Services;

namespace ImageView
{
    public partial class FormSettings : Form
    {
        public FormSettings()
        {
            InitializeComponent();
        }

        private void FormSettings_Load(object sender, EventArgs e)
        {
            ImageViewApplicationSettings settings = ApplicationSettingsService.Instance.Settings;
            chkAutoRandomize.Checked = settings.AutoRandomizeCollection;
            chkPasswordProtectBookmarks.Checked = settings.PasswordProtectBookmarks;
            chkShowSwitchImgButtons.Checked = settings.ShowSwitchImageButtons;
            chkEnableAutoload.Checked = settings.EnableAutoLoadFunctionFromMenu;

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

            UpdateCacheStats();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            ImageViewApplicationSettings settings = ApplicationSettingsService.Instance.Settings;
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

            ApplicationSettingsService.Instance.Settings.ShowSwitchImageButtons = chkShowSwitchImgButtons.Checked;
            ApplicationSettingsService.Instance.Settings.EnableAutoLoadFunctionFromMenu = chkEnableAutoload.Checked;
            ApplicationSettingsService.Instance.Settings.ShowNextPrevControlsOnEnterWindow = rbOverWindow.Checked;

            if (rbImgTransformFadeIn.Checked)
                settings.NextImageAnimation = ImageViewApplicationSettings.ChangeImageAnimation.FadeIn;

            settings.ImageTransitionTime = trackBarFadeTime.Value;
            settings.ScreenMinXOffset = Convert.ToInt32(numericScreenMinOffset.Value);
            settings.ScreenWidthOffset = Convert.ToInt32(numericScreenWidthOffset.Value);

            ApplicationSettingsService.Instance.Settings.ImageCacheSize = trackBarCacheSize.Value*1048576;
            ImageCacheService.Instance.CacheSize = ApplicationSettingsService.Instance.Settings.ImageCacheSize;

            ApplicationSettingsService.Instance.SaveSettings();
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

                        ApplicationSettingsService.Instance.Settings.EnablePasswordProtectBookmarks(formSetPassword.VerifiedPassword);
                        ApplicationSettingsService.Instance.SaveSettings();
                        BookmarkService.Instance.SaveBookmarkFile();
                    }
                    else
                        chkPasswordProtectBookmarks.Checked = false;
                }
            else
            {
                using (var formgetPassword = new FormGetPassword {PasswordDerivedString = ApplicationSettingsService.Instance.Settings.PasswordDerivedString})
                {
                    if (formgetPassword.ShowDialog() == DialogResult.OK && formgetPassword.PasswordVerified)
                    {
                        ApplicationSettingsService.Instance.Settings.DisablePasswordProtectBookmarks();
                        ApplicationSettingsService.Instance.SaveSettings();

                        // Check bookmark status
                        if (BookmarkService.Instance.BookmarksContainer == null)
                            BookmarkService.Instance.Dispose();
                    }
                    else
                        chkPasswordProtectBookmarks.Checked = true;
                }
            }
        }

        private void UpdateCacheStats()
        {
            int cacheSize = ImageCacheService.Instance.CacheSize/1048576;
            long cacheUsage = ImageCacheService.Instance.GetCacheUsage() / 1048576;
            const int maxSize = ImageCacheService.MaxCacheSize/1048576;
            const int minSize = ImageCacheService.MinCacheSize/1048576;

            lblCacheItems.Text = ImageCacheService.Instance.CachedItems.ToString();
            lblUsedSpace.Text = cacheUsage + Resources.FormSettings_UpdateCacheSizeLabel__MB;
            lblFreeSpace.Text = cacheSize - cacheUsage + Resources.FormSettings_UpdateCacheSizeLabel__MB;
            pbarPercentUsed.Value = Convert.ToInt32(cacheUsage / maxSize)*100;

            trackBarCacheSize.Minimum = minSize;
            trackBarCacheSize.Maximum = maxSize;
            trackBarCacheSize.Value = cacheSize;
            UpdateCacheSizeLabel();
        }

        private void UpdateCacheSizeLabel()
        {
            lblCacheSize.Text = trackBarCacheSize.Value + Resources.FormSettings_UpdateCacheSizeLabel__MB;
        }

        private void trackBarFadeTime_Scroll(object sender, EventArgs e)
        {
            lblFadeTime.Text = trackBarFadeTime.Value + " ms";
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            UpdateCacheSizeLabel();
        }
    }
}