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
           ref List<double> grid1,
           ref List<double> grid2,
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
                var curveConverterValues = CurveConverter.ConvertInputString(
                    inputString: inputStringDict[index],
                    inputFormat: inputFormat);

                matlabGridStringDict[index] = curveConverterValues.MatlabGridString;
                matlabCurveStringDict[index] = curveConverterValues.MatlabCurveString;
                cSharpGridStringDict[index] = curveConverterValues.CSharpGridString;
                cSharpCurveStringDict[index] = curveConverterValues.CSharpCurveString;
                curveDict[index] = curveConverterValues.Curve;
                gridDict[index] = curveConverterValues.Grid;
            }

            curve1 = curveDict[0].ToList();
            curve2 = curveDict[1].ToList();
            grid1 = gridDict[0].ToList();
            grid2 = gridDict[1].ToList();
 
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

            resultingGrid = (gridDict[0].Count > 0) ? gridDict[0].ToList() : gridDict[1].ToList();

            resultingMatlabGridString = (matlabGridStringDict[0].Count() > 0) ?
                matlabGridStringDict[0] : matlabGridStringDict[1];

            resultingCSharpGridString = (cSharpGridStringDict[0].Count() > 0) ?
                cSharpGridStringDict[0] : cSharpGridStringDict[1];

            deltaCurveStringCSharp = CurveConverter.ConvertMatlabCurveStringToCSharpCurveString(
                deltaCurveStringMatlab);
        }
    }
}
