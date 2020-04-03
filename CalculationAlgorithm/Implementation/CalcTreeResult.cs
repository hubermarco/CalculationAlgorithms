using System.Collections.Generic;
using System.Linq;

namespace CalculationAlgorithm
{
    internal class CalcTreeResult : ICalcTreeResult
    {
        private readonly ICalcTreeElement _root;
        private readonly IDictionary<string, IList<ICalcTreeElement>> _variableDict;

        internal CalcTreeResult(ICalcTreeElement root, IDictionary<string, IList<ICalcTreeElement>> variableDict)
        {
            _root = root;
            _variableDict = variableDict;
        }

        public void SetVariable(string variableString, double value)
        {
            var variableList = _variableDict[variableString];

            foreach (var calcTreeElement in variableList)
            {
                calcTreeElement.SetValue(value);
                calcTreeElement.SetStringValue(value.ToString());
            }
        }

        public IList<string> GetVariableList()
        {
            var variableList = _variableDict.Keys;

            return variableList.ToList();
        }

        public double GetResult()
        {
            var result = _root.GetResult();

            return result;
        }

        public string GetResultString()
        {
            var result = _root.GetResultString();

            return result;
        }
    }
}
