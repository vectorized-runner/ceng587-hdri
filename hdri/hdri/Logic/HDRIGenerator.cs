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
        private const int MinPixelValue = 0;
        private const int MaxPixelValue = 255;

        private static int[] GetSampleIndices(Random random, int pixelCount, int sampleCount)
        {
            var result = new int[sampleCount];

            for (int i = 0; i < sampleCount; i++)
            {
                result[i] = random.Next(0, pixelCount);
            }

            return result;
        }

        private static int GetRequiredSampleCount(int textureCount, float multiplier)
        {
            // In the Paper 50 pixels for (255 - 0) / (11 - 1), so I can multiply by 2
            var baseValue = 2 * ((MaxPixelValue - MinPixelValue) / (textureCount - 1));
            var finalValue = (int)Math.Ceiling(baseValue * multiplier);
            return finalValue;
        }

        private static int WeightFunction(int pixelValue)
        {
            var threshold = (MaxPixelValue + MinPixelValue) / 2;
            if (pixelValue <= threshold)
            {
                return pixelValue - MinPixelValue;
            }

            return MaxPixelValue - pixelValue;
        }

        private static byte GetChannel(Color color, Channel channel)
        {
            return channel switch
            {
                Channel.Red => color.R,
                Channel.Green => color.G,
                Channel.Blue => color.B,
                _ => throw new ArgumentOutOfRangeException(nameof(channel), channel, null)
            };
        }

        private static byte[,] GetSamples(ImageInfo[] images, int[] sampleIndices, Channel channel)
        {
            var imageCount = images.Length;
            var sampleCount = sampleIndices.Length;
            var samples = new byte[imageCount, sampleCount];

            for (int imageIndex = 0; imageIndex < imageCount; imageIndex++)
            {
                for (var sampleIndex = 0; sampleIndex < sampleIndices.Length; sampleIndex++)
                {
                    var sample = sampleIndices[sampleIndex];
                    var color = images[imageIndex].GetPixel(sample);
                    var channelValue = GetChannel(color, channel);
                    samples[imageIndex, sampleIndex] = channelValue;
                }
            }

            return samples;
        }

        public static GFunctions SolveDebevec(RunParameters parameters, ImageInfo[] images)
        {
            return new GFunctions(
                SolveDebevec(parameters, images, Channel.Red),
                SolveDebevec(parameters, images, Channel.Green),
                SolveDebevec(parameters, images, Channel.Blue));
        }

        public static double[] SolveDebevec(RunParameters parameters, ImageInfo[] images, Channel channel)
        {
            var pixelCount = images[0].GetPixelCount();
            var imageCount = images.Length;
            var sampleCount = GetRequiredSampleCount(imageCount, parameters.SampleCountMultiplier);
            var sampleIndices = GetSampleIndices(parameters.Random, pixelCount, sampleCount);
            var sampledPixels = GetSamples(images, sampleIndices, channel);
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

            // Set Middle Value
            A[k, (MinPixelValue + MaxPixelValue) / 2] = 1;
            k++;

            // Smoothness
            var smoothnessFactor = parameters.SmoothnessFactor;
            for (int i = MinPixelValue + 1; i < MaxPixelValue; i++)
            {
                var weight = WeightFunction(i);
                A[k, i - 1] = weight * smoothnessFactor;
                A[k, i] = -2 * weight * smoothnessFactor;
                A[k, i + 1] = weight * smoothnessFactor;
                k++;
            }

            var x = A.Solve(b);
            var g = new double[256];
            for (int i = 0; i <= 255; i++)
            {
                g[i] = x[i];
            }

            return g;
        }
    }
}