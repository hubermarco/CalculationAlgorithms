
namespace CalculationAlgorithm
{
    internal class CalcTreeElement : ICalcTreeElement
    {
        private ICalcTreeBranch _parent;
        private double _value;
        private string _stringValue;
        private readonly string _variableString;

        internal CalcTreeElement(ICalcTreeBranch parent, double value, string stringValue = "", string variableString = "")
        {
            _value = value;
            _stringValue = stringValue;
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

        public string GetResultString()
        {
            return _stringValue;
        }

        public void SetParent(ICalcTreeBranch parent)
        {
            _parent = parent;
        }

        public void SetValue(double value)
        {
            _value = value;
        }

        public void SetStringValue(string stringValue)
        {
            _stringValue = stringValue;
        }
    }
}
