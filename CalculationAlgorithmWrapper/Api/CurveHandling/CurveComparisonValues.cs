
using System;
using System.Collections.Generic;
using System.Linq;

namespace CalculationAlgorithmWrapper
{
    public class CurveComparisonValues
    {
        public CurveComparisonValues(CurveConverterValues curveConverterValues1, CurveConverterValues curveConverterValues2)
        {
            CurveConverterValues1 = curveConverterValues1;
            CurveConverterValues2 = curveConverterValues2;
        }

        public CurveConverterValues CurveConverterValues1 { get; }
        public CurveConverterValues CurveConverterValues2 { get; }

        public IList<double> GetDeltaCurve()
        {
            IList<double> deltaCurve = null;
            var curve1 = CurveConverterValues1.Curve;
            var curve2 = CurveConverterValues2.Curve;

            if (curve1.Count == curve2.Count)
            {
                deltaCurve = curve1.Select((x, index) => Math.Round(curve2[index] - x, 2)).ToList();
            }

            return deltaCurve;
        }

        public string GetDeltaCurveStringMatlab(string curveName, int? decimalPlaces = null)
        {
            string deltaCurveStringMatlab;
            var curve1 = CurveConverterValues1.Curve;
            var curve2 = CurveConverterValues2.Curve;

            if (curve1.Count == curve2.Count)
            {
                IList<double> deltaCurve;

                if (decimalPlaces.HasValue)
                    deltaCurve = curve1.Select((x, index) => Math.Round(curve2[index] - x, decimalPlaces.Value)).ToList();
                else
                    deltaCurve = curve1.Select((x, index) => curve2[index] - x).ToList();

                deltaCurveStringMatlab = CurveConverter.ConvertCurveToMatlabCurveString(
                    curve: deltaCurve.ToList(),
                    curveName: curveName);
            }
            else
            {
                deltaCurveStringMatlab = "No comparison possible!";
            }

            return deltaCurveStringMatlab;
        }

        public IList<double> GetGesultingGrid()
        {
            var grid1 = CurveConverterValues1.Grid;
            var grid2 = CurveConverterValues2.Grid;

            var resultingGrid = (grid1.Count > 0) ? grid1.ToList() : grid2.ToList();
            return resultingGrid;
        }

        public string GetResultingMatlabGridString(string curveName, int? decimalPlaces = null)
        {
            var matlabGridString1 = CurveConverterValues1.GetMatlabGridString(curveName, decimalPlaces);
            var matlabGridString2 = CurveConverterValues2.GetMatlabGridString(curveName, decimalPlaces);

            var resultingCSharpGridString = (matlabGridString1.Count() > 0) ?
                matlabGridString1 : matlabGridString2;

            return resultingCSharpGridString;
        }

        public string GetResultingCSharpGridString(string curveName, int? decimalPlaces = null)
        {
            var cSharpGridString1 = CurveConverterValues1.GetCSharpGridString(curveName, decimalPlaces);
            var cSharpGridString2 = CurveConverterValues2.GetCSharpGridString(curveName, decimalPlaces);

            var resultingCSharpGridString = (cSharpGridString1.Count() > 0) ?
                cSharpGridString1 : cSharpGridString2;

            return resultingCSharpGridString;
        }

        public string GetDeltaCurveStringCSharp(string curveName, int? decimalPlaces = null)
        {
            var deltaCurveStringMatlab = GetDeltaCurveStringMatlab(curveName, decimalPlaces);
            var deltaCurveStringCSharp = CurveConverterValues.ConvertMatlabCurveStringToCSharpCurveString(
                deltaCurveStringMatlab);

            return deltaCurveStringCSharp;
        }
    }
}
