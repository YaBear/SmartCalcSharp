using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCalc
{
    internal class Converter
    {
        private readonly Dictionary<string, int> precedence_ = new()
        {
          {"+", 1},    {"-", 1},    {"*", 2},   {"/", 2},    {"^", 4},
          {"cos", 4},  {"sin", 4},  {"tan", 4}, {"acos", 4}, {"asin", 4},
          {"atan", 4}, {"sqrt", 4}, {"log", 4}, {"ln", 4},   {"mod", 2}
        };
        private string rpn_ = "";

        private bool IsOperator(string op)
        {
            return precedence_.ContainsKey(op);
        }

        private int CheckDigit(string infix, int pos)
        {
            int endPos = pos;
            while (endPos < infix.Length && (char.IsDigit(infix[endPos]) || infix[endPos] == '.'))
            {
                endPos++;
                if (endPos < infix.Length && ((infix[endPos] == 'e' || infix[endPos] == 'E') && (infix[endPos + 1] == '-' || infix[endPos + 1] == '+')))
                {
                    endPos += 2;
                }
            }
            rpn_ += infix[pos..endPos] + ' ';
            return endPos;
        }
        private void CheckOperand(string infix, int pos, Stack<string> operators)
        {
            string? op = infix[pos].ToString();
            if (pos == 0 || (pos > 0 && infix[pos - 1] == '('))
            {
                if (op == "-")
                {
                    op = "unaryminus ";
                } else if (op == "+")
                {
                    op = "unaryplus ";
                }
            }
            while (operators.Count > 0 && IsOperator(operators.Peek()) && (precedence_[op] < precedence_[operators.Peek()] || (precedence_[op] == precedence_[operators.Peek()] && infix[pos] != '^')))
            {
                rpn_ += operators.Pop() + ' ';
            }
            operators.Push(op);
        }

        public string InfixToRPN(string infix)
        {
            rpn_ = rpn_.Remove(0);
            Stack<string> operators = new();
            int pos = 0;
            while (pos < infix.Length)
            {
                if (infix[pos] == ' ')
                {
                    pos++;
                    continue;
                }

                if (char.IsDigit(infix[pos]))
                {
                    pos = CheckDigit(infix, pos);
                } else if (char.IsLetter(infix[pos]))
                {
                    int end_pos = pos;
                    while (end_pos < infix.Length && char.IsLetter(infix[end_pos]))
                    {
                        end_pos++;
                    }
                    string func = infix[pos..end_pos] + ' ';
                    operators.Push(func);
                    pos = end_pos;
                } else if (infix[pos] == '(')
                {
                    operators.Push("( ");
                    pos++;
                } else if (infix[pos] == ')')
                {
                    while (operators.Count > 0 && operators.Peek() != "( ") 
                    {
                        rpn_ += operators.Pop() + ' ';
                    }
                    operators.Pop();
                    pos++;
                } else
                {
                    CheckOperand(infix, pos, operators);
                    pos++;
                }
            }

            while (operators.Count > 0)
            {
                rpn_ += operators.Pop() + ' ';
            }
            return rpn_;
        }
    }
}
