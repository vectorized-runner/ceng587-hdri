using System;
using System.Drawing;
using System.IO;

namespace HDRI
{
    public static class ImageReader
    {
        public static ImageInfo[] GetAllImagesAtPath(string relativeFolderName)
        {
            var projectPath = AppDomain.CurrentDomain.BaseDirectory;
            var absolutePath = Path.Combine(projectPath, relativeFolderName);
            Debug.Log($"AbsolutePath for checking images: '{absolutePath}'");

            if (!Directory.Exists(absolutePath))
            {
                throw new Exception($"Couldn't find the Directory {absolutePath}");
            }

            var imageFiles = Directory.GetFiles(absolutePath, "*.jpg");
            Debug.Log($"Found '{imageFiles.Length}' images: '{imageFiles}'");

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

            return new ImageInfo[0];
        }
    }
}