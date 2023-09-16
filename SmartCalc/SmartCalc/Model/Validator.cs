using System.Collections.Generic;

namespace SmartCalc
{
    internal class Validator
    {
        private readonly HashSet<char> operators_ = new(new char[] { '+', '-', '*', '/', '^' });
        private readonly HashSet<string> functions_ = new(new string[] { "cos", "sin", "tan", "acos", "asin", "atan", "sqrt", "log", "ln", "mod" });
        public Validator() { }
        private bool IsOperator(char token)
        {
            return operators_.Contains(token);
        }

        private bool IsFunction (string function)
        {
            return functions_.Contains(function);
        }

        private static int DigitCheck(string expression, int end_pos)
        {
            int dotCount = 0;
            while (end_pos < expression.Length && (char.IsDigit(expression[end_pos]) || expression[end_pos] == '.'))
            {
                if (expression[end_pos] == '.') ++dotCount;
                if (dotCount > 1)
                {
                    return 0;
                }
                ++end_pos;
                if (end_pos < expression.Length && (expression[end_pos] == 'e' || expression[end_pos] == 'E'))
                {
                    if (expression[end_pos + 1] == '-' || expression[end_pos + 1] == '+')
                    {
                        end_pos += 2;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
            return end_pos;
        }

        private static bool ModCheck(string expression, int pos) 
        {
            if (char.IsDigit(expression[pos]) || expression[pos] == '(')
            {
                if (char.IsDigit(expression[pos - 4]) || expression[pos - 4] == ')')
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

        }
        public bool IsValid(string expression)
        {
            if (expression[0] == '*' || expression[0] == '/' || expression[0] == '^' ||
                expression[0] == ')' || expression[0] == 'm' || expression == "")
            {
                return false;
            }
            int pos = 0;
            int left_bracket_count = 0;
            int right_bracket_count = 0;
            int max_size = expression.Length;
            int number_count = 0;
            while (pos < max_size)
            {
                if (char.IsDigit(expression[pos]))
                {
                    int end_pos = pos;
                    if (pos > 1 && expression[pos - 1] == ')') return false;
                    end_pos = DigitCheck(expression, end_pos);
                    if (end_pos == 0) return false;
                    pos = end_pos;
                    ++number_count;
                }
                else if (char.IsLetter(expression[pos]))
                {
                    int end_pos = pos;
                    while (end_pos < expression.Length && char.IsLetter(expression[end_pos]))
                    {
                        ++end_pos;
                    }
                    string func = expression[pos..end_pos];
                    if (!IsFunction(func)) return false;
                    if (pos > 0 && char.IsDigit(expression[pos - 1]) && func != "mod") return false;
                    pos = end_pos;
                    if (func == "mod" && !ModCheck(expression, pos)) return false;
                    if (func != "mod" && pos <= max_size && expression[pos] != '(')
                        return false;
                }
                else if (expression[pos] == '(')
                {
                    ++left_bracket_count;
                    ++pos;
                    if (pos < max_size &&
                        (expression[pos] == '*' || expression[pos] == '/' ||
                         expression[pos] == '^' || expression[pos] == 'd'))
                    {
                        return false;
                    }
                }
                else if (expression[pos] == ')')
                {
                    ++right_bracket_count;
                    ++pos;
                }
                else
                {
                    if (pos > 0 && IsOperator(expression[pos - 1])) return false;
                    if (!IsOperator(expression[pos])) return false;
                    ++pos;
                }
            }
            if (IsOperator(expression[^1])) return false;
            if (left_bracket_count != right_bracket_count) return false;
            if (number_count == 0) return false;
            return true;
        }
    }
}

        
