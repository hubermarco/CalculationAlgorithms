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

            var curveConverterValues = CurveConverter.ConvertInputString(
                inputString: debuggerString,
                inputFormat: InputFormat.Debug);

            Assert.AreEqual(228, curveConverterValues.Curve.Count);
            Assert.AreEqual(228, curveConverterValues.Grid.Count);
        }

        [Test]
        public void When_debugger_string_containing_simple_list_is_converted_then_corresponding_result_is_returned()
        {
            var currentDirectory = GetCurrentDirectory();
            var inputPath = currentDirectory + "\\" + "CurveInput.txt";
            var debuggerString = File.ReadAllText(inputPath);

            var curveConverterValues = CurveConverter.ConvertInputString(
                inputString: debuggerString,
                inputFormat: InputFormat.Debug);

            Assert.AreEqual(1000, curveConverterValues.Curve.Count);
            Assert.AreEqual(0, curveConverterValues.Grid.Count);
        }

        [Test]
        public void When_debugger_string_containing_dictionary_is_converted_then_corresponding_result_is_returned()
        {
            var currentDirectory = GetCurrentDirectory();
            var inputPath = currentDirectory + "\\" + "DictionaryInput.txt";
            var debuggerString = File.ReadAllText(inputPath);

            var curveConverterValues = CurveConverter.ConvertInputString(
                inputString: debuggerString,
                inputFormat: InputFormat.Debug);

            Assert.AreEqual(300, curveConverterValues.Curve.Count);
            Assert.AreEqual(300, curveConverterValues.Grid.Count);
        }

        [Test]
        public void When_text_string_containing_list_with_of_curve_values_is_converted_then_corresponding_result_is_returned()
        {
            var currentDirectory = GetCurrentDirectory();
            var inputPath = currentDirectory + "\\" + "TextInput.txt";
            var inputString = File.ReadAllText(inputPath);

            var curveConverterValues = CurveConverter.ConvertInputString(
                inputString: inputString,
                inputFormat: InputFormat.Text);

            Assert.AreEqual(228, curveConverterValues.Curve.Count, "curve.Count");
            Assert.AreEqual(0, curveConverterValues.Grid.Count, "grid.Count");
        }

        [Test]
        public void When_text_string_containing_curve_values_is_converted_then_corresponding_result_is_returned()
        {
            var inputString = "ert45,45,56,56 66 77pp";

            var curveConverterValues = CurveConverter.ConvertInputString(
                inputString: inputString,
                inputFormat: InputFormat.Text);

            var expectedCurve = new List<double> { 45, 45, 56, 56, 66, 77 };

            Assert.AreEqual(6, curveConverterValues.Curve.Count, "curve.Count");
            CollectionAssert.AreEqual(expectedCurve, curveConverterValues.Curve);
        }

        [Test]
        public void When_text_string_containing_curve_values_is_converted_then_corresponding_result_is_returned_2()
        {
            var inputString = "ert";

            var curveConverterValues = CurveConverter.ConvertInputString(
                inputString: inputString,
                inputFormat: InputFormat.Text);

            Assert.AreEqual(0, curveConverterValues.Curve.Count, "curve.Count");
        }

        [Test]
        public void When_text_string_containing_curve_values_is_converted_then_corresponding_result_is_returned_3()
        {
            var inputString = "new List<double> {                    45, 45, 56, 56, 66, 77            };";

            var curveConverterValues = CurveConverter.ConvertInputString(
                inputString: inputString,
                inputFormat: InputFormat.Text);

            var expectedCurve = new List<double> { 45, 45, 56, 56, 66, 77 };

            Assert.AreEqual(6, curveConverterValues.Curve.Count, "curve.Count");
            CollectionAssert.AreEqual(expectedCurve, curveConverterValues.Curve);
            Assert.AreEqual("curve = [45 45 56 56 66 77];", curveConverterValues.GetMatlabCurveString("curve"));
            Assert.AreEqual("x = [];", curveConverterValues.GetMatlabGridString("x", null) );
            Assert.AreEqual("var curve = new List<double> {45, 45, 56, 56, 66, 77};", curveConverterValues.GetCSharpCurveString("curve"));
            Assert.AreEqual("var x = new List<double> {};", curveConverterValues.GetCSharpGridString("x"));
        }

        [Test]
        public void When_text_string_containing_curve_values_is_converted_then_corresponding_result_is_returned_4()
        {
            var inputString = "new List<double> {                    45, 45, 56, 56, 66, 77            ;";

            var curveConverterValues = CurveConverter.ConvertInputString(
                inputString: inputString,
                inputFormat: InputFormat.Text);

            var expectedCurve = new List<double> { 45, 45, 56, 56, 66, 77 };

            Assert.AreEqual(6, curveConverterValues.Curve.Count, "curve.Count");
            CollectionAssert.AreEqual(expectedCurve, curveConverterValues.Curve);
            Assert.AreEqual("curve = [45 45 56 56 66 77];", curveConverterValues.GetMatlabCurveString("curve"));
            Assert.AreEqual("x = [];", curveConverterValues.GetMatlabGridString("x"));
            Assert.AreEqual("var curve = new List<double> {45, 45, 56, 56, 66, 77};", curveConverterValues.GetCSharpCurveString("curve"));
            Assert.AreEqual("var x = new List<double> {};", curveConverterValues.GetCSharpGridString("x"));
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
