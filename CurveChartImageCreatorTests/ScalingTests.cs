using CurveChartImageCreater;
using NUnit.Framework;
using System.Collections.Generic;

namespace CurveChartImageCreatorTests
{
    [TestFixture]
    public class ScalingTests
    {
        [Test]
        public void When_curve_has_not_elements_and_apply_method_is_called_then_an_empty_curve_is_returned()
        {
            IList<double> curveOutput = new List<double>();
            var scalingExponent = 0;

            var curve = new List<double>();

            Scaling.Apply(
               curve: curve,
               minExponent: 0,
               maxExponent: 2,
               curveOutput: ref curveOutput,
               scalingExponent: ref scalingExponent);

            Assert.AreEqual(0, scalingExponent);
            Assert.AreEqual(0, curveOutput.Count, "curveOutput is not supposed to have elements");
        }

        [Test]
        public void When_curve_is_a_null_reference_and_apply_method_is_called_then_an_empty_curve_is_returned()
        {
            IList<double> curveOutput = new List<double>();
            var scalingExponent = 0;

            Scaling.Apply(
               curve: null,
               minExponent: 0,
               maxExponent: 2,
               curveOutput: ref curveOutput,
               scalingExponent: ref scalingExponent);

            Assert.AreEqual(0, scalingExponent);
            Assert.IsNull(curveOutput, "curveOutput NOT null");
        }

        [Test]
        public void When_curve_is_set_to_apply_method_then_corresponding_output_curve_is_returned()
        {
            IList<double> curveOutput = new List<double>();
            var scalingExponent = 0;
            var curve = new List<double> { 10, 20, 30 };
            var outputCurveExpected = new List<double> { 100, 200, 300 };

            Scaling.Apply(
               curve: curve,
               minExponent: 2,
               maxExponent: 2,
               curveOutput: ref curveOutput,
               scalingExponent: ref scalingExponent);

            Assert.AreEqual(-1, scalingExponent);
            CollectionAssert.AreEqual(outputCurveExpected, curveOutput);
        }

        [Test]
        public void When_curve_is_set_to_apply_method_then_corresponding_output_curve_is_returned_2()
        {
            IList<double> curveOutput = new List<double>();
            var scalingExponent = 0;
            var curve = new List<double> { 0.01, 0.02, 0.03 };
            var outputCurveExpected = new List<double> { 10, 20, 30 };

            Scaling.Apply(
               curve: curve,
               minExponent: 1,
               maxExponent: 1,
               curveOutput: ref curveOutput,
               scalingExponent: ref scalingExponent);

            Assert.AreEqual(-3, scalingExponent);
            CollectionAssert.AreEqual(outputCurveExpected, curveOutput);
        }

        [Test]
        public void When_curves_have_different_number_of_elements_and_apply_method_is_called_then_an_empty_curve_is_returned()
        {
            IList<double> curveOutput1 = new List<double>();
            IList<double> curveOutput2 = new List<double>();
            var scalingExponent = 0;

            var curve1 = new List<double> { 1, 2, 3 };
            var curve2 = new List<double> { 1, 2, 3, 4 };

            Scaling.Apply(
               curve1: curve1,
               curve2: curve2,
               minExponent: 0,
               maxExponent: 2,
               curveOutput1: ref curveOutput1,
               curveOutput2: ref curveOutput2,
               scalingExponent: ref scalingExponent);

            Assert.AreEqual(0, scalingExponent);
            CollectionAssert.AreEqual(curve1, curveOutput1, "curveOutput1 NOT equal to curve1");
            CollectionAssert.AreEqual(curve2, curveOutput2, "curveOutput2 NOT equal to curve2");
        }

        [Test]
        public void When_one_curve_is_a_null_reference_and_apply_method_is_called_then_an_empty_curve_is_returned()
        {
            IList<double> curveOutput1 = new List<double>();
            IList<double> curveOutput2 = new List<double>();
            var scalingExponent = 0;

            var curve1 = new List<double> { 1, 2, 3 };
            IList<double> curve2 = null;

            Scaling.Apply(
               curve1: new List<double> { 1, 2, 3 },
               curve2: null,
               minExponent: 0,
               maxExponent: 2,
               curveOutput1: ref curveOutput1,
               curveOutput2: ref curveOutput2,
               scalingExponent: ref scalingExponent);

            Assert.AreEqual(0, scalingExponent);
            CollectionAssert.AreEqual(curve1, curveOutput1, "curveOutput1 NOT equal to curve1");
            CollectionAssert.AreEqual(curve2, curveOutput2, "curveOutput2 NOT equal to curve2");
        }

        [Test]
        public void When_one_curve_is_a_null_reference_and_apply_method_is_called_then_an_empty_curve_is_returned_2()
        {
            IList<double> curveOutput1 = new List<double>();
            IList<double> curveOutput2 = new List<double>();
            var scalingExponent = 0;

            var curve1 = new List<double> { 10, 20, 30 };
            IList<double> curve2 = null;

            Scaling.Apply(
               curve1: curve1,
               curve2: curve2,
               minExponent: 1,
               maxExponent: 1,
               curveOutput1: ref curveOutput1,
               curveOutput2: ref curveOutput2,
               scalingExponent: ref scalingExponent);

            Assert.AreEqual(0, scalingExponent);
            CollectionAssert.AreEqual(curve1, curveOutput1, "curveOutput1 NOT equal to curve1");
            CollectionAssert.AreEqual(curve2, curveOutput2, "curveOutput2 NOT equal to curve2");
        }

        [Test]
        public void When_apply_method_is_called_then_an_corresponding_curves_are_returned()
        {
            IList<double> curveOutput1 = new List<double>();
            IList<double> curveOutput2 = new List<double>();
            var scalingExponent = 0;

            var curve1 = new List<double> { 0.01, 0.02, 0.03 };
            var curve2 = new List<double> { 0.04, 0.01, 0.02 };

            var curveOutputExpected1 = new List<double> { 1, 2, 3 };
            var curveOutputExpected2 = new List<double> { 4, 1, 2 };

            Scaling.Apply(
               curve1: curve1,
               curve2: curve2,
               minExponent: 0,
               maxExponent: 2,
               curveOutput1: ref curveOutput1,
               curveOutput2: ref curveOutput2,
               scalingExponent: ref scalingExponent);

            Assert.AreEqual(-2, scalingExponent);
            CollectionAssert.AreEqual(curveOutputExpected1, curveOutput1, "curveOutput1 NOT equal to curve1");
            CollectionAssert.AreEqual(curveOutputExpected2, curveOutput2, "curveOutput2 NOT equal to curve2");
        }
    }
}
