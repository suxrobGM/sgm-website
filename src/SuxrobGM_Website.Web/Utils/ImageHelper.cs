using System.IO;
using ImageMagick;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace SuxrobGM_Website.Web.Utils
{
    public class ImageHelper
    {
        private readonly IWebHostEnvironment _env;
        public const string DefaultUserAvatarPath = "/img/default_user_avatar.png";
        public const string DefaultBlogCoverPhotoPath = "/img/default_blog_cover.png";

        public ImageHelper(IWebHostEnvironment env)
        {
            _env = env;
        }

        /// <summary>
        /// Upload image file to server
        /// </summary>
        /// <param name="image">Image file from form</param>
        /// <param name="imageFileName">Image file name without extension</param>
        /// <param name="resizeToQuadratic">Flag that indicates resize image to quadratic proportion</param>
        /// <param name="resizeToRectangle">Flag that indicates resize image to rectangle proportion</param>
        /// <returns>Output file path</returns>
        public string UploadImage(IFormFile image, string imageFileName, 
            bool resizeToQuadratic = false, bool resizeToRectangle = false)
        {
            try
            {
                var fileExtension = Path.GetExtension(image.FileName);
                var isAnimatedImage = fileExtension != null && fileExtension.ToLower() == ".gif";
                var imagePath = $"{imageFileName}{fileExtension}";
                var absolutePath = Path.Combine(_env.WebRootPath, "db_files", "img", imagePath);

                if (isAnimatedImage)
                {
                    using var magickAnimatedImage = new MagickImageCollection(image.OpenReadStream());
                    foreach (var imageFrame in magickAnimatedImage)
                    {
                        if (resizeToQuadratic)
                        {
                            ResizeToQuadratic(imageFrame);
                        }

                        if (resizeToRectangle)
                        {
                            ResizeToRectangle(imageFrame);
                        }
                    }

                    magickAnimatedImage.Write(absolutePath, MagickFormat.Gif);
                }
                else
                {
                    using var magickImage = new MagickImage(image.OpenReadStream());

                    if (resizeToQuadratic)
                    {
                        ResizeToQuadratic(magickImage);
                    }

                    if (resizeToRectangle)
                    {
                        ResizeToRectangle(magickImage);
                    }

                    magickImage.Write(absolutePath);
                }

                return $"/db_files/img/{imagePath}";
            }
            catch (MagickException)
            {
                return DefaultUserAvatarPath;
            }
        }

        public void RemoveImage(string imgPath)
        {
            if (imgPath == DefaultUserAvatarPath || imgPath == DefaultBlogCoverPhotoPath)
            {
                return;
            }

            var imgFileName = Path.GetFileName(imgPath);
            var imgFullPath = Path.Combine(_env.WebRootPath, "db_files", "img", imgFileName);

            if (File.Exists(imgFullPath))
            {
                File.Delete(imgFullPath);
            }
        }

        public static void ResizeToQuadratic(IMagickImage<ushort> image, int xySize = 225)
        {
            if (image.Height > xySize || image.Width > xySize)
            {
                image.Resize(xySize, xySize);
                image.Strip();
            }
        }

        public static void ResizeToRectangle(IMagickImage<ushort> image, int width = 850)
        {
            if (image.Width > width)
            {
                var proportion = 1 - ((image.Width - width) / image.Width);
                var resizingHeight = image.Height * proportion;
                image.Resize(width, resizingHeight);
                image.Strip();
            }
        }
    }
}
