using ImageProcessing.DataModels;

namespace ImageProcessing.ImageFilterImplementations
{
    public class BlurFilter : IImageFilter
    {
        private readonly int _amount;

        /// <summary>
        ///     factor from 1 to 100
        /// </summary>
        /// <param name="factor"></param>
        public BlurFilter(double factor)
        {
            _amount = (int) factor;
            if (factor <= 0)
                _amount = 1;
            if (factor > 10)
                _amount = 10;
        }

        public Pixel Map(ISourceData data)
        {
            Pixel p0 = data.GetPixel(0, 0);
            Pixel p1 = data.GetPixel(-1 * _amount, -1 * _amount);
            Pixel p2 = data.GetPixel(1 * _amount, 1 * _amount);
            Pixel p3 = data.GetPixel(-1 * _amount, 1 * _amount);
            Pixel p4 = data.GetPixel(1 * _amount, -1 * _amount);

            int r = ((p1.R + p2.R + p3.R + p4.R + p0.R)) / 5;
            int g = ((p1.G + p2.G + p3.G + p4.G + p0.G)) / 5;
            int b = ((p1.B + p2.B + p3.B + p4.B + p0.B)) / 5;
            return new Pixel(r, g, b, 255);
        }
    }
}