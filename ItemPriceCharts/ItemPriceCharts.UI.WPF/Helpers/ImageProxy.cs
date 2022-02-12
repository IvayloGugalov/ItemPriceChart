using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace ItemPriceCharts.UI.WPF.Helpers
{
    public class ImageProxy : IImageProxy
    {
        public Bitmap ResizeImage(string sourceFilePath, int newWidth, int newHeight, bool enableBackgroundFill = false)
        {
            var image = Image.FromFile(sourceFilePath);
            int sizeWidth, sizeHeight;

            //Scaling
            var imageSize = new Size(image.Width, image.Height);

            if (enableBackgroundFill)
            {
                // Resize the image and don't change the aspect ratio
                if (imageSize.Width > newHeight || imageSize.Width > newWidth)
                {
                    if ((imageSize.Width * newHeight) > (imageSize.Height * newWidth))
                    {
                        sizeWidth = newWidth;
                        sizeHeight = (newWidth * imageSize.Height) / imageSize.Width;
                    }
                    else
                    {
                        sizeHeight = newHeight;
                        sizeWidth = (imageSize.Width * newHeight) / imageSize.Height;
                    }
                }
                else
                {
                    sizeWidth = imageSize.Width;
                    sizeHeight = imageSize.Height;
                }
            }
            else
            {
                sizeWidth = newWidth;
                sizeHeight = newHeight;
            }
            
            var bitmapImage = new Bitmap(newWidth, newHeight);
            var graphics = Graphics.FromImage(bitmapImage);

            var srcRect = new Rectangle((newWidth - sizeWidth) / 2, (newHeight - sizeHeight) / 2, sizeWidth, sizeHeight);
            var destRect = new Rectangle(0, 0, image.Width, image.Height);

            graphics.Clear(Color.White);
            graphics.CompositingQuality = CompositingQuality.HighQuality;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.DrawImage(image, destRect, srcRect, GraphicsUnit.Pixel);

            graphics.Dispose();
            image.Dispose();

            return bitmapImage;
        }

        public void SaveImageFile(string destinationFilePath, Bitmap bitmapImage, int compressionLevel = 100)
        {
            if (compressionLevel is < 1 or > 100)
            {
                throw new ArgumentOutOfRangeException($"Can't set compression level to {compressionLevel}. Compression level must be between 1 and 100.");
            }

            var encoderParams = new EncoderParameters();

            //Set the compression ratio 1-100
            var eParam = new EncoderParameter(Encoder.Quality, new long[] {compressionLevel});
            encoderParams.Param[0] = eParam;

            var codecInfos = ImageCodecInfo.GetImageEncoders();
            ImageCodecInfo imageCodecInfo = null;
            foreach (var info in codecInfos)
            {
                if (info.FormatDescription is "JPEG")
                {
                    imageCodecInfo = info;
                    break;
                }
            }

            //The following code sets the compression quality when saving pictures
            if (imageCodecInfo != null)
            {
                bitmapImage.Save(destinationFilePath, imageCodecInfo, encoderParams);
            }
            else
            {
                bitmapImage.Save(destinationFilePath, bitmapImage.RawFormat);
            }
        }
    }

    public interface IImageProxy
    {
        /// <summary>
        /// Resizes an image to the specified width and height
        /// </summary>
        /// <param name="sourceFilePath"></param>
        /// <param name="newHeight"></param>
        /// <param name="newWidth"></param>
        /// <param name="enableBackgroundFill">Resizes to the specified size and retains the image aspect ratio.</param>
        /// <returns>Resized Bitmap</returns>
        Bitmap ResizeImage(string sourceFilePath, int newWidth, int newHeight, bool enableBackgroundFill = false);

        /// <summary>
        /// Saves an image to the specified path.
        /// Image can be compressed during the save operation.
        /// </summary>
        /// <param name="destinationFilePath"></param>
        /// <param name="bitmapImage"></param>
        /// <param name="compressionLevel">Compression level: 0 - lowest quality; 100 - highest quality</param>
        void SaveImageFile(string destinationFilePath, Bitmap bitmapImage, int compressionLevel = 100);
    }
}
