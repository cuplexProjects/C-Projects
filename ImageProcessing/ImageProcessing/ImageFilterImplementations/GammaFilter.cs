using System;
using ImageProcessing.DataModels;

namespace ImageProcessing.ImageFilterImplementations
{
    // Nonlinear operation used to encode luminance or tristimulus values in pixel data
    public class GammaFilter : IImageFilter
    {
        private readonly double _gammaCorrection;

        public GammaFilter(double amount)
        {
            _gammaCorrection = amount;
        }

        public Pixel Map(ISourceData data)
        {
            Pixel p = data.GetPixel(0, 0);
            int r = PixelUtils.Clamp((int)((255.0 * Math.Pow(p.R / 255.0, 1.0 / _gammaCorrection)) + 0.5), 255, 0);
            int g = PixelUtils.Clamp((int)((255.0 * Math.Pow(p.G / 255.0, 1.0 / _gammaCorrection)) + 0.5), 255, 0);
            int b = PixelUtils.Clamp((int)((255.0 * Math.Pow(p.B / 255.0, 1.0 / _gammaCorrection)) + 0.5), 255, 0);
            int a = PixelUtils.Clamp((int)((255.0 * Math.Pow(p.A / 255.0, 1.0 / _gammaCorrection)) + 0.5), 255, 0);
            
            return new Pixel(r, g, b, a);
        }

    }
}