using System;

namespace HDRI
{
    public static class Debug
    {
        public static void Assert(bool condition)
        {
            if (!condition)
            {
                Console.WriteLine("Assertion Failed");
            }
        }

        public static void Log(string message)
        {
            Console.WriteLine(message);
        }
    }
}