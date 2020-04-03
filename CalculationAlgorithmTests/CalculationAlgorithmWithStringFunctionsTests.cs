using CalculationAlgorithm;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace CalculationAlgorithmTests
{
    [TestFixture]
    public class CalculationAlgorithmWithStringFunctionsTests
    {
        [Test]
        public void When_calculation_string_contains_logic_rules_then_corresponding_result_is_calculated()
        {
            var valueDict = new Dictionary<string, IList<string>>
            {
                { "CalenderWeek", new List<string> {"36", "37", "38", "39"} },
                { "HouseNumber", new List<string> {"2", "9", "13", "23"} },
            };

            var ruleSet = CreateRuleSet(valueDict);

            var calculationAlgorithm = CalculationAlgorithmFactory.Create(ruleSet);

            var calcTreeResult = calculationAlgorithm.CreateCalcTreeResult("(CalenderWeek(x) == 37) -> (HouseNumber(x) == 9)");

            calcTreeResult.SetVariable("x", 0);

            Assert.AreEqual("1", calcTreeResult.GetResultString());

            calcTreeResult.SetVariable("x", 1);

            Assert.AreEqual("1", calcTreeResult.GetResultString());
        }

        [Test]
        public void When_calculation_string_contains_logic_rules_then_corresponding_result_is_calculated_2()
        {
            var valueDict = new Dictionary<string, IList<string>>
            {
                { "CalenderWeek", new List<string> {"36", "37", "38", "39"} },
                { "HouseNumber", new List<string> {"2", "9", "13", "23"} },
            };

            var ruleSet = CreateRuleSet(valueDict);

            var calculationAlgorithm = CalculationAlgorithmFactory.Create(ruleSet);

            var calcTreeResult = calculationAlgorithm.CreateCalcTreeResult("(CalenderWeek(x) == 36) -> (HouseNumber(x) == 9)");

            calcTreeResult.SetVariable("x", 0);

            Assert.AreEqual("0", calcTreeResult.GetResultString());

            calcTreeResult.SetVariable("x", 1);

            Assert.AreEqual("1", calcTreeResult.GetResultString());
        }

        [Test]
        public void When_calculation_string_contains_logic_rules_and_ruleset_is_created_automatically_then_corresponding_result_is_calculated()
        {
            var valueDict = new Dictionary<string, IList<string>>
            {
                { "CalenderWeek", new List<string> {"36", "37", "38", "39"} },
                { "HouseNumber", new List<string> {"2", "9", "13", "23"} },
            };

            var ruleSet = CreateRuleSet(valueDict);

            var calculationAlgorithm = CalculationAlgorithmFactory.Create(ruleSet);

            var calcTreeResult = calculationAlgorithm.CreateCalcTreeResult("(CalenderWeek(x) == 37) -> (HouseNumber(x) == 9)");

            calcTreeResult.SetVariable("x", 0);

            Assert.AreEqual("1", calcTreeResult.GetResultString());

            calcTreeResult.SetVariable("x", 1);

            Assert.AreEqual("1", calcTreeResult.GetResultString());

            var calcTreeResult2 = calculationAlgorithm.CreateCalcTreeResult("(CalenderWeek(x) == 36) -> (HouseNumber(x) == 9)");

            calcTreeResult2.SetVariable("x", 0);

            Assert.AreEqual("0", calcTreeResult2.GetResultString());

            calcTreeResult2.SetVariable("x", 1);

            Assert.AreEqual("1", calcTreeResult2.GetResultString());

            valueDict["CalenderWeek"] = new List<string> { "37", "36", "38", "39" };
            valueDict["HouseNumber"] = new List<string> { "23", "9", "13", "2" };

            var calcTreeResult3 = calculationAlgorithm.CreateCalcTreeResult("(CalenderWeek(x) == 37) -> (HouseNumber(x) == 23)");

            calcTreeResult3.SetVariable("x", 0);

            Assert.AreEqual("1", calcTreeResult3.GetResultString());
        }

        private static RuleSet CreateRuleSet(IDictionary<string, IList<string>> valueDict)
        {
            var stringFunctions = ConvertValueDictToStringFunctions(valueDict);

            var stringOperators = new Dictionary<string, Tuple<int, Func<string, string, string>>>
            {
                { "+", new Tuple<int, Func<string, string, string>>(0, (x, y) => (double.Parse(x.Replace(".",",")) + double.Parse(x.Replace(".",","))).ToString()) },
                { "*", new Tuple<int, Func<string, string, string>>(0, (x, y) => (double.Parse(x.Replace(".",",")) * double.Parse(x.Replace(".",","))).ToString()) },
                { "==", new Tuple<int, Func<string, string, string>>(0, (x, y) =>  ((x == y) ? 1 : 0).ToString()) },
                { "->", new Tuple<int, Func<string, string, string>>(0, (x, y) => ((double.Parse(x)==0) || (double.Parse(y)==1) ? 1 : 0).ToString()) },
                { ">", new Tuple<int, Func<string, string, string>>(0, (x, y) => ((double.Parse(x.Replace(".",",")) > double.Parse(y)) ? 1 : 0).ToString()) },
                { "<", new Tuple<int, Func<string, string, string>>(0, (x, y) => ((double.Parse(x.Replace(".",",")) < double.Parse(y)) ? 1 : 0).ToString()) },
                { "&", new Tuple<int, Func<string, string, string>>(0, (x, y) => ((double.Parse(x)==1) && (double.Parse(y)==1) ? 1 : 0).ToString()) },
                { "v", new Tuple<int, Func<string, string, string>>(0, (x, y) => ((double.Parse(x)==1) || (double.Parse(y)==1) ? 1 : 0).ToString()) }
            };

            var variableList = new List<string> { "x", "y", "z" };

            var ruleSet = new RuleSet(
                null,
                null,
                null,
                stringFunctions,
                stringOperators,
                variableList);

            return ruleSet;
        }

        private static Dictionary<string, Func<IList<string>, string>> ConvertValueDictToStringFunctions(
            IDictionary<string, IList<string>> valueDict)
        {
            var stringFunctions = new Dictionary<string, Func<IList<string>, string>>();

            foreach (var value in valueDict)
            {
                stringFunctions.Add(value.Key, inputList =>
                {
                    var list = value.Value;
                    return list[int.Parse(inputList[0])];
                });
            }

            return stringFunctions;
        }
    }
}

