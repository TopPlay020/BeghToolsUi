using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using WebpWrapper;

namespace BeghToolsUi.Services
{
    public class ImageConversionService : ISingletonable
    {
        static public List<string> SupportedFormats = ["png", "jpg", "jpeg", "bmp", "gif", "ico", "webp"];
        public void ConvertImage(string imagePath, string outputPath, string outputFormat, Color backgroundColor = default)
        {
            Bitmap image;
            if (IsWebP(imagePath))
            {
                using (var webp = new WebP())
                    image = webp.Load(imagePath);
            }
            else
            {
                image = new Bitmap(imagePath);
            }

            if (outputFormat.Equals("ico", StringComparison.OrdinalIgnoreCase))
                SaveIconFile(outputPath, image);
            else if (outputFormat.Equals("webp", StringComparison.OrdinalIgnoreCase))
            {
                using (var webp = new WebP())
                {
                    webp.Save(image, outputPath, 75); // Quality set to 75
                }
            }
            else if (outputFormat.Equals("jpg", StringComparison.OrdinalIgnoreCase) || outputFormat.Equals("jpeg", StringComparison.OrdinalIgnoreCase))
            {
                using (Bitmap bmp = new Bitmap(image.Width, image.Height, PixelFormat.Format24bppRgb))
                {
                    using (Graphics g = Graphics.FromImage(bmp))
                    {
                        if (backgroundColor == default)
                            backgroundColor = Color.White;
                        g.Clear(backgroundColor);
                        g.DrawImage(image, 0, 0, image.Width, image.Height);
                    }
                    bmp.Save(outputPath, ImageFormat.Jpeg);
                }
            }
            else
            {
                image.Save(outputPath, GetImageFormatFromString(outputFormat));
            }
            image.Dispose();
        }

        bool IsWebP(string imagePath)
        {
            byte[] header = new byte[4];
            using (var fs = new FileStream(imagePath, FileMode.Open, FileAccess.Read))
            {
                fs.ReadExactly(header, 0, 4);
            }
            // WebP files start with "RIFF" and "WEBP" in bytes 8-11
            return header[0] == 'R' && header[1] == 'I' && header[2] == 'F' && header[3] == 'F' &&
                   File.ReadAllBytes(imagePath)[8] == 'W' && File.ReadAllBytes(imagePath)[9] == 'E' &&
                   File.ReadAllBytes(imagePath)[10] == 'B' && File.ReadAllBytes(imagePath)[11] == 'P';
        }

        private static void SaveIconFile(string outputPath, Bitmap image)
        {
            Size[] iconSizes = {
            new Size(16, 16),
            new Size(32, 32),
            new Size(48, 48),
            new Size(64, 64),
            new Size(256, 256)};

            using (FileStream stream = new FileStream(outputPath, FileMode.Create))
            using (BinaryWriter writer = new BinaryWriter(stream))
            {
                // Write ICO header
                writer.Write((short)0); // Reserved (must be 0)
                writer.Write((short)1); // Type (1 for ICO)
                writer.Write((short)iconSizes.Length); // Number of images

                // Write image entries
                int offset = 6 + (16 * iconSizes.Length); // Header size + (16 bytes per image entry)
                foreach (var size in iconSizes)
                {
                    using (Bitmap resizedImage = new Bitmap(image, size))
                    using (MemoryStream pngStream = new MemoryStream())
                    {
                        // Save the resized image as a PNG
                        resizedImage.Save(pngStream, ImageFormat.Png);

                        // Write image entry
                        writer.Write((byte)size.Width); // Width
                        writer.Write((byte)size.Height); // Height
                        writer.Write((byte)0); // Color palette (0 for no palette)
                        writer.Write((byte)0); // Reserved (must be 0)
                        writer.Write((short)1); // Color planes (1 for RGB)
                        writer.Write((short)32); // Bits per pixel (32 for ARGB)
                        writer.Write((int)pngStream.Length); // Size of the image data
                        writer.Write(offset); // Offset to the image data

                        // Update the offset for the next image
                        offset += (int)pngStream.Length;
                    }
                }

                // Write image data
                foreach (var size in iconSizes)
                {
                    using (Bitmap resizedImage = new Bitmap(image, size))
                    using (MemoryStream pngStream = new MemoryStream())
                    {
                        // Save the resized image as a PNG
                        resizedImage.Save(pngStream, ImageFormat.Png);

                        // Write the PNG data to the ICO file
                        writer.Write(pngStream.ToArray());
                    }
                }
            }
        }

        private static ImageFormat GetImageFormatFromString(string format)
        {
            switch (format.ToLower())
            {
                case "png":
                    return ImageFormat.Png;
                case "gif":
                    return ImageFormat.Gif;
                case "bmp":
                    return ImageFormat.Bmp;
                case "ico":
                    return ImageFormat.Icon;
                default:
                    return ImageFormat.Png; // default
            }
        }

        public static ImageFormat GetImageFormat(string path)
        {
            try
            {
                using (var img = Image.FromFile(path))
                {
                    return img.RawFormat;
                }
            }
            catch
            {
                return null;
            }
        }

    }
}
