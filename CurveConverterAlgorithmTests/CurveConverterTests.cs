using CurveConverterAlgorithm;
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
        public void When_text_string_containing_2_lists_with_of_curve_values_is_converted_then_corresponding_result_is_returned()
        {
            var currentDirectory = GetCurrentDirectory();
            var inputPath = currentDirectory + "\\" + "CurveConverterTextInput.txt";
            var debuggerString = File.ReadAllText(inputPath);

            var curveConverterValues = CurveConverter.ConvertInputString(
                inputString: debuggerString,
                inputFormat: InputFormat.Automatic);

            Assert.AreEqual(228, curveConverterValues.Curve.Count);
            Assert.AreEqual(1.01, curveConverterValues.Curve[0]);
            Assert.AreEqual(228, curveConverterValues.Grid.Count);
            Assert.AreEqual(100, curveConverterValues.Grid[0]);
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

        [Test]
        public void When_text_string_containing_curve_values_is_converted_then_corresponding_result_is_returned_5()
        {
            var inputString = "-4, 5, 7 8, 9; 10         ";

            var curveConverterValues = CurveConverter.ConvertInputString(
                inputString: inputString,
                inputFormat: InputFormat.Text);

            var expectedCurve = new List<double> { -4, 5, 7, 8, 9, 10 };

            Assert.AreEqual(6, curveConverterValues.Curve.Count, "curve.Count");
            CollectionAssert.AreEqual(expectedCurve, curveConverterValues.Curve);
            Assert.AreEqual("curve = [-4 5 7 8 9 10];", curveConverterValues.GetMatlabCurveString("curve"));
            Assert.AreEqual("x = [];", curveConverterValues.GetMatlabGridString("x"));
            Assert.AreEqual("var curve = new List<double> {-4, 5, 7, 8, 9, 10};", curveConverterValues.GetCSharpCurveString("curve"));
            Assert.AreEqual("var x = new List<double> {};", curveConverterValues.GetCSharpGridString("x"));
        }

        [Test]
        public void When_text_string_containing_curve_values_is_converted_then_corresponding_result_is_returned_6()
        {
            var inputString = "cgmParameters.FrontMicFbcCoefficients: [-0.000278472900390625 -9.918212890625e-05 0.00034332275390625 0.00286865234375 0.010986328125 0.00665283203125 -0.03466796875 -0.033203125 0.03271484375 0.00274658203125 0.00384521484375 0.06640625 -0.029052734375 -0.046875 0.0048828125 -0.018798828125 -0.000177383422851563 0.02685546875 0.02587890625 -0.00616455078125 -0.0123291015625 -0.0108642578125 -0.0128173828125 -0.00177001953125 0.01953125 0.010986328125 -0.0052490234375 -0.00653076171875 -0.010986328125 -0.00390625 0.00714111328125 0.010986328125 0.00335693359375 -0.003173828125 -0.008544921875 -0.0047607421875 -0.00091552734375 0.00592041015625 0.003631591796875 0.000457763671875 -0.00250244140625 -0.0012969970703125 -0.000461578369140625 0.001556396484375 0.00017547607421875 -0.000823974609375 -0.001708984375 -0.00061798095703125 0.001007080078125 0.001800537109375 0.001007080078125 -0.001312255859375 -0.002288818359375 -0.0019378662109375 -9.1552734375e-05 0.00103759765625 0.0011444091796875 -0.00093841552734375 -0.0018463134765625 -0.002349853515625 -0.000278472900390625 0.000823974609375 0.0015106201171875 -0.000324249267578125]";

            var curveConverterValues = CurveConverter.ConvertInputString(
                inputString: inputString,
                inputFormat: InputFormat.Text);

            Assert.AreEqual(64, curveConverterValues.Curve.Count, "curve.Count");
            Assert.AreEqual("x = [];", curveConverterValues.GetMatlabGridString("x"));
        }

        private static string GetCurrentDirectory()
        {
            var executionDirectory = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);
            return executionDirectory;
        }
    }
}
