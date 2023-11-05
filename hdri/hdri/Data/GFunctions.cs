namespace HDRI
{
    public readonly struct GFunctions
    {
        public readonly double[] Red;
        public readonly double[] Green;
        public readonly double[] Blue;

        public GFunctions(double[] red, double[] green, double[] blue)
        {
            Red = red;
            Green = green;
            Blue = blue;
        }
    }
}