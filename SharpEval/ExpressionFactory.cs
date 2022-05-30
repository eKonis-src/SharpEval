using System;
namespace SharpEval
{
    public class ExpressionFactory
    {
        public static IExpression BinaryExpressionGenerator(string op, IExpression left, IExpression right)
        {
            switch (op)
            {
                case "+":
                    return new BinaryExpression(left, right, (a, b) => a + b);
                case "-":
                    return new BinaryExpression(left, right, (a, b) => a - b);
                case "*":
                    return new BinaryExpression(left, right, (a, b) => a * b);
                case "^":
                    return new BinaryExpression(left, right, Power);
                case "/":
                    return new BinaryExpression(left, right, (a, b) => a / b);
                case "%":
                    return new BinaryExpression(left, right, (a, b) => a % b);
                default:
                    throw new Exception("Unknown operator");
            }
        }

        public static IExpression UnaryExpressionGenerator(string op, IExpression left)
        {
            switch (op)
            {
                case "!":
                    return new UnaryExpression(left, Factorial);
                case "n":
                    return new UnaryExpression(left, Negative);
                default:
                    throw new Exception("Unknown operator");
            }
        }

        private static double Factorial(double a)
        {
            double r = 1;
            int s = (int) a;
            for (int i = 1; i <= s; i++)
            {
                r *= i;
            }
            return r;
        }

        private static double Negative(double a)
        {
            return -a;
        }
        
        private static double Power(double a, double b)
        {
            double r = a;
            int s = (int)b;
            for (double i = 1; i < s; i++)
            {
                r *= a;
            }
            return r;
        }
    }
}