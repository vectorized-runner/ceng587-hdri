using System;
using System.Drawing;
using MathNet.Numerics.LinearAlgebra;

namespace HDRI
{
    // N -> sampled pixel count
    // P -> photograph count
    // O -> objective function
    // w(z) -> weighting function
    // g(z) -> inverse response function 
    // Z(i, j) -> pixel value at ij
    // i -> pixel index 
    // j -> photo index
    // lambda -> smoothness factor
    // t(j) -> exposure time
    // E(i) -> irradiance
    // X -> exposure
    // ReSharper disable once InconsistentNaming
    public static class HDRIGenerator
    {
        public const int MinPixelValue = 0;
        public const int MaxPixelValue = 255;
        public const float SmoothnessFactor = 100.0f;

        private static int[] GetSampleIndices(ref Random random, int pixelCount, int sampleCount)
        {
            var result = new int[sampleCount];

            for (int i = 0; i < sampleCount; i++)
            {
                result[i] = random.Next(0, pixelCount);
            }

            return result;
        }

        private static int GetRequiredSampleCount(int textureCount)
        {
            // In the Paper 50 pixels for (255 - 0) / (11 - 1), so I can multiply by 2
            return 2 * ((MaxPixelValue - MinPixelValue) / (textureCount - 1));
        }

        private static int WeightFunction(int pixelValue)
        {
            var threshold = (MaxPixelValue + MinPixelValue) / 2;
            if (pixelValue <= threshold)
            {
                // Z - ZMin
                return pixelValue;
            }

            // ZMax - Z
            return MaxPixelValue - pixelValue;
        }

        private static int GetChannel(Color color, Channel channel)
        {
            return channel switch
            {
                Channel.Red => color.R,
                Channel.Green => color.G,
                Channel.Blue => color.B,
                _ => throw new ArgumentOutOfRangeException(nameof(channel), channel, null)
            };
        }

        private static int[,] GetSamples(ImageInfo[] images, int[] sampleIndices, Channel channel)
        {
            var textureCount = images.GetLength(0);
            var sampleCount = sampleIndices.Length;
            var samples = new int[textureCount, sampleCount];

            for (int imageIndex = 0; imageIndex < textureCount; imageIndex++)
            {
                for (var sampleIndex = 0; sampleIndex < sampleIndices.Length; sampleIndex++)
                {
                    var sample = sampleIndices[sampleIndex];
                    var color = images[imageIndex].GetPixel(sample);
                    var pixel = GetChannel(color, channel);
                    samples[imageIndex, sampleIndex] = pixel;
                }
            }

            return samples;
        }

        public static float[] SolveDebevec(Random random, ImageInfo[] images)
        {
            var pixelCount = images[0].GetPixelCount();
            var imageCount = images.Length;
            var sampleCount = GetRequiredSampleCount(imageCount);
            var sampleIndices = GetSampleIndices(ref random, pixelCount, sampleCount);
            var sampledPixels = GetSamples(images, sampleIndices, Channel.Green);
            var aRowCount = sampleCount * imageCount + 255;
            var aColumnCount = 256 + sampleCount;
            var k = 0;
            var A = Matrix<float>.Build.DenseOfArray(new float[aRowCount, aColumnCount]);
            var b = Vector<float>.Build.Dense(new float[aRowCount]);

            // Data Fitting
            for (int imageIndex = 0; imageIndex < imageCount; imageIndex++)
            for (int sampleIndex = 0; sampleIndex < sampleCount; sampleIndex++)
            {
                var pixel = sampledPixels[imageIndex, sampleIndex];
                var weightedPixel = WeightFunction(pixel);
                A[k, pixel] = weightedPixel;
                A[k, sampleIndex + 256] = -weightedPixel;
                var exposure = images[imageIndex].ExposureTime;
                b[k] = weightedPixel * exposure;
                k++;
            }

            A[k, (MinPixelValue + MaxPixelValue) / 2] = 1;
            k++;

            for (int i = MinPixelValue + 1; i < MaxPixelValue; i++)
            {
                var weight = WeightFunction(i);
                A[k, i - 1] = weight * SmoothnessFactor;
                A[k, i] = -2 * weight * SmoothnessFactor;
                A[k, i + 1] = weight * SmoothnessFactor;
                k++;
            }

            var x = A.Solve(b);

            var g = new float[256];
            for (int i = 0; i <= 255; i++)
            {
                g[i] = x[i];
            }

            return g;
        }
    }
}