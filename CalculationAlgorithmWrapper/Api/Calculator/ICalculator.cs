namespace CalculationAlgorithmWrapper
{
    public interface ICalculator
    {
        string SetKey(string key);
        string ClearAll();
        string Delete();
        string CalculateForArithmetricInputs(string calculationString);
        string CalculateForStringInputs(string calculationString);

        // needed for Android Calculator
        string CalculateForArithmetricInputs();

        // needed for WebApplication
        string SetCalculationString(string calculationString);
        string CalculateForArithmetricOrStringInputs(string calculationString);
    }
}
