using System;
using System.Drawing;
using System.IO;
using Anotar.Serilog;
using ImageProcessor;

namespace ImageView.Models
{
    public class RawImageModel
    {
        private readonly byte[] _imageBytes;

        public RawImageModel(byte[] imageBytes)
        {
            _imageBytes = imageBytes;
        }

        public byte[] ImageData
        {
            get { return _imageBytes; }
        }

        public Image LoadImage()
        {
            Image image = null;
            try
            {
                var imgFactory = new ImageFactory();
                imgFactory.Load(_imageBytes);
                image = imgFactory.Image;
            }
            catch (Exception exception)
            {
                LogTo.Error(exception,"LoadImageException {Message}",exception.Message);
            }

            return image;
        }

        public static RawImageModel CreateFromImage(Image img, Size size)
        {
            var imgFactory = new ImageFactory();
            imgFactory.Load(img);
            imgFactory.Resize(size);

            var ms = new MemoryStream();
            imgFactory.Save(ms);
            ms.Flush();
            return new RawImageModel(ms.ToArray());
        }

        public static RawImageModel CreateFromImage(Image img)
        {
            var imgFactory = new ImageFactory();
            imgFactory.Load(img);

            var ms = new MemoryStream();
            imgFactory.Save(ms);
            ms.Flush();
            return new RawImageModel(ms.ToArray());
        }
    }
}
