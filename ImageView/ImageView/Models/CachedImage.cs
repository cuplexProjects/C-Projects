using System;
using System.Drawing;
using System.IO;
using ImageProcessor;
using ImageView.Models.Interface;
using Serilog;
using ImageProcessor.Common;

namespace ImageView.Models
{
    public class CachedImage : IDisposable
    {
        private readonly ImageFactory _imageFactory;
        public CachedImage(string filename, ImageFactory imageFactory)
        {
            CreatedDate = DateTime.Now;
            Filename = filename;
            _imageFactory = imageFactory;
        }

        private byte[] _imageData;

        public string Filename { get; }
        public DateTime CreatedDate { get; private set; }
        public DateTime ModifiedDate { get; private set; }
        public DateTime ImageCreateDate { get; private set; }
        public long Size { get; private set; }
        public Image ImageObject => _imageFactory.Load(_imageData).Image;

        public bool LoadImage()
        {
            
            try
            {
                _imageFactory.Load(Filename);
                MemoryStream ms = new MemoryStream();
                 _imageFactory.Save(ms);

                ms.Flush();
                _imageData = ms.ToArray();
                ms.Close();
                ms.Dispose();


                ModifiedDate = DateTime.Now;

                var fileInfo = new FileInfo(Filename);
                Size = fileInfo.Length;
                ImageCreateDate = fileInfo.CreationTime;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error Loading image");
                return false;
            }
            finally
            {
                
            }

            return ImageObject != null;
        }

        public void Dispose()
        {
            _imageFactory?.Dispose();
        }
    }
}