using ImageProcessing.DataModels;

namespace ImageProcessing.ImageFilterImplementations
{
    public class MosaicFilter : IImageFilter
    {
        private readonly int amount = 25;

        public Pixel Map(ISourceData data)
        {
            return Pixel.GetMeanValuePixel(
                data.GetPixel(0 * amount, -1 * amount),
                data.GetPixel(1 * amount, -1 * amount),
                data.GetPixel(1 * amount, 0 * amount),
                data.GetPixel(1 * amount, 1 * amount),
                data.GetPixel(0 * amount, 1 * amount),
                data.GetPixel(-1 * amount, -1 * amount),
                data.GetPixel(-1 * amount, 0 * amount),
                data.GetPixel(-1 * amount, 1 * amount));
        }
    }
}