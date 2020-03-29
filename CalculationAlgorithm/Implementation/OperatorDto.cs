namespace CalculationAlgorithm
{
    internal class OperatorDto
    {
        internal OperatorDto(bool isOperator, bool isStringOperator, string operatorString)
        {
            IsOperator = isOperator;
            IsStringOperator = isStringOperator;
            OperatorString = operatorString;
        }

        internal bool IsOperator { get; }
        public bool IsStringOperator { get; }
        internal string OperatorString { get; }
    }
}
