using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace CalculationAlgorithmWrapper
{
    public class CurveConverter
    {
         public static int ConvertInputString(
            string inputString,
            InputFormat inputFormat,
            ref string matlabGridString,
            ref string matlabCurveString,
            ref string cSharpGridString,
            ref string cSharpCurveString,
            ref List<double> curve,
            ref List<double> grid)
        {
            int valueCount;
            var usedInputFormat = GetUsedInputFormat(inputString, inputFormat);

            if (usedInputFormat == InputFormat.Debug)
            {
                valueCount = ConvertDebuggerString(
                    inputString,
                    ref matlabGridString,
                    ref matlabCurveString,
                    ref cSharpGridString,
                    ref cSharpCurveString,
                    ref curve,
                    ref grid);
            }
            else if(usedInputFormat == InputFormat.Text)
            {
                valueCount = ConvertTextString(
                    inputString,
                    ref matlabGridString,
                    ref matlabCurveString,
                    ref cSharpGridString,
                    ref cSharpCurveString,
                    ref curve,
                    ref grid);
            }
            else
            {
                throw new ArgumentException($"inputFormat:{inputFormat} not valid");
            }
            
            return valueCount;
        }

        public static int ConvertDebuggerString(
            string inputString,
            ref string matlabGridString,
            ref string matlabCurveString,
            ref string cSharpGridString,
            ref string cSharpCurveString,
            ref List<double> curve,
            ref List<double> grid)
        {
            var valueCount = ConvertDebuggerStringToMatlabCurveString(
                inputString,
                ref matlabGridString,
                ref matlabCurveString,
                ref curve,
                ref grid);

            var cSharpCurveStringOutput = ConvertMatlabCurveStringToCSharpCurveString(matlabCurveString);
            var cSharpGridStringOutput = ConvertMatlabGridStringToCSharpGridString(matlabGridString);

            cSharpGridString = cSharpGridStringOutput;
            cSharpCurveString = cSharpCurveStringOutput;

            return valueCount;
        }

        public static int ConvertTextString(
            string inputString,
            ref string matlabGridString,
            ref string matlabCurveString,
            ref string cSharpGridString,
            ref string cSharpCurveString,
            ref List<double> curve,
            ref List<double> grid)
        {
            var valueCount = ConvertTextStringToMatlabCurveString(
                inputString,
                ref matlabGridString,
                ref matlabCurveString,
                ref curve,
                ref grid);

            var cSharpCurveStringOutput = ConvertMatlabCurveStringToCSharpCurveString(matlabCurveString);
            var cSharpGridStringOutput = ConvertMatlabGridStringToCSharpGridString(matlabGridString);

            cSharpGridString = cSharpGridStringOutput;
            cSharpCurveString = cSharpCurveStringOutput;

            return valueCount;
        }

        public static string GetFrequencyCurveString()
        {
            const string freqs =
                "f = [100 105 110 115 120 125 130 135 145 150 160 165 175 180 190 200 210 220 230 240 250 260 275 290 300 315 330 345 360 380 400 420 440 460 480 500 525 550 575 600 630 660 700 730 760 800 830 870 900 950 1000 1050 1100 1150 1200 1250 1312.5 1375 1437.5 1500 1562.5 1625 1687.5 1750 1812.5 1875 1937.5 2000 2062.5 2125 2187.5 2250 2312.5 2375 2437.5 2500 2562.5 2625 2687.5 2750 2812.5 2875 2937.5 3000 3062.5 3125 3187.5 3250 3312.5 3375 3437.5 3500 3562.5 3625 3687.5 3750 3812.5 3875 3937.5 4000 4062.5 4125 4187.5 4250 4312.5 4375 4437.5 4500 4562.5 4625 4687.5 4750 4812.5 4875 4937.5 5000 5062.5 5125 5187.5 5250 5312.5 5375 5437.5 5500 5562.5 5625 5687.5 5750 5812.5 5875 5937.5 6000 6062.5 6125 6187.5 6250 6312.5 6375 6437.5 6500 6562.5 6625 6687.5 6750 6812.5 6875 6937.5 7000 7062.5 7125 7187.5 7250 7312.5 7375 7437.5 7500 7562.5 7625 7687.5 7750 7812.5 7875 7937.5 8000 8062.5 8125 8187.5 8250 8312.5 8375 8437.5 8500 8562.5 8625 8687.5 8750 8812.5 8875 8937.5 9000 9062.5 9125 9187.5 9250 9312.5 9375 9437.5 9500 9562.5 9625 9687.5 9750 9812.5 9875 9937.5 10000 10062.5 10125 10187.5 10250 10312.5 10375 10437.5 10500 10562.5 10625 10687.5 10750 10812.5 10875 10937.5 11000 11062.5 11125 11187.5 11250 11312.5 11375 11437.5 11500 11562.5 11625 11687.5 11750 11812.5 11875 11937.5 12000];";

            return freqs;
        }

        public static int ConvertDebuggerStringToMatlabCurveString(
            string debuggerString,
            ref string matlabGridString,
            ref string outputStringMatlab,
            ref List<double> curve,
            ref List<double> grid)
        {
            var valueCount = 0;
            matlabGridString = "x = [";
            outputStringMatlab = "curve = [";
            curve.Clear();
            grid.Clear();

            var debuggerLines = debuggerString.Split('\n');

            var debuggerLinesFiltered = debuggerLines.
                Where(line => (line != "\r") && (line != "") && (line != "\t\t\r") &&
                !line.Contains("Count") && !line.Contains("Raw View") && !line.Contains("double[]") ).ToArray();

            foreach (var fileLine in debuggerLinesFiltered)
            {
                var columns = fileLine.Split('\t');

                var numberString = (columns.Length >= 4) ? columns[3] : string.Empty;
                numberString = numberString.Replace(" ", "");

                var splittedSubStringList = numberString.Split(new[] { ':', ',', '{', '}', '[', ']' }, StringSplitOptions.RemoveEmptyEntries);

                string numberSubString;
                string gridString;

                if (splittedSubStringList.Length == 4)
                {
                    gridString = splittedSubStringList[1];
                    numberSubString = splittedSubStringList[3];
                }
                else if (splittedSubStringList.Length == 2)
                {
                    gridString = splittedSubStringList[0];
                    numberSubString = splittedSubStringList[1];
                }
                else
                {
                    gridString = string.Empty;
                    numberSubString = numberString;
                }

                if (double.TryParse(numberSubString, out _))
                {
                    outputStringMatlab += numberSubString + " ";
                    valueCount++;

                    curve.Add(double.Parse(numberSubString, CultureInfo.InvariantCulture));

                    if(double.TryParse(gridString, out _))
                    {
                        matlabGridString += gridString + " ";
                        grid.Add(double.Parse(gridString, CultureInfo.InvariantCulture));
                    }
                }
            }

            // remove last blank of outputStringMatlab
            if (outputStringMatlab[outputStringMatlab.Length - 1] == ' ')
            {
                outputStringMatlab = outputStringMatlab.Remove(outputStringMatlab.Length - 1);
            }
            
            outputStringMatlab += "];";

            // remove last blank of matlabGridString
            if (matlabGridString[matlabGridString.Length - 1] == ' ')
            {
                matlabGridString = matlabGridString.Remove(matlabGridString.Length - 1);
            }

            matlabGridString += "];";

            return valueCount;
        }

        public static int ConvertTextStringToMatlabCurveString(
           string textString,
           ref string matlabGridString,
           ref string outputStringMatlab,
           ref List<double> curve,
           ref List<double> grid)
        {
            var valueCount = 0;
            var valueString = string.Empty;
            grid.Clear();

            if(textString.Length != 0)
            {
                if(GetStartAndEndChar(textString, out char startChar, out char endChar))
                {
                    valueString = ConvertTextStringToValueStringWithStartCharAndStopChar(textString, startChar, endChar);
                }
                else
                {
                    valueString = ConvertTextStringToValueStringSearchingForNumbers(textString);
                }

                curve = ConvertValueStringToCurve(valueString, ' ');

                valueCount = curve.Count;
            }

            CreateMatlabString(valueString, out outputStringMatlab, out matlabGridString);

            return valueCount;
        }

        public static string ConvertMatlabCurveStringToCSharpCurveString(string matlabCurveString)
        {
            var outputStringCsharp = matlabCurveString.Replace(" ", ", ").Replace("curve,", "var curve").Replace("=,", "=")
                .Replace("[", "new List<double> {").Replace("]", "}").Replace(", }", "}").Replace(", =", " =");

            return outputStringCsharp;
        }

        public static string ConvertMatlabGridStringToCSharpGridString(string matlabGridString)
        {
            var outputStringCsharp = matlabGridString.Replace(" ", ", ").Replace("x,", "var x").Replace("=,", "=")
                .Replace("[", "new List<double> {").Replace("]", "}").Replace(", }", "}").Replace(", =", " =");

            return outputStringCsharp;
        }

        public static string ConvertCurveToMatlabCurveString(List<double> curve, string curveName)
        {
            var deltaCurveString = $"{curveName} = [";
            curve.ForEach(x => deltaCurveString += $"{x} ");
            deltaCurveString += "];";

            var deltaCurveStringAdapted = deltaCurveString.Replace(',', '.');

            return deltaCurveStringAdapted;
        }

        public static IList<double> CalculateXGrid(
            IList<double> grid,
            int numberOfCurvePoints,
            bool linearFreqAxis)
        {
            IList<double> xGrid = null;

            if ((grid != null) && (grid.Count > 0) )
            {
                xGrid = grid;
            }
            else if (numberOfCurvePoints == 228)
            {
                xGrid = new List<double> { 100, 105, 110, 115, 120, 125, 130, 135, 145, 150, 160, 165, 175, 180, 190, 200, 210, 220, 230, 240, 250, 260, 275, 290, 300, 315, 330, 345, 360, 380, 400, 420, 440, 460, 480, 500, 525, 550, 575, 600, 630, 660, 700, 730, 760, 800, 830, 870, 900, 950, 1000, 1050, 1100, 1150, 1200, 1250, 1312.5, 1375, 1437.5, 1500, 1562.5, 1625, 1687.5, 1750, 1812.5, 1875, 1937.5, 2000, 2062.5, 2125, 2187.5, 2250, 2312.5, 2375, 2437.5, 2500, 2562.5, 2625, 2687.5, 2750, 2812.5, 2875, 2937.5, 3000, 3062.5, 3125, 3187.5, 3250, 3312.5, 3375, 3437.5, 3500, 3562.5, 3625, 3687.5, 3750, 3812.5, 3875, 3937.5, 4000, 4062.5, 4125, 4187.5, 4250, 4312.5, 4375, 4437.5, 4500, 4562.5, 4625, 4687.5, 4750, 4812.5, 4875, 4937.5, 5000, 5062.5, 5125, 5187.5, 5250, 5312.5, 5375, 5437.5, 5500, 5562.5, 5625, 5687.5, 5750, 5812.5, 5875, 5937.5, 6000, 6062.5, 6125, 6187.5, 6250, 6312.5, 6375, 6437.5, 6500, 6562.5, 6625, 6687.5, 6750, 6812.5, 6875, 6937.5, 7000, 7062.5, 7125, 7187.5, 7250, 7312.5, 7375, 7437.5, 7500, 7562.5, 7625, 7687.5, 7750, 7812.5, 7875, 7937.5, 8000, 8062.5, 8125, 8187.5, 8250, 8312.5, 8375, 8437.5, 8500, 8562.5, 8625, 8687.5, 8750, 8812.5, 8875, 8937.5, 9000, 9062.5, 9125, 9187.5, 9250, 9312.5, 9375, 9437.5, 9500, 9562.5, 9625, 9687.5, 9750, 9812.5, 9875, 9937.5, 10000, 10062.5, 10125, 10187.5, 10250, 10312.5, 10375, 10437.5, 10500, 10562.5, 10625, 10687.5, 10750, 10812.5, 10875, 10937.5, 11000, 11062.5, 11125, 11187.5, 11250, 11312.5, 11375, 11437.5, 11500, 11562.5, 11625, 11687.5, 11750, 11812.5, 11875, 11937.5, 12000 };
            }
            else
            {
                if (linearFreqAxis)
                {
                    xGrid = Enumerable.Range(0, numberOfCurvePoints).Select(x => (double)x).ToList();
                }
                else
                {
                    xGrid = Enumerable.Range(1, numberOfCurvePoints).Select(x => (double)x).ToList();
                }
            }

            return xGrid;
        }

        public static InputFormat Convert2InputFormat(
            bool isRadioButtonFormatAutomaticChecked,
            bool isRadioButtonFormatDebug)
        {
            var inputFormat = InputFormat.Text;

            if (isRadioButtonFormatAutomaticChecked)
            {
                inputFormat = InputFormat.Automatic;
            }
            else if (isRadioButtonFormatDebug)
            {
                inputFormat = InputFormat.Debug;
            }

            return inputFormat;
        }

        private static InputFormat GetUsedInputFormat(string inputString, InputFormat inputFormat)
        {
            var usedInputFormat = inputFormat;
            var isInputStringDebugString = inputString.Contains("\t");
     
            if (inputFormat == InputFormat.Automatic)
            {
                usedInputFormat = isInputStringDebugString ? InputFormat.Debug : InputFormat.Text;
            }
            return usedInputFormat;
        }

        private static bool GetStartAndEndChar(string textString, out char startChar, out char endChar)
        {
            var startStopCharDict = new Dictionary<char, char> { { '[', ']' } , { '{', '}' }, { '(', ')' }, { ' ', ' ' } };

            startChar = textString.Contains('[') ?
                    '[' :
                    textString.Contains('{') ?
                    '{' :
                    textString.Contains('(') ?
                    '(' :
                    ' ';

            endChar = startStopCharDict[startChar];

            return (startChar != ' ');
        }

        private static string ConvertTextStringToValueStringWithStartCharAndStopChar(string textString, char startChar, char endChar)
        {
            var textLines = textString.Split('\n');

            var textLinesFiltered = textLines.
                Where(line => (line != "\r") && (line != "") && (line != "\t\t\r")).ToArray();

            var relevantLineString = textLinesFiltered.First(x => x.Contains(startChar));
            var startIndex = relevantLineString.IndexOf(startChar) + 1;
            var subStringLength = relevantLineString.IndexOf(endChar) - startIndex;

            var valueStringLine = relevantLineString.Substring(startIndex: startIndex, length: subStringLength);

            var valueStringFiltered = valueStringLine.Replace(",", " ").Replace(";", " ");

            return valueStringFiltered;
        }

        private static string ConvertTextStringToValueStringSearchingForNumbers(string textString)
        {
            var textLines = textString.Split('\n');

            var textLinesFiltered = textLines.
                Where(line => (line != "\r") && (line != "") && (line != "\t\t\r")).ToArray();

            var relevantLineString = textLinesFiltered.First(
                x => x.ToCharArray().FirstOrDefault(c => Char.IsDigit(c)) != default(char));

            var startIndex = relevantLineString.IndexOfAny(new[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' });
            var stopIndex = relevantLineString.LastIndexOfAny(new[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' });

            var valueStringLine = relevantLineString.Substring(startIndex: startIndex, length: stopIndex + 1 - startIndex);

            var valueStringFiltered = valueStringLine.Replace(",", " ").Replace(";", " ");

            return valueStringFiltered;
        }


        private static List<double> ConvertValueStringToCurve(string valueString, char splitChar)
        {
            var curve = new List<double>();

            var valueStringList = valueString.Split(splitChar);

            foreach (var valueStringEntry in valueStringList)
            {
                if (double.TryParse(valueStringEntry, out _))
                {
                    curve.Add(double.Parse(valueStringEntry, CultureInfo.InvariantCulture));
                }
            }
            return curve;
        }

        private static void CreateMatlabString(
            string valueString, 
            out string matlabCurveString,
            out string matlabGridString)
        {
            matlabGridString = "x = [";
            matlabCurveString = "curve = [";

            matlabCurveString += valueString;

            // remove last blank of outputStringMatlab
            if (matlabCurveString[matlabCurveString.Length - 1] == ' ')
            {
                matlabCurveString = matlabCurveString.Remove(matlabCurveString.Length - 1);
            }

            matlabCurveString += "];";

            // remove last blank of matlabGridString
            if (matlabGridString[matlabGridString.Length - 1] == ' ')
            {
                matlabGridString = matlabGridString.Remove(matlabGridString.Length - 1);
            }

            matlabGridString += "];";
        }
    }
}
