using System.Collections.Generic;

namespace CalculationAlgorithm
{
    public interface ICalculationStringList
    {
        bool IsStringInput(string inputString);

        IList<string> Create(string inputString);
    }
}
