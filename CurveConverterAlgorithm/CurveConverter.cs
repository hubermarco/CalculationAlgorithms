using CalculationAlgorithmWrapper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace CurveConverterAlgorithm
{
    public class CurveConverter
    {
        private static readonly string _datePattern = @"\b\d{2}([/.\\])\d{2}\1\d{4}\b|\b\d{2}/\d{4}\b";

        public static CurveConverterValues ConvertInputString(
            string inputString,
            InputFormat inputFormat)
        {
            CurveConverterValues curveConverterValues;

            var usedInputFormat = GetUsedInputFormat(inputString, inputFormat);
            if (usedInputFormat == InputFormat.Invest)
            {
                curveConverterValues = ConvertInvestmentString(inputString);
            }
            else if (usedInputFormat == InputFormat.Debug)
            {
                curveConverterValues = ConvertDebuggerString(inputString);
            }
            else if (usedInputFormat == InputFormat.Arithmetic)
            {
                curveConverterValues = ConvertArithmetricString(inputString);
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

        public static int GetDecimalPlacesFromString(string decimalPlacesString, int defaultDecimalPlaces)
        {
            var correctedDecimalPlacesText = Regex.Replace(decimalPlacesString, "[a-df-zA-DF-Z]", "");

            if (!double.TryParse(correctedDecimalPlacesText, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var decimalPlaces))
                decimalPlaces = defaultDecimalPlaces;

            var decimalPlacesLimited = (int)Math.Round(Math.Min(Math.Max(decimalPlaces, 0), 15));

            return decimalPlacesLimited;
        }

        public static InputFormat GetUsedInputFormat(string inputString, InputFormat inputFormat)
        {
            var usedInputFormat = inputFormat;
            var isInputStringDebugString = inputString.Contains("\t");
            var isInvestmentString = inputString.Contains("Date") ||
                (Regex.Matches(inputString, _datePattern).Count > 0);
          
            // RegexOptions.IgnoreCase ignoriert die Groß- und Kleinschreibung von x, y, z
            var isArithmetricString = Regex.Matches(inputString, @"\|\s*[xyz]\s*=\s*", RegexOptions.IgnoreCase).Count > 0;

            if (inputFormat == InputFormat.Automatic)
            {
                usedInputFormat = isInvestmentString ?
                    InputFormat.Invest :
                    isInputStringDebugString ?
                    InputFormat.Debug :
                    isArithmetricString ?
                    InputFormat.Arithmetic :
                    InputFormat.Text;
            }
            return usedInputFormat;
        }

        private static CurveConverterValues ConvertInvestmentString(string inputString)
        {
            var grid = new List<double>();
            var curve = new List<double>();

            var textStringWithoutQuotationMarks = inputString.Replace("\",\"", "\";\"").Replace(",", " ").Replace("\"", "");
            var trimmedTextString = textStringWithoutQuotationMarks.TrimEnd(new[] { '\n', '\r', ' ', });

            var textLines = trimmedTextString.Split('\n').Where(line => !string.IsNullOrEmpty(line)).ToList();

            if (!(Regex.Matches(textLines[0], _datePattern).Count > 0))
                textLines.RemoveAt(0);

            if (textLines[0].Split(new[] { ';', ',', ' ' })[0].Split('/').Length == 3)
                textLines.Reverse();

            var lineTrimChars = new[] { ',', ';', '\n', '\r', '\t', ' ' };
            var columnTrimChars = new[] { ';', ',', '\t' };
            var dateTrimChars = new[] { '/', '\\', '.' };

            var europeanDateFormat =  textLines.Any(
                line => int.Parse(line.TrimStart(lineTrimChars).
                Split(columnTrimChars)[0].
                Split(dateTrimChars)[0], CultureInfo.InvariantCulture) > 12);

            foreach (var textLine in textLines)
            {
                var trimArray = new[] { ',', ';', '\n', '\r', '\t', ' ' };

                var textLineWithSemicolon = textLine.TrimStart(lineTrimChars).TrimEnd(lineTrimChars).Replace(' ', ';');

                var columns = textLineWithSemicolon.Split(columnTrimChars);

                if( (columns.Length < 2) || columns[0].Split(dateTrimChars).Length < 2)
                    continue;

                var dateArray = columns[0].Split(dateTrimChars);
                var year = int.Parse(dateArray[dateArray.Length == 3 ? 2 : 1], CultureInfo.InvariantCulture);

                var monthIndex = europeanDateFormat ? 1 : 0;
                var dayIndex = europeanDateFormat ? 0 : 1;

                var month = int.Parse(dateArray[monthIndex], CultureInfo.InvariantCulture);
                var day = (dateArray.Length == 3) ? int.Parse(dateArray[dayIndex], CultureInfo.InvariantCulture) : (int?)null;

                var dateTime = new DateTime(year, month, (day != null) ? day.Value : 1);
                var dateTimeBeginningOfTheYear = new DateTime(year, month: 1, day: 1);

                var deltaYear = (dateTime - dateTimeBeginningOfTheYear).TotalDays / 365.0;
                grid.Add(year + deltaYear);

                var price = double.Parse(columns[1], CultureInfo.InvariantCulture);
                curve.Add(price);
            }

            return new CurveConverterValues(
               curve: curve,
               grid: grid,
               checkDoubles: false);
        }

        private static CurveConverterValues ConvertDebuggerString(
            string debuggerString)
        {
            var curve = new List<double>();
            var grid = new List<double>();

            // E must not be replaced because it's part of a number as exponent (1.2246063538223773E-15)
            var debuggerStringWithoutDouble = Regex.Replace(debuggerString, "double", "d");
            var debuggerStringWithoutLetters = Regex.Replace(debuggerStringWithoutDouble, "[a-df-zA-DF-Z]", " ");
            var debuggerStringWithSingleSpaces = Regex.Replace(debuggerStringWithoutLetters, " {2,}", " ");
            
            var debuggerLines = debuggerStringWithSingleSpaces.Split('\n');

            var debuggerLinesFiltered = debuggerLines.
                Where(line => (line != "\r") && (line != "") && (line != "\t\t\r") &&
                !line.Contains("Count") && !line.Contains("Raw View") && !line.Contains("double[]")).ToArray();

            foreach (var fileLine in debuggerLinesFiltered)
            {
                var columns = fileLine.Split('\t');
                var columnsFiltered = columns.Where(
                    column => !column.Contains("=") && !Regex.IsMatch(column, @"\[\d+\]") && column.Any(char.IsDigit)).ToArray();
                var numberString = columnsFiltered.FirstOrDefault() ?? string.Empty;

                var splittedSubStringList = SplitString(numberString);
                string numberSubString;
                string gridString;

                if (splittedSubStringList.Length >= 2)
                {
                    gridString = splittedSubStringList[0];
                    numberSubString = splittedSubStringList[1];
                }
                else if(splittedSubStringList.Length == 1)
                {
                    if(columnsFiltered.Length > 1)
                    {
                        gridString = splittedSubStringList[0];
                        numberSubString = SplitString(columnsFiltered[1]).First();
                    }
                    else
                    {
                        gridString = string.Empty;
                        numberSubString = splittedSubStringList[0];
                    }      
                }
                else
                {
                    gridString = string.Empty;
                    numberSubString = numberString;
                }

                if (double.TryParse(numberSubString, out _))
                {
                    var value = double.Parse(numberSubString, CultureInfo.InvariantCulture);
                    
                    curve.Add(value);

                    if (double.TryParse(gridString, out _))
                        grid.Add(double.Parse(gridString, CultureInfo.InvariantCulture));
                }
            }

            return new CurveConverterValues(
               curve: curve,
               grid: grid);
        }

        private static CurveConverterValues ConvertTextString(
           string textString)
        {
            var grid = new List<double>();
            var curve = new List<double>();

            // E must not be replaced because it's part of a number as exponent (1.2246063538223773E-15)
            var textStringWithoutLetters = Regex.Replace(textString, "[a-df-zA-DF-Z\t\r]", " ");
            var textStringWithSingleSpaces = Regex.Replace(textStringWithoutLetters, " {2,}", " ");

            var textStringLines = textStringWithSingleSpaces.Split(
                new[] {'\n'}, StringSplitOptions.RemoveEmptyEntries).
                Where(line => line.Any(char.IsDigit)).ToList();

            if (textStringLines.Count >= 2)
            {
                grid = SplitString(textStringLines[0]).Select(gridString => double.Parse(gridString, CultureInfo.InvariantCulture)).ToList();
                curve = SplitString(textStringLines[1]).Select(valueString => double.Parse(valueString, CultureInfo.InvariantCulture)).ToList();
            }
            else
            {
                curve = SplitString(textStringLines.FirstOrDefault() ?? string.Empty).
                    Select(valueString => double.Parse(valueString, CultureInfo.InvariantCulture)).ToList();
            }

            return new CurveConverterValues(
               curve: curve,
               grid: grid);
        }

     
        private static CurveConverterValues ConvertArithmetricString(
            string arithmetricString)
        {
            var calculator = CalculatorFactory.Create();

            var result = calculator.CalculateForArithmetricInputsWithRange(arithmetricString, decimalPlaces: -1);

            var grid = result.Input.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).
                Where(value => value.Any(char.IsDigit)).Select(line => double.Parse(line, CultureInfo.InvariantCulture)).ToList();

            var curve = result.Output.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).
                Where(value => value.Any(char.IsDigit)).Select(line => double.Parse(line, CultureInfo.InvariantCulture)).ToList();

            return new CurveConverterValues(
               curve: curve,
               grid: grid);
        }

        private static string[] SplitString(string str) => 
            str.Split(new[] { ':', ',', '{', '}', '[', ']', '(', ')', ' ', ';', '/', '\\', '\r' }, StringSplitOptions.RemoveEmptyEntries).
            Where(line => line.Any(char.IsDigit)).ToArray();
    }
}
