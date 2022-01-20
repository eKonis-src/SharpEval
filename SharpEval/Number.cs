namespace SharpEval
{
    public class Number : IExpression
    {
        private readonly double _val;

        public Number(double val)
        {
            _val = val;
        }
        
        public double Resolve()
        {
            return _val;
        }
    }
}