using System;

namespace SharpEval
{
    public class BinaryExpression : IExpression
    {
        private readonly Func<double, double, double> _func;
        private readonly IExpression _left;
        private readonly IExpression _right;

        public BinaryExpression(IExpression left, IExpression right, Func<double, double, double> func)
        {
            _func = func;
            _left = left;
            _right = right;
        }
        
        public double Resolve()
        {
            return _func(_left.Resolve(), _right.Resolve());
        }
    }
}