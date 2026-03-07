using AngouriMath;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace CalculationAlgorithmWrapper.Implementation
{
    internal class EquationSolver
    {
        internal static string Solve(IList<string> inputList)
        {
            try
            {
                var result = SolveAlgebraic(inputList);
                if (!string.IsNullOrEmpty(result))
                    return result;
            }
            catch
            {
                // Algebraic solver failed, fall through to numerical solver
            }

            try
            {
                return SolveNumerical(inputList);
            }
            catch
            {
                return "No solution found";
            }
        }

        private static string SolveAlgebraic(IList<string> inputList)
        {
            Entity expr = inputList[0];
            var solutions = expr.Solve(inputList[1]).ToString();
            var solutionsWithoutBrackets = solutions.Replace("{", "").Replace("}", "");
            var solutionStringList = solutionsWithoutBrackets.Split(',').Select(s => s.Trim()).Where(x => !string.IsNullOrEmpty(x)).ToList();

            if (solutionStringList.Count == 0)
                return null;

            var returnString = "";
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
                 $"{roundedImag}i" : (Math.Sign(roundedImag) > 0) ?
                 $"{roundedReal} + {Math.Abs(roundedImag)}i" : $"{roundedReal} + {Math.Abs(roundedImag)}i";

                var correctedPartSolutionString = partSolutionString.Replace("1i", "i");

                returnString += correctedPartSolutionString + ", ";
            }
            return returnString.TrimEnd().TrimEnd(',');
        }

        // Created by Claude Opus 4.6 
        private static string SolveNumerical(IList<string> inputList)
        {
            var equationString = inputList[0];
            var variable = inputList[1].Trim();

            var sides = equationString.Split('=');
            if (sides.Length != 2)
                return "No solution found";

            var lhs = sides[0].Trim();
            var rhs = sides[1].Trim();

            Func<double, double> f = x =>
            {
                var lhsVal = EvaluateExpression(lhs, variable, x);
                var rhsVal = EvaluateExpression(rhs, variable, x);
                return lhsVal - rhsVal;
            };

            double solution = NewtonRaphson(f, initialGuess: 1.0);

            if (double.IsNaN(solution) || double.IsInfinity(solution))
            {
                double[] guesses = { 0.0, -1.0, 2.0, 5.0, 10.0, -5.0, -10.0, 0.5, 100.0 };
                foreach (var guess in guesses)
                {
                    solution = NewtonRaphson(f, initialGuess: guess);
                    if (!double.IsNaN(solution) && !double.IsInfinity(solution))
                        break;
                }
            }

            if (double.IsNaN(solution) || double.IsInfinity(solution))
                return "No solution found";

            double rounded = Math.Round(solution, 8);
            return $"{rounded}";
        }

        // Created by Claude Opus 4.6 
        private static double EvaluateExpression(string expression, string variable, double value)
        {
            Entity expr = expression.Replace(variable, $"({value})");
            var result = (Complex)expr.EvalNumerical();
            return result.Real;
        }

        // Created by Claude Opus 4.6 
        private static double NewtonRaphson(Func<double, double> f, double initialGuess, double tolerance = 1e-12, int maxIterations = 1000)
        {
            double x = initialGuess;
            double h = 1e-8;

            for (int i = 0; i < maxIterations; i++)
            {
                double fx = f(x);

                if (Math.Abs(fx) < tolerance)
                    return x;

                double fxPlusH = f(x + h);
                double derivative = (fxPlusH - fx) / h;

                if (Math.Abs(derivative) < 1e-15)
                    return double.NaN;

                x = x - fx / derivative;

                if (double.IsNaN(x) || double.IsInfinity(x))
                    return double.NaN;
            }

            if (Math.Abs(f(x)) < tolerance * 100)
                return x;

            return double.NaN;
        }
    }
}
