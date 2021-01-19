
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

        public string GetDeltaCurveStringMatlab()
        {
            string deltaCurveStringMatlab;
            var curve1 = CurveConverterValues1.Curve;
            var curve2 = CurveConverterValues2.Curve;

            if (curve1.Count == curve2.Count)
            {
                var deltaCurve = curve1.Select((x, index) => Math.Round(curve2[index] - x, 2)).ToList();

                deltaCurveStringMatlab = CurveConverter.ConvertCurveToMatlabCurveString(
                    curve: deltaCurve.ToList(),
                    curveName: "deltaCurve");
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

        public string GetResultingMatlabGridString()
        {
            var matlabGridString1 = CurveConverterValues1.MatlabGridString;
            var matlabGridString2 = CurveConverterValues2.MatlabGridString;

            var resultingMatlabGridString = (matlabGridString1.Count() > 0) ?
                matlabGridString1 : matlabGridString2;

            return resultingMatlabGridString;
        }

        public string GetResultingCSharpGridString()
        {
            var cSharpGridString1 = CurveConverterValues1.CSharpGridString;
            var cSharpGridString2 = CurveConverterValues2.CSharpGridString;

            var resultingCSharpGridString = (cSharpGridString1.Count() > 0) ?
                cSharpGridString1 : cSharpGridString2;

            return resultingCSharpGridString;
        }

        public string GetDeltaCurveStringCSharp()
        {
            var deltaCurveStringMatlab = GetDeltaCurveStringMatlab();
            var deltaCurveStringCSharp = CurveConverterValues.ConvertMatlabCurveStringToCSharpCurveString(
                deltaCurveStringMatlab);

            return deltaCurveStringCSharp;
        }
    }
}
