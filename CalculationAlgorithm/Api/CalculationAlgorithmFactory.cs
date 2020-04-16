

using System.Collections.Generic;

namespace CalculationAlgorithm
{
    public class CalculationAlgorithmFactory
    {
        public static ICalculationAlgorithm Create(RuleSet ruleSet)
        {
            var calculationStringList = CreateCalculationStringList(ruleSet);
            var calcTree = new CalcTree(new CalcTreeElementFactory(), ruleSet);

            return new CalculationAlgorithm(calcTree, calculationStringList);
        }

        public static ICalculationStringList CreateCalculationStringList(RuleSet ruleSet)
        {
            var operatorList = ruleSet.GetOperatorList();
            var arithmetricStringOperatorList = ruleSet.GetArithmetricStringOperatorList();
            var calculationStringList = new CalculationStringList(operatorList, arithmetricStringOperatorList);
            return calculationStringList;
        }

        public static ICalculationStringList CreateCalculationStringList(
            IList<string> operatorList,
            IList<string> arithmetricStringOperatorList = null)
        {
            return new CalculationStringList(operatorList, arithmetricStringOperatorList);
        }
    }
}
