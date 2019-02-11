using System;

namespace ImageProcessing.DataModels
{
    public enum EdgeHandling
    {
        Wrap,
        Extend
    }

    public static class PixelUtils
    {
        public static int Clamp(int Value, int Max, int Min)
        {
            Value = Value > Max ? Max : Value;
            Value = Value < Min ? Min : Value;
            return Value;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="scale">The number of surounding pixels. Input 1 yeals a 3x3 matrix</param>
        /// <returns></returns>
        public static Pixel[,] GetPixelMatrix(ISourceData data, int scale)
        {
            if (scale < 1)
                scale = 1;

            int dimLength = scale * 3;
            Pixel[,] matrix = new Pixel[dimLength, dimLength];

            for (int r = 0; r < dimLength; r++)
            {
                int y = r-1;
                for (int i = 0; i < dimLength; i++)
                {
                    int x = i - 1;
                    matrix[r, i] = data.GetPixel(x, y);
                }
            }

            //matrix[0, 0] = data.GetPixel(-1, -1);
            //matrix[0, 1] = data.GetPixel(0, -1);
            //matrix[0, 2] = data.GetPixel(1, -1);

            //matrix[1, 0] = data.GetPixel(-1, 0);
            //matrix[1, 1] = data.GetPixel(0, 0);
            //matrix[1, 2] = data.GetPixel(1, 0);

            //matrix[2, 0] = data.GetPixel(-1, 1);
            //matrix[2, 1] = data.GetPixel(0, 1);
            //matrix[2, 2] = data.GetPixel(1, 1);


            return matrix;
        }

        public static Pixel GetMeanValuePixelFromMatrix( Pixel[,] pixels)
        {
            int r=0;
            int g=0;
            int b=0;
            int a=0;

            int amount = 0;
            int rank = pixels.Rank + 1;
            for (int i = 0; i < rank; i++)
            {
                for (int j = 0; j < rank; j++)
                {
                    int sum = pixels[i, j].R + pixels[i, j].G + pixels[i, j].B + pixels[i, j].A;

                    if (sum <= 0) continue;
                    r += pixels[i, j].R;
                    g += pixels[i, j].G;
                    b += pixels[i, j].B;
                    a += pixels[i, j].A;
                    amount++;
                }
            }

            return amount == 0 ? new Pixel(0, 0, 0, 0) : new Pixel(r / amount, g / amount, b / amount, a / amount);
        }
    }
}