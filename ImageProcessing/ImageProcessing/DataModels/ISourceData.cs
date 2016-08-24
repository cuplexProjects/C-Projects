namespace ImageProcessing.DataModels
{
    public interface ISourceData
    {
        int ImageWidth { get; }
        int ImageHeight { get; }
        Pixel GetPixel(int x, int y);
        void SetPixel(Pixel p, int x, int y);
    }
}