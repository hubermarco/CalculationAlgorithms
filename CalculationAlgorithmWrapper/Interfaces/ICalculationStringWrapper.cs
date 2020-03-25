namespace CalculatorAlgorithmsWrapper
{
    internal interface ICalculationStringWrapper
    {
        bool IsCalculationValid(string calculationString);
        string SetCalculationString(string calculationString);
        string SetKey(string key);
        string ClearAll();
        string Delete();
        string GetCalculationString();
    }

}
