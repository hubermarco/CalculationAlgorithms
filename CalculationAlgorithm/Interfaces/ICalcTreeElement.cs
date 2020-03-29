namespace CalculationAlgorithm
{
    internal interface ICalcTreeElement
    {
        ICalcTreeBranch GetParent();

        ICalcTreeBranch GetBranchAccess();

        void SetParent(ICalcTreeBranch parent);

        double GetResult();

        string GetResultString();

        void SetValue(double value);
    }
}
