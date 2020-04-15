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
        public void When_debugger_string_conataining_list_with_key_value_pairs_is_converted_then_corresponding_result_is_returned()
        {
            var currentDirectory = GetCurrentDirectory();
            var inputPath = currentDirectory + "\\" + "CurveConverterInput.txt";
            var debuggerString = File.ReadAllText(inputPath);

            var matlabGridString = string.Empty;
            var matlabCurveString = string.Empty;
            var cSharpGridString = string.Empty;
            var cSharpCurveString = string.Empty;
            var curve = new List<double>();
            var grid = new List<double>();

            CurveConverter.ConvertDebuggerString(
                debuggerString,
                ref matlabGridString,
                ref matlabCurveString,
                ref cSharpGridString,
                ref cSharpCurveString,
                ref curve,
                ref grid);

            Assert.AreEqual(228, curve.Count);
            Assert.AreEqual(228, grid.Count);
        }

        [Test]
        public void When_debugger_string_containing_simple_list_is_converted_then_corresponding_result_is_returned()
        {
            var currentDirectory = GetCurrentDirectory();
            var inputPath = currentDirectory + "\\" + "CurveInput.txt";
            var debuggerString = File.ReadAllText(inputPath);

            var matlabGridString = string.Empty;
            var matlabCurveString = string.Empty;
            var cSharpGridString = string.Empty;
            var cSharpCurveString = string.Empty;
            var curve = new List<double>();
            var grid = new List<double>();

            CurveConverter.ConvertDebuggerString(
                debuggerString,
                ref matlabGridString,
                ref matlabCurveString,
                ref cSharpGridString,
                ref cSharpCurveString,
                ref curve,
                ref grid);

            Assert.AreEqual(1000, curve.Count);
            Assert.AreEqual(0, grid.Count);
        }

        [Test]
        public void When_debugger_string_containing_dictionary_is_converted_then_corresponding_result_is_returned()
        {
            var currentDirectory = GetCurrentDirectory();
            var inputPath = currentDirectory + "\\" + "DictionaryInput.txt";
            var debuggerString = File.ReadAllText(inputPath);

            var matlabGridString = string.Empty;
            var matlabCurveString = string.Empty;
            var cSharpGridString = string.Empty;
            var cSharpCurveString = string.Empty;
            var curve = new List<double>();
            var grid = new List<double>();

            CurveConverter.ConvertDebuggerString(
                debuggerString,
                ref matlabGridString,
                ref matlabCurveString,
                ref cSharpGridString,
                ref cSharpCurveString,
                ref curve,
                ref grid);

            Assert.AreEqual(300, curve.Count);
            Assert.AreEqual(300, grid.Count);
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
