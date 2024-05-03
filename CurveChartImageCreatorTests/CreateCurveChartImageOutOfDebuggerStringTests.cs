
using CurveChartImageCreator;
using CurveConverterAlgorithm;
using NUnit.Framework;

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

            var curveConverterValues = CurveConverter.ConvertInputString(
               inputString: debuggerString,
               inputFormat: InputFormat.Debug);

            var outPutDir = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory) + "\\Test";

            CurveChartImageApi.Create(
                 fileNameWithoutExtention: TestContext.CurrentContext.Test.Name,
                 headerCaption: "Header",
                 xGrid1: curveConverterValues.Grid,
                 xGrid2: curveConverterValues.Grid,
                 curveList1: null,
                 curveList2: new List<List<double>> { curveConverterValues.Curve.ToList() },
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

            var curveConverterValues = CurveConverter.ConvertInputString(
                inputString: debuggerString,
                inputFormat: InputFormat.Debug);

            var outPutDir = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory) + "\\Test";

            CurveChartImageApi.Create(
                 fileNameWithoutExtention: TestContext.CurrentContext.Test.Name,
                 headerCaption: "Header",
                 xGrid1: curveConverterValues.Grid,
                 xGrid2: curveConverterValues.Grid,
                 curveList1: null,
                 curveList2: new List<List<double>> { curveConverterValues.Curve.ToList() },
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

            var curveConverterValues = CurveConverter.ConvertInputString(
              inputString: debuggerString,
              inputFormat: InputFormat.Debug);

            var manipulatedCurve = curveConverterValues.Curve.Where((value, index) => (index < 250) ).ToList();

            var xGrid = Enumerable.Range(0, manipulatedCurve.Count).Select((value, index) => (double)(index+1)).ToList();

            var outPutDir = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory) + "\\Test";

            CurveChartImageApi.Create(
                 fileNameWithoutExtention: TestContext.CurrentContext.Test.Name,
                 headerCaption: "Header",
                 xGrid1: xGrid,
                 xGrid2: xGrid,
                 curveList1: null,
                 curveList2: new List<List<double>> { manipulatedCurve },
                 outputDir: outPutDir,
                 linearFreqAxis: false,
                 imageWidth: 900,
                 imageHeight: 600);
        }

        private static string GetCurrentDirectory()
        {
            var executionDirectory = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);
            return executionDirectory;
        }
    }
}
