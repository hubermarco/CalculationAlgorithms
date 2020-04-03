

using System.Collections.Generic;

namespace CalculationAlgorithm
{
    public class CalculationAlgorithmFactory
    {
        public static ICalculationAlgorithm Create(RuleSet ruleSet)
        {
            var operatorList = ruleSet.GetOperatorList();
            var arithmetricStringOperatorList = ruleSet.GetArithmetricStringOperatorList();
            var calculationStringList = new CalculationStringList(operatorList, arithmetricStringOperatorList);
            var calcTree = new CalcTree(new CalcTreeElementFactory(), ruleSet);

            return new CalculationAlgorithm(calcTree, calculationStringList);
        }

        public static ICalculationStringList CreateCalculationStringList(
            IList<string> operatorList,
            IList<string> arithmetricStringOperatorList = null)
        {
            return new CalculationStringList(operatorList, arithmetricStringOperatorList);
        }
    }
}
