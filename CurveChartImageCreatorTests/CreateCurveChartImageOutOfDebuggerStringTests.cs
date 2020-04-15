using CalculationAlgorithmWrapper;
using CurveChartImageCreator;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CurveChartImageCreatorTests
{
    [TestFixture]
    public class CreateCurveChartImageOutOfDebuggerStringTests
    {
        [Test]
        public void When_curve_chart_is_created_out_of_curve_converter_input_then_it_is_as_expected()
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

            var outPutDir = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory) + "\\Test";

            CurveChartImageApi.Create(
                 fileNameWithoutExtention: TestContext.CurrentContext.Test.Name,
                 headerCaption: "Header",
                 xGrid: grid,
                 curveList1: null,
                 curveList2: new List<List<double>> { curve },
                 outputDir: outPutDir,
                 linearFreqAxis: false,
                 imageWidth: 900,
                 imageHeight: 600);
        }

        [Test]
        public void When_curve_chart_is_created_out_of_dictionary_in_debugger_input_then_it_is_as_expected()
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

            var outPutDir = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory) + "\\Test";

            CurveChartImageApi.Create(
                 fileNameWithoutExtention: TestContext.CurrentContext.Test.Name,
                 headerCaption: "Header",
                 xGrid: grid,
                 curveList1: null,
                 curveList2: new List<List<double>> { curve },
                 outputDir: outPutDir,
                 linearFreqAxis: true,
                 imageWidth: 900,
                 imageHeight: 600);
        }

        [Test]
        public void When_curve_chart_is_created_out_of_curve_input_then_it_is_as_expected()
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

            var manipulatedCurve = curve.Where((value, index) => (index < 250) ).ToList();

            var xGrid = Enumerable.Range(0, manipulatedCurve.Count).Select((value, index) => (double)(index+1)).ToList();

            var outPutDir = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory) + "\\Test";

            CurveChartImageApi.Create(
                 fileNameWithoutExtention: TestContext.CurrentContext.Test.Name,
                 headerCaption: "Header",
                 xGrid: xGrid,
                 curveList1: null,
                 curveList2: new List<List<double>> { manipulatedCurve },
                 outputDir: outPutDir,
                 linearFreqAxis: false,
                 imageWidth: 900,
                 imageHeight: 600);
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
