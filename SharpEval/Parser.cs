using System;
using System.Collections.Generic;

namespace SharpEval
{
    public class Parser
    {
        private static readonly string _unaryOperators = "!n";
        private static readonly string _binaryOperators = "+-*^/%";
        private readonly string _toParse;
//        private List<IExpression> _expressions;
        private IExpression _expression;

        public Parser(string toParse)
        {
            _toParse = toParse;
            _expression = Analyse(_toParse);
        }

        public static double parse(string expression)
        {
            return Analyse(expression).Resolve();
        }
        
        private static IExpression Analyse(string toParse)
        {
            IExpression expression;
            int depth = 0;
            List<List<string>> operators = new List<List<string>>();
            operators.Add(new List<string>());
            List<IExpression> operands = new List<IExpression>();
            foreach (string s in toParse.Split(' '))
            {
                Console.Write("Lettre : " + s + "\n");
                if ("(".Contains(s))
                {
                    depth++;
                    operators.Add(new List<string>());

                }
                if (")".Contains(s))
                {
                    unstack(operators[depth],operands);
                    depth--;

                }
                if (_binaryOperators.Contains(s) || _unaryOperators.Contains(s))
                {
                    operators[depth].Insert(0,s);
                }
                else if (!"()".Contains(s))
                {
                    operands.Insert(0,new Number(Double.Parse(s)));
                }
            }
            if (depth != 0)
                throw new Exception("Incorrect Parenthesis");
            unstack(operators[0],operands);
            return operands[0];
        }

        private static void unstack(List<string> operators, List<IExpression> operands)
        {
            while (operators.Count != 0)
            {
                var b = operands[0];
                operands.RemoveAt(0);
                if (!_unaryOperators.Contains(operators[0]))
                {
                    var a = operands[0];
                    operands.RemoveAt(0);
                    operands.Insert(0,ExpressionFactory.BinaryExpressionGenerator(operators[0],a,b));
                    Console.Write(a.Resolve() + " "+operators[0]+" " + b.Resolve() +"\n");
                }
                else
                {
                    Console.Write(operators[0]+" " + b.Resolve() +"\n");
                    operands.Insert(0,ExpressionFactory.UnaryExpressionGenerator(operators[0],b));
                }
                Console.Write(operands[0].Resolve());
                Console.Write("\n");
                operators.RemoveAt(0);
            }
        }

        public string ToParse => _toParse;

        public double Val => _expression.Resolve();
    }
}