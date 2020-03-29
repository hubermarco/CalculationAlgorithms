namespace CalculationAlgorithm
{
    internal interface ICalcTreeElementFactory
    {
        ICalcTreeElement CreateCalcTreeElement(ICalcTreeBranch parent, double value, string stringValue, string variableString);

        ICalcTreeBranch CreateCalcTreeBranch(
            ICalcTreeBranch parent, 
            string operatorString, 
            RuleSet ruleSet, 
            bool isOpenBracket = false);
    }
}
