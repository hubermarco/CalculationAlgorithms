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
                var findIndex = inputString.IndexOf(operatorString);

                if (findIndex >= 0)
                {
                    var isCharBeforeALetter = (findIndex > 0) && char.IsLetter(inputString[findIndex - 1]);
                    var isCharAfterALetter = (findIndex + operatorString.Length) < (inputString.Length - 1) &&
                                    char.IsLetter(inputString[findIndex + operatorString.Length]);

                    if (!isCharBeforeALetter && !isCharAfterALetter)
                    {
                        doesStringContainOperator = true;
                    }
                }
            }

            return doesStringContainOperator;
        }
    }
}
