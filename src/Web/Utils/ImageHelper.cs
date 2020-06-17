using System.IO;
using ImageMagick;

namespace SuxrobGM_Website.Utils
{
    public static class ImageHelper
    {
        public static void ResizeToQuadratic(Stream imageStream, string outputFile, int XY_size = 225)
        {
            using var image = new MagickImage(imageStream);
            if (image.Height > XY_size || image.Width > XY_size)
            {
                image.Resize(XY_size, XY_size);
                image.Strip();
            }

            image.Write(outputFile);
        }

        public static void ResizeToRectangle(Stream imageStream, string outputFile, int width = 850)
        {
            using var image = new MagickImage(imageStream);
            if (image.Width > width)
            {
                var proportion = 1 - (image.Width - width) / image.Width;
                var resizingHeight = image.Height * proportion;
                image.Resize(width, resizingHeight);
                image.Strip();
            }

            image.Write(outputFile);
        }

        public static void SaveImage(Stream imageStream, string outputFile)
        {
            using var image = new MagickImage(imageStream);
            image.Write(outputFile);
        }

        public static void DeleteImage(string imagePath)
        {
            if (File.Exists(imagePath))
            {
                File.Delete(imagePath);
            }
        }
    }
}
