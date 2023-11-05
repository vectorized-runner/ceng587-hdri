using System;
using System.Drawing;
using System.IO;

namespace HDRI
{
    public static class ImageReader
    {
        public static ImageInfo[] GetAllImagesAtPath()
        {
            string folderPath = "path_to_your_image_folder";

            if (!Directory.Exists(folderPath))
            {
                throw new Exception($"Couldn't find the Directory {folderPath}");
            }

            var imageFiles = Directory.GetFiles(folderPath, "*.jpg");

            foreach (var filePath in imageFiles)
            {
                using var bitmap = new Bitmap(filePath);
                var width = bitmap.Width;
                var height = bitmap.Height;
                var pixels = new Color[width * height];

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        var color = bitmap.GetPixel(x, y);
                        var pixelIndex = x + width * y;
                        pixels[pixelIndex] = color;
                    }
                }

                // Process the image using the 'bitmap' object
                Console.WriteLine(
                    $"Read image: {Path.GetFileName(filePath)}, Width: {bitmap.Width}, Height: {bitmap.Height}");
            }
        }
    }
}