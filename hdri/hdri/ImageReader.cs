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

            var imageCount = imageFiles.Length;
            var result = new ImageInfo[imageCount];

            for (var index = 0; index < imageFiles.Length; index++)
            {
                var filePath = imageFiles[index];
                result[index] = new ImageInfo(new Bitmap(filePath), 0.0f);
            }

            return result;
        }
    }
}