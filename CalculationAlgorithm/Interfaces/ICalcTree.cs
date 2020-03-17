using System.Collections.Generic;

namespace CalculationAlgorithm
{
    internal interface ICalcTree
    {
        ICalcTreeResult Create(IList<string> inputList);
    }
}
