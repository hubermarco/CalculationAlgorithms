using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CurveConverterAlgorithm
{
    public class PythonCode
    {
        public static IList<string> CreateDeltaStringList(
            CurveConverterValues curveConverterValues1, 
            CurveConverterValues curveConverterValues2, 
            bool linearFreqAxis, 
            int numberDecimalPlaces)
        {
            var stringList = new List<string>();

            var gridString1 = curveConverterValues1.GetUsedPythonGridString(
                curveName: "x1", decimalPlaces: numberDecimalPlaces, linearFreqAxis: linearFreqAxis);
            var curveString1 = curveConverterValues1.GetPythonCurveString(
                curveName: "curve1", decimalPlaces: numberDecimalPlaces);
     
            var gridString2 = curveConverterValues2.GetUsedPythonGridString(
                curveName: "x2", decimalPlaces: numberDecimalPlaces, linearFreqAxis: linearFreqAxis);
            var curveString2 = curveConverterValues2.GetPythonCurveString(
                curveName: "curve2", decimalPlaces: numberDecimalPlaces);

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

            if(curveConverterValues1.ValueCount == curveConverterValues2.ValueCount)
            {
                var curveConverterValuesDelta = curveConverterValues2 - curveConverterValues1;

                var gridString = curveConverterValuesDelta.GetUsedPythonGridString(
                    "x", decimalPlaces: numberDecimalPlaces, linearFreqAxis: linearFreqAxis);
                var deltaCurveString = curveConverterValuesDelta.GetPythonCurveString(
                    curveName: "deltaCurve", decimalPlaces: numberDecimalPlaces);

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

        public static IList<string> CreateStringList(
            CurveConverterValues curveConverterValues,
            bool linearFreqAxis,
            int numberDecimalPlaces)
        {
            var stringList = new List<string>();

            var gridString = curveConverterValues.GetUsedPythonGridString(
                curveName: "x", decimalPlaces: numberDecimalPlaces, linearFreqAxis: linearFreqAxis);
            var curveString = curveConverterValues.GetPythonCurveString(
                curveName: "curve", decimalPlaces: numberDecimalPlaces);

            stringList.Add("import numpy as np");
            stringList.Add("import matplotlib.pyplot as plt");
            stringList.Add("");
            stringList.Add(gridString);
            stringList.Add("");
            stringList.Add(curveString);
            stringList.Add("");
            stringList.Add("plt.figure()");

            var plotString = linearFreqAxis ? "plot" : "semilogx";
            stringList.Add($"plt.{plotString}(x, curve)");

            stringList.Add("plt.grid()");
            stringList.Add("plt.title('Curve')");
            stringList.Add("plt.xlabel('x')");
            stringList.Add("plt.ylabel('y')");
            stringList.Add("plt.legend(['curve'])");
            stringList.Add("plt.show()");

            return stringList;
        }

        public static void CreateDeltaFile(
           string outputDir,
           string fileNameWithoutExtension,
           CurveConverterValues curveConverterValues1,
           CurveConverterValues curveConverterValues2,
           bool linearFreqAxis,
           int numberDecimalPlaces)
        {
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            var filePath = Path.Combine(outputDir, $"{fileNameWithoutExtension}.txt");

            var stringLines = CreateDeltaStringList(
                curveConverterValues1: curveConverterValues1,
                curveConverterValues2: curveConverterValues2,
                linearFreqAxis: linearFreqAxis,
                numberDecimalPlaces: numberDecimalPlaces);

            File.WriteAllLines(filePath, stringLines);
        }

        public static void CreateFile(
            string outputDir,
            string fileNameWithoutExtension,
            CurveConverterValues curveConverterValues,
            bool linearFreqAxis,
            int numberDecimalPlaces)
        {
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            var filePath = Path.Combine(outputDir, $"{fileNameWithoutExtension}.txt");

            var stringLines = CreateStringList(
                curveConverterValues: curveConverterValues,
                linearFreqAxis: linearFreqAxis,
                numberDecimalPlaces: numberDecimalPlaces);

            File.WriteAllLines(filePath, stringLines);
        }
    }
}
