using CalculationAlgorithm;
using NUnit.Framework;
using System;
using System.Collections.Generic;

using Expr = MathNet.Symbolics.SymbolicExpression;

namespace CalculationAlgorithmTests
{
    [TestFixture]
    public class CalculationAlgorithmTests
    {
        private ICalculationAlgorithm _calculationAlgorithm;
        private readonly double _delta = 1e-15;

        [SetUp]
        public void Setup()
        {
            var arithmetricOperators = new Dictionary<string, Tuple<int, Func<double, double, double>>>
            {
                { "+", new Tuple<int, Func<double, double, double>>(0, (x, y) => x + y) },
                { "-", new Tuple<int, Func<double, double, double>>(0, (x, y) => x - y) },
                { "*", new Tuple<int, Func<double, double, double>>(1, (x, y) => x * y) },
                { "/", new Tuple<int, Func<double, double, double>>(1, (x, y) => x / y) },
                { "^", new Tuple<int, Func<double, double, double>>(2, (x, y) => Math.Pow(x,y)) },
                { "->", new Tuple<int, Func<double, double, double>>(0, (x, y) => (x==0) || (y==1) ? 1 : 0) },
                { "&", new Tuple<int, Func<double, double, double>>(0, (x, y) => (x==1) && (y==1) ? 1 : 0) }
            };

            var arithmetricFunctions = new Dictionary<string, Func<IList<double>, double>>
            {
                { "sum",
                    inputList =>
                    {
                        var sum = 0.0;
                        foreach(var input in inputList)
                        {
                            sum += input;
                        }
                        return sum;
                    }
                },
                { "sin", inputList => Math.Sin(inputList[0]) },
                { "cos", inputList => Math.Cos(inputList[0]) },
                { "log", inputList => Math.Log10(inputList[0]) },
                { "plus", inputList => inputList[0] + inputList[1] },
                { "IsGreater", inputList => inputList[0] > inputList[1] ? 1 : 0 },
                { "plusplus", inputList => inputList[0] + inputList[1] + inputList[2]}
            };

            var stringFunctions = new Dictionary<string, Func<IList<string>, string>>
            {
                 { "StringTest", inputList => "Hallo " + inputList[0] },
                 { "d", inputList => Expr.Parse(inputList[0]).Differentiate(Expr.Parse(inputList[1])).ToString() }, 
                 { "Expand", inputList => Expr.Parse(inputList[0]).Expand().ToString() }
            };


            var ruleSet = new RuleSet(
                arithmetricOperators,
                arithmetricFunctions,
                stringFunctions);

            _calculationAlgorithm = CalculationAlgorithmFactory.Create(ruleSet);
        }

        [Test]
        public void When_addition_is_performed_then_result_is_as_exptect()
        {
            var result = _calculationAlgorithm.Calculate("3+4");

            Assert.AreEqual(7, result);
        }

        [Test]
        public void When_multiplication_is_performed_then_result_is_as_exptect()
        {
            var result = _calculationAlgorithm.Calculate("3*4");

            Assert.AreEqual(12, result);
        }

        [Test]
        public void When_potentiation_is_performed_then_result_is_as_exptect()
        {
            var result = _calculationAlgorithm.Calculate("2^4");

            Assert.AreEqual(16, result);
        }

        [Test]
        public void When_multiplication_and_addition_is_performed_then_result_is_as_exptect()
        {
            var result = _calculationAlgorithm.Calculate("3*4+2");

            Assert.AreEqual(14, result);
        }

        [Test]
        public void When_addition_and_multiplication_is_performed_then_result_is_as_exptect()
        {
            var result = _calculationAlgorithm.Calculate("3+4*2");

            Assert.AreEqual(11, result);
        }

        [Test]
        public void When_potentiation_and_addition_is_performed_then_result_is_as_exptect()
        {
            var result = _calculationAlgorithm.Calculate("3^4+2");

            Assert.AreEqual(83, result);
        }

        [Test]
        public void When_addition_and_potentiation_is_performed_then_result_is_as_exptect()
        {
            var result = _calculationAlgorithm.Calculate("3+4^2");

            Assert.AreEqual(19, result);
        }

        [Test]
        public void When_addition_and_potentiation_and_multipication_is_performed_then_result_is_as_exptect()
        {
            var result = _calculationAlgorithm.Calculate("3+4^2*3");

            Assert.AreEqual(51, result);
        }

        [Test]
        public void When_calculation_with_brackets_at_the_beginning_is_performed_then_result_is_as_exptect()
        {
            var result = _calculationAlgorithm.Calculate("(3+4)*2");

            Assert.AreEqual(14, result);
        }

        [Test]
        public void When_calculation_with_brackets_is_performed_then_result_is_as_exptect()
        {
            var result = _calculationAlgorithm.Calculate("2*(3+4)");

            Assert.AreEqual(14, result);
        }

        [Test]
        public void When_calculation_with_brackets_and_potentiation_is_performed_then_result_is_as_exptect()
        {
            var result = _calculationAlgorithm.Calculate("2*(3+4)^2");

            Assert.AreEqual(98, result);
        }

        [Test]
        public void When_a_mathematical_operation_is_done_then_correct_result_is_calculated_01()
        {
            var result = _calculationAlgorithm.Calculate("  3+4*  5+2 ");

            Assert.AreEqual(25.0, result, _delta);
        }

        [Test]
        public void When_a_mathematical_operation_is_done_then_correct_result_is_calculated_02()
        {
            var result = _calculationAlgorithm.Calculate("(3+4)*5");

            Assert.AreEqual(35.0, result, _delta);
        }

        [Test]
        public void When_a_mathematical_operation_is_done_then_correct_result_is_calculated_03()
        {
            var result = _calculationAlgorithm.Calculate("4*(5+1)");

            Assert.AreEqual(24.0, result, _delta);
        }

        [Test]
        public void When_a_mathematical_operation_is_done_then_correct_result_is_calculated_04()
        {
            var result = _calculationAlgorithm.Calculate("(3+4)*(5+1)");

            Assert.AreEqual(42.0, result, _delta);
        }

        [Test]
        public void When_a_mathematical_operation_is_done_then_correct_result_is_calculated_05()
        {
            var result = _calculationAlgorithm.Calculate("(3+4)*(5+1)");

            Assert.AreEqual(42.0, result, _delta);
        }

        [Test]
        public void When_a_mathematical_operation_is_done_then_correct_result_is_calculated_06()
        {
            var result = _calculationAlgorithm.Calculate("(((3)))*(((5)))");

            Assert.AreEqual(15.0, result, _delta);
        }

        [Test]
        public void When_a_mathematical_operation_is_done_then_correct_result_is_calculated_07()
        {
            var result = _calculationAlgorithm.Calculate("(3+(5+1))*(2+7)");

            Assert.AreEqual(81.0, result, _delta);
        }

        [Test]
        public void When_a_mathematical_operation_is_done_then_correct_result_is_calculated_08()
        {
            var result = _calculationAlgorithm.Calculate("1.5+(2.2*4+6)-(8+9)*14+2-5*3");

            Assert.AreEqual(-234.7, result, _delta);
        }

        [Test]
        public void When_a_mathematical_operation_is_done_then_correct_result_is_calculated_08a()
        {
            var result = _calculationAlgorithm.Calculate("1.5+(2.2*4+6)-(8+9)*14-5");

            Assert.AreEqual(-226.7, result, _delta);
        }

        [Test]
        public void When_a_mathematical_operation_is_done_then_correct_result_is_calculated_08b()
        {
            var result = _calculationAlgorithm.Calculate("1-20-5");

            Assert.AreEqual(-24, result, _delta);
        }

        [Test]
        public void When_a_mathematical_operation_is_done_then_correct_result_is_calculated_08c()
        {
            var result = _calculationAlgorithm.Calculate("1-4*5-5");

            Assert.AreEqual(-24, result, _delta);
        }

        [Test]
        public void When_a_mathematical_operation_is_done_then_correct_result_is_calculated_08d()
        {
            var result = _calculationAlgorithm.Calculate("1-4*5-5*2");

            Assert.AreEqual(-29, result, _delta);
        }

        [Test]
        public void When_a_mathematical_operation_is_done_then_correct_result_is_calculated_08e()
        {
            var result = _calculationAlgorithm.Calculate("1-4^5-2");

            Assert.AreEqual(-1025, result, _delta);
        }

        [Test]
        public void When_a_mathematical_operation_is_done_then_correct_result_is_calculated_08f()
        {
            var result = _calculationAlgorithm.Calculate("1-4^5*3-2");

            Assert.AreEqual(-3073, result, _delta);
        }

        [Test]
        public void When_a_mathematical_operation_is_done_then_correct_result_is_calculated_08g()
        {
            var result = _calculationAlgorithm.Calculate("1-4^5*2+2");

            Assert.AreEqual(-2045, result, _delta);
        }

        [Test]
        public void When_a_mathematical_operation_is_done_then_correct_result_is_calculated_08h()
        {
            var result = _calculationAlgorithm.Calculate("2*3*4-2");

            Assert.AreEqual(22, result, _delta);
        }


        [Test]
        public void When_a_mathematical_operation_is_done_then_correct_result_is_calculated_09()
        {
            var result = _calculationAlgorithm.Calculate("5*(3-3)");

            Assert.AreEqual(0.0, result, _delta);
        }

        [Test]
        public void When_a_mathematical_operation_is_done_then_correct_result_is_calculated_10()
        {
            var result = _calculationAlgorithm.Calculate("5^2");

            Assert.AreEqual(25.0, result, _delta);
        }

        [Test]
        public void When_a_mathematical_operation_is_done_then_correct_result_is_calculated_11()
        {
            var result = _calculationAlgorithm.Calculate("(2+3)^2");

            Assert.AreEqual(25.0, result, _delta);
        }

        [Test]
        public void When_a_mathematical_operation_is_done_then_correct_result_is_calculated_12()
        {
            double result = _calculationAlgorithm.Calculate("(6+3)^(1/2)");

            Assert.AreEqual(3.0, result, _delta);
        }

        [Test]
        public void When_a_mathematical_operation_is_done_then_correct_result_is_calculated_13()
        {
            var result = _calculationAlgorithm.Calculate("2*(6+3)^(1/2)");

            Assert.AreEqual(6.0, result, _delta);
        }

        [Test]
        public void When_a_mathematical_operation_is_done_then_correct_result_is_calculated_14()
        {
            var result = _calculationAlgorithm.Calculate("9^(1/2)*2");

            Assert.AreEqual(6.0, result, _delta);
        }

        [Test]
        public void When_a_mathematical_operation_is_done_then_correct_result_is_calculated_15()
        {
            var result = _calculationAlgorithm.Calculate("(6+3)^(1/2)*2");

            Assert.AreEqual(6.0, result, _delta);
        }

        [Test]
        public void When_a_mathematical_operation_is_done_then_correct_result_is_calculated_16()
        {
            var result = _calculationAlgorithm.Calculate("((6+3)^(1/2))*2");

            Assert.AreEqual(6.0, result, _delta);
        }

        [Test]
        public void When_a_mathematical_operation_is_done_then_correct_result_is_calculated_17()
        {
            var result = _calculationAlgorithm.Calculate("9^0.5");

            Assert.AreEqual(3.0, result, _delta);
        }

        [Test]
        public void When_a_mathematical_operation_is_done_then_correct_result_is_calculated_18()
        {
            var result = _calculationAlgorithm.Calculate("9^0.5*2");

            Assert.AreEqual(6.0, result, _delta);
        }

        [Test]
        public void When_a_mathematical_operation_is_done_then_correct_result_is_calculated_19()
        {
            var result = _calculationAlgorithm.Calculate("9^0.5+2");

            Assert.AreEqual(5.0, result, _delta);
        }

        [Test]
        public void When_a_mathematical_operation_is_done_then_correct_result_is_calculated_20()
        {
            var result = _calculationAlgorithm.Calculate("(3^2+4^2)^0.5");

            Assert.AreEqual(5.0, result, _delta);
        }

        [Test]
        public void When_a_mathematical_operation_is_done_then_correct_result_is_calculated_21()
        {
            var result = _calculationAlgorithm.Calculate("(3^2+4^2)");

            Assert.AreEqual(25.0, result, _delta);
        }

        [Test]
        public void When_a_mathematical_operation_is_done_then_correct_result_is_calculated_22()
        {
            var result = _calculationAlgorithm.Calculate("((((1*2)^3^2))");

            Assert.AreEqual(64.0, result, _delta);
        }

        [Test]
        public void When_a_mathematical_operation_is_done_then_correct_result_is_calculated_23()
        {
            var result = _calculationAlgorithm.Calculate("3*2^4");

            Assert.AreEqual(48.0, result, _delta);
        }

        [Test]
        public void When_a_mathematical_operation_is_done_then_correct_result_is_calculated_24()
        {
            var result = _calculationAlgorithm.Calculate("2*3+2^4");

            Assert.AreEqual(22.0, result, _delta);
        }

        [Test]
        public void When_a_mathematical_operation_is_done_then_correct_result_is_calculated_25()
        {
            var result = _calculationAlgorithm.Calculate("3*2^(2*2)");

            Assert.AreEqual(48.0, result, _delta);
        }

        [Test]
        public void When_a_mathematical_operation_is_done_then_correct_result_is_calculated_26()
        {
            var result = _calculationAlgorithm.Calculate("(3*2)^(2*2)");

            Assert.AreEqual(1296, result, _delta);
        }

        [Test]
        public void When_a_mathematical_operation_is_done_then_correct_result_is_calculated_27()
        {
            var result = _calculationAlgorithm.Calculate("(3*2+4*2)*0.5");

            Assert.AreEqual(7.0, result, _delta);
        }

        [Test]
        public void When_a_mathematical_operation_is_done_then_correct_result_is_calculated_28()
        {
            var result = _calculationAlgorithm.Calculate("2^3-2");

            Assert.AreEqual(6.0, result, _delta);
        }

        [Test]
        public void When_a_mathematical_operation_is_done_then_correct_result_is_calculated_29()
        {
            var result = _calculationAlgorithm.Calculate("(3+4)^2");

            Assert.AreEqual(49.0, result, _delta);
        }

        [Test]
        public void When_logarithm_for_base_10_is_calculated_then_result_is_as_expected()
        {
            var result = _calculationAlgorithm.Calculate("log(1000*100)*2^3");

            Assert.AreEqual(40.0, result, _delta);
        }

        [Test]
        public void When_cosinus_is_calculated_then_result_is_as_expected()
        {
            var result = _calculationAlgorithm.Calculate("cos(5-5)*2");

            Assert.AreEqual(2.0, result, _delta);
        }

        [Test]
        public void When_function_with_2_arguments_is_calculated_then_result_is_as_expected()
        {
            var result = _calculationAlgorithm.Calculate("plus(3,6)");

            Assert.AreEqual(9.0, result, _delta);
        }

        [Test]
        public void When_function_with_2_arguments_including_operatators_is_calculated_then_result_is_as_expected()
        {
            var result = _calculationAlgorithm.Calculate("plus(2^2,6)");

            Assert.AreEqual(10.0, result, _delta);
        }

        [Test]
        public void When_function_with_2_arguments_is_calculated_then_result_is_as_expected_02()
        {
            var result = _calculationAlgorithm.Calculate("IsGreater(6,3)");

            Assert.AreEqual(1, result, _delta);
        }

        [Test]
        public void When_function_with_2_arguments_is_calculated_then_result_is_as_expected_03()
        {
            var result = _calculationAlgorithm.Calculate("IsGreater(3,6)");

            Assert.AreEqual(0, result, _delta);
        }

        [Test]
        public void When_function_with_3_arguments_is_calculated_then_result_is_as_expected()
        {
            var result = _calculationAlgorithm.Calculate("plusplus(1,2,3)");

            Assert.AreEqual(6, result, _delta);
        }

        [Test]
        public void When_function_with_3_arguments_is_calculated_then_result_is_as_expected_01()
        {
            var result = _calculationAlgorithm.Calculate("plusplus(1,2,plus(1,2))");

            Assert.AreEqual(6, result, _delta);
        }

        [Test]
        public void When_function_with_3_arguments_is_calculated_then_result_is_as_expected_02()
        {
            var result = _calculationAlgorithm.Calculate("2-plusplus(1,2,plus(1,2))*3");

            Assert.AreEqual(-16, result, _delta);
        }

        [Test]
        public void When_implication_is_calculated_then_result_is_as_expected()
        {
            var result = _calculationAlgorithm.Calculate("0->1");

            Assert.AreEqual(1, result, _delta);
        }

        [Test]
        public void When_logical_and_is_calculated_then_result_is_as_expected()
        {
            var result = _calculationAlgorithm.Calculate("1&1");

            Assert.AreEqual(1, result, _delta);
        }

        [Test]
        public void When_sum_is_calculated_out_of_2_inputs_then_result_is_as_expected()
        {
            var result = _calculationAlgorithm.Calculate("sum(4,5)");

            Assert.AreEqual(9, result, _delta);
        }

        [Test]
        public void When_sum_is_calculated_out_of_2_inputs_with_calculation_then_result_is_as_expected()
        {
            var result = _calculationAlgorithm.Calculate("sum(2^2,5)");

            Assert.AreEqual(9, result, _delta);
        }

        [Test]
        public void When_sum_is_calculated_out_of_4_inputs_then_result_is_as_expected()
        {
            var result = _calculationAlgorithm.Calculate("sum(4,5,6,7)");

            Assert.AreEqual(22, result, _delta);
        }

        [Test]
        public void When_string_method_is_performed_then_corresponding_result_is_returned()
        {
            var stringResult = _calculationAlgorithm.CalculateString("StringTest(Marco)");

            Assert.AreEqual("Hallo Marco", stringResult);
        }

        [Test]
        public void When_string_method_is_performed_in_a_nested_way_then_corresponding_result_is_returned()
        {
            var stringResult = _calculationAlgorithm.CalculateString("StringTest(StringTest(Marco))");

            Assert.AreEqual("Hallo Hallo Marco", stringResult);
        }

        [Test]
        public void When_differentiate_is_performed_in_a_nested_way_then_corresponding_result_is_returned()
        {
            var stringResult = _calculationAlgorithm.CalculateString("d(x^2,x)");

            Assert.AreEqual("2*x", stringResult);
        }

        [Test]
        public void When_differentiate_is_performed_in_a_nested_way_then_corresponding_result_is_returned_2()
        {
            var stringResult = _calculationAlgorithm.CalculateString("d(1/x,x)");

            Assert.AreEqual("-1/x^2", stringResult);
        }

        [Test]
        public void When_differentiate_is_performed_in_a_nested_way_then_corresponding_result_is_returned_3()
        {
            var stringResult = _calculationAlgorithm.CalculateString("Expand(d((x+4)^2,x)))");

            Assert.AreEqual("8 + 2*x", stringResult);
        }
    }
}
