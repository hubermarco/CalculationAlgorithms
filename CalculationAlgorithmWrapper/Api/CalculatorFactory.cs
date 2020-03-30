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
            var stringOperatorList = ruleSet.GetStringOperatorList();
            var calculationStringList = CalculationAlgorithmFactory.CreateCalculationStringList(
                operatorList,
                stringOperatorList);

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
                { "Double2Bool", ArithmetricFunctions.Double2Bool },
                { "Bool2Double", ArithmetricFunctions.Bool2Double },
                { "sum", ArithmetricFunctions.Sum },
                { "sin", inputList => Math.Sin(inputList[0]) },
                { "cos", inputList => Math.Cos(inputList[0]) },
                { "log", inputList => Math.Log10(inputList[0]) },
            };

            var stringFunctions = new Dictionary<string, Func<IList<string>, string>>
            {
                 { "d", inputList => Expr.Parse(inputList[0]).Differentiate(Expr.Parse(inputList[1])).ToString() },
                 { "exp", inputList => Expr.Parse(inputList[0]).Expand().ToString() },
                 { "taylor", inputList => StringFunctions.Taylor(inputList) }
            };

            var ruleSet = new RuleSet(
                arithmetricOperators,
                arithmetricFunctions,
                stringFunctions);

            return ruleSet;
        }
    }
}
