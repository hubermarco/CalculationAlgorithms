using System;
using System.Collections.Generic;
using System.Linq;

namespace CurveChartImageCreater
{
    public class Scaling
    {
        public static void Apply(
           IList<double> curve,
           int minExponent,
           int maxExponent,
           ref IList<double> curveOutput,
           ref int scalingExponent)
        {
            var rangeExponent = CalculateRangeExponent(curve);

            var skalingExponentResulting = rangeExponent > 0 ?
                    rangeExponent - maxExponent :
                    rangeExponent + minExponent;

            curveOutput = curve.Select(x => x / Math.Pow(10, skalingExponentResulting)).ToList();
            scalingExponent = skalingExponentResulting;
        }

        public static void Apply(
            IList<double> curve1, 
            IList<double> curve2,
            int minExponent,
            int maxExponent,
            ref IList<double> curveOutput1,
            ref IList<double> curveOutput2,
            ref int scalingExponent)
        {
            var rangeExponent1 = CalculateRangeExponent(curve1);
            var rangeExponent2 = CalculateRangeExponent(curve2);

            var resultingExponent = (rangeExponent1 > rangeExponent2) ? rangeExponent1 : rangeExponent2;
            var exponentLimit = resultingExponent > 0 ? maxExponent : minExponent;

            var skalingExponentResulting = (Math.Abs(resultingExponent) < Math.Abs(exponentLimit)) ?
                0 :
                resultingExponent > 0 ?
                    resultingExponent - maxExponent :
                    resultingExponent + minExponent;

            curveOutput1 = curve1.Select(x => x / Math.Pow(10, skalingExponentResulting)).ToList();
            curveOutput2 = curve2.Select(x => x / Math.Pow(10, skalingExponentResulting)).ToList();
            scalingExponent = skalingExponentResulting;
        }

        private static int CalculateRangeExponent(IList<double> curve)
        {
            var range = (curve.Count > 0) ? curve.Max() - curve.Min() : 0;
            var usedRange = (range != 0) ? range : 1;
            var exponent = (int)Math.Round(Math.Log10(Math.Abs(usedRange)));
            return exponent;
        }
    }
}
