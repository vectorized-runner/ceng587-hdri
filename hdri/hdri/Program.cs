using System;

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

            foreach (var img in imageInfos)
            {
                img.Dispose();
            }
            
            Debug.Log("Program End");
        }
    }
}