using CalculationAlgorithmWrapper;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;

namespace CalculationAlgorithmWrapperTests
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

            CurveConverter.ConvertInputString(
                inputString: debuggerString,
                inputFormat: InputFormat.Debug,
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

            CurveConverter.ConvertInputString(
                inputString: debuggerString,
                inputFormat: InputFormat.Debug,
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

            CurveConverter.ConvertInputString(
                inputString: debuggerString,
                inputFormat: InputFormat.Debug,
                ref matlabGridString,
                ref matlabCurveString,
                ref cSharpGridString,
                ref cSharpCurveString,
                ref curve,
                ref grid);

            Assert.AreEqual(300, curve.Count);
            Assert.AreEqual(300, grid.Count);
        }

        [Test]
        public void When_text_string_containing_list_with_of_curve_values_is_converted_then_corresponding_result_is_returned()
        {
            var currentDirectory = GetCurrentDirectory();
            var inputPath = currentDirectory + "\\" + "TextInput.txt";
            var inputString = File.ReadAllText(inputPath);

            var matlabGridString = string.Empty;
            var matlabCurveString = string.Empty;
            var cSharpGridString = string.Empty;
            var cSharpCurveString = string.Empty;
            var curve = new List<double>();
            var grid = new List<double>();

            CurveConverter.ConvertInputString(
                inputString: inputString,
                inputFormat: InputFormat.Text,
                ref matlabGridString,
                ref matlabCurveString,
                ref cSharpGridString,
                ref cSharpCurveString,
                ref curve,
                ref grid);

            Assert.AreEqual(228, curve.Count, "curve.Count");
            Assert.AreEqual(0, grid.Count, "grid.Count");
        }

        [Test]
        public void When_text_string_containing_curve_values_is_converted_then_corresponding_result_is_returned()
        {
            var inputString = "ert45,45,56,56 66 77pp";

            var matlabGridString = string.Empty;
            var matlabCurveString = string.Empty;
            var cSharpGridString = string.Empty;
            var cSharpCurveString = string.Empty;
            var curve = new List<double>();
            var grid = new List<double>();

            CurveConverter.ConvertInputString(
                inputString: inputString,
                inputFormat: InputFormat.Text,
                ref matlabGridString,
                ref matlabCurveString,
                ref cSharpGridString,
                ref cSharpCurveString,
                ref curve,
                ref grid);

            var expectedCurve = new List<double> { 45, 45, 56, 56, 66, 77 };

            Assert.AreEqual(6, curve.Count, "curve.Count");
            CollectionAssert.AreEqual(expectedCurve, curve);
        }

        [Test]
        public void When_text_string_containing_curve_values_is_converted_then_corresponding_result_is_returned_2()
        {
            var inputString = "ert";

            var matlabGridString = string.Empty;
            var matlabCurveString = string.Empty;
            var cSharpGridString = string.Empty;
            var cSharpCurveString = string.Empty;
            var curve = new List<double>();
            var grid = new List<double>();

            CurveConverter.ConvertInputString(
                inputString: inputString,
                inputFormat: InputFormat.Text,
                ref matlabGridString,
                ref matlabCurveString,
                ref cSharpGridString,
                ref cSharpCurveString,
                ref curve,
                ref grid);

            Assert.AreEqual(0, curve.Count, "curve.Count");
        }

        [Test]
        public void When_text_string_containing_curve_values_is_converted_then_corresponding_result_is_returned_3()
        {
            var inputString = "new List<double> {                    45, 45, 56, 56, 66, 77            };";

            var matlabGridString = string.Empty;
            var matlabCurveString = string.Empty;
            var cSharpGridString = string.Empty;
            var cSharpCurveString = string.Empty;
            var curve = new List<double>();
            var grid = new List<double>();

            CurveConverter.ConvertInputString(
                inputString: inputString,
                inputFormat: InputFormat.Text,
                ref matlabGridString,
                ref matlabCurveString,
                ref cSharpGridString,
                ref cSharpCurveString,
                ref curve,
                ref grid);

            var expectedCurve = new List<double> { 45, 45, 56, 56, 66, 77 };

            Assert.AreEqual(6, curve.Count, "curve.Count");
            CollectionAssert.AreEqual(expectedCurve, curve);
            Assert.AreEqual("curve = [45 45 56 56 66 77];", matlabCurveString);
            Assert.AreEqual("x = [];", matlabGridString);
            Assert.AreEqual("var curve = new List<double> {45, 45, 56, 56, 66, 77};", cSharpCurveString);
            Assert.AreEqual("var x = new List<double> {};", cSharpGridString);
        }

        [Test]
        public void When_text_string_containing_curve_values_is_converted_then_corresponding_result_is_returned_4()
        {
            var inputString = "new List<double> {                    45, 45, 56, 56, 66, 77            ;";

            var matlabGridString = string.Empty;
            var matlabCurveString = string.Empty;
            var cSharpGridString = string.Empty;
            var cSharpCurveString = string.Empty;
            var curve = new List<double>();
            var grid = new List<double>();

            CurveConverter.ConvertInputString(
                inputString: inputString,
                inputFormat: InputFormat.Text,
                ref matlabGridString,
                ref matlabCurveString,
                ref cSharpGridString,
                ref cSharpCurveString,
                ref curve,
                ref grid);

            var expectedCurve = new List<double> { 45, 45, 56, 56, 66, 77 };

            Assert.AreEqual(6, curve.Count, "curve.Count");
            CollectionAssert.AreEqual(expectedCurve, curve);
            Assert.AreEqual("curve = [45 45 56 56 66 77];", matlabCurveString);
            Assert.AreEqual("x = [];", matlabGridString);
            Assert.AreEqual("var curve = new List<double> {45, 45, 56, 56, 66, 77};", cSharpCurveString);
            Assert.AreEqual("var x = new List<double> {};", cSharpGridString);
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
