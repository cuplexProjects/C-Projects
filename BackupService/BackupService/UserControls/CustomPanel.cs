using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace BackupService.UserControls
{
    [DesignerCategory("Code")]
    public class CustomPanel : Panel
    {
        public CustomPanel()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.DoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
        }

        [DispId(-505)]
        [Browsable(true)]
        [DefaultValue(typeof (Color), "0xCCCCCC")]
        public Color BorderColor { get; set; }

        [DispId(-505)]
        [DefaultValue(typeof (Int32), "0x1")]
        [Browsable(true)]
        public Int32 BorderWidth { get; set; }

        protected override void OnPaint(PaintEventArgs e)
        {
            using (var brush = new SolidBrush(BackColor))
                e.Graphics.FillRectangle(brush, ClientRectangle);
            e.Graphics.DrawRectangle(new Pen(BorderColor), 0, 0, ClientSize.Width - 1, ClientSize.Height - 1);
        }
    }
}