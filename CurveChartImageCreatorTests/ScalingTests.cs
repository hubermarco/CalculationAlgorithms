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

            Assert.AreEqual(0, curveOutput.Count, "curveOutput is not supposed to have elements");
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

            CollectionAssert.AreEqual(curve1, curveOutput1, "curveOutput1 NOT equal to curve1");
            CollectionAssert.AreEqual(curve2, curveOutput2, "curveOutput2 NOT equal to curve2");
        }
    }
}
