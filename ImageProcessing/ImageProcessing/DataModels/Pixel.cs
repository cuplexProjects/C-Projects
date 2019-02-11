using System;
using System.Drawing;

namespace ImageProcessing.DataModels
{
    public struct Pixel
    {
        private readonly int a;
        private readonly int b;
        private readonly int g;
        private readonly int r;

        public int R
        {
            get { return r; }
        }

        public int G
        {
            get { return g; }
        }

        public int B
        {
            get { return b; }
        }

        public int A
        {
            get { return a; }
        }

        public Pixel(int r, int g, int b, int a)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a;
        }

        public Pixel(int r, int g, int b)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            a = 255;
        }

        public Color ToColor()
        {
            return ToColor(A);
        }

        public Color ToColor(int alpha)
        {
            int red = r;
            int green = g;
            int blue = b;
            ScaleToColorRange(ref red, ref green, ref blue);
            return Color.FromArgb(alpha, red, green, blue);
        }

        public static Pixel operator +(Pixel p1, Pixel p2)
        {
            return new Pixel(p1.r + p2.r, p1.g + p2.g, p1.b + p2.b);
        }

        public static Pixel operator -(Pixel p1, Pixel p2)
        {
            return new Pixel(p1.r - p2.r, p1.g - p2.g, p1.b - p2.b);
        }

        public static Pixel operator *(double scale, Pixel p)
        {
            return new Pixel((int) (scale * p.r), (int) (scale * p.g), (int) (scale * p.b), p.a);
        }

        public static Pixel operator *(Pixel p, double scale)
        {
            return scale * p;
        }

        public static Pixel GetMeanValuePixel(params Pixel[] pixels)
        {
            int r = 255;
            int g = 255;
            int b = 255;
            int a = 128;

            int amount = pixels.Length + 1;
            foreach (Pixel pixel in pixels)
            {
                r += pixel.R;
                g += pixel.G;
                b += pixel.B;
                a += pixel.A;
            }
            r = r / amount;
            g = g / amount;
            b = b / amount;
            a = a / amount;
            return new Pixel(r, g, b, a);
        }

        private static void ScaleToColorRange(ref int r, ref int g, ref int b)
        {
            int min = Math.Min(r, Math.Min(g, b));
            if (min < 0)
            {
                r -= min;
                g -= min;
                b -= min;
            }
            double max = Math.Max(r, Math.Max(g, b));
            if (max > 255)
            {
                r = (int) (r / max * 255);
                g = (int) (g / max * 255);
                b = (int) (b / max * 255);
            }
        }
    }
}