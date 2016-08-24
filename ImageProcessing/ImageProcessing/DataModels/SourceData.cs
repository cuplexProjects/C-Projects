using System;

namespace ImageProcessing.DataModels
{
    // Source data for filter usage
    public class SourceData : ISourceData
    {
        private readonly SourceDataBase _sourceDataBase;
        public int Offset_X { get; private set; }
        public int Offset_Y { get; private set; }

        public SourceData(SourceDataBase sourceDataBase)
        {
            _sourceDataBase = sourceDataBase;
            ImageWidth = _sourceDataBase.ImageWidth;
            ImageHeight = _sourceDataBase.ImageHeight;
        }

        public Pixel GetPixel(int x, int y)
        {
            return _sourceDataBase.GetPixel(x + Offset_X, y + Offset_Y);
        }

        public void SetPixel(Pixel p, int x, int y)
        {
            _sourceDataBase.SetPixel(p, x, y);
        }

        public int ImageWidth { get; private set; }
        public int ImageHeight { get; private set; }

        public void SetPixelIndex(int index)
        {
            Offset_X = index % ImageWidth;
            Offset_Y = (int) Math.Floor((double) (index) / ImageWidth);
        }
    }
}