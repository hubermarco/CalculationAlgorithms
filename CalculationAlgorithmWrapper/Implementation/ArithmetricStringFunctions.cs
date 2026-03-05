using AngouriMath;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Expr = MathNet.Symbolics.SymbolicExpression;

namespace CalculationAlgorithmWrapper
{
    internal class ArithmetricStringFunctions
    {
        internal static string Solve(IList<string> inputList)
        {
            Entity expr = inputList[0];
            var solutions = expr.Solve(inputList[1]).ToString();
            var solutionsWithoutBrackets = solutions.Replace("{", "").Replace("}", "");
            var solutionStringList = solutionsWithoutBrackets.Split(',').Select(s => s.Trim()).Where(x => !string.IsNullOrEmpty(x)).ToList();
            var returnString = "\n";

            var complexNumberList = new List<Complex>();
            foreach (var solution in solutionStringList)
            {
                Entity solutionExpr = solution;

                var partSolution = (Complex)solutionExpr.EvalNumerical();

                double roundedReal = Math.Round(partSolution.Real, 8);
                double roundedImag = Math.Round(partSolution.Imaginary, 8);

                var complexNumber = new Complex(roundedReal, roundedImag);

                complexNumberList.Add(complexNumber);
            }

            var complexNumberListWithoutDoubles = complexNumberList.Distinct();

            foreach (var partSolution in complexNumberListWithoutDoubles)
            {
                double roundedReal = Math.Round(partSolution.Real, 8);
                double roundedImag = Math.Round(partSolution.Imaginary, 8);

                var partSolutionString = (roundedImag == 0) ?
                 $"{roundedReal}" : (roundedReal == 0) ?
                 $"{roundedImag}" : (Math.Sign(roundedImag) > 0) ?
                 $"{roundedReal} + {Math.Abs(roundedImag)}i" : $"{roundedReal} + {Math.Abs(roundedImag)}i";

                returnString += partSolutionString + "\n";
            }
            return returnString.TrimEnd('\n');
        }

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
