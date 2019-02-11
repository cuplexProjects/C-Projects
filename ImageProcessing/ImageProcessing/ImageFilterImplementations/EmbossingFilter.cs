using ImageProcessing.DataModels;

namespace ImageProcessing.ImageFilterImplementations
{
    public class EmbossingFilter : IImageFilter
    {
        //TODO
        public Pixel Map(ISourceData data)
        {
            return data.GetPixel(0, 0);
        }
    }
}