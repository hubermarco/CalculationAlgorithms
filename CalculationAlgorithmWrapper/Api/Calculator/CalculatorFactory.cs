
using CalculationAlgorithm;
using CalculationAlgorithmWrapper.Implementation;
using SHS.SAT.HsmlFormula;
using System;
using System.Collections.Generic;
using System.Globalization;
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
                { "Reg2Val_fix", inputList => (inputList.Count == 3) ? Formula.BitsToLinear((uint)inputList[0], $"fix<{inputList[1]},{inputList[2]}>") : 0 },
                { "Val2Reg_fix", inputList => (inputList.Count == 3) ? Formula.LinearToBits(inputList[0], $"fix<{inputList[1]},{inputList[2]}>") : 0 },
                { "Reg2Val_ufix", inputList => (inputList.Count == 3) ? Formula.BitsToLinear((uint)inputList[0], $"ufix<{inputList[1]},{inputList[2]}>") : 0 },
                { "Val2Reg_ufix", inputList => (inputList.Count == 3) ? Formula.LinearToBits(inputList[0], $"ufix<{inputList[1]},{inputList[2]}>") : 0 },
                { "Reg2Val_wfloat", inputList => (inputList.Count == 3) ? Formula.BitsToLinear((uint)inputList[0], $"wfloat<{inputList[1]},{inputList[2]}>") : 0 },
                { "Val2Reg_wfloat", inputList => (inputList.Count == 3) ? Formula.LinearToBits(inputList[0], $"wfloat<{inputList[1]},{inputList[2]}>") : 0 },
                { "Reg2Val_rfloat", inputList => (inputList.Count == 3) ?
                    Formula.BitsToLinear((uint)inputList[0], $"rfloat<{inputList[1]},{inputList[2]}>") :
                    (inputList.Count == 4 ) ?
                    Formula.BitsToLinear((uint)inputList[0], $"rfloat<{inputList[1]},{inputList[2]},{inputList[3]}>") : 0 },
                { "Val2Reg_rfloat", inputList => (inputList.Count == 3) ?
                    Formula.LinearToBits(inputList[0], $"rfloat<{inputList[1]},{inputList[2]}>") :
                    (inputList.Count == 4 ) ?
                    Formula.LinearToBits(inputList[0], $"rfloat<{inputList[1]},{inputList[2]},{inputList[3]}>") : 0 },
                { "Reg2Val_float", inputList => (inputList.Count == 3) ? Formula.BitsToLinear((uint)inputList[0], $"float<{inputList[1]},{inputList[2]}>") : 0 },
                { "Val2Reg_float", inputList => (inputList.Count == 3) ? Formula.LinearToBits(inputList[0], $"float<{inputList[1]},{inputList[2]}>") : 0 },
                { "Fix2Double", ArithmetricFunctions.Fix2Double },
                { "Double2Fix", ArithmetricFunctions.Double2Fix },
                { "Double2Bin", ArithmetricFunctions.Double2Bin },
                { "Bin2Double", ArithmetricFunctions.Bin2Double },
                { "sum", ArithmetricFunctions.Sum },
                { "sin", inputList => Math.Sin(inputList[0]) },
                { "cos", inputList => Math.Cos(inputList[0]) },
                { "log", inputList => Math.Log10(inputList[0]) }
            };

            var arithmetricStringFunctions = new Dictionary<string, Func<IList<string>, string>>
            {
                 { "d", inputList => Expr.Parse(inputList[0]).Differentiate(Expr.Parse(inputList[1])).ToString() },
                 { "exp", inputList => Expr.Parse(inputList[0]).Expand().ToString() },
                 { "taylor", ArithmetricStringFunctions.Taylor },
                 { "eval", inputList => Expr.Parse(inputList[0]).ToString() },
                 { "LinearToBits", inputList => Formula.LinearToBits(double.Parse(inputList[0], CultureInfo.InvariantCulture), inputList[1]).ToString(CultureInfo.InvariantCulture) },
                 { "BitsToLinear", inputList => Formula.BitsToLinear(uint.Parse(inputList[0], CultureInfo.InvariantCulture), inputList[1]).ToString(CultureInfo.InvariantCulture) },
                 { "solve", EquationSolver.Solve }
            };

            var variableList = new List<string> { "x", "y", "z" };

            var ruleSet = new RuleSet(
                arithmetricOperators,
                arithmetricFunctions,
                arithmetricStringFunctions,
                stringFunctions: null,
                stringOperators: null,
                variableList);

            return ruleSet;
        }
    }
}
