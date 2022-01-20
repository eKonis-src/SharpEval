using System;

namespace SharpEval
{
    public interface IExpression
    {
        double Resolve();
    }
}