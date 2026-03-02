using System.Globalization;
using System.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace CalculationAlgorithm
{
    internal class CalculationAlgorithm : ICalculationAlgorithm
    {
        private readonly ICalcTree _calcTree;
        private readonly ICalculationStringList _calculationStringList;

        internal CalculationAlgorithm(ICalcTree calcTree, ICalculationStringList calculationStringList)
        {
            _calcTree = calcTree;
            _calculationStringList = calculationStringList;
        }

        public bool IsStringInput(string input)
        {
            var isStringInput = _calculationStringList.IsStringInput(input);
            return isStringInput;
        }

        public string Calculate(string input)
        {
            string result = "";

            var inputList = input.Split('|').Select(s => s.Trim()).ToArray();

            var termString = inputList.FirstOrDefault();

            var calcTreeResult = CreateCalcTreeResult(termString);

            if(inputList.Count() > 1)
            {
                var variableString = inputList[1];

                GetVariableNameAndValues(variableString, out var variableNameString, out var variableValues);
              
                foreach(var variableValue in variableValues)
                {
                    calcTreeResult.SetVariable(
                        variableNameString, double.Parse(variableValue, CultureInfo.InvariantCulture));

                    result += $"{calcTreeResult.GetResult()}, ";
                }

                result = result.Remove(result.Length - 2);
            }
            else
            {
                result = $"{calcTreeResult.GetResult()}";
            } 

            return $"{result}";
        }

        public string CalculateString(string input)
        {
            var calcTreeResult = CreateCalcTreeResult(input);

            var result = calcTreeResult.GetResultString();

            return result;
        }

        public ICalcTreeResult CreateCalcTreeResult(string input)
        {
            var calculationStringList = _calculationStringList.Create(input);

            var calcTreeResult = _calcTree.Create(calculationStringList);

            return calcTreeResult;
        }

        private void GetVariableNameAndValues(string variableString, out string variableNameString, out string[] variableValues)
        {
            var variableStringList = variableString.Split('=').Select(s => s.Trim()).ToArray();
            variableNameString = variableStringList[0];
            var variableValuesString = variableStringList[1];

            var variableValuesStringWithoutBracktes = variableValuesString.
                Replace("[", "").Replace("]", "").Replace("(", "").Replace(")", "").Replace("{", "").Replace("}", "");

            variableValues = variableValuesStringWithoutBracktes.Split(',', ' ', ';').
                Select(s => s.Trim()).Where(x => !string.IsNullOrEmpty(x)).ToArray();
        }
    }
}
