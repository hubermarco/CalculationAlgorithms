
namespace CalculationAlgorithmWrapper
{
    public class CurveComparison
    {
        public static CurveComparisonValues Compare(
           string string1,
           string string2,
           InputFormat inputFormat)
        {
            var curveConverterValues1 = CurveConverter.ConvertInputString(
                    inputString: string1,
                    inputFormat: inputFormat);

            var curveConverterValues2 = CurveConverter.ConvertInputString(
                    inputString: string2,
                    inputFormat: inputFormat);

            return new CurveComparisonValues(
                curveConverterValues1: curveConverterValues1,
                curveConverterValues2: curveConverterValues2);
        }
    }
}
