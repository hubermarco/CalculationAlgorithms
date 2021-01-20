﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace CalculationAlgorithmWrapper
{
    public class CurveConverter
    {
         public static CurveConverterValues ConvertInputString(
            string inputString,
            InputFormat inputFormat)
        {
            CurveConverterValues curveConverterValues;

            var usedInputFormat = GetUsedInputFormat(inputString, inputFormat);

            if (usedInputFormat == InputFormat.Debug)
            {
                curveConverterValues = ConvertDebuggerString(inputString, numberOfDigits:3);
            }
            else if(usedInputFormat == InputFormat.Text)
            {
                curveConverterValues = ConvertTextString(inputString);
            }
            else
            {
                throw new ArgumentException($"inputFormat:{inputFormat} not valid");
            }

            return curveConverterValues;
        }
 
        public static string ConvertCurveToMatlabCurveString(List<double> curve, string curveName, bool commanSeparation=false)
        {
            var curveString = $"{curveName} = [";

            var separationString = commanSeparation ? ", " : " ";

            curve.ForEach(x => 
            { 
                var xAdapted = x.ToString().Replace(',', '.'); 
                curveString += $"{xAdapted}{separationString}"; 
            });

            // Remove last separarationString
            if(curve.Count > 0)
                curveString = curveString.Remove(curveString.Length - separationString.Length);
            
            if (commanSeparation) 
                curveString += "]"; 
            else 
                curveString += "];";

            return curveString;
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

        private static CurveConverterValues ConvertDebuggerString(
            string debuggerString,
            int numberOfDigits)
        {
            var matlabGridString = "x = [";
            var outputStringMatlab = "curve = [";
            var curve = new List<double>();
            var grid = new List<double>();

            // E must not be replaced because it's part of a number as exponent (1.2246063538223773E-15)
            var debuggerStringWithoutLetters = Regex.Replace(debuggerString, "[a-zA-DF-Z]", " ");
            var debuggerStringWithSingleSpaces = Regex.Replace(debuggerStringWithoutLetters, " {2,}", " ");

            var debuggerLines = debuggerStringWithSingleSpaces.Split('\n');

            var debuggerLinesFiltered = debuggerLines.
                Where(line => (line != "\r") && (line != "") && (line != "\t\t\r") &&
                !line.Contains("Count") && !line.Contains("Raw View") && !line.Contains("double[]")).ToArray();

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
                    var value = double.Parse(numberSubString, CultureInfo.InvariantCulture);
                    var usedNumberSubString = $"{Math.Round(value, numberOfDigits)}".Replace(",", ".");

                    outputStringMatlab += usedNumberSubString + " ";

                    curve.Add(value);

                    if (double.TryParse(gridString, out _))
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

            return new CurveConverterValues(
               curve: curve,
               grid: grid);
        }

        private static CurveConverterValues ConvertTextString(
           string textString)
        {
            var valueString = string.Empty;
            var grid = new List<double>();
            var curve = new List<double>();

            var textStringWithoutLetters = Regex.Replace(textString, "[a-zA-Z]", " ");
            var textStringWithSingleSpaces = Regex.Replace(textStringWithoutLetters, " {2,}", " ");
            textStringWithSingleSpaces = (textStringWithSingleSpaces == " ") ? string.Empty : textStringWithSingleSpaces;

            if (textStringWithSingleSpaces.Length != 0)
            {
                if (GetStartAndEndChar(textStringWithSingleSpaces, out char startChar, out char endChar))
                {
                    valueString = ConvertTextStringToValueStringWithStartCharAndStopChar(
                        textStringWithSingleSpaces, startChar, endChar);
                }
                else
                {
                    valueString = ConvertTextStringToValueStringSearchingForNumbers(textStringWithSingleSpaces);
                }

                curve = ConvertValueStringToCurve(valueString, ' ');
            }

            CreateMatlabString(valueString, out var outputStringMatlab, out var matlabGridString);

            return new CurveConverterValues(
               curve: curve,
               grid: grid);
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
            var startAndEndCharFound = false;
            startChar = ' ';
            endChar = ' ';
            var startStopCharDict = new Dictionary<char, char> { { '[', ']' } , { '{', '}' }, { '(', ')' }};

            foreach(var startStopCharValuePair in startStopCharDict)
            {
                if(textString.Contains(startStopCharValuePair.Key) && textString.Contains(startStopCharValuePair.Value))
                {
                    startChar = startStopCharValuePair.Key;
                    endChar = startStopCharValuePair.Value;
                    startAndEndCharFound = true;
                }
            }

            return startAndEndCharFound;
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

            var valueStringWithSingleSpaces = Regex.Replace(valueString, " {2,}", " ");
            var valueStringWithoutSpacesAtTheBeginning = Regex.Replace(valueStringWithSingleSpaces, "^\\s+", "");

            matlabCurveString += valueStringWithoutSpacesAtTheBeginning;

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
