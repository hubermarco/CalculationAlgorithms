namespace CalculationAlgorithm
{
    public interface ICalculationAlgorithm
    {
        bool IsStringInput(string input);

        double Calculate(string input);

        string CalculateString(string input);

        ICalcTreeResult CreateCalcTreeResult(string input);
    }
}
