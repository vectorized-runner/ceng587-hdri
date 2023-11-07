using System;
using System.Diagnostics;

namespace HDRI
{
    internal static class Program
    {
        private static readonly string[] _folderNames =
        {
            "Canon_EOS_550D",
            "Minolta_DiMAGE_A1",
            "Nikon_Coolpix_E5400",
            "Nikon_D2H"
        };

        public static void Main(string[] args)
        {
            Console.WriteLine("Program Start");

            var folder = _folderNames[0];

            for (int i = 1; i <= 4; i++)
            {
                var random = new Random(Seed: 1);
                var smoothness = 100.0f;
                var sampleMultiplier = i;
                var parameters = new RunParameters(random, sampleMultiplier, smoothness);
                Console.WriteLine($"Running for '{sampleMultiplier}'");
                Run(folder, parameters);
            }

            //  GraphDrawer.Draw(cameraFolder, "Z", "g(Z)", $"{cameraFolder}.png", gFunctions);
            Console.WriteLine("Program End");
        }

        private static void Run(string folder, RunParameters parameters)
        {
            var images = ImageReader.ExtractImageInfo($"exposure sequences/{folder}");

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var gFunctions = HDRIGenerator.SolveDebevec(parameters, images);
            stopwatch.Stop();

            Console.WriteLine($"Running '{folder}' took {stopwatch.ElapsedMilliseconds} ms.");

            foreach (var img in images)
            {
                img.Dispose();
            }
        }
    }
}