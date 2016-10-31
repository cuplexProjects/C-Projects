using System;
using System.Drawing;
using GeneralToolkitLib.Log;
using ImageView.Managers;
using ImageView.Utility;

namespace ImageView.Services
{
    public class ThumbnailService
    {
        private readonly string _basePath;
        private readonly ThumbnailManager _thumbnailManager;

        public ThumbnailService(string basePath)
        {
            _basePath = basePath;
            string thumbnailDataStorageFilename = "f:\\temp\\";


            _thumbnailManager = ThumbnailManager.CreateNew(thumbnailDataStorageFilename);
        }

        public void Init()
        {
            _thumbnailManager.StartThumbnailScan("F:\\MB\\2013\\");
            _thumbnailManager.SaveThumbnailDatabase();
        }

        public Image GetThumbnail(string filename)
        {
            try
            {
                return _thumbnailManager.GetThumbnail(filename);
                
            }
            catch (Exception ex)
            {
                LogWriter.LogError("Error in ThumbnailService.GetThumbnail()", ex);
            }
           

            return null;
        }

    }
}
