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
            var preparedInput = PrepareString(input);

            var calculationStringList = _calculationStringList.Create(preparedInput);

            var calcTreeResult = _calcTree.Create(calculationStringList);

            return calcTreeResult;
        }

        private static string PrepareString(string inputString)
        {
            var preparedInput = InputStringHelper.PrepareInputString(inputString);

            preparedInput = preparedInput.Replace(" ", "");

            if ((preparedInput.Length > 0) && (preparedInput[0] == '-'))
            {
                preparedInput = preparedInput.Insert(0, "0");
            }

            if (preparedInput != "0")
            {
                preparedInput = preparedInput.Replace("(-", "(0-");
            }

            return preparedInput;
        }
    }
}
