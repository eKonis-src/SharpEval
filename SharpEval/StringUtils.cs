using System;
using System.Text;

namespace SharpEval
{
    public class StringUtils
    {
        private static readonly String UnaryOperators = "!n";
        private static readonly String BinaryOperators = "+-*^/%";
        private static readonly string Numerics = "0123456789.,";
        
        public static string InParenthesis(string s)
        {
            int start = 0;
            int c = 0;
            int end = s.Length;
            for (int i = 0; i < s.Length; i++)
            {
                switch (s[i])
                {
                    case '(': 
                        if (c == 0)
                            start = i;
                        c++;
                        break;
                    case ')':
                        c--;
                        if (c == 0)
                            end = i;
                        break;
                }
            }
            if (c == 0)
                return s.Substring(start + 1, end - start - 1).Trim(' ');
            throw new Exception("Incorrect Parenthesis");
        }

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