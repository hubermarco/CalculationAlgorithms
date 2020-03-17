using CalculationAlgorithm;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace CalculationAlgorithmTests
{
    [TestFixture]
    public class RuleSetTests
    {
        [Test]
        public void When_rule_set_is_created_then_operator_list_is_as_expected()
        {
            var arithmetricOperators = new Dictionary<string, Tuple<int, Func<double, double, double>>>
            {
                { "+", new Tuple<int, Func<double, double, double>>(0, (x, y) => x + y) },
                { "-", new Tuple<int, Func<double, double, double>>(0, (x, y) => x - y) },
                { "*", new Tuple<int, Func<double, double, double>>(1, (x, y) => x * y) },
                { "/", new Tuple<int, Func<double, double, double>>(1, (x, y) => x / y) },
                { "^", new Tuple<int, Func<double, double, double>>(1, (x, y) => Math.Pow(x,y)) }
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

            var variableList = new List<string> { "x", "y" };

            var ruleSet = new RuleSet(
                arithmetricOperators, 
                arithmetricFunctions,
                variableList: variableList);

            var operatorList = ruleSet.GetOperatorList();

            Assert.IsTrue(operatorList.Contains("+"));
            Assert.IsTrue(operatorList.Contains("-"));
            Assert.IsTrue(operatorList.Contains("*"));
            Assert.IsTrue(operatorList.Contains("/"));
            Assert.IsTrue(operatorList.Contains("^"));
            Assert.IsTrue(operatorList.Contains("sin"));
            Assert.IsTrue(operatorList.Contains("cos"));
            Assert.IsTrue(operatorList.Contains("log"));
            Assert.IsTrue(operatorList.Contains("x"));
            Assert.IsTrue(operatorList.Contains("y"));
        }
    }
}
