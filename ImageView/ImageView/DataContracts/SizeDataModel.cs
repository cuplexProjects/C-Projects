using System;
using System.Drawing;
using System.Runtime.Serialization;

namespace ImageView.DataContracts
{
    [Serializable]
    [DataContract]
    public class SizeDataModel
    {
        [DataMember(Name = "Width", Order = 1)]
        public int Width { get; set; }

        [DataMember(Name = "Height", Order = 2)]
        public int Height { get; set; }

        public SizeDataModel()
        {

        }

        public SizeDataModel(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public Size ToSize()
        {
            return new Size(Width, Height);
        }

        public static SizeDataModel CreateFromSize(Size size)
        {
            return new SizeDataModel(size.Width, size.Height);
        }
    }
}
