using System.Windows.Forms;

namespace SecureMemo.Utility
{
    public static class ControlHelper
    {
        public static Control GetChildControlByName(Control parent, string name)
        {
            Control userControl = null;
            foreach (Control control in parent.Controls)
            {
                if (control.Name == name)
                {
                    userControl = control;
                    break;
                }
                userControl = GetChildControlByName(control, name);
            }

            return userControl;
        }
    }
}