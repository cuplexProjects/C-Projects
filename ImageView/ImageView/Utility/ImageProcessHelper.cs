using System.Drawing;
using System.IO;
using ImageProcessor;

namespace ImageViewer.Utility
{
    /// <summary>
    /// ImageProcessHelper
    /// </summary>
    public static class ImageProcessHelper
    {
        /// <summary>
        /// The img factory
        /// </summary>
        private static readonly ImageFactory ImgFactory = new ImageFactory();
        /// <summary>
        /// Creates the thumbnail image.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="size">The size.</param>
        /// <returns></returns>
        public static Image CreateThumbnailImage(string filePath, Size size)
        {
            ImgFactory.Reset();
            ImgFactory.Load(filePath);
            ImgFactory.Resize(size);
            var img = ImgFactory.Image;
            ImgFactory.Reset();

            return img;
        }

        /// <summary>
        /// Gets the image from byte array.
        /// </summary>
        /// <param name="imageBytes">The image bytes.</param>
        /// <returns></returns>
        public static Image GetImageFromByteArray(byte[] imageBytes)
        {
            using (var ms = new MemoryStream(imageBytes))
            {
                return Image.FromStream(ms, true);
            }
        }
    }
}
