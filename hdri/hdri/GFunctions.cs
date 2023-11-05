namespace HDRI
{
    public readonly struct GFunctions
    {
        public readonly float[] Red;
        public readonly float[] Green;
        public readonly float[] Blue;

        public GFunctions(float[] red, float[] green, float[] blue)
        {
            Red = red;
            Green = green;
            Blue = blue;
        }
    }
}