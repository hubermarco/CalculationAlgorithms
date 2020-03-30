using CalculationAlgorithm;
using System.Collections.Generic;
using System.Linq;

namespace CalculationAlgorithmWrapper
{
    internal class CalculationStringWrapper : ICalculationStringWrapper
    {
        private readonly ICalculationStringList _calculationStringList;
        private string _calculationString;

        internal CalculationStringWrapper(ICalculationStringList calculationStringList)
        {
            _calculationStringList = calculationStringList;
            _calculationString = "";
        }

        public bool IsStringInput(string inputString)
        {
            var isStringInput = _calculationStringList.IsStringInput(inputString);
            return isStringInput;
        }

        public string SetCalculationString(string calculationString)
        {
            var calculationStringPrepared = InputStringHelper.PrepareInputString(calculationString);

            if ( (calculationStringPrepared != "0") && !calculationStringPrepared.Contains("=") )
            {
                _calculationString = calculationStringPrepared;
            }

            return _calculationString;
        }

        public string SetKey(string key)
        {
            if (CanStringBeAppended(_calculationString, _calculationStringList, key))
            {
                _calculationString += key;
            }

            return _calculationString;
        }

        public string ClearAll()
        {
            _calculationString = "";

            return _calculationString;
        }

        public string Delete()
        {
            if (_calculationString.Length > 0)
            {
                _calculationString = _calculationString.Remove(_calculationString.Length - 1);
            }

            return _calculationString;
        }

        public string GetCalculationString()
        {
            return _calculationString;
        }

        public bool IsCalculationValid(string calculationString)
        {
            calculationString = calculationString.Replace(" ", "");

            var isStringValid = _calculationStringList.Create(calculationString).Count > 0;

            if(!_calculationStringList.IsStringInput(calculationString))
            {
                for (var i = 0; i < calculationString.Length - 1; i++)
                {
                    var substring = calculationString.Substring(0, i + 1);
                    var key = calculationString[i + 1].ToString();

                    if (!CanStringBeAppended(substring, _calculationStringList, key))
                    {
                        isStringValid = false;
                        break;
                    }
                }
            }

            return isStringValid;
        }

        private static bool CanStringBeAppended(
            string calculationString, 
            ICalculationStringList calculationStringList, 
            string key)
        {
            var keyChar = key.ElementAt(0);

            var retVal = calculationString.Length == 0 ? CheckFirstKey(keyChar) : CheckNextKey(calculationString, keyChar);

            retVal = retVal && CheckIfDotIsAllowed(calculationString, calculationStringList, keyChar);

            retVal = retVal && CheckIfCloseBracketIsAllowed(calculationString, keyChar);

            return retVal;
        }

        private static bool CheckFirstKey(char keyChar)
        {
            var retVal = true;

            var notAllowedCharList = CreateNotAllowedCharListForFirstKey();

            foreach (string t in notAllowedCharList)
            {
                var notAllowedChar = t[0];

                if (keyChar == notAllowedChar)
                {
                    retVal = false;
                    break;
                }
            }

            return retVal;
        }

        private static bool CheckNextKey(string calculationString, char keyChar)
        {
            bool retVal = true;

            var lastCharField = CreateLastCharField();
            var notAllowedCharField = CreateNotAllowedCharField();

            var lastChar = calculationString.ElementAt(calculationString.Length - 1);

            for (var outerIndex = 0; outerIndex < lastCharField.Count; outerIndex++)
            {
                var lastCharList = lastCharField[outerIndex];
                var notAllowedCharList = notAllowedCharField[outerIndex];

                foreach (var t1 in lastCharList)
                {
                    var lastCharOfList = t1[0];

                    foreach (var t in notAllowedCharList)
                    {
                        var notAllowedChar = t[0];

                        if ((lastChar == lastCharOfList) && (keyChar == notAllowedChar))
                        {
                            retVal = false;
                            break;
                        }
                    }
                }
            }
            return retVal;
        }

        private static bool CheckIfDotIsAllowed(
            string calculationString, 
            ICalculationStringList calculationStringList, 
            char keyChar)
        {
            bool retVal = true;

            if (keyChar == '.')
            {
                var inputStringList = calculationStringList.Create(calculationString);
                string lastCalculationString = inputStringList.Last();

                if (InputStringHelper.IsNumeric(lastCalculationString) &&
                    lastCalculationString.Contains("."))
                {
                    retVal = false;
                }
            }

            return retVal;
        }

        private static bool CheckIfCloseBracketIsAllowed(string calculationString, char keyChar)
        {
            bool retVal = true;

            if (keyChar == ')')
            {
                int numberBracketsOpen = GetNumberOfCharOccurenceInString(calculationString, '(');
                int numberBracketsClose = GetNumberOfCharOccurenceInString(calculationString, ')');

                if (numberBracketsClose >= numberBracketsOpen)
                {
                    retVal = false;
                }
            }

            return retVal;
        }

        private static IList<IList<string>> CreateLastCharField()
        {
            var lastCharField = new List<IList<string>>();

            var charList1 = new List<string> {"+", "-", "*", "/", ".", "^"};

            lastCharField.Add(charList1);

            var charList2 = new List<string> {"0", "1", "2", "3", "4", "5", "6", "7", "8", "9"};
            lastCharField.Add(charList2);

            var charList3 = new List<string> {")"};
            lastCharField.Add(charList3);

            var charList4 = new List<string> { "(" };
            lastCharField.Add(charList4);

            return lastCharField;
        }

        private static IList<IList<string>> CreateNotAllowedCharField()
        {
            var notAllowedCharField = new List<IList<string>>();

            var charList1 = new List<string> {"+", "-", "*", "/", ".", ")", "^"};
            notAllowedCharField.Add(charList1);

            var charList2 = new List<string> {"("};
            notAllowedCharField.Add(charList2);

            var charList3 = new List<string> {"0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "(", "."};
            notAllowedCharField.Add(charList3);

            var charList4 = new List<string> { "+", "*", "/", ".", ")", "^" };
            notAllowedCharField.Add(charList4);

            return notAllowedCharField;
        }

        private static IEnumerable<string> CreateNotAllowedCharListForFirstKey()
        {
            var charList1 = new List<string> {"+", "*", "/", ".", ")", "^"};

            return charList1;
        }

        private static int GetNumberOfCharOccurenceInString(string str, char character)
        {
            var counter = 0;

            for (int i = 0; i < str.Length; i++)
            {
                if (str.ElementAt(i) == character)
                {
                    counter++;
                }
            }

            return counter;
        }
    }
}
