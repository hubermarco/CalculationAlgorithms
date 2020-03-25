
using CalculationAlgorithm;
using CalculatorAlgorithmsWrapper;
using NUnit.Framework;
using System.Collections.Generic;

namespace CalculatorAlgorithmsWrapperTests
{
    [TestFixture]
    public class CalculationStringWrapperTests
    {
        private ICalculationStringWrapper _calculationStringWrapper;

        [SetUp]
        public void SetUp()
        {
            var operatorList = new List<string> { "+", "-", "*", "/" };
            _calculationStringWrapper = new CalculationStringWrapper(CalculationAlgorithmFactory.CreateCalculationStringList(operatorList));
        }

        [Test]
        public void When_set_key_is_called_then_key_string_is_added_to_input_string()
        {
            _calculationStringWrapper.SetKey("3");
            _calculationStringWrapper.SetKey("+");
            var inputString = _calculationStringWrapper.SetKey("5");

            Assert.AreEqual("3+5", inputString);

            inputString = _calculationStringWrapper.SetKey("+");
            Assert.AreEqual("3+5+", inputString);
        }

        [Test]
        public void When_set_key_is_called_and_input_string_ends_with_plus_then_key_string_is_not_added_to_input_string()
        {
            _calculationStringWrapper.SetKey("3");
            _calculationStringWrapper.SetKey("+");
            _calculationStringWrapper.SetKey("5");
            _calculationStringWrapper.SetKey("+");
            var inputString = _calculationStringWrapper.SetKey("+");

            Assert.AreEqual("3+5+", inputString);
        }

        [Test]
        public void When_set_key_is_called_and_input_string_ends_with_a_digit_then_key_string_is_not_added_to_input_string()
        {
            _calculationStringWrapper.SetKey("3");
            _calculationStringWrapper.SetKey("+");
            _calculationStringWrapper.SetKey("5");
            var inputString = _calculationStringWrapper.SetKey("(");

            Assert.AreEqual("3+5", inputString);
        }

        [Test]
        public void When_set_key_is_called_and_input_string_ends_with_a_close_bracket_then_not_allowed_key_string_is_not_added_to_input_string()

        {
            _calculationStringWrapper.SetKey("(");
            _calculationStringWrapper.SetKey("6");
            _calculationStringWrapper.SetKey(")");
            var inputString = _calculationStringWrapper.SetKey("(");

            Assert.AreEqual("(6)", inputString);
        }

        [Test]
        public void  When_set_key_is_called_and_input_string_ends_with_a_close_bracket_then_not_allowed_key_string_is_not_added_to_input_string_2()
        {
            _calculationStringWrapper.SetKey("(");
            _calculationStringWrapper.SetKey("6");
            _calculationStringWrapper.SetKey(")");
            var inputString = _calculationStringWrapper.SetKey(".");

            Assert.AreEqual("(6)", inputString);
        }

        [Test]
        public void When_set_key_is_called_and_input_string_ends_with_a_decimal_number_then_dot_key_string_is_not_added_to_input_string ()
        {
            _calculationStringWrapper.SetKey("6");
            _calculationStringWrapper.SetKey(".");
            _calculationStringWrapper.SetKey("3");
            var inputString = _calculationStringWrapper.SetKey(".");

            Assert.AreEqual("6.3", inputString);
        }

        [Test]
        public void When_set_key_is_called_and_input_string_is_empty_then_not_all_keys_are_allowed_to_add()
        {
            var inputString = _calculationStringWrapper.SetKey(")");
            Assert.AreEqual("", inputString);

            inputString = _calculationStringWrapper.SetKey("*");
            Assert.AreEqual("", inputString);

            inputString = _calculationStringWrapper.SetKey("/");
            Assert.AreEqual("", inputString);

            inputString = _calculationStringWrapper.SetKey("+");
            Assert.AreEqual("", inputString);

            inputString = _calculationStringWrapper.SetKey("(");
            Assert.AreEqual("(", inputString);
        }

        [Test]
        public void When_close_brackets_are_more_than_open_brackets_then_input_string_is_not_appended()
        {
            _calculationStringWrapper.SetKey("(");
            _calculationStringWrapper.SetKey("3");
            _calculationStringWrapper.SetKey("+");
            _calculationStringWrapper.SetKey("5");
            var inputString = _calculationStringWrapper.SetKey(")");

            Assert.AreEqual("(3+5)", inputString);

            inputString = _calculationStringWrapper.SetKey(")");

            Assert.AreEqual("(3+5)", inputString);
        }

        [Test]
        public void When_close_brackets_are_more_than_open_brackets_then_input_string_is_not_appended_2()
        {
            _calculationStringWrapper.SetKey("3");
            _calculationStringWrapper.SetKey("+");
            var inputString = _calculationStringWrapper.SetKey("5");

            Assert.AreEqual("3+5", inputString);

            inputString = _calculationStringWrapper.SetKey(")");

            Assert.AreEqual("3+5", inputString);
        }

        [Test]
        public void When_two_decimal_number_are_added_then_input_string_is_appended_2()
        {
            _calculationStringWrapper.SetKey("3");
            _calculationStringWrapper.SetKey(".");
            _calculationStringWrapper.SetKey("5");
            _calculationStringWrapper.SetKey("+");
            var inputString = _calculationStringWrapper.SetKey("4");

            Assert.AreEqual("3.5+4", inputString);

            inputString = _calculationStringWrapper.SetKey(".");

            Assert.AreEqual("3.5+4.", inputString);
        }

        [Test]
        public void When_input_string_is_valid_then_true_is_returned()
        {
            Assert.IsTrue(_calculationStringWrapper.IsCalculationValid("3+4"));
        }

        [Test]
        public void When_input_string_is_not_valid_then_false_is_returned()
        {
            Assert.IsFalse(_calculationStringWrapper.IsCalculationValid("3--4"));
        }
    }
}