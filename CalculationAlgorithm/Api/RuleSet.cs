using System;
using System.Collections.Generic;
using System.Linq;

namespace CalculationAlgorithm
{
    public class RuleSet
    {
        public RuleSet(IDictionary<string, Tuple<int, Func<double, double, double>>> arithmetricOperators = null,
                       IDictionary<string, Func<IList<double>, double>> arithmetricFunctions = null,
                       IDictionary<string, Func<IList<string>, string>> arithmetricStringFunctions = null,
                       IDictionary<string, Func<IList<string>, string>> stringFunctions = null,
                       IDictionary<string, Tuple<int, Func<string, string, string>>> stringOperators = null,
                       IList<string> variableList = null )
        {
            ArithmetricOperators = arithmetricOperators;
            ArithmetricFunctions = arithmetricFunctions;
            ArithmetricStringFunctions = arithmetricStringFunctions;
            StringFunctions = stringFunctions;
            StringOperators = stringOperators;
            VariableList = variableList;
        }
        public IDictionary<string, Tuple<int, Func<double, double, double>>> ArithmetricOperators { get; set; }
        public IDictionary<string, Func<IList<double>, double>> ArithmetricFunctions { get; set; }
        public IDictionary<string, Func<IList<string>, string>> ArithmetricStringFunctions { get; set; }
        public IDictionary<string, Func<IList<string>, string>> StringFunctions { get; set; }
        public IDictionary<string, Tuple<int, Func<string, string, string>>> StringOperators { get; set; }
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

            if (StringFunctions != null)
            {
                operatorList = operatorList.Concat(StringFunctions.Keys).ToList();
            }

            if (StringOperators != null)
            {
                operatorList = operatorList.Concat(StringOperators.Keys).ToList();
            }

            if (VariableList != null)
            {
                operatorList = operatorList.Concat(VariableList).ToList();
            }

            operatorList.Sort();
            operatorList.Reverse();

            return operatorList;
        }

        public IList<string> GetArithmetricStringOperatorList()
        {
            var operatorList = new List<string>();

            if (ArithmetricStringFunctions != null)
            {
                operatorList = operatorList.Concat(ArithmetricStringFunctions.Keys).ToList();
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
            var isOperator = (ruleCategory == RuleCategory.ArithmetricOperant) ||
                             (ruleCategory == RuleCategory.StringOperant);

            return isOperator;
        }

        public bool IsFunction(string operatorString)
        {
            var ruleCategory = GetRuleCategory(operatorString);
            var isFunction = (ruleCategory == RuleCategory.ArithmetricFunction) ||
                             (ruleCategory == RuleCategory.ArithmetricStringFunction) ||
                             (ruleCategory == RuleCategory.StringFunction);

            return isFunction;
        }

        public bool IsArithmetricStringFunction(string operatorString)
        {
            var ruleCategory = GetRuleCategory(operatorString);
            var isFunction = (ruleCategory == RuleCategory.ArithmetricStringFunction);

            return isFunction;
        }

        public bool IsStringFunction(string operatorString)
        {
            var ruleCategory = GetRuleCategory(operatorString);
            var isFunction = (ruleCategory == RuleCategory.StringFunction);

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

                case RuleCategory.StringOperant:
                    operatorHierarchyLevel = StringOperators.Where(x => x.Key == operatorString).Select(x => x.Value.Item1).First();
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
            else if ((ArithmetricStringFunctions != null) && ArithmetricStringFunctions.Keys.Contains(operatorString))
            {
                ruleCategory = RuleCategory.ArithmetricStringFunction;
            }
            else if ((StringFunctions != null) && StringFunctions.Keys.Contains(operatorString))
            {
                ruleCategory = RuleCategory.StringFunction;
            }
            else if ((StringOperators != null) && StringOperators.Keys.Contains(operatorString))
            {
                ruleCategory = RuleCategory.StringOperant;
            }
            else if ((VariableList != null) && VariableList.Contains(operatorString))
            {
                ruleCategory = RuleCategory.Variable;
            }
            return ruleCategory;
        }
    }
}
