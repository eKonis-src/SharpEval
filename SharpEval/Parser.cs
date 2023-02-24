using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpEval
{
    public static class Parser
    {
        private static readonly string UnaryOperators = "!n";
        private static readonly Dictionary<char, Func<double, double>> CustomUnaryOperators = 
            new Dictionary<char, Func<double, double>>();

        private static readonly Dictionary<char, Func<double, double, double>> CustomBinaryOperators =
            new Dictionary<char, Func<double, double, double>>();
        
        private static readonly Dictionary<char, Func<double, double>> PrimaryCustomUnaryOperators = 
            new Dictionary<char, Func<double, double>>();

        private static readonly Dictionary<char, Func<double, double, double>> PrimaryCustomBinaryOperators =
            new Dictionary<char, Func<double, double, double>>();
        
        private static readonly string BinaryOperators = "+-*^/%";

        public static void AddOperator(char op, Func<double, double> expression, bool primary)
        {
            if (primary)
                PrimaryCustomUnaryOperators.Add(op,expression);
            else
                CustomUnaryOperators.Add(op,expression);
        }

        public static void AddOperator(char op, Func<double, double, double> expression, bool primary)
        {
            if (primary)
                PrimaryCustomBinaryOperators.Add(op, expression);
            else
                CustomBinaryOperators.Add(op,expression);
        }

        private static string GetCustomOperators()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var key in CustomUnaryOperators.Keys) { sb.Append(key); }
            foreach (var key in PrimaryCustomUnaryOperators.Keys) { sb.Append(key); }
            foreach (var key in CustomBinaryOperators.Keys) { sb.Append(key); }
            foreach (var key in PrimaryCustomBinaryOperators.Keys) { sb.Append(key); }

            return sb.ToString();
        }
        public static double Parse(string expression)
        {
            return Analyse(StringUtils.FormatInput(expression, GetCustomOperators())).Resolve();
        }

        private static bool IsPrimaryOperator(string op)
        {
            return "^*/!n".Contains(op)
                   || PrimaryCustomBinaryOperators.Keys.Contains(op[0])
                   || PrimaryCustomUnaryOperators.Keys.Contains(op[0]);
        }

        private static bool IsCustomOperator(string op)
        {
            return CustomUnaryOperators.Keys.Contains(op[0])
                   || CustomBinaryOperators.Keys.Contains(op[0])
                   || PrimaryCustomBinaryOperators.Keys.Contains(op[0])
                   || PrimaryCustomUnaryOperators.Keys.Contains(op[0]);
        }
        

        private static void StackOperators(List<string> operators, string op)
        {
            int i = 0;
            if (!IsPrimaryOperator(op))
            {
                while (i < operators.Count && IsPrimaryOperator(operators[i]))
                    i++;
            }
            operators.Insert(i, op);
        }

        private static void CheckPriority(List<string> operators, List<IExpression> operands)
        {
            if (operators.Count == 0)
                return;
            if (IsPrimaryOperator(operators[0]))
            {
                if("!".Contains(operators[0]) && operands.Count > 0)
                    Unstack(operators,operands);
                else if (operands.Count > 1)
                    Unstack(operators, operands);
            }
        }
        
        private static IExpression Analyse(string toParse)
        {
            //IExpression expression;
            int depth = 0;
            var operators = new List<List<string>> { new List<string>() };
            var operands = new List<IExpression>();
            try
            {
                foreach (string s in toParse.Split(' '))
                {
                    if ("(".Contains(s))
                    {
                        depth++;
                        operators.Add(new List<string>());

                    }

                    if (")".Contains(s))
                    {
                        Unstack(operators[depth], operands);
                        depth--;
                        CheckPriority(operators[depth], operands);

                    }

                    if (BinaryOperators.Contains(s) || UnaryOperators.Contains(s) || IsCustomOperator(s))
                    {
                        StackOperators(operators[depth], s);
                        if ("!".Contains(s))
                            Unstack(operators[depth], operands);
                    }
                    else if (!"()".Contains(s))
                    {
                        operands.Insert(0, new Number(double.Parse(s)));
                        CheckPriority(operators[depth], operands);
                    }
                }

                if (depth != 0)
                    throw new Exception("Incorrect Parenthesis");
                Unstack(operators[0], operands);
                return operands[0];
            }
            catch (InvalidExpressionException e)
            {
                Console.Error.Write(e.Message + "\n");
                return new Number(0);
            }
        }

        
        private static void Unstack(List<string> operators, List<IExpression> operands)
        {
            try
            {
                while (operators.Count != 0)
                {
                    var b = operands[0];
                    operands.RemoveAt(0);
                    if (BinaryOperators.Contains(operators[0]))
                    {
                        var a = operands[0];
                        operands.RemoveAt(0);
                        operands.Insert(0,
                            ExpressionFactory.BinaryExpressionGenerator(operators[0], a, b));
                    }
                    else if (CustomBinaryOperators.Keys.Contains(operators[0][0]))
                    {
                        var a = operands[0];
                        operands.RemoveAt(0);
                        operands.Insert(0,
                            ExpressionFactory.BinaryExpressionGenerator(CustomBinaryOperators[operators[0][0]], a, b));
                    }
                    else if (PrimaryCustomBinaryOperators.Keys.Contains(operators[0][0]))
                    {
                        var a = operands[0];
                        operands.RemoveAt(0);
                        operands.Insert(0,
                            ExpressionFactory.BinaryExpressionGenerator(CustomBinaryOperators[operators[0][0]], a, b));
                    }
                    else if (CustomUnaryOperators.Keys.Contains(operators[0][0]))
                    {
                        operands.Insert(0,
                            ExpressionFactory.UnaryExpressionGenerator(CustomUnaryOperators[operators[0][0]], b));
                    }
                    else if (PrimaryCustomUnaryOperators.Keys.Contains(operators[0][0]))
                    {
                        operands.Insert(0,
                            ExpressionFactory.UnaryExpressionGenerator(CustomUnaryOperators[operators[0][0]], b));
                    }
                    else
                    {
                        operands.Insert(0,
                            ExpressionFactory.UnaryExpressionGenerator(operators[0], b));
                    }

                    operators.RemoveAt(0);
                }
            }
            catch (Exception)
            {
                throw new InvalidExpressionException("Incorrect operator in expression");
            }
        }
    }
}