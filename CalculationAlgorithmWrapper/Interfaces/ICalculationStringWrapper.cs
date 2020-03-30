namespace CalculationAlgorithmWrapper
{
    internal interface ICalculationStringWrapper
    {
        bool IsStringInput(string inputString);
        bool IsCalculationValid(string calculationString);
        string SetCalculationString(string calculationString);
        string SetKey(string key);
        string ClearAll();
        string Delete();
        string GetCalculationString();
    }

}
