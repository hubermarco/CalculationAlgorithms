namespace CalculationAlgorithm
{
    internal interface ICalcTreeBranch : ICalcTreeElement
    {
        void AddElement(ICalcTreeElement element);
       
        void RemoveElement(ICalcTreeElement element);

        void SetOperator(string operatorString);

        ICalcTreeElement GetLastChild();

        string GetOperator();

        bool HasBrackets();
    }
}
