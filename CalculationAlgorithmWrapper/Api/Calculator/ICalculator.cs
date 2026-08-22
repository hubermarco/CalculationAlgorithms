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
        string CalculateForArithmetricOrStringInputs(string calculationString, int decimalPlaces = -1);

        (string Input, string Output) CalculateForArithmetricInputsWithRange(string input, int decimalPlaces = -1);
    }
}
