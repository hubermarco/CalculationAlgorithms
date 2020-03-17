namespace CalculationAlgorithm
{
    internal interface ICalcTreeElementFactory
    {
        ICalcTreeElement CreateCalcTreeElement(double value, string variableString, ICalcTreeBranch parent);

        ICalcTreeBranch CreateCalcTreeBranch(ICalcTreeBranch parent, string operatorString, RuleSet ruleSet, bool isOpenBracket = false);
    }
}
