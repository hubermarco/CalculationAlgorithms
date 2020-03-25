using CalculationAlgorithm;

namespace CalculatorAlgorithmsWrapper
{
    internal class Calculator : ICalculator
    {
        private readonly ICalculationAlgorithm _calculationAlgorithm;
        private readonly ICalculationStringWrapper _calculationStringWrapper;

        internal Calculator(ICalculationAlgorithm calculatorCore,
            ICalculationStringWrapper calculationStringWrapper)
        {
            _calculationAlgorithm = calculatorCore;
            _calculationStringWrapper = calculationStringWrapper;
        }

        public string SetCalculationString(string calculationString)
        {
            return _calculationStringWrapper.SetCalculationString(calculationString);
        }

        public string SetKey(string key)
        {
            return _calculationStringWrapper.SetKey(key);
        }

        public string Delete()
        {
            return _calculationStringWrapper.Delete();
        }

        public string Calculate()
        {
            var calculationString = _calculationStringWrapper.GetCalculationString();
            var result = _calculationAlgorithm.Calculate(calculationString);

            var resultString = $"{result}";

            string outputString = calculationString + "\n= " + resultString;

            _calculationStringWrapper.ClearAll();

            return outputString;
        }

        public double Calculate(string calculationString)
        {
            var result = _calculationAlgorithm.Calculate(calculationString);
            return result;
        }

        public string CalculateAndReturnString(string calculationString)
        {
            string outputString;

            if (_calculationStringWrapper.IsCalculationValid(calculationString) )
            {
                var result = _calculationAlgorithm.Calculate(calculationString);

                var resultString = $"{result}";

                outputString = calculationString + "\n= " + resultString;
            }
            else
            {
                outputString = calculationString + "\n" + "INPUT IS NOT VALID";
            }

            _calculationStringWrapper.ClearAll();

            return outputString;
        }

        public string ClearAll()
        {
            return _calculationStringWrapper.ClearAll();
        }
    }
}
