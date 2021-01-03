namespace CalculationAlgorithmWrapper
{
    public interface ICalculator
    {
        string SetKey(string key);
        string ClearAll();
        string Delete();
        double Calculate(string calculationString);
        string CalculateString(string calculationString);

        // needed for Android Calculator
        string Calculate();

        // needed for WebApplication
        string SetCalculationString(string calculationString);
        string CalculateAndReturnString(string calculationString);
    }
}
