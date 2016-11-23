using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace GeneralToolkitLib.UserControls
{
    public class SelectionBar : Panel
    {
        private readonly MouseAction _mouseAction;
        private int _selectionEnd;
        private int _selectionStart;

        public SelectionBar()
        {
            MinValue = 0;
            MaxValue = 100;
            SelectionEnd = 100;

            SetStyle(ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.DoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            UpdateStyles();
            _mouseAction = new MouseAction();
        }

        [Browsable(true)]
        [Category("Behavior")]
        [Description("The timestamp of the latest entry.")]
        public int MinValue { get; protected set; }

        [Browsable(true)]
        [Category("Behavior")]
        [Description("The timestamp of the latest entry.")]
        [DefaultValue(100)]
        public int MaxValue { get; protected set; }

        [Browsable(true)]
        [Category("Behavior")]
        [Description("The timestamp of the latest entry.")]
        public int SelectionStart
        {
            get { return _selectionStart; }
            set
            {
                if (value >= MinValue && value <= MaxValue && value < SelectionEnd)
                {
                    _selectionStart = value;
                    SelectionChanged?.Invoke(this, new EventArgs());
                    Invalidate();
                }
            }
        }

        [Browsable(true)]
        [Category("Behavior")]
        [Description("The timestamp of the latest entry.")]
        [DefaultValue(100)]
        public int SelectionEnd
        {
            get { return _selectionEnd; }
            set
            {
                if (value >= MinValue && value <= MaxValue && value > SelectionStart)
                {
                    _selectionEnd = value;
                    SelectionChanged?.Invoke(this, new EventArgs());
                    Invalidate();
                }
            }
        }

        private bool IsPointOverSelectionStart(Point point)
        {
            double delta1 = (double)point.X / ClientSize.Width * 100;
            double delta2 = (double)SelectionStart / MaxValue * 100;

            return delta1 < delta2 + 2 && delta1 > delta2 - 2;
        }

        private bool IsPointOverSelectionEnd(Point point)
        {
            double delta1 = (double)point.X / ClientSize.Width * 100;
            double delta2 = (double)SelectionEnd / MaxValue * 100;

            return delta1 < delta2 + 2 && delta1 > delta2 - 2;
        }

        private bool IsPointWithinBar(Point point)
        {
            double delta1 = (double)point.X / ClientSize.Width;
            double delta2 = (double)SelectionStart / MaxValue;
            double delta3 = (double)SelectionEnd / MaxValue;

            return delta1 >= delta2 && delta1 <= delta3;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                _mouseAction.MouseDown(e.Location);

            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _mouseAction.MouseUp(e.Location);
            }

            base.OnMouseUp(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            bool pointOverResizeLeft = IsPointOverSelectionStart(e.Location);
            bool pointOverResizeRight = IsPointOverSelectionEnd(e.Location);
            bool mouseOverResize = pointOverResizeLeft || pointOverResizeRight;

            //Set cursor
            if (!_mouseAction.MouseButtonDown)
            {
                Cursor.Current = mouseOverResize ? Cursors.SizeWE : DefaultCursor;
                _mouseAction.Resizing = mouseOverResize;
            }
            else if (Enabled)
            {
                if (_mouseAction.BarState == BarState.None)
                {
                    if (_mouseAction.Resizing)
                    {
                        if (pointOverResizeLeft)
                            _mouseAction.BarState = BarState.ResizingLeft;
                        else if (pointOverResizeRight)
                            _mouseAction.BarState = BarState.ResizingRight;
                    }
                    else if (IsPointWithinBar(e.Location))
                        _mouseAction.BarState = BarState.Moving;
                }

                double delta;
                if ((_mouseAction.BarState | BarState.Resizing) == BarState.Resizing)
                {
                    delta = (double)e.Location.X / ClientSize.Width;
                    ResizeBar(delta);
                }
                else if (_mouseAction.BarState == BarState.Moving)
                {
                    delta = (double)(e.Location.X - _mouseAction.MousePosition.X) / ClientSize.Width * MaxValue;
                    MoveBar(delta);
                    _mouseAction.SetMousePosition(e.Location);
                }
            }

            base.OnMouseMove(e);
        }

        private void ResizeBar(double delta)
        {
            if (_mouseAction.BarState == BarState.ResizingLeft)
            {
                SelectionStart = Convert.ToInt32(MaxValue * delta);
                if (SelectionStart < 1 - MinValue)
                    SelectionStart = MinValue;
                Invalidate();
            }
            else if (_mouseAction.BarState == BarState.ResizingRight)
            {
                SelectionEnd = Convert.ToInt32(MaxValue * delta);
                if (SelectionEnd > MaxValue - 1)
                    SelectionEnd = MaxValue;
                Invalidate();
            }
        }

        private void MoveBar(double delta)
        {
            int selectionLength = SelectionEnd - SelectionStart;
            int minSelectionStart = MinValue;
            int maxSelectionStart = MaxValue - selectionLength;
            int delta_i = Convert.ToInt32(delta);

            _mouseAction.DeltaRemainder += delta - delta_i;

            if (_mouseAction.DeltaRemainder > 1)
            {
                _mouseAction.DeltaRemainder--;
                delta_i++;
            }
            else if (_mouseAction.DeltaRemainder < -1)
            {
                _mouseAction.DeltaRemainder++;
                delta_i--;
            }

            int newSelectionStart = SelectionStart + delta_i;

            if (newSelectionStart < minSelectionStart)
                newSelectionStart = minSelectionStart;

            if (newSelectionStart > maxSelectionStart)
                newSelectionStart = maxSelectionStart;

            if (SelectionStart != newSelectionStart)
            {
                SelectionStart = newSelectionStart;
                SelectionEnd = SelectionStart + selectionLength;
                Invalidate();
            }
        }

        protected override void OnResize(EventArgs eventargs)
        {
            Invalidate();
            base.OnResize(eventargs);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (e.ClipRectangle.Width < ClientRectangle.Width || e.ClipRectangle.Height < ClientRectangle.Height)
            {
                Invalidate();
            }

            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBilinear;

            //Draw frame
            Rectangle drawRect = e.ClipRectangle;
            Brush b = new SolidBrush(ForeColor);
            var p = new Pen(b, 2f);
            g.DrawRectangle(p, drawRect);

            // Draw background
            Color c = Color.MediumSeaGreen;
            drawRect.Inflate(new Size(-2, -2));
            b = new HatchBrush(HatchStyle.LargeGrid, c);
            g.FillRectangle(b, drawRect);

            // Draw bar
            c = Color.LawnGreen;

            int x = drawRect.Left;
            int length = drawRect.Right - drawRect.Left;
            int selectionLength = Convert.ToInt32(length * (double)(SelectionEnd - SelectionStart) / (MaxValue - MinValue));
            x += Convert.ToInt32((double)SelectionStart / MaxValue * length);

            //Draw Inner Frame
            p = new Pen(Color.Black, 1);
            g.DrawRectangle(p, drawRect);
            drawRect.Inflate(new Size(-1, -1));

            //Fill Inner
            b = new SolidBrush(c);
            drawRect = new Rectangle(x, drawRect.Y, selectionLength, drawRect.Height);
            g.FillRectangle(b, drawRect);


            // Blend fill inner
            g.CompositingMode = CompositingMode.SourceCopy;
            b = new HatchBrush(HatchStyle.DarkUpwardDiagonal, c);
            g.FillRectangle(b, drawRect);

            base.OnPaint(e);
        }

        public event EventHandler SelectionChanged;
    }

    public class MouseAction
    {
        public Point MousePosition { get; private set; }
        public bool MouseButtonDown { get; private set; }
        public bool Resizing { get; set; }
        public BarState BarState { get; set; }
        public double DeltaRemainder { get; set; }

        public void MouseDown(Point mousePosition)
        {
            MouseButtonDown = true;
            MousePosition = mousePosition;
        }

        public void MouseUp(Point mousePosition)
        {
            MousePosition = mousePosition;
            MouseButtonDown = false;
            BarState = BarState.None;
        }

        public void SetMousePosition(Point mousePosition)
        {
            MousePosition = mousePosition;
        }
    }

    [Flags]
    public enum BarState
    {
        None = 1,
        ResizingLeft = 2,
        ResizingRight = 4,
        Resizing = 6,
        Moving = 8
    }
}
