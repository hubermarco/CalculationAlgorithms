

using System.Collections.Generic;

namespace CalculationAlgorithm
{
    public class CalculationAlgorithmFactory
    {
        public static ICalculationAlgorithm Create(RuleSet ruleSet)
        {
            var operatorList = ruleSet.GetOperatorList();
            var calculationStringList = new CalculationStringList(operatorList);
            var calcTree = new CalcTree(new CalcTreeElementFactory(), ruleSet);

            return new CalculationAlgorithm(calcTree, calculationStringList);
        }

        public static ICalculationStringList CreateCalculationStringList(IList<string> operatorList)
        {
            return new CalculationStringList(operatorList);
        }
    }
}
