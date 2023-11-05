namespace HDRI
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            Debug.Log("Program Start");

            ImageReader.GetAllImagesAtPath("exposure sequences/Canon_EOS_550D");

            Debug.Log("Program End");
        }
    }
}