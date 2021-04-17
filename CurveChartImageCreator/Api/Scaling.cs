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

            var skalingExponentResulting = (rangeExponent > 0) ?
                    (rangeExponent <= maxExponent) ? 0 : rangeExponent - maxExponent :
                    (rangeExponent >= minExponent) ? 0 : rangeExponent + minExponent;

            curveOutput = curve?.Select(x => x / Math.Pow(10, skalingExponentResulting)).ToList();
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

            var resultingRangeExponent = (rangeExponent1 > rangeExponent2) ? rangeExponent1 : rangeExponent2;
            var exponentLimit = resultingRangeExponent > 0 ? maxExponent : minExponent;

            var skalingExponentResulting = (Math.Abs(resultingRangeExponent) < Math.Abs(exponentLimit)) ?
                0 :
                (resultingRangeExponent > 0) ?
                    (resultingRangeExponent <= maxExponent) ? 0 : resultingRangeExponent - maxExponent :
                    (resultingRangeExponent >= minExponent) ? 0 : resultingRangeExponent + minExponent;

            curveOutput1 = curve1?.Select(x => x / Math.Pow(10, skalingExponentResulting)).ToList();
            curveOutput2 = curve2?.Select(x => x / Math.Pow(10, skalingExponentResulting)).ToList();
            scalingExponent = skalingExponentResulting;
        }

        private static int CalculateRangeExponent(IList<double> curve)
        {
            var range = ( (curve == null) || (curve.Count == 0)) ? 0 : curve.Max() - curve.Min();
            var usedRange = (range != 0) ? range : 1;
            var exponent = Math.Log10(Math.Abs(usedRange));
            var roundedExponent = exponent < 0 ? (int)Math.Floor(exponent) : (int)Math.Ceiling(exponent);
            return roundedExponent;
        }
    }
}
