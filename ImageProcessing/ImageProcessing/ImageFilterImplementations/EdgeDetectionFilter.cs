using ImageProcessing.DataModels;

namespace ImageProcessing.ImageFilterImplementations
{
    public class EdgeDetectionFilter : IImageFilter
    {
        public Pixel Map(ISourceData data)
        {
            return data.GetPixel(0, 0)
                   - data.GetPixel(1, 0)
                   - data.GetPixel(-1, 0)
                   - data.GetPixel(0, 1)
                   - (data.GetPixel(0, -1) * (1/64d));
        }
    }
}