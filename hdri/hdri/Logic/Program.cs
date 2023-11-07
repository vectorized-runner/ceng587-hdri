using System;
using System.Collections.Generic;
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

            var random = new Random(Seed: 1);
            var smoothness = 100.0f;
            var sampleMultiplier = 1.0f;
            var parameters = new RunParameters(random, sampleMultiplier, smoothness);
            var labelAndGs = new List<(string, double[])>();

            foreach (var folder in _folderNames)
            {
                var g = Run(folder, parameters, Channel.Red);
                labelAndGs.Add((folder, g));
            }

            GraphDrawer.DrawMultiple("G Functions for Red Channels", "Z", "g(Z)", "RedChannel_result.png", labelAndGs);
            
            labelAndGs = new List<(string, double[])>();

            foreach (var folder in _folderNames)
            {
                var g = Run(folder, parameters, Channel.Green);
                labelAndGs.Add((folder, g));
            }

            GraphDrawer.DrawMultiple("G Functions for Green Channels", "Z", "g(Z)", "GreenChannel_result.png", labelAndGs);
            
            labelAndGs = new List<(string, double[])>();

            foreach (var folder in _folderNames)
            {
                var g = Run(folder, parameters, Channel.Blue);
                labelAndGs.Add((folder, g));
            }

            GraphDrawer.DrawMultiple("G Functions for Blue Channels", "Z", "g(Z)", "BlueChannel_result.png", labelAndGs);
            
            Console.WriteLine("Program End");
        }

        private static double[] Run(string folder, RunParameters parameters, Channel channel)
        {
            var images = ImageReader.ExtractImageInfo($"exposure sequences/{folder}");

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var gFunctions = HDRIGenerator.SolveDebevec(parameters, images, channel);
            stopwatch.Stop();

            Console.WriteLine($"Running '{folder}' took {stopwatch.ElapsedMilliseconds} ms.");

            foreach (var img in images)
            {
                img.Dispose();
            }

            return gFunctions;
        }

        private static GFunctions Run(string folder, RunParameters parameters)
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

            return gFunctions;
        }
    }
}