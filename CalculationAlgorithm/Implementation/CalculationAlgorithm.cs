using System.Globalization;
using System.Linq;

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

        public double Calculate(string input)
        {
            var inputList = input.Split('|').Select(s => s.Trim()).ToArray();

            var termString = inputList.FirstOrDefault();

            var calcTreeResult = CreateCalcTreeResult(termString);

            if(inputList.Count() > 1)
            {
                var variableString = inputList[1];
                var variableStringList = variableString.Split('=').Select(s => s.Trim()).ToArray();
                calcTreeResult.SetVariable(
                    variableStringList[0], double.Parse(variableStringList[1], CultureInfo.InvariantCulture));
            }
             
            var result = calcTreeResult.GetResult();

            return result;
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
    }
}
