
using System.Collections.Generic;
using System.Linq;

namespace CalculationAlgorithm
{
    internal class CalculationStringList : ICalculationStringList
    {
        private readonly IList<string> _operatorList;
        private readonly IList<string> _arithmetricStringOperatorList;

        internal CalculationStringList(IList<string> operatorList, IList<string> arithmetricStringOperatorList)
        {
            _operatorList = operatorList;
            _arithmetricStringOperatorList = arithmetricStringOperatorList;
        }

        public bool IsStringInput(string inputString)
        {
            var isStringInput = IsOperatorOfListInInputString(inputString, _arithmetricStringOperatorList);

            return isStringInput;
        }

        public IList<string> Create(string inputString)
        {
            List<string> inputStringList;

            var preparedInput = PrepareString(inputString);

            if (IsOperatorOfListInInputString(preparedInput, _arithmetricStringOperatorList))
            {
                inputStringList = CreateFromArithmetricStringOperatorList(preparedInput, _arithmetricStringOperatorList);
            }
            else
            {
                inputStringList = CreateFromOperatorList(preparedInput, _operatorList);
            }

            return inputStringList;
        }

        private static List<string> CreateFromOperatorList(
            string inputString,
            IList<string> operatorList)
        {
            var inputStringList = new List<string>();

            var numberString = "";
            var valueString = "";
            var isValueString = false;

            for (var i = 0; i < inputString.Length; i++)
            {
                var operatorDto = GetOperatorInfo(operatorList, null, inputString, index: i);
                var currentChar = inputString[i];
                var currentString = $"{currentChar}";

                if (operatorDto.IsOperator && !(isValueString && char.IsLetter(currentChar)) )
                {
                    isValueString = false;

                    if (numberString.Length > 0)
                    {
                        inputStringList.Add(numberString);
                        numberString = "";
                    }

                    if (valueString.Length > 0)
                    {
                        inputStringList.Add(valueString);
                        valueString = "";
                    }

                    inputStringList.Add(operatorDto.OperatorString);
                    i += operatorDto.OperatorString.Length - 1;
                }
                else if (IsNumber(currentChar))
                {
                    isValueString = false;

                    numberString += currentString;

                    if (i == inputString.Length - 1)
                    {
                        inputStringList.Add(numberString);
                    }
                }
                else if (IsBracket(currentChar) ||
                        IsComma(currentChar))
                {
                    isValueString = false;

                    if (numberString.Length > 0)
                    {
                        inputStringList.Add(numberString);
                        numberString = "";
                    }

                    if (valueString.Length > 0)
                    {
                        inputStringList.Add(valueString);
                        valueString = "";
                    }

                    inputStringList.Add(currentString);
                }
                else // if inputString contains unknown letters then they are added to valueString
                {
                    isValueString = true;

                    valueString += currentString;

                    if (i == inputString.Length - 1)
                    {
                        inputStringList.Add(valueString);
                    }
                }
            }
            return inputStringList;
        }

        private static List<string> CreateFromArithmetricStringOperatorList(
           string inputString,
           IList<string> arithmetricStringOperatorList)
        {
            var inputStringList = new List<string>();
            var bracketLevel = 0;
            var stringOperatorBracketLevel = 0;

            var operationString = "";

            for (var i = 0; i < inputString.Length; i++)
            {
                var operatorDto = GetOperatorInfo(null, arithmetricStringOperatorList, inputString, index: i);
                var currentChar = inputString[i];
                var currentString = $"{currentChar}";

                if (operatorDto.IsStringOperator)
                {
                    stringOperatorBracketLevel++;

                    if (operationString.Length > 0)
                    {
                        inputStringList.Add(operationString);
                        operationString = "";
                    }
                    inputStringList.Add(operatorDto.OperatorString);
                    i += operatorDto.OperatorString.Length - 1;
                }
                else if (IsOpenBracket(currentChar))
                {
                    bracketLevel++;

                    if(bracketLevel == stringOperatorBracketLevel)
                    {
                        if (operationString.Length > 0)
                        {
                            inputStringList.Add(operationString);
                            operationString = "";
                        }

                        inputStringList.Add(currentString);
                    }      
                    else
                    {
                        operationString += currentString;

                        if (i == inputString.Length - 1)
                        {
                            inputStringList.Add(operationString);
                        }
                    }
                }
                else if(IsCloseBracket(currentChar))
                {
                    if (bracketLevel == stringOperatorBracketLevel)
                    {
                        if (operationString.Length > 0)
                        {
                            inputStringList.Add(operationString);
                            operationString = "";

                            inputStringList.Add(currentString);
                        }

                        stringOperatorBracketLevel--;
                    }
                    else
                    {
                        operationString += currentString;

                        if (i == inputString.Length - 1)
                        {
                            inputStringList.Add(operationString);
                        }
                    }
                    bracketLevel--;
                }
                else if (IsComma(currentChar))
                {
                    if (operationString.Length > 0)
                    {
                        inputStringList.Add(operationString);
                        operationString = "";
                    }

                    inputStringList.Add(currentString);
                }
                else
                {
                    operationString += currentString;

                    if (i == inputString.Length - 1)
                    {
                        inputStringList.Add(operationString);
                    }
                }
            }
            return inputStringList;
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

        private static bool IsOpenBracket(char currentChar)
        {
            bool retVal = false;

            switch (currentChar)
            {
                case '(':
                case '[':
              
                    retVal = true;
                    break;
            }
            return retVal;
        }

        private static bool IsCloseBracket(char currentChar)
        {
            bool retVal = false;

            switch (currentChar)
            {
                case ')':
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

        private static OperatorDto GetOperatorInfo(
            IList<string> operatorList, 
            IList<string> stringOperatorList, 
            string inputString, 
            int index)
        {
            var operatorDto = new OperatorDto(isOperator: false, isStringOperator: false, operatorString: "");

            if(operatorList != null)
            {
                for (var i = 0; i < operatorList.Count; i++)
                {
                    var findIndex = inputString.IndexOf(operatorList[i], index);

                    if(findIndex == index)
                    {
                        operatorDto = new OperatorDto(isOperator: true, isStringOperator: false, operatorString: operatorList[i]);
                        break;
                    }
                }
            }

            if (stringOperatorList != null)
            {
                for (var i = 0; i < stringOperatorList.Count; i++)
                {
                    var findIndex = inputString.IndexOf(stringOperatorList[i], index);

                    if (findIndex == index)
                    {
                        operatorDto = new OperatorDto(isOperator: false, isStringOperator: true, operatorString: stringOperatorList[i]);
                        break;
                    }
                }
            }

            return operatorDto;
        }

        private static bool IsOperatorOfListInInputString(string inputString, IList<string> operatorList)
        {
            var isOperatorOfListInInputString = false;

            if(operatorList != null)
            {
                foreach (var operatorString in operatorList)
                {
                    if (InputStringHelper.DoesStringContainOperator(inputString, operatorString))
                    {
                        isOperatorOfListInInputString = true;
                        break;
                    }
                }
            }

            return isOperatorOfListInInputString;
        }

        private static string PrepareString(string inputString)
        {
            var preparedInput = InputStringHelper.PrepareInputString(inputString);

            preparedInput = preparedInput.Replace(" ", "");

            if ((preparedInput.Length > 0) && (preparedInput[0] == '-'))
            {
                preparedInput = preparedInput.Insert(0, "0");
            }

            if (preparedInput != "0")
            {
                preparedInput = preparedInput.Replace("(-", "(0-");
            }

            return preparedInput;
        }
    }
}
