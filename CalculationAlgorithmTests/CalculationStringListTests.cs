using CalculationAlgorithm;
using NUnit.Framework;
using System.Collections.Generic;

namespace CalculatorTests
{
    [TestFixture]
    public class CalculationStringListTests
    {
        private readonly ICalculationStringList _calculationStringList;

        public CalculationStringListTests()
        {
            _calculationStringList = CalculationAlgorithmFactory.CreateCalculationStringList(
                operatorList: new List<string>  { "+", "-", "*", "/", "^", "log", "sin", "cos", "x", "y" },
                arithmetricStringOperatorList: new List<string> { "Differentiate" });
        }

        [Test]
        public void When_create_input_string_list_is_called_then_corresponding_string_list_is_created()
        {
            var inputList = _calculationStringList.Create("3+5");

            Assert.AreEqual("3", inputList[0]);
            Assert.AreEqual("+", inputList[1]);
            Assert.AreEqual("5", inputList[2]);
            Assert.AreEqual(3, inputList.Count);
        }

        [Test]
        public void When_create_input_string_list_is_called_then_corresponding_string_list_is_created_2()
        {
            var inputList = _calculationStringList.Create("3.2+5.9");

            Assert.AreEqual("3.2", inputList[0]);
            Assert.AreEqual("+", inputList[1]);
            Assert.AreEqual("5.9", inputList[2]);
            Assert.AreEqual(3, inputList.Count);
        }

        [Test]
        public void When_create_input_string_list_is_called_then_corresponding_string_list_is_created_3()
        {
            var inputList = _calculationStringList.Create("(3.2+5.9)*3");

            Assert.AreEqual("(", inputList[0]);
            Assert.AreEqual("3.2", inputList[1]);
            Assert.AreEqual("+", inputList[2]);
            Assert.AreEqual("5.9", inputList[3]);
            Assert.AreEqual(")", inputList[4]);
            Assert.AreEqual("*", inputList[5]);
            Assert.AreEqual("3", inputList[6]);
            Assert.AreEqual(7, inputList.Count);
        }

        [Test]
        public void When_create_input_string_list_is_called_then_corresponding_string_list_is_created_4()
        {
            var inputList = _calculationStringList.Create("log(3.2+5.9)*sin(3.4)");

            Assert.AreEqual("log", inputList[0]);
            Assert.AreEqual("(", inputList[1]);
            Assert.AreEqual("3.2", inputList[2]);
            Assert.AreEqual("+", inputList[3]);
            Assert.AreEqual("5.9", inputList[4]);
            Assert.AreEqual(")", inputList[5]);
            Assert.AreEqual("*", inputList[6]);
            Assert.AreEqual("sin", inputList[7]);
            Assert.AreEqual("(", inputList[8]);
            Assert.AreEqual("3.4", inputList[9]);
            Assert.AreEqual(")", inputList[10]);
            Assert.AreEqual(11, inputList.Count);
        }

        [Test]
        public void When_create_input_string_list_is_called_then_corresponding_string_list_is_created_5()
        {
            var inputList = _calculationStringList.Create("(2*x+3+y)^2");

            Assert.AreEqual("(", inputList[0]);
            Assert.AreEqual("2", inputList[1]);
            Assert.AreEqual("*", inputList[2]);
            Assert.AreEqual("x", inputList[3]);
            Assert.AreEqual("+", inputList[4]);
            Assert.AreEqual("3", inputList[5]);
            Assert.AreEqual("+", inputList[6]);
            Assert.AreEqual("y", inputList[7]);
            Assert.AreEqual(")", inputList[8]);
            Assert.AreEqual("^", inputList[9]);
            Assert.AreEqual("2", inputList[10]);
            Assert.AreEqual(11, inputList.Count);
        }

        [Test]
        public void When_create_input_string_list_is_called_with_string_operator_list_then_corresponding_string_list_is_created()
        {
            var inputList = _calculationStringList.Create("Differentiate((x+1)^9,x)");

            Assert.AreEqual("Differentiate", inputList[0]);
            Assert.AreEqual("(", inputList[1]);
            Assert.AreEqual("(x+1)^9", inputList[2]);
            Assert.AreEqual(",", inputList[3]);
            Assert.AreEqual("x", inputList[4]);
            Assert.AreEqual(")", inputList[5]);
           
            Assert.AreEqual(6, inputList.Count);
        }

        [Test]
        public void When_create_input_string_list_is_called_with_nested_operators_with_string_operator_list_then_corresponding_string_list_is_created()
        {
            var inputList = _calculationStringList.Create("Differentiate(Differentiate((x+1)^9,x),x)");

            Assert.AreEqual("Differentiate", inputList[0]);
            Assert.AreEqual("(", inputList[1]);
            Assert.AreEqual("Differentiate", inputList[2]);
            Assert.AreEqual("(", inputList[3]);
            Assert.AreEqual("(x+1)^9", inputList[4]);
            Assert.AreEqual(",", inputList[5]);
            Assert.AreEqual("x", inputList[6]);
            Assert.AreEqual(")", inputList[7]);
            Assert.AreEqual(",", inputList[8]);
            Assert.AreEqual("x", inputList[9]);
            Assert.AreEqual(")", inputList[10]);

            Assert.AreEqual(11, inputList.Count);
        }
    }
}
