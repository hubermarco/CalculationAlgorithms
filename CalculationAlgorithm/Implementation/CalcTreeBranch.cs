using System.Collections.Generic;
using System.Linq;

namespace CalculationAlgorithm
{
    internal class CalcTreeBranch : ICalcTreeBranch
    {
        private ICalcTreeBranch _parent;
        private readonly RuleSet _ruleSet;
        private bool _hasBrackets;
        private readonly IList<ICalcTreeElement> _calcTreeElements;
        private string _operatorString;

        internal CalcTreeBranch(ICalcTreeBranch parent, string operatorString, RuleSet ruleSet, bool isOpenBracket = false)
        {
            _parent = parent;
            _operatorString = operatorString;
            _ruleSet = ruleSet;
            _hasBrackets = isOpenBracket;
            _calcTreeElements = new List<ICalcTreeElement>();
        }

        public void AddElement(ICalcTreeElement element)
        {
            _calcTreeElements.Add(element);
        }

        public ICalcTreeBranch GetParent()
        {
            return _parent;
        }

        public ICalcTreeBranch GetBranchAccess()
        {
            return this;
        }

        public void SetParent(ICalcTreeBranch parent)
        {
            _parent = parent;
        }

        public void RemoveElement(ICalcTreeElement element)
        {
            for (var i = 0; i < _calcTreeElements.Count; i++)
            {
                if (element == _calcTreeElements[i])
                {
                    _calcTreeElements.Remove(element);
                }
            }
        }

        public void SetOperator(string operatorString)
        {
            _operatorString = operatorString;
        }

        public string GetOperator()
        {
            return _operatorString;
        }

        public double GetResult()
        {
            double result = 0;

            if (string.IsNullOrEmpty(_operatorString))
            {
                if (_calcTreeElements.Count > 0)
                {
                    result = _calcTreeElements.First().GetResult();
                }
            }
            else if (_ruleSet.IsOperator(_operatorString))
            {
                var operation = _ruleSet.ArithmetricOperators[_operatorString].Item2;
                var value1 = _calcTreeElements.First().GetResult();
                var value2 = _calcTreeElements.Last().GetResult();

                result = operation(value1, value2);
            }
            else if (_ruleSet.IsFunction(_operatorString))
            {
                var operation = _ruleSet.ArithmetricFunctions[_operatorString];

                var inputList = _calcTreeElements.Select(x => x.GetResult()).ToList();

                result = operation(inputList);
            }
            

            return result;
        }

        public string GetResultString()
        {
            string resultString = string.Empty;

            if (string.IsNullOrEmpty(_operatorString))
            {
                if (_calcTreeElements.Count > 0)
                {
                    resultString = _calcTreeElements.First().GetResultString();
                }
            }
            else if (_ruleSet.IsArithmetricStringFunction(_operatorString))
            {
                var operation = _ruleSet.ArithmetricStringFunctions[_operatorString];

                var inputList = _calcTreeElements.Select(x => x.GetResultString()).ToList();

                resultString = operation(inputList);
            }
            else if (_ruleSet.IsStringFunction(_operatorString))
            {
                var operation = _ruleSet.StringFunctions[_operatorString];

                var inputList = _calcTreeElements.Select(x => x.GetResultString()).ToList();

                resultString = operation(inputList);
            }

            return resultString;
        }

        public ICalcTreeElement GetLastChild()
        {
            var lastChild = _calcTreeElements.Last();
            return lastChild;
        }

        public bool HasBrackets()
        {
            return _hasBrackets;
        }

        public void SetValue(double value)
        {
            throw new System.Exception("CalcTreeBranch.SetValue() not allowed");
        }
    }
}
