using System;
using System.Collections.Generic;
using System.Text;

namespace SharpEval
{
    public class Parser
    {
        private readonly string _unaryOperators = "!";
        private readonly string _binaryOperators = "+-*^/%";
        private readonly string _toParse;
//        private List<IExpression> _expressions;
        private IExpression _expression;

        public Parser(string toParse)
        {
            _toParse = toParse;
            _expression = Analyse(_toParse);
        }

        private string join(string[] splitted, int start, int end)
        {
            StringBuilder sb = new StringBuilder();
            
            for (int i = start; i < end; i++)
            {
                sb.Append(splitted[i]);
            }

            return sb.ToString();
        }
        
        private IExpression Analyse(string toParse)
        {
            List<string> operators = new List<string>();
            List<Double> operands = new List<double>();
            foreach (string s in toParse.Split(' '))
            {
                if ("()".Contains(s))
                {
                    continue;
                }
                if (_unaryOperators.Contains(s) || _binaryOperators.Contains(s))
                {
                    operators.Insert(0,s);
                }
                else
                {
                    operands.Insert(0, Double.Parse(s));
                }
            }

            foreach (string op in operators)
            {
                IExpression tmp;
                if (_unaryOperators.Contains(op))
                {
                    tmp = ExpressionFactory.UnaryExpressionGenerator(op, new Number(operands[0]));
                    operands.RemoveAt(0);
                    operands.Insert(0, tmp.Resolve());
                }
                else
                {
                    tmp = ExpressionFactory.BinaryExpressionGenerator(op, new Number(operands[1]), new Number(operands[0]));
                    operands.RemoveAt(1);
                    operands.RemoveAt(0);
                    operands.Insert(0, tmp.Resolve());                    
                }
            }

            return new Number(operands[0]);
        }

        public string ToParse => _toParse;

        public double val => _expression.Resolve();
    }
}