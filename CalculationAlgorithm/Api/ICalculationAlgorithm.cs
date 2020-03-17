namespace CalculationAlgorithm
{
    public interface ICalculationAlgorithm
    {
        double Calculate(string input);
        ICalcTreeResult CreateCalcTreeResult(string input);
    }
}
