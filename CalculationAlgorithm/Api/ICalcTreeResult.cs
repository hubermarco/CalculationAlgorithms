

using System.Collections.Generic;

namespace CalculationAlgorithm
{
    public interface ICalcTreeResult
    {
        void SetVariable(string variableString, double value);

        IList<string> GetVariableList();

        double GetResult();
    }
}
