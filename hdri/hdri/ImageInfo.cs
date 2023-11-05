using System.Drawing;

namespace HDRI
{
    public readonly struct ImageInfo
    {
        public readonly Bitmap Image;
        public readonly float ExposureTime;

        public ImageInfo(Bitmap image, float exposureTime)
        {
            Image = image;
            ExposureTime = exposureTime;
        }

        public int GetPixelCount()
        {
            return Image.Width * Image.Height;
        }

        public Color GetPixel(int index)
        {
            var width = Image.Width;
            var x = index % width;
            var y = index / width;
            return Image.GetPixel(x, y);
        }

        public void Dispose()
        {
            Image.Dispose();
        }
    }
}