using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace SmartCalc
{
    internal class Calculation
    {
        private readonly HashSet<char> operators_ = new(new char[] { '+', '-', '*', '/', '^' } );
        private string result_ = "";
        private readonly Stack<double> numbers_ = new();

        private int CheckDigit(string expression, int pos)
        {
            string current_number = "";
            int end_pos = pos;
            while (end_pos < expression.Length && (char.IsDigit(expression[end_pos]) || expression[end_pos] == '.'))
            {
                end_pos++;
                if (end_pos < expression.Length && ((expression[end_pos] == 'e' || expression[end_pos] == 'E') && (expression[end_pos + 1] == '-' || expression[end_pos + 1] == '+')))
                {
                    end_pos += 2;
                }
            }
            current_number += expression[pos..end_pos];
            numbers_.Push(double.Parse(current_number));
            _ = current_number.Remove(0);
            return end_pos;
        }

        private bool IsOperator(char token)
        {
            return operators_.Contains(token);
        }

        void Operands(string expression, int pos)
        {
            double second_value = numbers_.Pop();
            double first_value = numbers_.Pop();
            if (expression[pos] == '+')
            {
                numbers_.Push(first_value + second_value);
            }
            else if (expression[pos] == '-')
            {
                numbers_.Push(first_value - second_value);
            }
            else if (expression[pos] == '*')
            {
                numbers_.Push(first_value * second_value);
            }
            else if (expression[pos] == '/')
            {
                numbers_.Push(first_value / second_value);
            } else if (expression[pos] == '^')
            {
                numbers_.Push(Math.Pow(first_value, second_value));
            }
        }

        private void Functions(string func)
        {
            double number = numbers_.Pop();
            if (func == "unaryplus")
            {
                numbers_.Push(number);
            }
            else if (func == "unaryminus")
            {
                numbers_.Push(-number);
            }
            else if (func == "cos")
            {
                numbers_.Push(Math.Cos(number));
            }
            else if (func == "sin")
            {
                numbers_.Push(Math.Sin(number));
            }
            else if (func == "tan")
            {
                numbers_.Push(Math.Tan(number));
            } else if (func == "acos")
            {
                numbers_.Push(Math.Acos(number));
            } else if (func == "asin")
            {
                numbers_.Push(Math.Asin(number));
            } else if (func == "atan")
            {
                numbers_.Push(Math.Atan(number));
            } else if (func == "sqrt")
            {
                numbers_.Push(Math.Sqrt(number));
            } else if (func == "log")
            {
                numbers_.Push(Math.Log10(number));
            } else if (func == "ln")
            {
                numbers_.Push(Math.Log(number));
            } else if (func == "mod")
            {
                double tempNumber = numbers_.Pop();
                numbers_.Push(tempNumber % number);
            }
        }

        public void Calculate(string expression)
        {
            result_ = result_.Remove(0);
            int pos = 0;
            while (pos < expression.Length)
            {
                if (expression[pos] == ' ')
                {
                    pos++;
                    continue;
                }

                if (char.IsDigit(expression[pos]))
                {
                    pos = CheckDigit(expression, pos);
                } else if (IsOperator(expression[pos]))
                {
                    Operands(expression, pos);
                    pos++;
                } else if (char.IsLetter(expression[pos]))
                {
                    int end_pos = pos;
                    while (end_pos < expression.Length && char.IsLetter(expression[end_pos]))
                    {
                        end_pos++;
                    }
                    string func = expression[pos..end_pos];
                    Functions(func);
                    pos = end_pos;
                }
            }

            result_ = numbers_.Pop().ToString();
        }

        public string GetResult()
        {
            return result_;
        }
    }
}
