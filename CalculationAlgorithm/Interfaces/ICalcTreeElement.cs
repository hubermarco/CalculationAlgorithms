namespace CalculationAlgorithm
{
    internal interface ICalcTreeElement
    {
        ICalcTreeBranch GetParent();

        ICalcTreeBranch GetBranchAccess();

        void SetParent(ICalcTreeBranch parent);

        double GetResult();

        void SetValue(double value);
    }
}
