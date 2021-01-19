using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace CalculationAlgorithmWrapper
{
    public class CurveConverterValues
    {
        public CurveConverterValues(
            int valueCount,
            string matlabGridString,
            string matlabCurveString,
            List<double> curve,
            List<double> grid)
        {
            ValueCount = valueCount;
            MatlabGridString = matlabGridString;
            MatlabCurveString = matlabCurveString;
            Curve = curve;
            Grid = grid;
        }

        public int ValueCount { get; }
        public string MatlabGridString { get; }
        public string MatlabCurveString { get; }
        public string CSharpGridString { get { return ConvertMatlabCurveStringToCSharpCurveString(MatlabGridString); } }
        public string CSharpCurveString { get { return ConvertMatlabCurveStringToCSharpCurveString(MatlabCurveString); } }
        public List<double> Curve { get; }
        public List<double> Grid { get; }

        public List<double> GetRoundedCurve(int decimalPlaces)
        {
            return Curve.Select(x => Math.Round(x, decimalPlaces)).ToList();
        }

        public string GetRoundedMatlabGridString(int decimalPlaces, string curveName)
        {
            var roundedMatlabGridString = CurveConverter.
                ConvertCurveToMatlabCurveString(Curve.Select(x => Math.Round(x, decimalPlaces)).ToList(), curveName: curveName);

            return roundedMatlabGridString;
        }

        public static string ConvertMatlabCurveStringToCSharpCurveString(string matlabCurveString)
        {
            var outputStringCsharp = matlabCurveString.Replace(" ", ", ").Replace("=,", "=")
                .Replace("[", "new List<double> {").Replace("]", "}").Replace(", }", "}").Replace(", =", " =");

            // replace "deltaCurve = new List<double> {3, 3, 3};" by "var deltaCurve = new List<double> {3, 3, 3};"
            var outputStringCsharpRegEx = Regex.Replace(input: outputStringCsharp, pattern: "(\\b^[^=]*\\b)", "var $1");

            return outputStringCsharpRegEx;
        }
    }
}
