namespace CalculatorAlgorithmsWrapper
{
    internal interface ICalculationStringWrapper
    {
        string SetCalculationString(string calculationString);
        string SetKey(string key);
        string ClearAll();
        string Delete();
        string GetCalculationString();
    }

}
