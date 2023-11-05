using System;
using System.Drawing;
using System.IO;
using ExifLib;

namespace HDRI
{
    public static class ImageReader
    {
        public static ImageInfo[] GetAllImagesAtPath(string relativeFolderName)
        {
            var exePath = AppDomain.CurrentDomain.BaseDirectory;
            var absolutePath = Path.Combine(exePath, relativeFolderName);
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
                var bitmap = new Bitmap(filePath);
                var exposureTime = ReadExposureTimeFromJPG(filePath);
                result[index] = new ImageInfo(bitmap, exposureTime);
            }

            return result;
        }

        // ReSharper disable once InconsistentNaming
        private static float ReadExposureTimeFromJPG(string path)
        {
            using var reader = new ExifReader(path);
            if (reader.GetTagValue(ExifTags.ExposureTime, out double exposureTime))
            {
                return (float)exposureTime;
            }

            throw new Exception($"Couldn't read exposure time from image: '{path}'");
        }
    }
}