using System;
using System.Collections.Generic;
using System.Linq;

namespace CalculationAlgorithmWrapper
{
    public class CurveConverterValues
    {
        public CurveConverterValues(
            int valueCount,
            string matlabGridString,
            string matlabCurveString,
            string cSharpGridString,
            string cSharpCurveString,
            List<double> curve,
            List<double> grid)
        {
            ValueCount = valueCount;
            MatlabGridString = matlabGridString;
            MatlabCurveString = matlabCurveString;
            CSharpGridString = cSharpGridString;
            CSharpCurveString = cSharpCurveString;
            Curve = curve;
            Grid = grid;
        }

        public int ValueCount { get; }
        public string MatlabGridString { get; }
        public string MatlabCurveString { get; }
        public string CSharpGridString { get; }
        public string CSharpCurveString { get; }
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

        public string ConvertMatlabCurveStringToCSharpCurveString(string matlabCurveString)
        {
            var cSharpCurveString =  CurveConverter.
                ConvertMatlabCurveStringToCSharpCurveString(matlabCurveString);

            return cSharpCurveString;
        }
    }
}
