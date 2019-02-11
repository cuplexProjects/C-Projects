using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Web;
using Encoder = System.Drawing.Imaging.Encoder;

namespace Cuplex.ImageProvider
{
	/// <summary>
	///     Class ImageProcessor.
	/// </summary>
	internal static class ImageProcessor
    {
        private const long JpegQuality = 72L;

        /// <summary>
        ///     Gets the encoder.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <returns></returns>
        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            return ImageCodecInfo.GetImageDecoders().FirstOrDefault(codec => codec.FormatID == format.Guid);
        }

	    /// <summary>
	    ///     Processes the image.
	    /// </summary>
	    /// <param name="targetPath">The target path.</param>
	    /// <param name="originalPath">The original path.</param>
	    /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
	    /// <exception cref="System.Exception">Unknown image format.</exception>
	    internal static bool ProcessImage(string targetPath, string originalPath)
        {
            var maxBredd = 0;
            var maxHojd = 0;
            var directory = Path.GetDirectoryName(targetPath) ?? "";

            #region Parse parameters

            // bildHantera ska innehålla den textsträng som gäller för vad som ska ske med bilden.
            // Exempelvis:   b100h75m
            var bildHantera = targetPath.Substring(targetPath.LastIndexOf("__", StringComparison.Ordinal) + 2);
            bildHantera = bildHantera.Substring(0, bildHantera.LastIndexOf('.'));
            bildHantera = bildHantera.Trim();

            // Tag ut de olika beståndsdelarna från bildHantera-strängen

            // Börja med att ta ut eventuell rubrik
            var rubrik = string.Empty;
            if (bildHantera.Contains("R-"))
            {
                rubrik = bildHantera.Substring(bildHantera.IndexOf("R-", StringComparison.Ordinal) + 2);
                rubrik = rubrik.Substring(0, rubrik.IndexOf("--", StringComparison.Ordinal));
                bildHantera = bildHantera.Replace($"R-{rubrik}--", string.Empty);
            }

            var rotera = string.Empty;
            if (bildHantera.Contains("Rotera("))
            {
                rotera = bildHantera.Substring(bildHantera.IndexOf("Rotera(", StringComparison.Ordinal) + 7);
                rotera = rotera.Substring(0, rotera.IndexOf(")", StringComparison.Ordinal));
                bildHantera = bildHantera.Replace($"Rotera({rotera})", string.Empty);
            }

            var special = string.Empty;
            if (bildHantera.Contains("Special("))
            {
                special = bildHantera.Substring(bildHantera.IndexOf("Special(", StringComparison.Ordinal) + 8);
                special = special.Substring(0, special.IndexOf(")", StringComparison.Ordinal));
                bildHantera = bildHantera.Replace($"Special({special})", string.Empty);
            }

            bildHantera = bildHantera.ToLower();

            const string delimStr = "bhtmufc";
            var delimiter = delimStr.ToCharArray();

            // b100h50
            if (bildHantera.Contains("b"))
            {
                // Tag reda på önskad bredd
                var tmp = bildHantera.Substring(bildHantera.IndexOf("b", StringComparison.Ordinal) + 1);
                if (tmp.IndexOfAny(delimiter) != -1) tmp = tmp.Substring(0, tmp.IndexOfAny(delimiter));

                maxBredd = int.Parse(tmp);
            }

            if (bildHantera.Contains("h"))
            {
                // Tag reda på önskad höjd
                var tmp = bildHantera.Substring(bildHantera.IndexOf("h", StringComparison.Ordinal) + 1);
                if (tmp.IndexOfAny(delimiter) != -1) tmp = tmp.Substring(0, tmp.IndexOfAny(delimiter));

                maxHojd = int.Parse(tmp);
            }

            #endregion

            if (!File.Exists(originalPath))
            {
                #region Create an image with text on it

                if (rubrik.Length > 0)
                {
                    // Om filen inte finns... (Antagligen en rubrik-text kanske?)
                    using (var bitmap = new Bitmap(maxBredd, maxHojd))
                    {
                        rubrik = HttpContext.Current.Server.UrlDecode(rubrik);
                        rubrik = new StringBuilder(rubrik).Replace("_", " ")
                            .Replace("escquestionmark", "?")
                            .Replace("esccolon", ":")
                            .Replace("esclt", "<")
                            .Replace("escgt", ">")
                            .Replace("escstar", "*")
                            .Replace("escplus", "+")
                            .Replace("escunderscore", "_")
                            .Replace("escperiod", ".")
                            .ToString();

                        BildText_HS2(bitmap, rubrik, new Font("Arial", 16, GraphicsUnit.Pixel), new Point(5, 5));

                        if (!string.IsNullOrEmpty(rotera))
                        {
                            var rft = (RotateFlipType) Enum.Parse(typeof(RotateFlipType), rotera, true);
                            bitmap.RotateFlip(rft);
                        }

                        EnsureDirectoryExists(directory);

                        using (var stream = File.Open(targetPath, FileMode.Create, FileAccess.Write, FileShare.None))
                        {
                            SaveJpegWithQuality(bitmap, stream, JpegQuality);
                        }
                    }

                    return true;
                }

                return false;

                #endregion
            }

            #region Resize an existing image

            ImageFormat originalImageFormat;
            var fileExtension = Path.GetExtension(targetPath);
            switch (fileExtension.ToLower())
            {
                case ".gif":
                    originalImageFormat = ImageFormat.Gif;
                    break;

                case ".png":
                    originalImageFormat = ImageFormat.Png;
                    break;

                case ".jpg":
                case ".jpeg":
                    originalImageFormat = ImageFormat.Jpeg;
                    break;

                case ".bmp":
                    originalImageFormat = ImageFormat.Bmp;
                    break;

                default:
                    throw new Exception("Unknown image format.");
            }

            // Originalbilden finns, som ska bildbehandlas
            Image img;
            try
            {
                using (var stream = File.Open(originalPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    img = Image.FromStream(stream);
                }
            }
            catch
            {
                // File not found, etc.
                return false;
            }

            // Tag reda på önskad scalingType för att storleksändra bilden.
            ScalingType scalingType;
            if (bildHantera.Contains("t"))
                scalingType = ScalingType.Force;
            else if (bildHantera.Contains("u"))
                scalingType = ScalingType.CutOut;
            else if (bildHantera.Contains("f")) // Face (Top)
                scalingType = ScalingType.Top;
            else if (bildHantera.Contains("c")) // Center and fill with white
                scalingType = ScalingType.Center;
            else
                scalingType = ScalingType.Max;

            // Specialhantering?
            if (special == "hjrotera") img = RoteraBild(img, 10);

            var resizedImage = img;
            // Storleksförändra bilden enligt önskemål

            if (originalImageFormat.Equals(ImageFormat.Gif))
                resizedImage = ResizeIndexedImage(img, maxBredd, maxHojd, scalingType);
            else if (originalImageFormat.Equals(ImageFormat.Png) ||
                     originalImageFormat.Equals(ImageFormat.Jpeg) ||
                     originalImageFormat.Equals(ImageFormat.Bmp))
                resizedImage = ResizeImage(img, maxBredd, maxHojd, scalingType);

            EnsureDirectoryExists(directory);

            if (rubrik.Length > 0)
            {
                rubrik = HttpContext.Current.Server.UrlDecode(rubrik) ?? rubrik;
                rubrik = rubrik.Replace("_", " ");

                resizedImage = BildText_HS1(img, 30, rubrik, new Font("Arial", 16, GraphicsUnit.Pixel),
                    new Point(5, 5));

                using (var stream = File.Open(targetPath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    SaveJpegWithQuality(resizedImage, stream, JpegQuality);
                }

                return true;
            }

            if (originalImageFormat.Equals(ImageFormat.Gif))
            {
                using (var stream = File.Open(targetPath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    resizedImage.Save(stream, img.RawFormat);
                }

                return true;
            }

            if (originalImageFormat.Equals(ImageFormat.Png) || originalImageFormat.Equals(ImageFormat.Bmp))
            {
                using (var memoryStream = new MemoryStream())
                {
                    resizedImage.Save(memoryStream, ImageFormat.Png);

                    using (var stream = File.Open(targetPath, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        memoryStream.WriteTo(stream);
                    }
                }

                return true;
            }

            if (originalImageFormat.Equals(ImageFormat.Jpeg))
            {
                using (var stream = File.Open(targetPath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    SaveJpegWithQuality(resizedImage, stream, JpegQuality);
                }

                return true;
            }

            #endregion

            return false;
        }

	    /// <summary>
	    ///     Ensures the directory exists.
	    /// </summary>
	    /// <param name="directory">The directory.</param>
	    private static void EnsureDirectoryExists(string directory)
        {
            if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);
        }

        /// <summary>
        ///     Storleksändrar bild med en maxstorlek som påverkar både höjd och bredd beroende på val av ScalingType
        /// </summary>
        /// <param name="image">Bild</param>
        /// <param name="width">Max Storlek Bredd</param>
        /// <param name="height">Max Storlek höjd</param>
        /// <param name="scalingType">Välj vilken scalingType för storleksändring som ska gälla.</param>
        /// <returns>Returnerar bildobjekt</returns>
        private static Image ResizeImage(Image image, float width, float height, ScalingType scalingType)
        {
            // Undersök om bilden redan är mindre än önskat
            if (scalingType == ScalingType.Max &&
                (Math.Abs(width) < float.Epsilon || image.Width <= width) &&
                (Math.Abs(height) < float.Epsilon || image.Height <= height))
                return image;

            // Tag reda på storleksförhållande för bilden
            var aspectRatioBild = (float) image.Width / image.Height;

            // Tag reda på önskat storleksförhållande för önskad bild
            var aspectRatioUt = aspectRatioBild;

            // Anpassa storleksförhållandet om man uteslutit något av höjd/bredd
            if (Math.Abs(width) > float.Epsilon && Math.Abs(height) > float.Epsilon) aspectRatioUt = width / height;

            // Storleksförändra beroende på vald scalingType.
            if (scalingType == ScalingType.Max)
            {
                if (Math.Abs(height) < float.Epsilon) height = width / aspectRatioUt;

                if (Math.Abs(width) < float.Epsilon) width = height * aspectRatioUt;

                if (image.Width > width)
                    if (width / aspectRatioBild < height)
                        height = width / aspectRatioBild;

                if (image.Height > height)
                    if (height * aspectRatioBild < width)
                        width = height * aspectRatioBild;

                var bitmap = new Bitmap((int) width, (int) height);
                bitmap.SetResolution(image.HorizontalResolution, image.VerticalResolution);
                using (var graphics = Graphics.FromImage(bitmap))
                {
                    graphics.CompositingQuality = CompositingQuality.HighQuality;
                    graphics.InterpolationMode = InterpolationMode.HighQualityBilinear;
                    graphics.SmoothingMode = SmoothingMode.HighQuality;
                    graphics.DrawImage(image, 0, 0, bitmap.Width, bitmap.Height);
                }

                return bitmap;
            }

            #region Force

            if (scalingType == ScalingType.Force)
            {
                if (Math.Abs(height) < float.Epsilon) height = width / aspectRatioBild;

                if (Math.Abs(width) < float.Epsilon) width = height * aspectRatioBild;

                var bitmap = new Bitmap((int) width, (int) height);
                using (var graphics = Graphics.FromImage(bitmap))
                {
                    graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.DrawImage(image, new Rectangle(-1, -1, (int) width + 2, (int) height + 2), 0, 0,
                        image.Width, image.Height, GraphicsUnit.Pixel);
                }

                return bitmap;
            }

            #endregion

            if (scalingType == ScalingType.Center)
            {
                // Make sure it works even if no width/height is specified
                if (Math.Abs(height) < float.Epsilon) height = width / aspectRatioUt;

                if (Math.Abs(width) < float.Epsilon) width = height * aspectRatioUt;

                // Create the target image
                var thumbnail = new Bitmap((int) width, (int) height);

                // Make target paint area heigh based on aspect ratio
                if (width / aspectRatioBild < height) height = width / aspectRatioBild;

                // Make target paint area width based on aspect ratio
                if (height * aspectRatioBild < width) width = height * aspectRatioBild;

                using (var graphics = Graphics.FromImage(thumbnail))
                {
                    // Set quality for new image
                    graphics.CompositingQuality = CompositingQuality.HighQuality;
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.SmoothingMode = SmoothingMode.HighQuality;

                    // Fill the destination image with white
                    graphics.Clear(Color.White);

                    var startX = 0;
                    var startY = 0;

                    // Set start patinging for Y
                    if (thumbnail.Width > height * aspectRatioUt)
                        startY = (int) ((thumbnail.Width - height * aspectRatioUt) / 2f);

                    // Set start patinging for X
                    if (thumbnail.Height > width / aspectRatioUt)
                        startX = (int) ((thumbnail.Height - width / aspectRatioUt) / 2f);

                    // Create painting rectangle
                    var destRect = new Rectangle(startX, startY, (int) width, (int) height);

                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel);
                }

                return thumbnail;
            }

            {
                var bitmap = new Bitmap((int) width, (int) height);
                using (var graphics = Graphics.FromImage(bitmap))
                {
                    graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;

                    if ((float) image.Width / image.Height >= width / height)
                    {
                        // Önskad bildutsnitt kräver att vi kapar till höger och vänster
                        // "Liggande"
                        var newWidth = (int) (image.Height * (width / height));
                        var widthDiff = image.Width - newWidth;

                        graphics.DrawImage(image, new Rectangle(-1, -1, (int) width + 2, (int) height + 2),
                            widthDiff / 2, 0, newWidth, image.Height, GraphicsUnit.Pixel);
                    }
                    else if (scalingType == ScalingType.Top)
                    {
                        // Önskad bildutsnitt kräver att vi kapar i botten (vi vill ha toppen)
                        // Porträtt-bild, stående
                        var newHeight = (int) (image.Width * (height / width));

                        graphics.DrawImage(image, new Rectangle(-1, -1, (int) width + 2, (int) height + 2), 0, 0,
                            image.Width, newHeight, GraphicsUnit.Pixel);
                    }
                    else
                    {
                        // Önskad bildutsnitt kräver att vi kapar i toppen och botten
                        // Stående
                        // Porträtt-bild, stående
                        var newHeight = (int) (image.Width * (height / width));
                        var heightDiff = image.Height - newHeight;

                        graphics.DrawImage(image, new Rectangle(-1, -1, (int) width + 2, (int) height + 2), 0,
                            heightDiff / 2, image.Width, newHeight, GraphicsUnit.Pixel);
                    }
                }

                return bitmap;
            }
        }

        /// <summary>
        ///     Storleksändrar bild med en maxstorlek som påverkar både höjd och bredd beroende på val av ScalingType
        /// </summary>
        /// <param name="originalImage">The original image.</param>
        /// <param name="maxWidth">Width of the max.</param>
        /// <param name="maxHeight">Height of the max.</param>
        /// <param name="scalingType">Välj vilken scalingType för storleksändring som ska gälla.</param>
        /// <returns>
        ///     Returnerar bildobjekt
        /// </returns>
        private static Image ResizeIndexedImage(Image originalImage, float maxWidth, float maxHeight, ScalingType scalingType)
        {
            Bitmap sourceBitmap;
            Bitmap targetBitmap;
            BitmapData sourceData, targetData;

            // Convert the image to an 8bpp image
            using (var stream = new MemoryStream())
            {
                originalImage.Save(stream, ImageFormat.Gif);
                stream.Position = 0;
                sourceBitmap = new Bitmap(stream);
            }

            // Undersök om bilden redan är mindre än önskat
            if (scalingType == ScalingType.Max &&
                (Math.Abs(maxWidth) < float.Epsilon || originalImage.Width <= maxWidth) &&
                (Math.Abs(maxHeight) < float.Epsilon || originalImage.Height <= maxHeight))
                return originalImage;

            // Tag reda på storleksförhållande för bilden
            var aspectRatioBild = (float) originalImage.Width / originalImage.Height;

            // Tag reda på önskat storleksförhållande för önskad bild
            var aspectRatioUt = aspectRatioBild;

            // Anpassa storleksförhållandet om man uteslutit något av höjd/bredd
            if (Math.Abs(maxWidth) > float.Epsilon && Math.Abs(maxHeight) > float.Epsilon)
                aspectRatioUt = maxWidth / maxHeight;

            // Storleksförändra beroande på vald scalingType.
            if (scalingType == ScalingType.Max || scalingType == ScalingType.Force)
            {
                if (scalingType == ScalingType.Max)
                {
                    if (Math.Abs(maxHeight) < float.Epsilon) maxHeight = maxWidth / aspectRatioUt;

                    if (Math.Abs(maxWidth) < float.Epsilon) maxWidth = maxHeight * aspectRatioUt;

                    if (originalImage.Width > maxWidth)
                        if (maxWidth / aspectRatioBild < maxHeight)
                            maxHeight = maxWidth / aspectRatioBild;

                    if (originalImage.Height > maxHeight)
                        if (maxHeight * aspectRatioBild < maxWidth)
                            maxWidth = maxHeight * aspectRatioBild;
                }
                else // scalingType == ScalingType.Force
                {
                    if (Math.Abs(maxHeight) < float.Epsilon) maxHeight = maxWidth / aspectRatioBild;

                    if (Math.Abs(maxWidth) < float.Epsilon) maxWidth = maxHeight * aspectRatioBild;
                }

                targetBitmap =
                    new Bitmap((int) maxWidth, (int) maxHeight, sourceBitmap.PixelFormat)
                    {
                        Palette = sourceBitmap.Palette
                    };

                sourceData = sourceBitmap.LockBits(new Rectangle(0, 0, sourceBitmap.Width, sourceBitmap.Height),
                    ImageLockMode.ReadOnly, sourceBitmap.PixelFormat);
                targetData = targetBitmap.LockBits(new Rectangle(0, 0, (int) maxWidth, (int) maxHeight),
                    ImageLockMode.WriteOnly, targetBitmap.PixelFormat);

                var xFactor = (double) sourceBitmap.Width / targetBitmap.Width;
                var yFactor = (double) sourceBitmap.Height / targetBitmap.Height;

                for (var y = 0; y < targetBitmap.Height; ++y)
                for (var x = 0; x < targetBitmap.Width; ++x)
                {
                    var yPosition = (int) Math.Floor(y * yFactor);
                    var xPosition = (int) Math.Floor(x * xFactor);
                    var origByte = Marshal.ReadByte(sourceData.Scan0, sourceData.Stride * yPosition + xPosition);
                    Marshal.WriteByte(targetData.Scan0, targetData.Stride * y + x, origByte);
                }

                targetBitmap.UnlockBits(targetData);
                sourceBitmap.UnlockBits(sourceData);
            }
            else //( scalingType == ScalingType.CutOut )  // Eller Top
            {
                int sourceX, sourceY;
                int sourceWidth, sourceHeight;

                if ((float) originalImage.Width / originalImage.Height >= maxWidth / maxHeight)
                {
                    // Önskad bildutsnitt kräver att vi kapar till höger och vänster
                    // "Liggande"
                    var newWidth = (int) (originalImage.Height * (maxWidth / maxHeight));
                    var widthDiff = originalImage.Width - newWidth;

                    sourceX = widthDiff / 2;
                    sourceY = 0;
                    sourceWidth = newWidth;
                    sourceHeight = originalImage.Height;
                }
                else
                {
                    // Önskad bildutsnitt kräver att vi kapar i toppen och botten
                    // "Stående"
                    // Porträtt-bild, stående
                    var newHeight = (int) (originalImage.Width * (maxHeight / maxWidth));
                    var heightDiff = originalImage.Height - newHeight;

                    sourceX = 0;
                    sourceY = heightDiff / 2;

                    // Om vi vill ha top, ta toppen på bilden, istället för "centrerad" yta
                    if (scalingType == ScalingType.Top) sourceY = 0;
                    //else
                    //{
                    //    // Ta centrerad yta
                    //    sourceY = (int)(heightDiff / 2);

                    //    // ...men om den dynamiska egenskapen "ImageHandlerUseTopWhenCroping" är satt, använd toppen ändå.
                    //    PageBase CurrentPage = HttpContext.Current.Handler as PageBase;
                    //    if (CurrentPage != null)
                    //    {
                    //        if (CurrentPage.IsValue("ImageHandlerUseTopWhenCroping"))
                    //            sourceY = 0;
                    //    }
                    //}

                    sourceWidth = originalImage.Width;
                    sourceHeight = newHeight;
                }

                targetBitmap =
                    new Bitmap((int) maxWidth, (int) maxHeight, sourceBitmap.PixelFormat)
                    {
                        Palette = sourceBitmap.Palette
                    };

                sourceData = sourceBitmap.LockBits(new Rectangle(0, 0, sourceBitmap.Width, sourceBitmap.Height),
                    ImageLockMode.ReadOnly, sourceBitmap.PixelFormat);
                targetData = targetBitmap.LockBits(new Rectangle(0, 0, targetBitmap.Width, targetBitmap.Height),
                    ImageLockMode.WriteOnly, targetBitmap.PixelFormat);

                var xFactor = (double) sourceWidth / targetBitmap.Width;
                var yFactor = (double) sourceHeight / targetBitmap.Height;

                for (var y = 0; y < maxHeight; ++y)
                for (var x = 0; x < maxWidth; ++x)
                {
                    var yPosition = (int) Math.Floor(y * yFactor) + sourceY;
                    var xPosition = (int) Math.Floor(x * xFactor) + sourceX;
                    var origByte = Marshal.ReadByte(sourceData.Scan0, sourceData.Stride * yPosition + xPosition);
                    Marshal.WriteByte(targetData.Scan0, targetData.Stride * y + x, origByte);
                }

                targetBitmap.UnlockBits(targetData);
                sourceBitmap.UnlockBits(sourceData);
            }

            return targetBitmap;
        }

        /// <summary>
        ///     Saves the JPEG with quality.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="stream">The stream.</param>
        /// <param name="quality">The quality.</param>
        private static void SaveJpegWithQuality(Image image, Stream stream, long quality)
        {
            var encoderParameters = new EncoderParameters(1);
            encoderParameters.Param[0] = new EncoderParameter(Encoder.Quality, quality);

            var encoder = GetEncoder(ImageFormat.Jpeg);
            image.Save(stream, encoder, encoderParameters);
        }

        #region Hennes

        /// <summary>
        ///     Vrider bilden valfritt antal grader
        ///     Denna bör bara användas om man vill rotera fritt.
        ///     Om man vill rotera 90, 180, eller 270 grader så är det RotateFlip man ska använda istället.
        /// </summary>
        private static Image RoteraBild(Image image, float grader)
        {
            const int justBredd = 5;
            const int justHöjd = 5;

            var bitmap = new Bitmap(image.Width + justBredd, image.Height + justHöjd);
            using (var graphics = Graphics.FromImage(bitmap))
            {
                //grp.RotateTransform(grader);
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                //grp.PixelOffsetMode = PixelOffsetMode.Half;

                graphics.Clear(Color.White);

                var pen = new Pen(Color.Gray);

                var matrix = new Matrix();
                //matrix.Scale(  (float)Math.Cos(grader), (float)Math.cos(grader) );

                matrix.Scale(0.8f, 0.8f);
                matrix.Translate(20, 20); //, MatrixOrder.Append);

                var roteringsPunkt = new PointF(image.Width / 2f, image.Height / 2f);
                matrix.RotateAt(grader, roteringsPunkt);

                //matrix.Scale( 1, 1 );

                graphics.Transform = matrix;

                //grp.DrawRectangle(pen, justBredd, justHöjd, img.Width, img.Height);

                // Ful-dropshadow
                var rectangle = new Rectangle(justBredd, justHöjd, image.Width, image.Height);
                graphics.DrawRectangle(pen, rectangle);

                var brush = new LinearGradientBrush(rectangle, Color.Silver, Color.Gray, 45f);
                var region = new Region(rectangle);
                graphics.FillRegion(brush, region);

                graphics.DrawImage(image, 0, 0);
                graphics.ResetTransform();
            }

            return bitmap;
        }

        /// <summary>
        ///     Vrider bilden till höger
        ///     Lägger på en svart marginal i ovankant (till vänster alltså)
        ///     Lägger på text i ovankant, i den svarta marginalen
        ///     Vrider tillbaka bilden
        /// </summary>
        /// <param name="image">The img.</param>
        /// <param name="extrabreddning">The extrabreddning.</param>
        /// <param name="text">The text.</param>
        /// <param name="font">The FNT.</param>
        /// <param name="point">The point.</param>
        /// <returns></returns>
        private static Image BildText_HS1(Image image, int extrabreddning, string text, Font font, Point point)
        {
            var bitmap = new Bitmap(image.Width + extrabreddning, image.Height);
            using (var graphics = Graphics.FromImage(bitmap))
            {
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.Clear(Color.Black);
                graphics.DrawImage(image, new Rectangle(extrabreddning, 0, image.Width, image.Height), 0, 0,
                    image.Width, image.Height, GraphicsUnit.Pixel);

                var img2 = bitmap;
                img2.RotateFlip(RotateFlipType.Rotate90FlipNone);

                // txtWidth innehåller storleken (boundary) för texten som ska skrivas
                var txtWidth = graphics.MeasureString(text, font);

                var x = bitmap.Width - (int) txtWidth.Width - point.Y;
                var y = point.X;
                var skrivPoint = new Point(x, y);

                using (var grp2 = Graphics.FromImage(img2))
                {
                    grp2.DrawString(text, font, Brushes.White, skrivPoint);
                    grp2.DrawImageUnscaled(bitmap, 0, 0);
                }
            }

            bitmap.RotateFlip(RotateFlipType.Rotate270FlipNone);

            return bitmap;
        }

	    /// <summary>
	    ///     Vrider bilden till höger
	    ///     Lägger på en svart marginal i ovankant (till vänster alltså)
	    ///     Lägger på text i ovankant, i den svarta marginalen
	    ///     Vrider tillbaka bilden
	    /// </summary>
	    /// <param name="image">The image.</param>
	    /// <param name="text">The text.</param>
	    /// <param name="font">The font.</param>
	    /// <param name="point">The point.</param>
	    /// <returns>Image.</returns>
	    private static void BildText_HS2(Image image, string text, Font font, Point point)
        {
            using (var graphics = Graphics.FromImage(image))
            {
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
                graphics.Clear(Color.Black);

                //bmp.RotateFlip(RotateFlipType.Rotate90FlipNone);
                var txtWidth = graphics.MeasureString(text, font);

                var x = image.Width - (int) txtWidth.Width - point.Y;
                var y = point.X;
                var skrivPoint = new Point(x, y);

                graphics.DrawString(text, font, Brushes.White, skrivPoint);
                //grp.DrawImageUnscaled(bmp, 0, 0);
            }

            //bmp.RotateFlip(RotateFlipType.Rotate270FlipNone);
        }

        #endregion
    }
}