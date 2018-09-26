using System;
using System.Drawing;
using System.IO;
using ImageView.Models.Interface;
using Serilog;

namespace ImageView.Models
{
    public class CachedImage
    {
        public CachedImage(string filename)
        {
            CreatedDate = DateTime.Now;
            Filename = filename;
        }

        public string Filename { get; }
        public DateTime CreatedDate { get; private set; }
        public DateTime ModifiedDate { get; private set; }
        public DateTime ImageCreateDate { get; private set; }
        public long Size { get; private set; }
        public Image ImageObject { get; private set; }

        public bool LoadImage()
        {
            Stream fileStream = null;
            try
            {
                fileStream = File.OpenRead(Filename);

                ImageObject =  Image.FromStream(fileStream);
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
                if (fileStream != null)
                {
                    fileStream.Close();
                    fileStream.Dispose();
                }
            }

            return ImageObject != null;
        }
    }
}