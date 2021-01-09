using System;
using System.Collections.Generic;
using System.Linq;

namespace CalculationAlgorithmWrapper.Api
{
    public class PythonCode
    {
        public static IList<string> Create(List<double> curve1, List<double> curve2, List<double> grid1, List<double> grid2, bool linearFreqAxis, int numberDecimalPlaces)
        {
            var stringList = new List<string>();

            var curve1Rounded = curve1.Select(x => Math.Round(x, numberDecimalPlaces)).ToList();
            var curve2Rounded = curve2.Select(x => Math.Round(x, numberDecimalPlaces)).ToList();

            var usedGrid1 = XGrid.Calculate(grid:grid1, numberOfCurvePoints:curve1.Count, linearFreqAxis:linearFreqAxis).ToList();
            var gridString1 = CurveConverter.ConvertCurveToMatlabCurveString(usedGrid1, "x1", commanSeparation: true);
            var curveString1 = CurveConverter.ConvertCurveToMatlabCurveString(curve1Rounded, "curve1", commanSeparation: true);

            var usedGrid2 = XGrid.Calculate(grid: grid2, numberOfCurvePoints: curve2.Count, linearFreqAxis: linearFreqAxis).ToList();
            var gridString2 = CurveConverter.ConvertCurveToMatlabCurveString(usedGrid2, "x2", commanSeparation: true);
            var curveString2 = CurveConverter.ConvertCurveToMatlabCurveString(curve2Rounded, "curve2", commanSeparation: true);

            stringList.Add("import numpy as np");
            stringList.Add("import matplotlib.pyplot as plt");
            stringList.Add("");
            stringList.Add(gridString1);
            stringList.Add("");
            stringList.Add(gridString2);
            stringList.Add("");
            stringList.Add(curveString1);
            stringList.Add("");
            stringList.Add(curveString2);
            stringList.Add("");
            stringList.Add("plt.figure()");

            var plotString = linearFreqAxis ? "plot" : "semilogx";
            stringList.Add($"plt.{plotString}(x1, curve1, x2, curve2)");
            
            stringList.Add("plt.grid()");
            stringList.Add("plt.title('CurveComparison')");
            stringList.Add("plt.xlabel('x')");
            stringList.Add("plt.ylabel('y')");
            stringList.Add("plt.legend(['curve1', 'curve2'])");

            if(curve1.Count == curve2.Count)
            {
                var gridString = CurveConverter.ConvertCurveToMatlabCurveString(usedGrid1, "x", commanSeparation: true);
                var deltaCurve = curve1.Select((value, index) => Math.Round(curve2[index] - value, numberDecimalPlaces)).ToList();
                var deltaCurveString = CurveConverter.ConvertCurveToMatlabCurveString(deltaCurve, "deltaCurve", commanSeparation: true);

                stringList.Add("");
                stringList.Add(gridString);
                stringList.Add("");
                stringList.Add(deltaCurveString);
                stringList.Add("");
                stringList.Add("plt.figure()");

                var plotString2 = linearFreqAxis ? "plot" : "semilogx";
                stringList.Add($"plt.{plotString2}(x, deltaCurve)");
         
                stringList.Add("plt.grid()");
                stringList.Add("plt.title('Curve2-Curve1')");
                stringList.Add("plt.xlabel('x')");
                stringList.Add("plt.ylabel('y')");
                stringList.Add("plt.legend(['deltaCurve'])");
            }
            stringList.Add("plt.show()");

            return stringList;
        }
    }
}
