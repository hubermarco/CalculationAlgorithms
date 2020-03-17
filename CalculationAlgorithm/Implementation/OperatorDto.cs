namespace CalculationAlgorithm
{
    internal class OperatorDto
    {
        internal OperatorDto(bool isOperator, string operatorString)
        {
            IsOperator = isOperator;
            OperatorString = operatorString;
        }

        internal bool IsOperator { get; }
        internal string OperatorString { get; }
    }
}
