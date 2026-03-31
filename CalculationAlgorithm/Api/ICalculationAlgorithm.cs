namespace CalculationAlgorithm
{
    public interface ICalculationAlgorithm
    {
        bool IsStringInput(string input);

        string CalculateForArithmetricInputs(string input, int decimalPlaces = -1);

        string CalculateForStringInputs(string input);

        ICalcTreeResult CreateCalcTreeResult(string input);
    }
}
