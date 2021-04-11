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
    }
}
