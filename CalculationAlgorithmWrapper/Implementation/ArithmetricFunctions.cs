using System.Collections.Generic;
using System.Linq;

namespace CalculationAlgorithmWrapper
{
    internal class ArithmetricFunctions
    {
        internal static double Fix2Double(IList<double> inputList) => 
            (inputList.Count == 3) ? Converters.Fix2Double((uint)inputList[0], (int)inputList[1], (int)inputList[2]) : 0;
       
        internal static double Double2Fix(IList<double> inputList) => 
            (inputList.Count == 3) ? Converters.Double2Fix(inputList[0], (int)inputList[1], (int)inputList[2]) : 0;
        
        internal static double Double2Bin(IList<double> inputList) => 
            (inputList.Count == 3) ? Converters.Double2Bin(inputList[0], (int)inputList[1], (int)inputList[2]) : 0;
        
        internal static double Bin2Double(IList<double> inputList) => 
            (inputList.Count == 3) ? Converters.Bin2Double((ulong)inputList[0], (int)inputList[1], (int)inputList[2]) : 0;
       
        internal static double Sum(IList<double> inputList) => inputList.Aggregate((total, next) => total + next);
    }
}
