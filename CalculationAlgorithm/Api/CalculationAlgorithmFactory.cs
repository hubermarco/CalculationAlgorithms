

using System.Collections.Generic;

namespace CalculationAlgorithm
{
    public class CalculationAlgorithmFactory
    {
        public static ICalculationAlgorithm Create(RuleSet ruleSet)
        {
            var operatorList = ruleSet.GetOperatorList();
            var stringOperatorList = ruleSet.GetStringOperatorList();
            var calculationStringList = new CalculationStringList(operatorList, stringOperatorList);
            var calcTree = new CalcTree(new CalcTreeElementFactory(), ruleSet);

            return new CalculationAlgorithm(calcTree, calculationStringList);
        }

        public static ICalculationStringList CreateCalculationStringList(
            IList<string> operatorList,
            IList<string> stringOperatorList = null)
        {
            return new CalculationStringList(operatorList, stringOperatorList);
        }
    }
}
