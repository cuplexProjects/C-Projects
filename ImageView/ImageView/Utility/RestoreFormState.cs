using System.Drawing;
using System.Windows.Forms;

namespace ImageView.Utility
{
    public static class RestoreFormState
    {
        public static bool SetFormSizeAndPosition(Form form, Size size, Point location, Rectangle screenArea)
        {
            if (size.Height <= 0 || size.Width <= 0)
                return false;

            if (form.MinimumSize.Height > 0 && size.Height < form.MinimumSize.Height)
                return false;

            if (form.MinimumSize.Width > 0 && size.Width < form.MinimumSize.Width)
                return false;

            Rectangle formRect = new Rectangle(location, size);
            if (formRect.Right > screenArea.Right|| formRect.Right < screenArea.Left)
                return false;

            if (formRect.Top > screenArea.Bottom|| formRect.Top < screenArea.Top)
                return false;

            form.Size = size;
            form.Location = location;

            return true;
        }
    }
}
