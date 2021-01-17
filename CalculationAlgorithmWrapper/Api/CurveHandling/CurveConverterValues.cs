using System.Collections.Generic;

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
    }
}
