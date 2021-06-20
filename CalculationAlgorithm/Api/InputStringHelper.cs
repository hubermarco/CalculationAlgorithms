using System;
using System.Collections.Generic;
using System.Linq;

namespace CalculationAlgorithm
{
    public class InputStringHelper
    {
        public static bool IsNumeric(string str)
        {
            return float.TryParse(str, out _);
        }

        public static bool IsOpenBracket(string str)
        {
            bool retVal = false;

            if (str.Length == 1)
            {
                if (str.ElementAt(0) == '(')
                {
                    retVal = true;
                }
            }

            return retVal;
        }

        public static bool IsCloseBracket(string str)
        {
            bool retVal = false;

            if (str.Length == 1)
            {
                if (str.ElementAt(0) == ')')
                {
                    retVal = true;
                }
            }
            return retVal;
        }

        public static bool IsComma(string str)
        {
            bool retVal = false;

            if (str.Length == 1)
            {
                if (str.ElementAt(0) == ',')
                {
                    retVal = true;
                }
            }
            return retVal;
        }

        public static string PrepareInputString(string inputString)
        {
            var inputStringWithoutBlanks = inputString.Replace("\n", "").Replace("\r", "");

            return inputStringWithoutBlanks;
        }

        public static bool DoesStringContainOperator(string inputString, string operatorString)
        {
            var doesStringContainOperator = false;

            if (inputString.Contains(operatorString))
            {
                var foundIndizes = AllIndexesOf(inputString, operatorString);
                
                if (foundIndizes.Count > 0)
                {
                    doesStringContainOperator = foundIndizes.Any(index => IsFoundOperatorStringOperator(
                        inputString, operatorString, doesStringContainOperator, index) );
                }
            }

            return doesStringContainOperator;
        }

        private static bool IsFoundOperatorStringOperator(string inputString, string operatorString, bool doesStringContainOperator, int findIndex)
        {
            var isCharBeforeALetter = (findIndex > 0) && char.IsLetter(inputString[findIndex - 1]);
            var isCharAfterALetter = (findIndex + operatorString.Length) < (inputString.Length - 1) &&
                            char.IsLetter(inputString[findIndex + operatorString.Length]);

            if (!isCharBeforeALetter && !isCharAfterALetter)
            {
                doesStringContainOperator = true;
            }

            return doesStringContainOperator;
        }

        public static IList<int> AllIndexesOf(string str, string value)
        {
            if (String.IsNullOrEmpty(value))
                throw new ArgumentException("the string to find may not be empty", "value");
            List<int> indexes = new List<int>();
            for (int index = 0; ; index += value.Length)
            {
                index = str.IndexOf(value, index);
                if (index == -1)
                    return indexes;
                indexes.Add(index);
            }
        }
    }
}
