using ImageProcessing.DataModels;

namespace ImageProcessing.ImageFilterImplementations
{
    public class SharpenFilter : IImageFilter
    {
        private readonly double _amount;

        private readonly int[,] filterMatrix = new int[,]
        {
            {-1, -1, -1,},
            {-1, 9, -1,},
            {-1, -1, -1,},
        };

        private double bias = 0.5;

        /// <summary>
        ///     amount 0.0 to 10.0
        /// </summary>
        /// <param name="amount"></param>
        public SharpenFilter(double amount)
        {
            _amount = amount;
        }

        public Pixel Map(ISourceData data)
        {
            double factor = _amount;
            Pixel[,] pixelMatrix = PixelUtils.GetPixelMatrix(data, 1);

            if (factor > 10)
                factor = 10;
            if (factor <= 0)
                factor = 0.1d;

            bias = 2.0;
            int red = 0;
            int green = 0;
            int blue = 0;

            int dimLength = pixelMatrix.GetLength(0);

            for (int x = 0; x < dimLength; x++)
            {
                for (int y = 0; y < dimLength; y++)
                {
                    red += pixelMatrix[x, y].R*filterMatrix[x, y];
                    green += pixelMatrix[x, y].G*filterMatrix[x, y];
                    blue += pixelMatrix[x, y].B*filterMatrix[x, y];
                }
            }

            int r = PixelUtils.Clamp((int) ((factor*red) + bias), 255, 0);
            int g = PixelUtils.Clamp((int) ((factor*green) + bias), 255, 0);
            int b = PixelUtils.Clamp((int) ((factor*blue) + bias), 255, 0);

            return new Pixel(r, g, b, pixelMatrix[1, 1].A);
        }
    }
}