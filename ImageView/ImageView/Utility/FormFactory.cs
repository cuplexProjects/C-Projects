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
            frmSettings.Width = frmSettings.Controls[0].Size.Width + 25;
            frmSettings.Height = frmSettings.Controls[0].Size.Height + 55;

            return frmSettings;
        }
    }
}
