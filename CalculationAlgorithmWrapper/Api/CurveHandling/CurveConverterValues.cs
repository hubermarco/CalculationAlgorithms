using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace CalculationAlgorithmWrapper
{
    public class CurveConverterValues
    {
        public CurveConverterValues(
            IList<double> curve,
            IList<double> grid)
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

        public string GetPythonGridString(string curveName, int? decimalPlaces = null)
        {
            var pythonGridString = GetRoundedMatlabCurveString(
                curve: Grid, curveName: curveName, decimalPlaces: decimalPlaces, commanSeparation: true);
            return pythonGridString;
        }

        public string GetPythonCurveString(string curveName, int? decimalPlaces = null)
        {
            var roundedPythonCurveString = GetRoundedMatlabCurveString(Curve, curveName, decimalPlaces, commanSeparation: true);
            return roundedPythonCurveString;
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

        public IList<double> Curve { get; }
        public IList<double> Grid { get; }

        public static CurveConverterValues operator -(
            CurveConverterValues curveConverterValues1, 
            CurveConverterValues curveConverterValues2)
        {
            var deltaCurve = GetDeltaCurve(
                curve1: curveConverterValues1.Curve,
                curve2: curveConverterValues2.Curve);

            var resultingGrid = GetGesultingGrid(
                grid1: curveConverterValues1.Grid,
                grid2: curveConverterValues2.Grid);

            return new CurveConverterValues(curve: deltaCurve, grid: resultingGrid) ;
        }

        private static string ConvertMatlabCurveStringToCSharpCurveString(string matlabCurveString)
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

        private static string GetRoundedMatlabCurveString(IList<double> curve, string curveName, int? decimalPlaces, bool commanSeparation = false)
        {
            Func<double, double> ElementManipulator;
            var roundedMatlabCurveString = string.Empty;

            double RoundCurveElement(double x) { return Math.Round(x, decimalPlaces.Value); }
            double BypassCurveElement(double x) { return x; }

            if(curve != null)
            {
                if (decimalPlaces.HasValue)
                    ElementManipulator = RoundCurveElement;
                else
                    ElementManipulator = BypassCurveElement;

                roundedMatlabCurveString = ConvertCurveToMatlabCurveString(
                    curve.Select(ElementManipulator).ToList(), curveName: curveName, commanSeparation: commanSeparation);
            }
            
            return roundedMatlabCurveString;
        }

        private static IList<double> GetDeltaCurve(IList<double> curve1, IList<double> curve2)
        {
            IList<double> deltaCurve = null;
          
            if (curve1.Count == curve2.Count)
            {
                deltaCurve = curve2.Select((x, index) => curve1[index] - x).ToList();
            }

            return deltaCurve;
        }

        private static IList<double> GetGesultingGrid(IList<double> grid1, IList<double> grid2)
        {
            var resultingGrid = (grid1.Count > 0) ? grid1.ToList() : grid2.ToList();
            return resultingGrid;
        }
    }
}
