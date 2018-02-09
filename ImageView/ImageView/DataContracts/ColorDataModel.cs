using System;
using System.Drawing;
using System.Runtime.Serialization;

namespace ImageView.DataContracts
{
    [Serializable]
    [DataContract(Name = "ColorDataModel")]
    public class ColorDataModel
    {
        [DataMember(Name = "R", Order = 1)]
        public byte R { get; set; }

        [DataMember(Name = "G", Order = 2)]
        public byte G { get; set; }

        [DataMember(Name = "B", Order = 3)]
        public byte B { get; set; }

        [DataMember(Name = "A", Order = 4)]
        public byte A { get; set; }

        public static ColorDataModel CreateFromColor(Color color)
        {
            return new ColorDataModel
            {
                R=color.R,
                G= color.G,
                B= color.B,
                A = color.A
            };
        }

        public static Color CreateFromColorDataModel(ColorDataModel model)
        {
            return Color.FromArgb(model.A, model.R, model.G, model.B);
        }

        public Color ToColor()
        {
            return CreateFromColorDataModel(this);
        }
    }
}