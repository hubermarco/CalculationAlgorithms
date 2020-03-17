
namespace CalculationAlgorithm
{
    internal class CalcTreeElement : ICalcTreeElement
    {
        private ICalcTreeBranch _parent;
        private double _value;
        private readonly string _variableString;

        internal CalcTreeElement(ICalcTreeBranch parent, double value, string variableString = "")
        {
            _value = value;
            _variableString = variableString;
            _parent = parent;
        }

        public ICalcTreeBranch GetParent()
        {
            return _parent;
        }

        public ICalcTreeBranch GetBranchAccess()
        {
            return null;
        }

        public double GetResult()
        {
            return _value;
        }

        public void SetParent(ICalcTreeBranch parent)
        {
            _parent = parent;
        }

        public void SetValue(double value)
        {
            _value = value;
        }
    }
}
