using System;

namespace HDRI
{
    public readonly struct RunParameters
    {
        public readonly Random Random;
        public readonly float SampleCountMultiplier;
        public readonly float SmoothnessFactor;

        public RunParameters(Random random, float sampleCountMultiplier, float smoothnessFactor)
        {
            Random = random;
            SampleCountMultiplier = sampleCountMultiplier;
            SmoothnessFactor = smoothnessFactor;
        }
    }
}