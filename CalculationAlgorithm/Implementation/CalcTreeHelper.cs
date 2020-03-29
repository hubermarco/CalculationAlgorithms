using System.Collections.Generic;

namespace CalculationAlgorithm
{
    internal class CalcTreeHelper
    {
        internal static ICalcTreeBranch CreateRootBranch(
            ICalcTreeElementFactory calcTreeElementFactory,
            RuleSet ruleSet)
        {
            ICalcTreeBranch calcTreeElementRoot = calcTreeElementFactory.CreateCalcTreeBranch
                (parent: null, operatorString: "", ruleSet: ruleSet);

            return calcTreeElementRoot;
        }

        internal static void AddNewCalcTreeElementWithCurrentValueAtCurrentPosition(
            string currentCalcString,
            RuleSet ruleSet,
            ICalcTreeElementFactory calcTreeElementFactory,
            ref ICalcTreeElement calcTreeElementCurrent)
        {
            ICalcTreeElement calcTreeElement;
            if (InputStringHelper.IsNumeric(currentCalcString))
            {
                var number = double.Parse(currentCalcString, System.Globalization.CultureInfo.InvariantCulture);

                calcTreeElement = calcTreeElementFactory.CreateCalcTreeElement(
                    parent: calcTreeElementCurrent.GetBranchAccess(),
                    value: number, 
                    stringValue: "",
                    variableString: "");
            }
            else if(ruleSet.IsVariable(currentCalcString))
            {
                calcTreeElement = calcTreeElementFactory.CreateCalcTreeElement(
                    parent: calcTreeElementCurrent.GetBranchAccess(),
                    value: 0,
                    stringValue: "",
                    variableString: currentCalcString);
            }
            else // currentCalcString is a parameter string
            {
                calcTreeElement = calcTreeElementFactory.CreateCalcTreeElement(
                   parent: calcTreeElementCurrent.GetBranchAccess(),
                   value: 0,
                   stringValue: currentCalcString,
                   variableString: ""
                   );
            }
            
            calcTreeElementCurrent.GetBranchAccess().AddElement(calcTreeElement);
            calcTreeElementCurrent = calcTreeElement;
        }

        internal static void AddCalcTreeElementToVariableDictionary(
            IDictionary<string, IList<ICalcTreeElement>> variableDict,
            string currentCalcString,
            ICalcTreeElement calcTreeElementCurrentPosition)
        {
            if (variableDict.ContainsKey(currentCalcString))
            {
                variableDict[currentCalcString].Add(calcTreeElementCurrentPosition);
            }
            else
            {
                variableDict.Add(currentCalcString, new List<ICalcTreeElement> { calcTreeElementCurrentPosition });
            }
        }

        internal static void AddNewBranchWithCurrentOperatorAtCorrectPosition(
            string currentCalcString,
            ICalcTreeElementFactory calcTreeElementFactory,
            RuleSet ruleSet,
            ref int operatorHierarchyLevelBefore,
            ref ICalcTreeElement calcTreeElementCurrent,
            ref ICalcTreeBranch calcTreeElementRoot)
        {
            var operatorHierarchyLevelDifference = ruleSet.GetOperatorHierarchyLevelDifference(
                        currentCalcString, operatorHierarchyLevelBefore);

            if (operatorHierarchyLevelDifference <= 0)
            {
                calcTreeElementCurrent = calcTreeElementCurrent.GetParent();

                if (IsRootBranchWithMissingOperator(calcTreeElementCurrent))
                {
                    calcTreeElementCurrent.GetBranchAccess().SetOperator(currentCalcString);
                }
                else
                {
                    GoUpUntilOperatorLevelHierarchyIsEqualOrGreaterThanCurrent(
                        currentCalcString,
                        ruleSet,
                        ref calcTreeElementCurrent);

                    AddBranchBetweenCurrentPositionAndItsParentWithOperatorString(
                       currentCalcString,
                       calcTreeElementFactory,
                       ruleSet,
                       ref calcTreeElementCurrent,
                       ref calcTreeElementRoot);
                }
            }
            else // (operatorHierarchyLevelDifference > 0)
            {
                AddBranchBetweenCurrentPositionAndItsParentWithOperatorString(
                    currentCalcString,
                    calcTreeElementFactory,
                    ruleSet,
                    ref calcTreeElementCurrent,
                    ref calcTreeElementRoot);
            }

            operatorHierarchyLevelBefore = ruleSet.GetOperatorHierarchyLevel(currentCalcString);
        }

        internal static void AddNewBranchWithOperatorStringBelowCurrentPosition(
            string currentCalcString,
            ICalcTreeElementFactory calcTreeElementFactory,
            RuleSet ruleSet,
            ref ICalcTreeElement calcTreeElementCurrent)
        {
            var calcTreeBranch = calcTreeElementFactory.CreateCalcTreeBranch(
                 parent: calcTreeElementCurrent.GetBranchAccess(), 
                 operatorString: currentCalcString, 
                 ruleSet: ruleSet, 
                 isOpenBracket: true);

            calcTreeElementCurrent.GetBranchAccess().AddElement(calcTreeBranch);
            calcTreeElementCurrent = calcTreeBranch;
        }

        internal static void NavigateUpToNextBranchWithBrackets(
            ref ICalcTreeElement calcTreeElementCurrent)
        {
            do
            {
                calcTreeElementCurrent = calcTreeElementCurrent.GetParent();
            }
            while (!calcTreeElementCurrent.GetBranchAccess().HasBrackets() &&
                           (calcTreeElementCurrent.GetParent() != null));
        }

        private static void AddBranchBetweenCurrentPositionAndItsParentWithOperatorString(
            string currentOperatorString,
            ICalcTreeElementFactory calcTreeElementFactory,
            RuleSet ruleSet,
            ref ICalcTreeElement calcTreeElementCurrent,
            ref ICalcTreeBranch calcTreeElementRoot)
        {
            var parent = calcTreeElementCurrent.GetParent();

            var calcTreeBranch = calcTreeElementFactory.CreateCalcTreeBranch(
                parent: parent, operatorString: currentOperatorString, ruleSet: ruleSet);

            calcTreeBranch.AddElement(calcTreeElementCurrent);

            calcTreeElementCurrent.SetParent(calcTreeBranch);

            if (parent == null)
            {
                calcTreeElementRoot = calcTreeBranch;
            }
            else
            {
                parent.RemoveElement(calcTreeElementCurrent);
                parent.AddElement(calcTreeBranch);
            }

            calcTreeElementCurrent = calcTreeBranch;
        }

        private static bool IsRootBranchWithMissingOperator(
            ICalcTreeElement calcTreeElementCurrent)
        {
            var hasRootBranchMissingOperator = (calcTreeElementCurrent.GetBranchAccess().GetOperator() == "") &&
                     (calcTreeElementCurrent.GetParent() == null);

            return hasRootBranchMissingOperator;
        }

        private static void GoUpUntilOperatorLevelHierarchyIsEqualOrGreaterThanCurrent(
            string currentCalcString,
            RuleSet ruleSet,
            ref ICalcTreeElement calcTreeElementCurrent)
        {
            while (ruleSet.GetOperatorHierarchyLevel(calcTreeElementCurrent.GetBranchAccess().GetOperator()) >
                   ruleSet.GetOperatorHierarchyLevel(currentCalcString))
            {
                calcTreeElementCurrent = calcTreeElementCurrent.GetParent();
            }

            if (ruleSet.GetOperatorHierarchyLevel(calcTreeElementCurrent.GetBranchAccess().GetOperator()) <
                ruleSet.GetOperatorHierarchyLevel(currentCalcString))
            {
                calcTreeElementCurrent = calcTreeElementCurrent.GetBranchAccess().GetLastChild();
            }
        }
    }
}
