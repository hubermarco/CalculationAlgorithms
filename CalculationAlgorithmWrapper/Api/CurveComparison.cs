using System;
using System.Collections.Generic;
using System.Linq;

namespace CalculationAlgorithmWrapper
{
    public class CurveComparison
    {
        public static void Compare(
           string string1,
           string string2,
           InputFormat inputFormat,
           ref List<double> curve1,
           ref List<double> curve2,
           ref int valueCount1,
           ref int valueCount2,
           ref string resultingMatlabGridString,
           ref string deltaCurveStringMatlab,
           ref string resultingCSharpGridString,
           ref string deltaCurveStringCSharp,
           ref List<double> deltaCurve,
           ref List<double> resultingGrid)
        {
            var matlabGridStringDict = new Dictionary<int, string>() { { 0, string.Empty }, { 1, string.Empty } };
            var matlabCurveStringDict = new Dictionary<int, string>() { { 0, string.Empty }, { 1, string.Empty } };
            var cSharpGridStringDict = new Dictionary<int, string>() { { 0, string.Empty }, { 1, string.Empty } };
            var cSharpCurveStringDict = new Dictionary<int, string>() { { 0, string.Empty }, { 1, string.Empty } };
            var curveDict = new Dictionary<int, IList<double>> { { 0, new List<double>() }, { 1, new List<double>() } };
            var gridDict = new Dictionary<int, IList<double>> { { 0, new List<double>() }, { 1, new List<double>() } };
            var valueCountDict = new Dictionary<int, int>() { { 0, 0 }, { 1, 0 } };
            var inputStringDict = new Dictionary<int, string> { { 0, string1 }, { 1, string2 } };

            for (var index = 0; index < matlabGridStringDict.Count(); index++)
            {
                var matlabGridString = string.Empty;
                var matlabCurveString = string.Empty;
                var cSharpGridString = string.Empty;
                var cSharpCurveString = string.Empty;
                var curve = new List<double>();
                var grid = new List<double>();

                valueCountDict[index] = CurveConverter.ConvertInputString(
                    inputString: inputStringDict[index],
                    inputFormat: inputFormat,
                    ref matlabGridString,
                    ref matlabCurveString,
                    ref cSharpGridString,
                    ref cSharpCurveString,
                    ref curve,
                    ref grid);

                matlabGridStringDict[index] = matlabGridString;
                matlabCurveStringDict[index] = matlabCurveString;
                cSharpGridStringDict[index] = cSharpGridString;
                cSharpCurveStringDict[index] = cSharpCurveString;
                curveDict[index] = curve;
                gridDict[index] = grid;
            }

            curve1 = curveDict[0].ToList();
            curve2 = curveDict[1].ToList();
            valueCount1 = curveDict[0].Count;
            valueCount2 = curveDict[1].Count;

            if(curve1.Count == curve2.Count)
            {
                deltaCurve = curveDict[0].Select((x, index) => Math.Round(curveDict[1][index] - x, 2)).ToList();

                deltaCurveStringMatlab = CurveConverter.ConvertCurveToMatlabCurveString(
                    curve: deltaCurve.ToList(),
                    curveName: "deltaCurve");
            }
            else
            {
                deltaCurveStringMatlab = "No comparison possible!";
            }

            resultingGrid = gridDict[0].ToList();

            resultingMatlabGridString = matlabGridStringDict[0];
        
            resultingCSharpGridString = cSharpGridStringDict[0];

            deltaCurveStringCSharp = CurveConverter.ConvertMatlabCurveStringToCSharpCurveString(
                deltaCurveStringMatlab);
        }
    }
}
