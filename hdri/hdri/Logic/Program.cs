using System;
using System.Diagnostics;

namespace HDRI
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            Debug.Log("Program Start");

            var folderNames = new string[]
            {
                "Canon_EOS_550D",
                "Minolta_DiMAGE_A1",
                "Nikon_Coolpix_E5400",
                "Nikon_D2H"
            };

            foreach (var folder in folderNames)
            {
                RunAlgorithm(folder);
            }

            //  GraphDrawer.Draw(cameraFolder, "Z", "g(Z)", $"{cameraFolder}.png", gFunctions);
            Debug.Log("Program End");
        }

        private static void RunAlgorithm(string folder)
        {
            var random = new Random(Seed: 1);
            var images = ImageReader.ExtractImageInfo($"exposure sequences/{folder}");
            var runParameters = new RunParameters(random, sampleCountMultiplier: 1.0f, smoothnessFactor: 100.0f);

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var gFunctions = HDRIGenerator.SolveDebevec(runParameters, images);
            stopwatch.Stop();

            Debug.Log($"Running '{folder}' took {stopwatch.ElapsedMilliseconds} ms.");

            foreach (var img in images)
            {
                img.Dispose();
            }
        }
    }
}