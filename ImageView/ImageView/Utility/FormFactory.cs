using System.Windows.Forms;
using ImageView.UserControls;

namespace ImageView.Utility
{
    public static class FormFactory
    {
        public static Form CreateSettingsForm(UserControl userControl)
        {
            Form frmSettings = new Form();
            frmSettings.Controls.Add(userControl);
            frmSettings.FormBorderStyle = FormBorderStyle.Fixed3D;
            frmSettings.StartPosition = FormStartPosition.CenterParent;
            frmSettings.ShowInTaskbar = false;
            frmSettings.ShowIcon = false;
            frmSettings.Width = frmSettings.Controls[0].Size.Width + 25;
            frmSettings.Height = frmSettings.Controls[0].Size.Height + 55;

            return frmSettings;
        }

        public static Form CreateModalForm(ThumbnailScanDirectory thumbnailScan)
        {
            Form frmModal = new Form();
            frmModal.Controls.Add(thumbnailScan);
            frmModal.FormBorderStyle = FormBorderStyle.Fixed3D;
            frmModal.StartPosition = FormStartPosition.CenterParent;
            frmModal.ShowInTaskbar = false;
            frmModal.ShowIcon = false;
            frmModal.Width = thumbnailScan.Controls[0].Size.Width + 25;
            frmModal.Height = thumbnailScan.Controls[0].Size.Height + 55;

            return frmModal;
        }
    }
}
