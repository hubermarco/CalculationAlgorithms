using System;
using System.Globalization;
using System.Collections.Generic;
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

        public string CalculateForArithmetricInputs(string input, int decimalPlaces = -1)
        {
            var result = CalculateForArithmetricInputsWithRange(input, decimalPlaces).Output;

            return result;
        }

        public (IList<string> VariableNameList, List<string> VariableValuesUsedList, string Output) CalculateForArithmetricInputsWithRange(string input, int decimalPlaces = -1)
        {
            string result = "";
            var variableValuesUsedList = new List<string>();
            var variableNameList = new List<string>();
            var variableValuesList = new List<string[]>();

            var inputList = input.Split('|').Select(s => s.Trim()).ToArray();

            var termString = inputList.FirstOrDefault();

            var calcTreeResult = CreateCalcTreeResult(termString);

            string RoundResult2DecimalPlaces(double value, int decimalPlacesTemp)
            {
                return decimalPlacesTemp > -1 ? Math.Round(value, decimalPlacesTemp).ToString(CultureInfo.InvariantCulture) : value.ToString(CultureInfo.InvariantCulture);
            }

            if (inputList.Count() > 1)
            {
                var variableStringList = inputList.Where((x,index) => index > 0).ToList();
                
                for (var i = 0; i < variableStringList.Count; i++)
                {
                    GetVariableNameAndValues(variableStringList[i], out var variableNameString, out var variableValues);
                    variableValuesUsedList.Add(variableValues.Aggregate((total, next) => total + ", " + next));
                    variableNameList.Add(variableNameString);
                    variableValuesList.Add(variableValues);
                }

                for (var i = 1; i < variableValuesList.Count; i++)
                {
                    if (variableValuesList[0].Length != variableValuesList[i].Length)
                        throw new ArgumentException($"The number of values of {variableNameList[0]} has " +
                            $"{variableValuesList[0].Length} values but {variableNameList[i]} has {variableValuesList[i].Length} values.");
                }
                
                for (var i = 0; i < variableValuesList[0].Length; i++)
                {
                    for (var k = 0; k < variableValuesList.Count; k++)
                    {
                        calcTreeResult.SetVariable(
                            variableNameList[k], double.Parse(variableValuesList[k][i], CultureInfo.InvariantCulture));
                    }
                    result += $"{RoundResult2DecimalPlaces(calcTreeResult.GetResult(), decimalPlaces)}, ";
                }

                result = result.Remove(result.Length - 2);
            }
            else
            {
                result = $"{RoundResult2DecimalPlaces(calcTreeResult.GetResult(), decimalPlaces)}";
            }

            return (VariableNameList: variableNameList, VariableValuesUsedList: variableValuesUsedList, Output:result);
        }

        public string CalculateForStringInputs(string input)
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

            var loopValues = variableValuesStringWithoutBracktes.Split(':').Select(s => s.Trim()).ToArray();

            if( loopValues.Length == 2)
            {                 
                var startValue = double.Parse(loopValues[0], CultureInfo.InvariantCulture);
                var endValue = double.Parse(loopValues[1], CultureInfo.InvariantCulture);

                variableValues = Enumerable.Range(0, (int)(endValue - startValue) + 1).
                    Select(i => (startValue + i).ToString(CultureInfo.InvariantCulture)).ToArray();
            }
            else if(loopValues.Length == 3)
            {
                var startValue = double.Parse(loopValues[0], CultureInfo.InvariantCulture);
                var stepValue = double.Parse(loopValues[1], CultureInfo.InvariantCulture);
                var endValue = double.Parse(loopValues[2], CultureInfo.InvariantCulture);
                
                variableValues = Enumerable.Range(0, (int)((endValue - startValue) / stepValue) + 1).
                    Select(i => (startValue + i * stepValue).ToString(CultureInfo.InvariantCulture)).ToArray();
            }
            else
            {
                variableValues = variableValuesStringWithoutBracktes.Split(',', ' ', ';').
                    Select(s => s.Trim()).Where(x => !string.IsNullOrEmpty(x)).ToArray();
            }    
        }
    }
}
