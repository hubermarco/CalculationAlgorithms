namespace CalculationAlgorithm
{
    internal class CalcTreeElementFactory : ICalcTreeElementFactory
    {
        public ICalcTreeElement CreateCalcTreeElement(double value, string variableString, ICalcTreeBranch parent)
        {
            return new CalcTreeElement(parent, value, variableString);
        }

        public ICalcTreeBranch CreateCalcTreeBranch(ICalcTreeBranch parent, string operatorString, RuleSet ruleSet, bool isOpenBracket)
        {
            return new CalcTreeBranch(parent, operatorString, ruleSet, isOpenBracket);
        }
    }
}
