using CalculationAlgorithm;
using System.Collections.Generic;
using System.Linq;

namespace CalculatorAlgorithmsWrapper
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

        public string SetCalculationString(string calculationString)
        {
            if (calculationString == "0")
            {
            }
            else if (calculationString.Contains("="))
            {
            }
            else
            {
                _calculationString = calculationString;
            }

            return _calculationString;
        }

        public string SetKey(string key)
        {
            if (CanStringBeAppended(key))
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

        private bool CanStringBeAppended(string key)
        {
            var keyChar = key.ElementAt(0);

            var retVal = _calculationString.Length == 0 ? CheckFirstKey(keyChar) : CheckNextKey(keyChar);

            retVal = retVal && CheckIfDotIsAllowed(keyChar);

            retVal = retVal && CheckIfCloseBracketIsAllowed(keyChar);

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

        private bool CheckNextKey(char keyChar)
        {
            bool retVal = true;

            var lastCharField = CreateLastCharField();
            var notAllowedCharField = CreateNotAllowedCharField();

            var lastChar = _calculationString.ElementAt(_calculationString.Length - 1);

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

        private bool CheckIfDotIsAllowed(char keyChar)
        {
            bool retVal = true;

            if (keyChar == '.')
            {
                var inputStringList = _calculationStringList.Create(_calculationString);
                string lastCalculationString = inputStringList.Last();

                if (InputStringHelper.IsNumeric(lastCalculationString) &&
                    lastCalculationString.Contains("."))
                {
                    retVal = false;
                }
            }

            return retVal;
        }

        private bool CheckIfCloseBracketIsAllowed(char keyChar)
        {
            bool retVal = true;

            if (keyChar == ')')
            {
                int numberBracketsOpen = GetNumberOfCharOccurenceInString(_calculationString, '(');
                int numberBracketsClose = GetNumberOfCharOccurenceInString(_calculationString, ')');

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

            var charList1 = new List<string> {"+", "-", "*", "/", ".", "(", "^"};

            lastCharField.Add(charList1);

            var charList2 = new List<string> {"0", "1", "2", "3", "4", "5", "6", "7", "8", "9"};
            lastCharField.Add(charList2);

            var charList3 = new List<string> {")"};
            lastCharField.Add(charList3);

            return lastCharField;
        }

        private static IList<IList<string>> CreateNotAllowedCharField()
        {
            var notAllowedCharField = new List<IList<string>>();

            var charList1 = new List<string> {"+", "*", "/", ".", ")", "^"};
            notAllowedCharField.Add(charList1);

            var charList2 = new List<string> {"("};
            notAllowedCharField.Add(charList2);

            var charList3 = new List<string> {"0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "(", "."};
            notAllowedCharField.Add(charList3);

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
