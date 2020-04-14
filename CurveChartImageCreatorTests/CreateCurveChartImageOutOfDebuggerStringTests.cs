﻿using CalculationAlgorithmWrapper;
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

            var matlabCurveString = string.Empty;
            var cSharpCurveString = string.Empty;
            var curve = new List<double>();
            var grid = new List<double>();

            CurveConverter.ConvertDebuggerString(debuggerString, ref matlabCurveString, ref cSharpCurveString, ref curve, ref grid);

            var f = new List<double> { 100, 105, 110, 115, 120, 125, 130, 135, 145, 150, 160, 165, 175, 180, 190, 200, 210, 220, 230, 240, 250, 260, 275, 290, 300, 315, 330, 345, 360, 380, 400, 420, 440, 460, 480, 500, 525, 550, 575, 600, 630, 660, 700, 730, 760, 800, 830, 870, 900, 950, 1000, 1050, 1100, 1150, 1200, 1250, 1312.5, 1375, 1437.5, 1500, 1562.5, 1625, 1687.5, 1750, 1812.5, 1875, 1937.5, 2000, 2062.5, 2125, 2187.5, 2250, 2312.5, 2375, 2437.5, 2500, 2562.5, 2625, 2687.5, 2750, 2812.5, 2875, 2937.5, 3000, 3062.5, 3125, 3187.5, 3250, 3312.5, 3375, 3437.5, 3500, 3562.5, 3625, 3687.5, 3750, 3812.5, 3875, 3937.5, 4000, 4062.5, 4125, 4187.5, 4250, 4312.5, 4375, 4437.5, 4500, 4562.5, 4625, 4687.5, 4750, 4812.5, 4875, 4937.5, 5000, 5062.5, 5125, 5187.5, 5250, 5312.5, 5375, 5437.5, 5500, 5562.5, 5625, 5687.5, 5750, 5812.5, 5875, 5937.5, 6000, 6062.5, 6125, 6187.5, 6250, 6312.5, 6375, 6437.5, 6500, 6562.5, 6625, 6687.5, 6750, 6812.5, 6875, 6937.5, 7000, 7062.5, 7125, 7187.5, 7250, 7312.5, 7375, 7437.5, 7500, 7562.5, 7625, 7687.5, 7750, 7812.5, 7875, 7937.5, 8000, 8062.5, 8125, 8187.5, 8250, 8312.5, 8375, 8437.5, 8500, 8562.5, 8625, 8687.5, 8750, 8812.5, 8875, 8937.5, 9000, 9062.5, 9125, 9187.5, 9250, 9312.5, 9375, 9437.5, 9500, 9562.5, 9625, 9687.5, 9750, 9812.5, 9875, 9937.5, 10000, 10062.5, 10125, 10187.5, 10250, 10312.5, 10375, 10437.5, 10500, 10562.5, 10625, 10687.5, 10750, 10812.5, 10875, 10937.5, 11000, 11062.5, 11125, 11187.5, 11250, 11312.5, 11375, 11437.5, 11500, 11562.5, 11625, 11687.5, 11750, 11812.5, 11875, 11937.5, 12000 };
           
            var outPutDir = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory) + "\\Test";

            CurveChartImageApi.Create(
                 fileNameWithoutExtention: TestContext.CurrentContext.Test.Name,
                 headerCaption: "Header",
                 xGrid: f,
                 curveList1: new List<List<double>> { curve },
                 curveList2: new List<List<double>> { curve },
                 outputDir: outPutDir,
                 linearFreqAxis: false,
                 imageWidth: 900,
                 imageHeight: 600);
        }

        [Test]
        public void When_curve_chart_is_created_out_of_curve_input_then_it_is_as_expected()
        {
            var currentDirectory = GetCurrentDirectory();
            var inputPath = currentDirectory + "\\" + "CurveInput.txt";
            var debuggerString = File.ReadAllText(inputPath);

            var matlabCurveString = string.Empty;
            var cSharpCurveString = string.Empty;
            var curve = new List<double>();
            var grid = new List<double>();

            CurveConverter.ConvertDebuggerString(debuggerString, ref matlabCurveString, ref cSharpCurveString, ref curve, ref grid);

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
