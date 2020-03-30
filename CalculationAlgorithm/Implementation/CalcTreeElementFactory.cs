namespace CalculationAlgorithm
{
    internal class CalcTreeElementFactory : ICalcTreeElementFactory
    {
        public ICalcTreeElement CreateCalcTreeElement(ICalcTreeBranch parent, double value, string stringValue, string variableString)
        {
            return new CalcTreeElement(parent, value, stringValue, variableString);
        }

        public ICalcTreeBranch CreateCalcTreeBranch(ICalcTreeBranch parent, string operatorString, RuleSet ruleSet, bool isOpenBracket)
        {
            return new CalcTreeBranch(parent, operatorString, ruleSet, isOpenBracket);
        }
    }
}
