using CalculatorAlgorithmsWrapper;
using NUnit.Framework;


namespace CalculatorAlgorithmsWrapperTests
{
    [TestFixture]
    public class ConvertersTests
    {
        [Test]
        public void When_fix_2_double_was_called_then_corresponding_result_is_returned()
        {
            var result = Converters.Fix2Double(27, 5, 2);
            Assert.AreEqual(-1.25, result);
        }

        [Test]
        public void When_fix_2_double_was_called_then_corresponding_result_is_returned_2()
        {
            var result = Converters.Fix2Double(27, 6, 0);
            Assert.AreEqual(27, result);
        }

        [Test]
        public void When_double_2_fix_was_called_then_corresponding_result_is_returned()
        {
            var result = Converters.Double2Fix(27, 6, 0);
            Assert.AreEqual(27, result);
        }

        [Test]
        public void When_double_2_fix_was_called_then_corresponding_result_is_returned_2()
        {
            var result = Converters.Double2Fix(-1.25, 5, 2);
            Assert.AreEqual(27, result);
        }
    }
}
