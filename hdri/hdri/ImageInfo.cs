using System.Drawing;

namespace HDRI
{
    public readonly struct ImageInfo
    {
        public readonly Color[] Pixels;
        public readonly float ExposureTime;

        public ImageInfo(Color[] pixels, float exposureTime)
        {
            Pixels = pixels;
            ExposureTime = exposureTime;
        }
    }
}