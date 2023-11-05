using System;

namespace HDRI
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            Debug.Log("Program Start");

            var random = new Random(1);
            var imageInfos = ImageReader.ExtractImageInfo("exposure sequences/Canon_EOS_550D");
            var g = HDRIGenerator.SolveDebevec(random, imageInfos);

            for (var index = 0; index < g.Length; index++)
            {
                var value = g[index];
                Debug.Log($"G#{index}: '{value}'");
            }

            Debug.Log("Program End");
        }
    }
}