using CalculationAlgorithm;
using System;
using System.Collections.Generic;

using Expr = MathNet.Symbolics.SymbolicExpression;

namespace CalculationAlgorithmWrapper
{
    public class CalculatorFactory
    {
        public static ICalculator Create()
        {
            RuleSet ruleSet = CreateRuleSet();

            var calculationAlgorithm = CalculationAlgorithmFactory.Create(ruleSet);
            var operatorList = ruleSet.GetOperatorList();
            var arithmetricStringOperatorList = ruleSet.GetArithmetricStringOperatorList();
            var calculationStringList = CalculationAlgorithmFactory.CreateCalculationStringList(
                operatorList,
                arithmetricStringOperatorList);

            return new Calculator(
                calculationAlgorithm,
                new CalculationStringWrapper(calculationStringList));
        }

        private static RuleSet CreateRuleSet()
        {
            var arithmetricOperators = new Dictionary<string, Tuple<int, Func<double, double, double>>>
            {
                { "+", new Tuple<int, Func<double, double, double>>(0, (x, y) => x + y) },
                { "-", new Tuple<int, Func<double, double, double>>(0, (x, y) => x - y) },
                { "*", new Tuple<int, Func<double, double, double>>(1, (x, y) => x * y) },
                { "/", new Tuple<int, Func<double, double, double>>(1, (x, y) => x / y) },
                { "^", new Tuple<int, Func<double, double, double>>(2, (x, y) => Math.Pow(x,y)) },
                { "->", new Tuple<int, Func<double, double, double>>(0, (x, y) => (x==0) || (y==1) ? 1 : 0) },
                { "&", new Tuple<int, Func<double, double, double>>(0, (x, y) => (x==1) && (y==1) ? 1 : 0) }
            };

            var arithmetricFunctions = new Dictionary<string, Func<IList<double>, double>>
            {
                { "Fix2Double", ArithmetricFunctions.Fix2Double },
                { "Double2Fix", ArithmetricFunctions.Double2Fix },
                { "Double2Bin", ArithmetricFunctions.Double2Bin },
                { "Bin2Double", ArithmetricFunctions.Bin2Double },
                { "sum", ArithmetricFunctions.Sum },
                { "sin", inputList => Math.Sin(inputList[0]) },
                { "cos", inputList => Math.Cos(inputList[0]) },
                { "log", inputList => Math.Log10(inputList[0]) },
            };

            var arithmetricStringFunctions = new Dictionary<string, Func<IList<string>, string>>
            {
                 { "d", inputList => Expr.Parse(inputList[0]).Differentiate(Expr.Parse(inputList[1])).ToString() },
                 { "exp", inputList => Expr.Parse(inputList[0]).Expand().ToString() },
                 { "taylor", inputList => ArithmetricStringFunctions.Taylor(inputList) },
                 { "eval", inputList => Expr.Parse(inputList[0]).ToString() }
            };

            var ruleSet = new RuleSet(
                arithmetricOperators,
                arithmetricFunctions,
                arithmetricStringFunctions);

            return ruleSet;
        }
    }
}
