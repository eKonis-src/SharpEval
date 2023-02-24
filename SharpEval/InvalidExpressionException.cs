using System;

namespace SharpEval
{
    public class InvalidExpressionException : Exception
    {
        public InvalidExpressionException()
        {
        }

        public InvalidExpressionException(string message)
            : base(message)
        {
        }

        public InvalidExpressionException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}