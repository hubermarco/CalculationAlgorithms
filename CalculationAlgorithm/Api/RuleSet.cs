using System;
using System.Collections.Generic;
using System.Linq;

namespace CalculationAlgorithm
{
    public class RuleSet
    {
        public RuleSet(IDictionary<string, Tuple<int, Func<double, double, double>>> arithmetricOperators = null,
                       IDictionary<string, Func<IList<double>, double>> arithmetricFunctions = null,
                       IDictionary<string, Func<IList<string>, string>> stringFunctions = null,
                       IList<string> variableList = null )
        {
            ArithmetricOperators = arithmetricOperators;
            ArithmetricFunctions = arithmetricFunctions;
            StringFunctions = stringFunctions;
            VariableList = variableList;
        }

        public IDictionary<string, Tuple<int, Func<double, double, double>>> ArithmetricOperators { get; }
        public IDictionary<string, Func<IList<double>, double>> ArithmetricFunctions { get; }
        public IDictionary<string, Func<IList<string>, string>> StringFunctions { get; }
        public IList<string> VariableList { get; }

        public IList<string> GetOperatorList()
        {
            var operatorList = new List<string>();

            if (ArithmetricOperators != null)
            {
                operatorList = operatorList.Concat(ArithmetricOperators.Keys).ToList();
            }

            if (ArithmetricFunctions != null)
            {
                operatorList = operatorList.Concat(ArithmetricFunctions.Keys).ToList();
            }

            if(VariableList != null)
            {
                operatorList = operatorList.Concat(VariableList).ToList();
            }

            operatorList.Sort();
            operatorList.Reverse();

            return operatorList;
        }

        public IList<string> GetStringOperatorList()
        {
            var operatorList = new List<string>();

            if (StringFunctions != null)
            {
                operatorList = operatorList.Concat(StringFunctions.Keys).ToList();
            }

            operatorList.Sort();
            operatorList.Reverse();

            return operatorList;
        }


        public bool IsVariable(string operatorString)
        {
            var ruleCategory = GetRuleCategory(operatorString);
            var isVariable = ruleCategory == RuleCategory.Variable;

            return isVariable;
        }

        public bool IsOperator(string operatorString)
        {
            var ruleCategory = GetRuleCategory(operatorString);
            var isOperator = (ruleCategory == RuleCategory.ArithmetricOperant);

            return isOperator;
        }

        public bool IsFunction(string operatorString)
        {
            var ruleCategory = GetRuleCategory(operatorString);
            var isFunction = (ruleCategory == RuleCategory.ArithmetricFunction);

            return isFunction;
        }

        public int GetOperatorHierarchyLevel(string operatorString)
        {
            var operatorHierarchyLevel = -1;
            var ruleCategory = GetRuleCategory(operatorString);
     
            switch (ruleCategory)
            {
                case RuleCategory.ArithmetricOperant:
                    operatorHierarchyLevel = ArithmetricOperators.Where(x => x.Key == operatorString).Select(x => x.Value.Item1).First();
                    break;
            }

            return operatorHierarchyLevel;
        }

        public int GetOperatorHierarchyLevelDifference(string operatorString, int operatorHierarchyLevelBefore)
        {
            var operatorHierarchyLevelDifference = GetOperatorHierarchyLevel(operatorString) - operatorHierarchyLevelBefore;

            return operatorHierarchyLevelDifference;
        }

        public RuleCategory GetRuleCategory(string operatorString)
        {
            RuleCategory ruleCategory = RuleCategory.Undefined;

            if ((ArithmetricOperators != null) && ArithmetricOperators.Keys.Contains(operatorString))
            {
                ruleCategory = RuleCategory.ArithmetricOperant;
            }
            else if ((ArithmetricFunctions != null) && ArithmetricFunctions.Keys.Contains(operatorString))
            {
                ruleCategory = RuleCategory.ArithmetricFunction;
            }
            else if ((VariableList != null) && VariableList.Contains(operatorString))
            {
                ruleCategory = RuleCategory.Variable;
            }
            return ruleCategory;
        }
    }
}
