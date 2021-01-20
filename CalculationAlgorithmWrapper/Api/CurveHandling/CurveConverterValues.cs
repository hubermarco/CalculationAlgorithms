using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace CalculationAlgorithmWrapper
{
    public class CurveConverterValues
    {
        public CurveConverterValues(
            List<double> curve,
            List<double> grid)
        {
            Curve = curve;
            Grid = grid;
        }

        public int ValueCount { get { return Curve.Count; } }

        public string GetMatlabGridString(string curveName, int? decimalPlaces = null)
        {
            var matlabGridString = GetRoundedMatlabCurveString(curve: Grid, curveName, decimalPlaces);
            return matlabGridString;
        }
        
        public string GetMatlabCurveString(string curveName, int? decimalPlaces = null)
        {
            var roundedMatlabCurveString = GetRoundedMatlabCurveString(Curve, curveName, decimalPlaces);
            return roundedMatlabCurveString;
        }

        public string GetCSharpGridString(string curveName, int? decimalPlaces = null)
        {
            var matlabGridString = GetMatlabGridString(curveName, decimalPlaces);
            return ConvertMatlabCurveStringToCSharpCurveString(matlabGridString);
        }

        public string GetCSharpCurveString(string curveName, int? decimalPlaces = null)
        {
            var matlabCurveString = GetMatlabCurveString(curveName, decimalPlaces);
            return ConvertMatlabCurveStringToCSharpCurveString(matlabCurveString);
        }

        public List<double> Curve { get; }
        public List<double> Grid { get; }

        public static string GetRoundedMatlabCurveString(IList<double> curve, string curveName, int? decimalPlaces)
        {
            double RoundCurveElement(double x){ return Math.Round(x, decimalPlaces.Value); }
            double BypassCurveElement(double x) { return x; }

            Func<double, double> ElementManipulator;
   
            if (decimalPlaces.HasValue)
                ElementManipulator = RoundCurveElement;
            else
                ElementManipulator = BypassCurveElement;

            var roundedMatlabCurveString = ConvertCurveToMatlabCurveString(
                curve.Select(ElementManipulator).ToList(), curveName: curveName);

            return roundedMatlabCurveString;
        }

        public static string ConvertMatlabCurveStringToCSharpCurveString(string matlabCurveString)
        {
            var outputStringCsharp = matlabCurveString.Replace(" ", ", ").Replace("=,", "=")
                .Replace("[", "new List<double> {").Replace("]", "}").Replace(", }", "}").Replace(", =", " =");

            // replace "deltaCurve = new List<double> {3, 3, 3};" by "var deltaCurve = new List<double> {3, 3, 3};"
            var outputStringCsharpRegEx = Regex.Replace(input: outputStringCsharp, pattern: "(\\b^[^=]*\\b)", "var $1");

            return outputStringCsharpRegEx;
        }

        public static string ConvertCurveToMatlabCurveString(List<double> curve, string curveName, bool commanSeparation = false)
        {
            var curveString = $"{curveName} = [";

            var separationString = commanSeparation ? ", " : " ";

            curve.ForEach(x =>
            {
                var xAdapted = x.ToString().Replace(',', '.');
                curveString += $"{xAdapted}{separationString}";
            });

            // Remove last separarationString
            if (curve.Count > 0)
                curveString = curveString.Remove(curveString.Length - separationString.Length);

            if (commanSeparation)
                curveString += "]";
            else
                curveString += "];";

            return curveString;
        }
    }
}
