using System.Collections.Generic;
using System.Drawing;

namespace ImageView.Utility
{
    public class UIHelper
    {
        public static List<Color> GetSelectableBackgroundColors()
        {
            var list = new List<Color>
            {
                SystemColors.ActiveBorder,
                SystemColors.Window,
                SystemColors.ScrollBar,
                SystemColors.MenuText,
                SystemColors.MenuHighlight,
                SystemColors.MenuBar,
                SystemColors.Menu,
                SystemColors.InfoText,
                SystemColors.Info,
                SystemColors.InactiveCaptionText,
                SystemColors.InactiveCaption,
                SystemColors.InactiveBorder,
                SystemColors.HotTrack,
                SystemColors.HighlightText,
                SystemColors.Highlight,
                SystemColors.WindowFrame,
                SystemColors.GrayText,
                SystemColors.GradientActiveCaption,
                SystemColors.Desktop,
                SystemColors.ControlText,
                SystemColors.ControlLightLight,
                SystemColors.ControlLight,
                SystemColors.ControlDarkDark,
                SystemColors.ControlDark,
                SystemColors.Control,
                SystemColors.ButtonShadow,
                SystemColors.ButtonHighlight,
                SystemColors.ButtonFace,
                SystemColors.AppWorkspace,
                SystemColors.ActiveCaptionText,
                SystemColors.ActiveCaption,
                SystemColors.GradientInactiveCaption,
                SystemColors.WindowText,
            };

            return list;
        }
    }
}
