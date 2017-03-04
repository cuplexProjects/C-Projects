using System.Windows.Forms;

namespace SecureMemo.Forms
{
    public static class FormFactory
    {
        public static Form CreateFormFromUserControl(UserControl userControl)
        {
            Form form = new Form();
            form.Controls.Add(userControl);
            form.StartPosition = FormStartPosition.CenterParent;
            form.Width = userControl.Width+25;
            form.Height = userControl.Height+25;
            form.MaximizeBox = false;
            form.MinimizeBox = false;
            form.ShowIcon = false;
            form.ShowInTaskbar = false;
            form.FormBorderStyle = FormBorderStyle.FixedSingle;

            return form;
        }
    }
}
