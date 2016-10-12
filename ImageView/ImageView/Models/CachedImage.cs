﻿using System;
using System.Drawing;
using System.IO;
using GeneralToolkitLib.Log;

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
            try
            {
                ImageObject = Image.FromFile(Filename);
                ModifiedDate = DateTime.Now;

                var fileInfo = new FileInfo(Filename);
                Size = fileInfo.Length;
                ImageCreateDate = fileInfo.CreationTime;
            }
            catch (Exception ex)
            {
                LogWriter.LogError("Error Loading image", ex);
            }
            return ImageObject != null;
        }
    }
}