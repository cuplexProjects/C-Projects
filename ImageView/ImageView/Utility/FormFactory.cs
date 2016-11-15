using System.Windows.Forms;

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
            frmSettings.MaximizeBox = false;
            frmSettings.Width = frmSettings.Controls[0].Size.Width + 25;
            frmSettings.Height = frmSettings.Controls[0].Size.Height + 55;

            return frmSettings;
        }

        public static Form CreateModalForm(UserControl userControl)
        {
            Form frmModal = new Form();
            frmModal.Controls.Add(userControl);
            frmModal.FormBorderStyle = FormBorderStyle.Fixed3D;
            frmModal.StartPosition = FormStartPosition.CenterParent;
            frmModal.ShowInTaskbar = false;
            frmModal.ShowIcon = false;
            frmModal.MaximizeBox = false;
            frmModal.Width = userControl.Controls[0].Size.Width + 25;
            frmModal.Height = userControl.Controls[0].Size.Height + 55;

            return frmModal;
        }
    }
}
