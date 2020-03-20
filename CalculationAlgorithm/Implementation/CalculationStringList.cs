using System.Collections.Generic;

namespace CalculationAlgorithm
{
    internal class CalculationStringList : ICalculationStringList
    {
        private readonly IList<string> _operatorList;

        internal CalculationStringList(IList<string> operatorList)
        {
            _operatorList = operatorList;
        }

        public IList<string> Create(string inputString)
        {
            var inputStringList = new List<string>();

            var preparedInputString = PrepareInputString(inputString);

            var numberString = "";

            for (var i = 0; i < preparedInputString.Length; i++)
            {
                var operatorDto = GetOperatorInfo(_operatorList, preparedInputString, index: i);
                var currentChar = preparedInputString[i];
                var currentString = $"{currentChar}";

                if (IsNumber(currentChar))
                {
                    numberString += currentString;

                    if (i == preparedInputString.Length - 1)
                    {
                        inputStringList.Add(numberString);
                    }
                }
                else if (operatorDto.IsOperator)
                {
                    if (numberString.Length > 0)
                    {
                        inputStringList.Add(numberString);
                        numberString = "";
                    }
                    inputStringList.Add(operatorDto.OperatorString);
                    i += operatorDto.OperatorString.Length - 1;
                }
                else if(IsBracket(currentChar) ||
                        IsComma(currentChar))
                {
                    if (numberString.Length > 0)
                    {
                        inputStringList.Add(numberString);
                        numberString = "";
                    }

                    inputStringList.Add(currentString);
                }
            }
            return inputStringList;
        }

        private string PrepareInputString(string inputString)
        {
            var inputStringWithoutBlanks = inputString.Replace(" ", "");
      
            if ((inputStringWithoutBlanks.Length > 0) && (inputStringWithoutBlanks[0] == '-'))
            {
                inputStringWithoutBlanks = inputStringWithoutBlanks.Insert(0, "0");
            }

            if( inputStringWithoutBlanks != "0")
            {
                inputStringWithoutBlanks = inputStringWithoutBlanks.Replace("(-", "(0-");
            }
            

            return inputStringWithoutBlanks;
        }

        private static bool IsNumber(char currentChar)
        {
            bool retVal = false;

            switch (currentChar)
            {
                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                case '.':
                    retVal = true;
                    break;
            }
            return retVal;
        }

        private static bool IsBracket(char currentChar)
        {
            bool retVal = false;

            switch (currentChar)
            {
                case '(':
                case ')':
                case '[':
                case ']':
                    retVal = true;
                    break;
            }
            return retVal;
        }

        private static bool IsComma(char currentChar)
        {
            bool retVal = currentChar == ',';

            return retVal;
        }

        private static OperatorDto GetOperatorInfo(IList<string> operatorList, string inputString, int index)
        {
            var operatorDto = new OperatorDto(isOperator: false, operatorString: "");

            for (var i = 0; i < operatorList.Count; i++)
            {
                var findIndex = inputString.IndexOf(operatorList[i], index);

                if(findIndex == index)
                {
                    operatorDto = new OperatorDto(isOperator: true, operatorString: operatorList[i]);
                    break;
                }
            }

            return operatorDto;
        }
    }
}
