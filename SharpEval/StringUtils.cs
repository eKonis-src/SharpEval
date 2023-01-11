using System.Linq;
using System.Text;

namespace SharpEval
{
    public static class StringUtils
    {
        private const string UnaryOperators = "!n";
        private const string BinaryOperators = "+-*^/%";
        private const string Numerics = "0123456789.,";
        
        public static string FormatInput(string input)
        {
            StringBuilder sb = new StringBuilder(" ");
            int i = 0;
            while (i < input.Length)
            {
                char c = input[i];
                if (UnaryOperators.Contains(c) || BinaryOperators.Contains(c) || c == '(' || c == ')')
                {
                    sb.Append(c);
                    sb.Append(" ");
                }

                if (Numerics.Contains(input[i]))
                {
                    int j = i;
                    while (i < input.Length && Numerics.Contains(input[i]))
                    {
                        i++;
                    }
                    sb.Append(input.Substring(j, i - j));
                    sb.Append(" ");
                    i--;
                }
                i++;
            }
            return sb.ToString().Trim();
        }
    }
}