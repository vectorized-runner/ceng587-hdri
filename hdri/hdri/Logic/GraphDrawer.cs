using ScottPlot;

namespace HDRI
{
    public static class GraphDrawer
    {
        public static void Draw(string graphName, string xAxis, string yAxis, string outputFileName,
            GFunctions gFunctions)
        {
            var plt = new Plot();

            var pixelValues = new double[256];
            for (int i = 0; i <= 255; i++)
            {
                pixelValues[i] = i;
            }

            plt.Add.Scatter(pixelValues, gFunctions.Red, GetRedColor());
            plt.Add.Scatter(pixelValues, gFunctions.Green, GetGreenColor());
            plt.Add.Scatter(pixelValues, gFunctions.Blue, GetBlueColor());

            plt.Title(graphName);
            plt.XLabel(xAxis);
            plt.YLabel(yAxis);

            plt.SavePng(outputFileName, 600, 600);
        }

        private static Color GetRedColor()
        {
            return Color.FromARGB((uint)System.Drawing.Color.Red.ToArgb());
        }

        private static Color GetGreenColor()
        {
            return Color.FromARGB((uint)System.Drawing.Color.Green.ToArgb());
        }

        private static Color GetBlueColor()
        {
            return Color.FromARGB((uint)System.Drawing.Color.Blue.ToArgb());
        }
    }
}