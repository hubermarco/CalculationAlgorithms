using CalculationAlgorithm;

namespace CalculationAlgorithmWrapper
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

        public string CalculateForArithmetricInputs()
        {
            var calculationString = _calculationStringWrapper.GetCalculationString();
            var result = _calculationAlgorithm.CalculateForArithmetricInputs(calculationString);

            var resultString = $"{result}";

            string outputString = calculationString + "\n= " + resultString;

            return outputString;
        }

        public string CalculateForArithmetricInputs(string calculationString)
        {
            var result = _calculationAlgorithm.CalculateForArithmetricInputs(calculationString);
            return result;
        }

        public string CalculateForStringInputs(string calculationString)
        {
            var resultString = _calculationAlgorithm.CalculateForStringInputs(calculationString);
            return resultString;
        }

        public string CalculateForArithmetricOrStringInputs(string calculationString)
        {
            string outputString;

            if(_calculationStringWrapper.IsStringInput(calculationString))
            {
                var resultString = _calculationAlgorithm.CalculateForStringInputs(calculationString);

                outputString = calculationString + "\n= " + resultString;
            }
            else
            {
                var result = _calculationAlgorithm.CalculateForArithmetricInputs(calculationString);

                var resultString = $"{result}";

                outputString = calculationString + "\n= " + resultString;
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
