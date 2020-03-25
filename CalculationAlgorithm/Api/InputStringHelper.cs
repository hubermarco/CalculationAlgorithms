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
            var inputStringWithoutBlanks = inputString.Replace(" ", "").Replace("\n", "").Replace("\r", "");

            return inputStringWithoutBlanks;
        }
    }
}
