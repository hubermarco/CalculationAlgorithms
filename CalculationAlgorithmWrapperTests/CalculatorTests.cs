using CalculationAlgorithmWrapper;
using NUnit.Framework;

namespace CalculationAlgorithmWrapperTests
{
    [TestFixture]
    public class CalculatorTests
    {
        private ICalculator _calculator;

        [SetUp]
        public void SetUp()
        {
            _calculator = CalculatorFactory.Create();
        }

        [Test]
        public void When_multiply_2_numbers_then_corresponding_result_is_returned()
        {
            _calculator.SetKey("3");
            _calculator.SetKey("*");
            _calculator.SetKey("4");
            var resultString = _calculator.Calculate();

            Assert.AreEqual("3*4\n= 12", resultString);
        }

        [Test]
        public void When_fix_2_double_is_called_then_corresponding_result_is_returned()
        {
            _calculator.SetKey("Fix2Double(");
            _calculator.SetKey("27");
            _calculator.SetKey(",");
            _calculator.SetKey("5");
            _calculator.SetKey(",");
            _calculator.SetKey("2");
            _calculator.SetKey(")");

            var resultString = _calculator.Calculate();

            Assert.AreEqual("Fix2Double(27,5,2)\n= -1,25", resultString);
        }

        [Test]
        public void When_fix_2_double_is_called_in_a_nested_way_then_corresponding_result_is_returned()
        {
            var result = _calculator.Calculate("Fix2Double(Fix2Double(4+4,6,1),6,1");

            Assert.AreEqual(2, result);
        }

        [Test]
        public void When_fix_2_double_is_called_with_2_input_arguments_then_0_is_returned()
        {
            var result = _calculator.Calculate("Fix2Double(27,5)");

            Assert.AreEqual(0, result);
        }

        [Test]
        public void When_double_2_fix_is_called_with_nested_fix_2_double_then_input_value_is_returned()
        {
            var result = _calculator.Calculate("Double2Fix(Fix2Double(27,5,2),5,2)");

            Assert.AreEqual(27, result);
        }

        [Test]
        public void When_fix_2_double_is_called_with_nested_doulbe_2_fix_then_input_value_is_returned()
        {
            var result = _calculator.Calculate("Fix2Double(Double2Fix(-1.25,5,2),5,2)");

            Assert.AreEqual(-1.25, result);
        }

        [Test]
        public void When_double_2_fix_is_called_then_correct_value_is_returned()
        {
            var result = _calculator.Calculate("Double2Fix(-1.25,5,2)");

            Assert.AreEqual(27, result);
        }

        [Test]
        public void When_double_2_bool_is_called_then_correct_value_is_returned()
        {
            var result = _calculator.Calculate("Double2Bool(-1, 6, 0)");

            Assert.AreEqual(111111, result);
        }

        [Test]
        public void When_bool_2_double_is_called_then_correct_value_is_returned()
        {
            var result = _calculator.Calculate("Bool2Double(1111, 4, 1)");

            Assert.AreEqual(-0.5, result);
        }

        [Test]
        public void When_bool_2_double_is_called_for_a_number_with_16_digits_then_correct_value_is_returned()
        {
            var result = _calculator.Calculate("Bool2Double(1111 1111 1111 1111, 16, 0)");

            Assert.AreEqual(-1, result);
        }

        [Test]
        public void When_calculcate_string_has_unknown_letters_then_0_is_returned()
        {
            var result = _calculator.Calculate("5+oo-7");

            Assert.AreEqual(0, result);
        }

        [Test]
        public void When_differentiate_is_performed_in_a_nested_way_then_corresponding_result_is_returned_3()
        {
            var stringResult = _calculator.CalculateString("exp(d((x+4)^2,x)))");

            Assert.AreEqual("8 + 2*x", stringResult);
        }

        [Test]
        public void When_calculate_and_return_string_method_is_called_with_string_function_then_corresponding_result_is_returned()
        {
            var stringResult = _calculator.CalculateAndReturnString("exp(d((x + 4) ^ 2, x)))");

            Assert.AreEqual("exp(d((x + 4) ^ 2, x)))\n= 8 + 2*x", stringResult);
        }

        [Test]
        public void When_taylor_string_function_is_calculated_then_corresponding_result_is_returned()
        {
            var stringResult = _calculator.CalculateAndReturnString("taylor(sin(x)+cos(x), x, 0, 4)");

            Assert.AreEqual("taylor(sin(x)+cos(x), x, 0, 4)\n= 1 + x - x^2/2 - x^3/6", stringResult);
        }

        [Test]
        public void When_eval_string_function_is_calculated_then_corresponding_result_is_returned()
        {
            var stringResult = _calculator.CalculateAndReturnString("eval(x+2+5)");

            Assert.AreEqual("eval(x+2+5)\n= 7 + x", stringResult);
        }

        [Test]
        public void When_calculate_and_return_string_method_is_called_with_arithmetric_function_then_corresponding_result_is_returned()
        {
            var stringResult = _calculator.CalculateAndReturnString("log(1000)");

            Assert.AreEqual("log(1000)\n= 3", stringResult);
        }
    }
}
