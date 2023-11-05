using System;
using System.Diagnostics;

namespace HDRI
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            Debug.Log("Program Start");

            var random = new Random(Seed: 1);
            var cameraFolder = "Canon_EOS_550D";
            var images = ImageReader.ExtractImageInfo($"exposure sequences/{cameraFolder}");
            var runParameters = new RunParameters(random, sampleCountMultiplier: 1.0f, smoothnessFactor: 100.0f);

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var gFunctions = HDRIGenerator.SolveDebevec(runParameters, images);

            stopwatch.Stop();
            Debug.Log($"Algorithm took '{stopwatch.ElapsedMilliseconds}' ms");

            GraphDrawer.Draw(cameraFolder, "Z", "g(Z)", $"{cameraFolder}.png", gFunctions);

            foreach (var img in images)
            {
                img.Dispose();
            }

            Debug.Log("Program End");
        }
    }
}