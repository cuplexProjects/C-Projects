using System;
using System.Drawing;
using System.Runtime.Serialization;

namespace SecureMemo.DataModels
{
    [Serializable]
    [DataContract(Name = "SecureMemoFontSettings")]
    public class SecureMemoFontSettings
    {
        [DataMember(Name = "FontFamily", Order = 1)]
        public FontFamily FontFamily { get; set; }

        [DataMember(Name = "FontFamilyName", Order = 2)]
        public string FontFamilyName { get; set; }

        [DataMember(Name = "Style", Order = 3)]
        public FontStyle Style { get; set; }

        [DataMember(Name = "FontSize", Order = 4)]
        public float FontSize { get; set; }
    }
}