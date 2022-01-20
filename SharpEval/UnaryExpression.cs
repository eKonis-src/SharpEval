using System;

namespace SharpEval
{
    public class UnaryExpression : IExpression
    {
        private readonly Func<double, double> _func;
        private readonly IExpression _right;

        public UnaryExpression(IExpression right, Func<double, double> func)
        {
            _func = func;
            _right = right;
        }

        public double Resolve()
        {
            return _func(_right.Resolve());
        }
    }
}