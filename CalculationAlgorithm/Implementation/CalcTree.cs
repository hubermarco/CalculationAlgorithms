using System.Collections.Generic;

namespace CalculationAlgorithm
{
    internal class CalcTree : ICalcTree
    {
        private readonly ICalcTreeElementFactory _calcTreeElementFactory;
        private readonly RuleSet _ruleSet;

        internal CalcTree(ICalcTreeElementFactory calcTreeElementFactory, RuleSet ruleSet)
        {
            _calcTreeElementFactory = calcTreeElementFactory;
            _ruleSet = ruleSet;
        }

        public ICalcTreeResult Create(IList<string> inputList)
        {
            var variableDict = new Dictionary<string, IList<ICalcTreeElement>>();
            var calcTreeElementRoot = CalcTreeHelper.CreateRootBranch(_calcTreeElementFactory, _ruleSet);
            ICalcTreeElement calcTreeElementCurrentPosition = calcTreeElementRoot;
            var operatorHierarchyLevelBefore = 0;

            for (var i = 0; i < inputList.Count; i++)
            {
                var currentCalcString = inputList[i];

                if (InputStringHelper.IsNumeric(currentCalcString))
                {
                    CalcTreeHelper.AddNewCalcTreeElementWithCurrentValueAtCurrentPosition(
                        currentCalcString,
                        _ruleSet,
                        _calcTreeElementFactory, 
                        ref calcTreeElementCurrentPosition);
                }

                else if(_ruleSet.IsVariable(currentCalcString))
                {
                    CalcTreeHelper.AddNewCalcTreeElementWithCurrentValueAtCurrentPosition(
                        currentCalcString,
                        _ruleSet,
                        _calcTreeElementFactory,
                        ref calcTreeElementCurrentPosition);

                    CalcTreeHelper.AddCalcTreeElementToVariableDictionary(
                        variableDict,
                        currentCalcString,
                        calcTreeElementCurrentPosition);
                }

                else if(_ruleSet.IsOperator(currentCalcString))
                {
                    CalcTreeHelper.AddNewBranchWithCurrentOperatorAtCorrectPosition(
                        currentCalcString, 
                        _calcTreeElementFactory, 
                        _ruleSet, 
                        ref operatorHierarchyLevelBefore, 
                        ref calcTreeElementCurrentPosition, 
                        ref calcTreeElementRoot);
                }

                else if(_ruleSet.IsFunction(currentCalcString))
                {
                    CalcTreeHelper.AddNewBranchWithOperatorStringBelowCurrentPosition(
                        currentCalcString,
                        _calcTreeElementFactory,
                        _ruleSet,
                        ref calcTreeElementCurrentPosition);

                    // Jump over open bracket
                    i++;
                }

                else if(InputStringHelper.IsOpenBracket(currentCalcString))
                {
                    CalcTreeHelper.AddNewBranchWithOperatorStringBelowCurrentPosition(
                       currentCalcString:"",
                       _calcTreeElementFactory,
                       _ruleSet,
                       ref calcTreeElementCurrentPosition);
                }

                else if(InputStringHelper.IsCloseBracket(currentCalcString) ||
                        InputStringHelper.IsComma(currentCalcString))
                {
                    CalcTreeHelper.NavigateUpToNextBranchWithBrackets(
                        ref calcTreeElementCurrentPosition);
                }
                else
                {
                    CalcTreeHelper.AddNewCalcTreeElementWithCurrentValueAtCurrentPosition(
                        currentCalcString,
                        _ruleSet,
                        _calcTreeElementFactory,
                        ref calcTreeElementCurrentPosition);
                }
            }

            return new CalcTreeResult(calcTreeElementRoot, variableDict);
        }
    }
}
