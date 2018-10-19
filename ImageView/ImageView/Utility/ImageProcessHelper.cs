using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageProcessor;

namespace ImageView.Utility
{
    public static class ImageProcessHelper
    {
        public static Image CreateThumbnailImage(string filePath, Size size)
        {
            ImageFactory imgFactory = new ImageFactory();
            imgFactory.Load(filePath);
            imgFactory.Resize(size);

            return imgFactory.Image;
        }

    }
}
