using System;
using System.Collections.Generic;
using System.Linq;

namespace SharpEval
{
    public class Parser
    {
        private static readonly string UnaryOperators = "!n";
        private static readonly string BinaryOperators = "+-*^/%";
        private readonly string _toParse;
//        private List<IExpression> _expressions;
        private IExpression _expression;

        public Parser(string toParse)
        {
            _toParse = StringUtils.FormatInput(toParse);
            _expression = Analyse(_toParse);
        }

        public static double Parse(string expression)
        {
            return Analyse(StringUtils.FormatInput(expression)).Resolve();
        }

        private static void StackOperators(List<string> operators, string op)
        {
            int i = 0;
            if (!"^*/!n".Contains(op))
            {
                while (i < operators.Count && "^*/!n".Contains(operators[i]))
                    i++;
            }
            operators.Insert(i, op);
        }

        private static void checkPrio(List<string> operators, List<IExpression> operands)
        {
            if (operators.Count == 0)
                return;
            if ("^*/!n".Contains(operators[0]))
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
            List<List<string>> operators = new List<List<string>>();
            operators.Add(new List<string>());
            List<IExpression> operands = new List<IExpression>();
            foreach (string s in toParse.Split(' '))
            {
                if ("(".Contains(s))
                {
                    depth++;
                    operators.Add(new List<string>());

                }
                if (")".Contains(s))
                {
                    Unstack(operators[depth],operands);
                    depth--;
                    checkPrio(operators[depth],operands);

                }
                if (BinaryOperators.Contains(s) || UnaryOperators.Contains(s))
                {
                    StackOperators(operators[depth], s);
                    if ("!".Contains(s))
                        Unstack(operators[depth],operands);
                }
                else if (!"()".Contains(s))
                {
                    operands.Insert(0,new Number(Double.Parse(s)));
                    checkPrio(operators[depth],operands);
                }
            }
            if (depth != 0)
                throw new Exception("Incorrect Parenthesis");
            Unstack(operators[0],operands);
            return operands[0];
        }

        
        private static void Unstack(List<string> operators, List<IExpression> operands)
        {
            while (operators.Count != 0)
            {
                var b = operands[0];
                operands.RemoveAt(0);
                if (!UnaryOperators.Contains(operators[0]))
                {
                    var a = operands[0];
                    operands.RemoveAt(0);
                    operands.Insert(0,ExpressionFactory.BinaryExpressionGenerator(operators[0],a,b));
                }
                else
                {
                    operands.Insert(0,ExpressionFactory.UnaryExpressionGenerator(operators[0],b));
                }
                operators.RemoveAt(0);
            }
        }

        public string ToParse => _toParse;

        public double Val => _expression.Resolve();
    }
}