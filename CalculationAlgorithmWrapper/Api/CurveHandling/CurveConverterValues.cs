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

        public int ValueCount { get { return (Curve != null) ? Curve.Count : 0; } }

        public string GetMatlabGridString(string curveName, int? decimalPlaces = null)
        {
            var matlabGridString = GetRoundedMatlabCurveString(curve: Grid, curveName, decimalPlaces);
            return matlabGridString;
        }

        public string GetUsedMatlabGridString(string curveName, int? decimalPlaces, bool linearFreqAxis)
        {
            var usedGrid = CalculateUsedGrid(grid: Grid, numberOfCurvePoints: ValueCount, linearFreqAxis: linearFreqAxis);
            var usedMatlabGridString = GetRoundedMatlabCurveString(curve: usedGrid, curveName, decimalPlaces);
            return usedMatlabGridString;
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

        public string GetUsedPythonGridString(string curveName, int? decimalPlaces, bool linearFreqAxis)
        {
            var usedGrid = CalculateUsedGrid(grid: Grid, numberOfCurvePoints: ValueCount, linearFreqAxis: linearFreqAxis);
            var usedPythonGridString = GetRoundedMatlabCurveString(
                curve: usedGrid, 
                curveName: curveName, 
                decimalPlaces: 
                decimalPlaces, 
                commanSeparation: true);

            return usedPythonGridString;
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

        public string GetUsedCSharpGridString(string curveName, int? decimalPlaces, bool linearFreqAxis)
        {
            var usedGrid = CalculateUsedGrid(grid: Grid, numberOfCurvePoints: ValueCount, linearFreqAxis: linearFreqAxis);
            var matlabGridString = GetRoundedMatlabCurveString(curve: usedGrid, curveName: curveName, decimalPlaces);
            return ConvertMatlabCurveStringToCSharpCurveString(matlabGridString);
        }

        public string GetCSharpCurveString(string curveName, int? decimalPlaces = null)
        {
            var matlabCurveString = GetMatlabCurveString(curveName, decimalPlaces);
            return ConvertMatlabCurveStringToCSharpCurveString(matlabCurveString);
        }

        public IList<double> Curve { get; }
        public IList<double> Grid { get; }

        public IList<double> GetUsedGrid(bool linearFreqAxis)
        {
            var usedGrid = CalculateUsedGrid(grid: Grid, numberOfCurvePoints: ValueCount, linearFreqAxis: linearFreqAxis);
            return usedGrid;
        }

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

        private static string ConvertCurveToMatlabCurveString(List<double> curve, string curveName, bool commanSeparation = false)
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

        private static IList<double> CalculateUsedGrid(
           IList<double> grid,
           int numberOfCurvePoints,
           bool linearFreqAxis)
        {
            IList<double> xGrid = null;

            if ((grid != null) && (grid.Count > 0))
            {
                xGrid = grid;
            }
            else if (numberOfCurvePoints == 228)
            {
                xGrid = new List<double> { 100, 105, 110, 115, 120, 125, 130, 135, 145, 150, 160, 165, 175, 180, 190, 200, 210, 220, 230, 240, 250, 260, 275, 290, 300, 315, 330, 345, 360, 380, 400, 420, 440, 460, 480, 500, 525, 550, 575, 600, 630, 660, 700, 730, 760, 800, 830, 870, 900, 950, 1000, 1050, 1100, 1150, 1200, 1250, 1312.5, 1375, 1437.5, 1500, 1562.5, 1625, 1687.5, 1750, 1812.5, 1875, 1937.5, 2000, 2062.5, 2125, 2187.5, 2250, 2312.5, 2375, 2437.5, 2500, 2562.5, 2625, 2687.5, 2750, 2812.5, 2875, 2937.5, 3000, 3062.5, 3125, 3187.5, 3250, 3312.5, 3375, 3437.5, 3500, 3562.5, 3625, 3687.5, 3750, 3812.5, 3875, 3937.5, 4000, 4062.5, 4125, 4187.5, 4250, 4312.5, 4375, 4437.5, 4500, 4562.5, 4625, 4687.5, 4750, 4812.5, 4875, 4937.5, 5000, 5062.5, 5125, 5187.5, 5250, 5312.5, 5375, 5437.5, 5500, 5562.5, 5625, 5687.5, 5750, 5812.5, 5875, 5937.5, 6000, 6062.5, 6125, 6187.5, 6250, 6312.5, 6375, 6437.5, 6500, 6562.5, 6625, 6687.5, 6750, 6812.5, 6875, 6937.5, 7000, 7062.5, 7125, 7187.5, 7250, 7312.5, 7375, 7437.5, 7500, 7562.5, 7625, 7687.5, 7750, 7812.5, 7875, 7937.5, 8000, 8062.5, 8125, 8187.5, 8250, 8312.5, 8375, 8437.5, 8500, 8562.5, 8625, 8687.5, 8750, 8812.5, 8875, 8937.5, 9000, 9062.5, 9125, 9187.5, 9250, 9312.5, 9375, 9437.5, 9500, 9562.5, 9625, 9687.5, 9750, 9812.5, 9875, 9937.5, 10000, 10062.5, 10125, 10187.5, 10250, 10312.5, 10375, 10437.5, 10500, 10562.5, 10625, 10687.5, 10750, 10812.5, 10875, 10937.5, 11000, 11062.5, 11125, 11187.5, 11250, 11312.5, 11375, 11437.5, 11500, 11562.5, 11625, 11687.5, 11750, 11812.5, 11875, 11937.5, 12000 };
            }
            else
            {
                if (linearFreqAxis)
                {
                    xGrid = Enumerable.Range(0, numberOfCurvePoints).Select(x => (double)x).ToList();
                }
                else
                {
                    xGrid = Enumerable.Range(1, numberOfCurvePoints).Select(x => (double)x).ToList();
                }
            }

            return xGrid;
        }
    }
}
