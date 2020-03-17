namespace CalculatorAlgorithmsWrapper
{
    public interface ICalculator
    {
        string SetCalculationString(string calculationString);
        string SetKey(string key);
        string ClearAll();
        string Delete();
        string Calculate();
        double Calculate(string calculationString);

        string CalculateAndReturnString(string calculationString);
    }
}
