using CalculatorAlgorithmsWrapper;
using NUnit.Framework;

namespace CalculatorAlgorithmsWrapperTests
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
    }
}
