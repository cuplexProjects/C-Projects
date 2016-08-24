namespace ImageProcessing.DataModels
{
    public interface IImageFilter
    {
        Pixel Map(ISourceData data);
    }
}