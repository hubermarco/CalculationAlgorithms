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

            var preparedInputForLeadingMinus = PrepareStringForLeadingMinus(preparedInput);

            var calculationStringList = _calculationStringList.Create(preparedInputForLeadingMinus);

            var calcTreeResult = _calcTree.Create(calculationStringList);

            return calcTreeResult;
        }

        private static string PrepareStringForLeadingMinus(string inputString)
        {
            if ((inputString.Length > 0) && (inputString[0] == '-'))
            {
                inputString = inputString.Insert(0, "0");
            }

            if (inputString != "0")
            {
                inputString = inputString.Replace("(-", "(0-");
            }

            return inputString;
        }
    }
}
