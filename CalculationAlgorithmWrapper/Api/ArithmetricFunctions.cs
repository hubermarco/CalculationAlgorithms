using System.Collections.Generic;

namespace CalculationAlgorithmWrapper
{
    internal class ArithmetricFunctions
    {
        internal static double Fix2Double(IList<double> inputList)
        {
            var returnValue = 0.0;
            if (inputList.Count == 3)
            {
                returnValue = Converters.Fix2Double((uint)inputList[0], (int)inputList[1], (int)inputList[2]);
            }
            return returnValue;
        }

        internal static double Double2Fix(IList<double> inputList)
        {
            var returnValue = 0.0;
            if (inputList.Count == 3)
            {
                returnValue = Converters.Double2Fix(inputList[0], (int)inputList[1], (int)inputList[2]);
            }
            return returnValue;
        }

        internal static double Double2Bool(IList<double> inputList)
        {
            var returnValue = 0.0;
            if (inputList.Count == 3)
            {
                returnValue = Converters.Double2Bool(inputList[0], (int)inputList[1], (int)inputList[2]);
            }
            return returnValue;
        }

        internal static double Bool2Double(IList<double> inputList)
        {
            var returnValue = 0.0;
            if (inputList.Count == 3)
            {
                returnValue = Converters.Bool2Double((ulong)inputList[0], (int)inputList[1], (int)inputList[2]);
            }
            return returnValue;
        }

        internal static double Sum(IList<double> inputList)
        {
            var sum = 0.0;
            foreach (var input in inputList)
            {
                sum += input;
            }
            return sum;
        }
    }
}
