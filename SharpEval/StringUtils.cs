using System;
using System.Text;

namespace SharpEval
{
    public class StringUtils
    {
        private static readonly String UnaryOperators = "!n";
        private static readonly String BinaryOperators = "+-*^/%";
        private static readonly string Numerics = "0123456789.,";
        
        
        private static bool Contains(string s, char c)
        {
            foreach (char ch in s)
            {
                if (ch == c)
                    return true;
            }
            return false;
        }

        public static string FormatInput(string input)
        {
            StringBuilder sb = new StringBuilder(" ");
            int i = 0;
            while (i < input.Length)
            {
                char c = input[i];
                if (Contains(UnaryOperators, c) || Contains(BinaryOperators, c) || c == '(' || c == ')')
                {
                    sb.Append(c);
                    sb.Append(" ");
                }

                if (Contains(Numerics, input[i]))
                {
                    int j = i;
                    while (i < input.Length && Contains(Numerics, input[i]))
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