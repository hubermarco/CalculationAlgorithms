using CalculationAlgorithm;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace CalculationAlgorithmTests
{
    [TestFixture]
    public class CalculationAlgorithmWithVariablesTests
    {
        private const double _delta = 1e-15;
        private ICalculationAlgorithm _calculationAlgorithm;

        [SetUp]
        public void Setup()
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
                { "sum",
                    inputList =>
                    {
                        var sum = 0.0;
                        foreach(var input in inputList)
                        {
                            sum += input;
                        }
                        return sum;
                    }
                },
                { "sin", inputList => Math.Sin(inputList[0]) },
                { "cos", inputList => Math.Cos(inputList[0]) },
                { "log", inputList => Math.Log10(inputList[0]) },
                { "plus", inputList => inputList[0] + inputList[1] },
                { "IsGreater", inputList => inputList[0] > inputList[1] ? 1 : 0 },
                { "plusplus", inputList => inputList[0] + inputList[1] + inputList[2]}
            };

            var variableList = new List<string> { "x", "y", "z" };

            var ruleSet = new RuleSet(
                arithmetricOperators,
                arithmetricFunctions,
                null,
                variableList);

            _calculationAlgorithm = CalculationAlgorithmFactory.Create(ruleSet);
        }

        [Test]
        public void When_addition_is_performed_then_result_is_as_exptect()
        {
            var calcTreeResult = _calculationAlgorithm.CreateCalcTreeResult("(3+x)*x+y");

            calcTreeResult.SetVariable("x", 4);
            calcTreeResult.SetVariable("y", 5);

            var result = calcTreeResult.GetResult();

            Assert.AreEqual(33, result, _delta);
        }

        [Test]
        public void When_addition_is_performed_then_result_is_as_exptect_01()
        {
            var calcTreeResult = _calculationAlgorithm.CreateCalcTreeResult("(x^2+y^2)^0.5");

            calcTreeResult.SetVariable("x", 3);
            calcTreeResult.SetVariable("y", 4);

            var result = calcTreeResult.GetResult();

            Assert.AreEqual(5, result, _delta);
        }
    }
}
