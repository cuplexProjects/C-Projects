using ImageProcessing.DataModels;

namespace ImageProcessing.ImageFilterImplementations
{
    public class GaussianBlurFilter : IImageFilter
    {
        private readonly double _pixels;

        /// <summary>
        ///     pixels from 0.01 to 1000
        /// </summary>
        /// <param name="pixels"></param>
        public GaussianBlurFilter(double pixels)
        {
            _pixels = pixels;
        }

        public Pixel Map(ISourceData data)
        {
            //TODO implement Gaussian filter
            return data.GetPixel(0, 0);
        }
    }
}