using CalculationAlgorithm;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

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

        [Test]
        public void When_calculation_string_contains_logic_rules_then_corresponding_result_is_calculated_3()
        {
            var valueDict = new Dictionary<string, IList<string>>
            {
                { "CalenderWeek", new List<string> {"36", "37", "38", "39"} },
                { "Street", new List<string> { "Lindenstraße", "Poststraße", "Rheinstrasse", "Schillerstrasse" } },
                { "HouseNumber", new List<string> {"2", "9", "13", "23"} },
                { "PlotArea", new List<string> {"550", "600", "650", "700"} },
                { "OldBuilding", new List<string> { "Bürohaus", "Kaufhaus", "Tankstelle", "Wohnhaus" } },
                { "NewBuilding", new List<string> { "Bürohaus", "Kindergarten", "Parkhaus", "Wohnhaus" } }
            };

            var ruleSet = CreateRuleSet(valueDict);

            var calculationAlgorithm = CalculationAlgorithmFactory.Create(ruleSet);

            var calcTreeResult = calculationAlgorithm.CreateCalcTreeResult("(OldBuilding(x) == Wohnhaus) & (PlotArea(x) > PlotArea(y)) -> (Street(y) == Schillerstrasse)");

            calcTreeResult.SetVariable("x", 0);
            calcTreeResult.SetVariable("y", 1);

            Assert.AreEqual("1", calcTreeResult.GetResultString());

            var calcTreeResult2 = calculationAlgorithm.CreateCalcTreeResult("(OldBuilding(x) == Bürohaus) & (PlotArea(x) < PlotArea(y)) -> (Street(y) == Schillerstrasse)");

            calcTreeResult2.SetVariable("x", 0);
            calcTreeResult2.SetVariable("y", 1);

            Assert.AreEqual("0", calcTreeResult2.GetResultString());

            var calcTreeResult3 = calculationAlgorithm.CreateCalcTreeResult("(OldBuilding(x) == Bürohaus) & (PlotArea(x) < PlotArea(y)) -> (Street(y) == Poststraße)");

            calcTreeResult3.SetVariable("x", 0);
            calcTreeResult3.SetVariable("y", 1);

            Assert.AreEqual("1", calcTreeResult3.GetResultString());
        }

        private static RuleSet CreateRuleSet(IDictionary<string, IList<string>> valueDict)
        {
            var variableStringFunctions = ConvertValueDictToStringFunctions(valueDict);

            var stringFunctions = new Dictionary<string, Func<IList<string>, string>>
            {
                 { "!", inputList => ((double.Parse(inputList[0]) == 1) ? 0 : 1).ToString() },
            };

            var stringFunctionsResulting = stringFunctions.Concat(variableStringFunctions).ToDictionary(x => x.Key, x => x.Value);

            var stringOperators = new Dictionary<string, Tuple<int, Func<string, string, string>>>
            {
                { "+", new Tuple<int, Func<string, string, string>>(4, (x, y) => (double.Parse(x.Replace(".",",")) + double.Parse(x.Replace(".",","))).ToString()) },
                { "*", new Tuple<int, Func<string, string, string>>(4, (x, y) => (double.Parse(x.Replace(".",",")) * double.Parse(x.Replace(".",","))).ToString()) },
                { "==", new Tuple<int, Func<string, string, string>>(0, (x, y) =>  ((x == y) ? 1 : 0).ToString()) },
                { "->", new Tuple<int, Func<string, string, string>>(1, (x, y) => ((double.Parse(x)==0) || (double.Parse(y)==1) ? 1 : 0).ToString()) },
                { ">", new Tuple<int, Func<string, string, string>>(4, (x, y) => ((double.Parse(x.Replace(".",",")) > double.Parse(y)) ? 1 : 0).ToString()) },
                { "<", new Tuple<int, Func<string, string, string>>(4, (x, y) => ((double.Parse(x.Replace(".",",")) < double.Parse(y)) ? 1 : 0).ToString()) },
                { "&", new Tuple<int, Func<string, string, string>>(3, (x, y) => ((double.Parse(x)==1) && (double.Parse(y)==1) ? 1 : 0).ToString()) },
                { "v", new Tuple<int, Func<string, string, string>>(2, (x, y) => ((double.Parse(x)==1) || (double.Parse(y)==1) ? 1 : 0).ToString()) }
            };

            var variableList = new List<string> { "x", "y", "z" };

            var ruleSet = new RuleSet(
                null,
                null,
                null,
                stringFunctionsResulting,
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

