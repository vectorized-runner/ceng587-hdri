using System;
using ScottPlot;

namespace HDRI
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            Debug.Log("Program Start");

            var random = new Random(Seed: 1);
            var imageInfos = ImageReader.ExtractImageInfo("exposure sequences/Canon_EOS_550D");
            var runParameters = new RunParameters(random, sampleCountMultiplier: 1.0f, smoothnessFactor: 100.0f);
            var gFunctions = HDRIGenerator.SolveDebevec(runParameters, imageInfos);

            var plt = new Plot();
            var pixelValues = new double[256];
            for (int i = 0; i <= 255; i++)
            {
                pixelValues[i] = i;
            }

            var red = Color.FromARGB((uint)System.Drawing.Color.Red.ToArgb());
            var green = Color.FromARGB((uint)System.Drawing.Color.Green.ToArgb());
            var blue = Color.FromARGB((uint)System.Drawing.Color.Blue.ToArgb());

            plt.Add.Scatter(pixelValues, gFunctions.Red, red);
            plt.Add.Scatter(pixelValues, gFunctions.Green, green);
            plt.Add.Scatter(pixelValues, gFunctions.Blue, blue);

            plt.Title("Line Plot with Custom Color");
            plt.XLabel("Pixel");
            plt.YLabel("g(Z)");

            plt.SavePng("xd.png", 600, 600);

            foreach (var img in imageInfos)
            {
                img.Dispose();
            }

            Debug.Log("Program End");
        }
    }
}