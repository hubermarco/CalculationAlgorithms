
using CalculationAlgorithm;
using System;
using System.Collections.Generic;

namespace CalculatorAlgorithmsWrapper
{
    public class CalculatorFactory
    {
        public static ICalculator Create()
        {
            RuleSet ruleSet = CreateRuleSet();

            var calculationAlgorithm = CalculationAlgorithmFactory.Create(ruleSet);
            var operatorList = ruleSet.GetOperatorList();
            var calculationStringList = CalculationAlgorithmFactory.CreateCalculationStringList(
                operatorList);

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
                { "Fix2Double", Fix2Double },
                { "Double2Fix", Double2Fix },
                { "Double2Bool", Double2Bool },
                { "Bool2Double", Bool2Double },
                { "sum", Sum },
                { "sin", inputList => Math.Sin(inputList[0]) },
                { "cos", inputList => Math.Cos(inputList[0]) },
                { "log", inputList => Math.Log10(inputList[0]) },
            };

            var ruleSet = new RuleSet(
                arithmetricOperators,
                arithmetricFunctions);

            return ruleSet;
        }

        private static double Fix2Double(IList<double> inputList)
        {
            var returnValue = 0.0;
            if (inputList.Count == 3)
            {
                returnValue = Converters.Fix2Double((uint)inputList[0], (int)inputList[1], (int)inputList[2]);
            }
            return returnValue;
        }

        private static double Double2Fix(IList<double> inputList)
        {
            var returnValue = 0.0;
            if (inputList.Count == 3)
            {
                returnValue = Converters.Double2Fix(inputList[0], (int)inputList[1], (int)inputList[2]);
            }
            return returnValue;
        }

        private static double Double2Bool(IList<double> inputList)
        {
            var returnValue = 0.0;
            if (inputList.Count == 3)
            {
                returnValue = Converters.Double2Bool(inputList[0], (int)inputList[1], (int)inputList[2]);
            }
            return returnValue;
        }

        private static double Bool2Double(IList<double> inputList)
        {
            var returnValue = 0.0;
            if (inputList.Count == 3)
            {
                returnValue = Converters.Bool2Double((ulong)inputList[0], (int)inputList[1], (int)inputList[2]);
            }
            return returnValue;
        }

        private static double Sum(IList<double> inputList)
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
