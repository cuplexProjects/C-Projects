using System;
using GeneralToolkitLib.Converters;

namespace ImageView.Models
{
    public class ImageInformationModel
    {
        public string ImageDimenstions { get; set; }
        public long Size { get; set; }
        public string SizeFormated => GeneralConverters.FormatFileSizeToString(Size);
        public string FileName { get; set; }
        public string FileDirectory { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
        
    }
}
