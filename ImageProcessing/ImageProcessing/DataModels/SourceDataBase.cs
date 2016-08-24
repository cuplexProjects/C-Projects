using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace ImageProcessing.DataModels
{
    public class SourceDataBase : ISourceData
    {
        private readonly EdgeHandling _edgeHandling;
        private readonly Size _imageSize;
        private readonly int _stride;
        private readonly object _threadLockObj = new object();
        private readonly byte[] _imageDataBytes;
        
        public int NumberOfPixels { get; private set; }

        public SourceDataBase(Bitmap source, EdgeHandling edgeHandling)
        {
            _imageSize = source.Size;
            _edgeHandling = edgeHandling;
            _imageDataBytes = GetImageBytes(source, out _stride);
            ImageWidth = source.Width;
            ImageHeight = source.Height;
            NumberOfPixels = source.Height * source.Width;
        }

        public int ImageWidth { get; private set; }
        public int ImageHeight { get; private set; }

        public Pixel GetPixel(int x, int y)
        {
            Pixel p;

            SetSafeCoordinates(ref x, ref y);

            int index = y * _stride + 4 * x;
            lock (_threadLockObj)
            {
                int b = _imageDataBytes[index];
                int g = _imageDataBytes[index + 1];
                int r = _imageDataBytes[index + 2];
                int a = _imageDataBytes[index + 3];
                p = new Pixel(r, g, b, a);
            }
            return p;
        }

        public void SetPixel(Pixel p, int x, int y)
        {
            SetSafeCoordinates(ref x, ref y);

            int stride = _imageSize.Width * 4;
            int index = y * stride + 4 * x;
            lock (_threadLockObj)
            {
                _imageDataBytes[index] = (byte) p.B;
                _imageDataBytes[index + 1] = (byte) p.G;
                _imageDataBytes[index + 2] = (byte) p.R;
                _imageDataBytes[index + 3] = (byte) p.A;
            }
        }

        private void SetSafeCoordinates(ref int x, ref int y)
        {
            switch (_edgeHandling)
            {
                case EdgeHandling.Wrap:
                    if (x >= _imageSize.Width)
                        x = Math.Max(x - _imageSize.Width, _imageSize.Width - 1);
                    else if (x < 0)
                        x = Math.Min(_imageSize.Width + x, 0);

                    if (y >= _imageSize.Height)
                        y = _imageSize.Height - 1;
                    else if (y < 0)
                        y = 0;

                    break;
                case EdgeHandling.Extend:
                    if (x >= _imageSize.Width)
                        x = _imageSize.Width - 1;
                    else if (x < 0)
                        x = 0;

                    if (y >= _imageSize.Height)
                        y = _imageSize.Height - 1;
                    else if (y < 0)
                        y = 0;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public Bitmap GetOutputBitmap()
        {
            return ToBitmap(_imageSize.Width, _imageSize.Height, _stride, PixelFormat.Format32bppArgb, _imageDataBytes);
        }

        private static byte[] GetImageBytes(Bitmap source, out int stride)
        {
            var bounds = new Rectangle(0, 0, source.Width, source.Height);
            BitmapData data = source.LockBits(bounds, ImageLockMode.ReadOnly, source.PixelFormat);
            stride = Math.Abs(data.Stride);
            int byteCount = stride * data.Height;
            var bytes = new byte[byteCount];
            Marshal.Copy(data.Scan0, bytes, 0, byteCount);
            source.UnlockBits(data);
            return bytes;
        }

        private static Bitmap ToBitmap(int width, int height, int stride, PixelFormat pixelFormat, byte[] bytes)
        {
            return new Bitmap(width, height, stride, pixelFormat, Marshal.UnsafeAddrOfPinnedArrayElement(bytes, 0));
        }
    }
}