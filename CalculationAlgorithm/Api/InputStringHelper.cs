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

            if ((inputStringWithoutBlanks.Length > 0) && (inputStringWithoutBlanks[0] == '-'))
            {
                inputStringWithoutBlanks = inputStringWithoutBlanks.Insert(0, "0");
            }

            if (inputStringWithoutBlanks != "0")
            {
                inputStringWithoutBlanks = inputStringWithoutBlanks.Replace("(-", "(0-");
            }


            return inputStringWithoutBlanks;
        }
    }
}
