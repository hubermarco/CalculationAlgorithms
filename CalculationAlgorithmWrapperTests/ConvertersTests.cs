using CalculationAlgorithmWrapper;
using NUnit.Framework;


namespace CalculationAlgorithmWrapperTests
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

        [Test]
        public void When_double_2_bool_was_called_then_corresponding_result_is_returned()
        {
            var result = Converters.Double2Bin(3, 5, 0);
            Assert.AreEqual(11, result);
        }

        [Test]
        public void When_double_2_bool_was_called_then_corresponding_result_is_returned_2()
        {
            var result = Converters.Double2Bin(7, 5, 0);
            Assert.AreEqual(111, result);
        }

        [Test]
        public void When_double_2_bool_was_called_then_corresponding_result_is_returned_3()
        {
            var result = Converters.Double2Bin(-1, 6, 0);
            Assert.AreEqual(111111, result);
        }

        [Test]
        public void When_bool_2_fix_was_called_then_corresponding_result_is_returned()
        {
            var result = Converters.Bool2Fix(111);
            Assert.AreEqual(7, result);
        }

        [Test]
        public void When_bool_2_double_was_called_then_corresponding_result_is_returned()
        {
            var result = Converters.Bin2Double(111, 4, 0);
            Assert.AreEqual(7, result);
        }

        [Test]
        public void When_bool_2_double_was_called_then_corresponding_result_is_returned_2()
        {
            var result = Converters.Bin2Double(1111, 4, 1);
            Assert.AreEqual(-0.5, result);
        }
    }
}
