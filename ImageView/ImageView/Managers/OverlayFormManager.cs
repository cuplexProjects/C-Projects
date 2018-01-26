using System;
using System.Drawing;
using System.Windows.Forms;
using ImageView.UserControls;
using ImageView.Utility;

namespace ImageView.Managers
{
    public class OverlayFormManager : IDisposable
    {
        private readonly Form _formOverlayImage;
        private readonly BookmarkPreviewOverlayUserControl _overlayUserControl;

        public bool IsEnabled { get; set; }
        public int ActiveRow { get; set; }


        public OverlayFormManager()
        {
            _overlayUserControl = new BookmarkPreviewOverlayUserControl();
            _formOverlayImage = FormFactory.CreateFloatingForm(_overlayUserControl, new Size(250, 250));
            _overlayUserControl.Dock = DockStyle.Fill;
        }

        public void LoadImageAndDisplayForm(string imagePath, Point mousePoint)
        {
            //Maximize display area
            var screenBounds = Screen.PrimaryScreen.Bounds;
            _overlayUserControl.LoadImage(imagePath);
            var imageSize = _overlayUserControl.GetImageSize();

            int maxWidth = Math.Min(imageSize.Width, Convert.ToInt32(screenBounds.Width / 1.3d));
            int maxHeight = Math.Min(imageSize.Height, Convert.ToInt32(screenBounds.Height / 1.1d));

            Rectangle formRectangle = new Rectangle(0, 0, maxWidth, maxHeight);

            if (mousePoint.X > screenBounds.Width / 2)
            {
                formRectangle.Width = Math.Max(maxWidth - mousePoint.X, maxWidth);
                formRectangle.X = mousePoint.X -formRectangle.Width-10;

            }
            else
            {
                formRectangle.Width = Math.Max(maxWidth - mousePoint.X, maxWidth);
                formRectangle.X = mousePoint.X + 10;
            }

            //Center form on the y axis
            formRectangle.Y = screenBounds.Height / 2 - Math.Min(maxHeight, imageSize.Height) / 2;

            _formOverlayImage.Left = formRectangle.X;
            _formOverlayImage.Top = formRectangle.Y;
            _formOverlayImage.Width = formRectangle.Width;
            _formOverlayImage.Height = formRectangle.Height;

            _formOverlayImage.Show();
        }

        public void HideForm()
        {
            _formOverlayImage.Hide();
        }

        public void Dispose()
        {
            _formOverlayImage?.Dispose();
            _overlayUserControl?.Dispose();
        }
    }
}
