namespace CalculationAlgorithm
{
    internal class CalculationAlgorithm : ICalculationAlgorithm
    {
        private readonly ICalcTree _calcTree;
        private readonly ICalculationStringList _calculationStringList;

        internal CalculationAlgorithm(ICalcTree calcTree, ICalculationStringList calculationStringList)
        {
            _calcTree = calcTree;
            _calculationStringList = calculationStringList;
        }

        public double Calculate(string input)
        {
            var calcTreeResult = CreateCalcTreeResult(input);

            var result = calcTreeResult.GetResult();

            return result;
        }

        public ICalcTreeResult CreateCalcTreeResult(string input)
        {
            var preparedInput = InputStringHelper.PrepareInputString(input);

            var calculationStringList = _calculationStringList.Create(preparedInput);

            var calcTreeResult = _calcTree.Create(calculationStringList);

            return calcTreeResult;
        }
    }
}
