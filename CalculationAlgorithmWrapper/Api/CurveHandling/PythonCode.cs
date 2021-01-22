using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CalculationAlgorithmWrapper
{
    public class PythonCode
    {
        public static IList<string> CreateStringList(
            CurveConverterValues curveConverterValues1, 
            CurveConverterValues curveConverterValues2, 
            bool linearFreqAxis, 
            int numberDecimalPlaces)
        {
            var stringList = new List<string>();

            var curve1 = curveConverterValues1.Curve;
            var curve2 = curveConverterValues2.Curve;
            var grid1 = curveConverterValues1.Grid;
            var grid2 = curveConverterValues2.Grid;

            var curve1Rounded = curve1.Select(x => Math.Round(x, numberDecimalPlaces)).ToList();
            var curve2Rounded = curve2.Select(x => Math.Round(x, numberDecimalPlaces)).ToList();

            var gridString1 = curveConverterValues1.GetUsedPythonGridString(curveName: "x1", decimalPlaces: numberDecimalPlaces, linearFreqAxis: linearFreqAxis);
            var curveString1 = curveConverterValues1.GetPythonCurveString(curveName: "curve1", decimalPlaces: numberDecimalPlaces);
     
            var gridString2 = curveConverterValues2.GetUsedPythonGridString(curveName: "x2", decimalPlaces: numberDecimalPlaces, linearFreqAxis: linearFreqAxis);
            var curveString2 = curveConverterValues2.GetPythonCurveString(curveName: "curve2", decimalPlaces: numberDecimalPlaces);

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
                var curveConverterValuesDelta = curveConverterValues2 - curveConverterValues1;

                var gridString = curveConverterValuesDelta.GetPythonGridString("x", decimalPlaces: numberDecimalPlaces);
                var deltaCurveString = curveConverterValuesDelta.GetPythonCurveString(curveName: "deltaCurve", decimalPlaces: numberDecimalPlaces);

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

        public static void CreateFile(
           string outputDir,
           string fileNameWithoutExtension,
           CurveConverterValues curveConverterValues1,
           CurveConverterValues curveConverterValues2,
           bool linearFreqAxis)
        {
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            var filePath = Path.Combine(outputDir, $"{fileNameWithoutExtension}.txt");

            var stringLines = PythonCode.CreateStringList(
                curveConverterValues1: curveConverterValues1,
                curveConverterValues2: curveConverterValues2,
                linearFreqAxis: linearFreqAxis,
                numberDecimalPlaces: 4);

            File.WriteAllLines(filePath, stringLines);
        }
    }
}
