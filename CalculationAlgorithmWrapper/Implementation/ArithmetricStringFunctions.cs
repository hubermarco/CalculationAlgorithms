using System.Collections.Generic;
using Expr = MathNet.Symbolics.SymbolicExpression;

namespace CalculationAlgorithmWrapper
{
    internal class ArithmetricStringFunctions
    {
        internal static string Taylor(IList<string> inputList)
        {
            var returnValue = string.Empty;

            if (inputList.Count == 4)
            {
                var function = Expr.Parse(inputList[0]);
                var variable = Expr.Parse(inputList[1]);
                var pos = Expr.Parse(inputList[2]);
                var k = int.Parse(inputList[3]);
                returnValue = Taylor(function, variable, pos, k).ToString();
            }
            return returnValue;
        }

        private static Expr Taylor(Expr function, Expr variable, Expr pos, int k)
        {
            int factorial = 1;
            Expr accumulator = Expr.Zero;
            Expr derivative = function;
            for (int i = 0; i < k; i++)
            {
                var subs = derivative.Substitute(variable, pos);
                derivative = derivative.Differentiate(variable);
                accumulator = accumulator + subs / factorial * ((variable - pos).Pow(i));
                factorial *= (i + 1);
            }
            return accumulator.Expand();
        }
    }
}
