using System.Collections.Generic;
using ScottPlot;

namespace HDRI
{
    public static class GraphDrawer
    {
        public static void DrawMultiple(string graphName, string xAxis, string yAxis, string outputFileName,
            List<(string, double[])> labelAndGFunctions)
        {
            var plt = new Plot();

            var pixelValues = new double[256];
            for (int i = 0; i <= 255; i++)
            {
                pixelValues[i] = i;
            }

            var colors = new[]
            {
                System.Drawing.Color.Orange,
                System.Drawing.Color.Aqua,
                System.Drawing.Color.Chartreuse,
                System.Drawing.Color.Magenta
            };

            for (var index = 0; index < labelAndGFunctions.Count; index++)
            {
                var (label, func) = labelAndGFunctions[index];
                plt.AddScatter(pixelValues, func, color: colors[index % colors.Length], label: label);
            }

            plt.Legend();
            plt.Title(graphName);
            plt.XLabel(xAxis);
            plt.YLabel(yAxis);

            plt.SaveFig(outputFileName, 400, 400);
        }

        // public static void Draw(string graphName, string xAxis, string yAxis, string outputFileName,
        //     GFunctions gFunctions)
        // {
        //     var plt = new Plot();
        //
        //     var pixelValues = new double[256];
        //     for (int i = 0; i <= 255; i++)
        //     {
        //         pixelValues[i] = i;
        //     }
        //     
        //     plt.Add.Scatter(pixelValues, gFunctions.Red, GetFromSystemColor(System.Drawing.Color.Red));
        //     plt.Add.Scatter(pixelValues, gFunctions.Green, GetFromSystemColor(System.Drawing.Color.Green));
        //     plt.Add.Scatter(pixelValues, gFunctions.Blue, GetFromSystemColor(System.Drawing.Color.Blue));
        //
        //     plt.Title(graphName);
        //     plt.XLabel(xAxis);
        //     plt.YLabel(yAxis);
        //
        //     plt.SavePng(outputFileName, 400, 400);
        // }
        //
        // private static Color GetFromSystemColor(System.Drawing.Color systemColor)
        // {
        //     return Color.FromARGB((uint)systemColor.ToArgb());
        // }
    }
}