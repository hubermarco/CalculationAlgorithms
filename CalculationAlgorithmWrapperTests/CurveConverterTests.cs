using CalculationAlgorithmWrapper;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;

namespace CalculatorAlgorithmsWrapperTests
{
    [TestFixture]
    public class CurveConverterTests
    {
        [Test]
        public void When_debugger_string_is_converted_then_corresponding_result_is_returned()
        {
            var currentDirectory = GetCurrentDirectory();
            var inputPath = currentDirectory + "\\" + "CurveConverterInput.txt";
            var debuggerString = File.ReadAllText(inputPath);

            var matlabCurveString = string.Empty;
            var cSharpCurveString = string.Empty;
            var curve = new List<double>();

            CurveConverter.ConvertDebuggerString(
                debuggerString,
                ref matlabCurveString,
                ref cSharpCurveString,
                ref curve);

            Assert.AreEqual(228, curve.Count);
        }

        private string GetCurrentDirectory()
        {
            var executionDirectory = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);
            var binDirectory = Directory.GetParent(executionDirectory).ToString();
            var currentDirectory = Directory.GetParent(binDirectory).ToString();

            return currentDirectory;
        }
    }
}
