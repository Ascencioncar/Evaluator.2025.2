using System;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Evaluator.UI.Windows
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void AppendText(string value)
        {
            display.Text += value;
        }

        private void DeleteLast(object sender, EventArgs e)
        {
            if (display.Text.Length > 0)
            {
                display.Text = display.Text.Substring(0, display.Text.Length - 1);
            }
        }

        // ------------------ Evaluador con Pilas ------------------

        private void EvaluateExpression()
        {
            try
            {
                string expr = display.Text ?? string.Empty;

                // Quitar espacios
                expr = expr.Replace(" ", "");

                // Manejar signos unarios: transformar "-X" al inicio o después de ( o un operador en "0-X"
                expr = InsertZeroForUnaryMinus(expr);

                // Convertir infijo -> postfijo (RPN)
                var rpn = InfixToPostfix(expr);

                // Evaluar postfijo
                double result = EvaluatePostfix(rpn);

                // Mostrar resultado
                display.Text = expr + "=" + result.ToString(CultureInfo.InvariantCulture);
            }
            catch
            {
                MessageBox.Show("Expresión inválida", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string InsertZeroForUnaryMinus(string expr)
        {
            if (string.IsNullOrEmpty(expr)) return expr;
            var sb = new StringBuilder();
            for (int i = 0; i < expr.Length; i++)
            {
                char c = expr[i];
                if (c == '-' && (i == 0 || "+-*/^(".IndexOf(expr[i - 1]) >= 0))
                {
                    sb.Append('0'); // convierte unary - en "0-"
                }
                sb.Append(c);
            }
            return sb.ToString();
        }

        private int Precedence(string op)
        {
            return op switch
            {
                "+" or "-" => 1,
                "*" or "/" => 2,
                "^" => 4, // más alta
                _ => 0,
            };
        }

        private bool IsRightAssociative(string op)
        {
            return op == "^";
        }

        private bool IsOperator(string token)
        {
            return token == "+" || token == "-" || token == "*" || token == "/" || token == "^";
        }

        private List<string> InfixToPostfix(string expr)
        {
            var matches = Regex.Matches(expr, @"\d+(\.\d+)?|[\+\-\*/\^\(\)]")
                               .Cast<Match>()
                               .Select(m => m.Value)
                               .ToList();

            var output = new List<string>();
            var ops = new Stack<string>();

            foreach (var token in matches)
            {
                if (double.TryParse(token, NumberStyles.Number, CultureInfo.InvariantCulture, out _))
                {
                    output.Add(token);
                }
                else if (token == "(")
                {
                    ops.Push(token);
                }
                else if (token == ")")
                {
                    while (ops.Count > 0 && ops.Peek() != "(")
                        output.Add(ops.Pop());
                    if (ops.Count == 0) throw new Exception("Paréntesis desbalanceados");
                    ops.Pop();
                }
                else if (IsOperator(token))
                {
                    while (ops.Count > 0 && IsOperator(ops.Peek()))
                    {
                        string top = ops.Peek();
                        int pTop = Precedence(top);
                        int pToken = Precedence(token);

                        if (pTop > pToken || (pTop == pToken && !IsRightAssociative(token)))
                            output.Add(ops.Pop());
                        else
                            break;
                    }
                    ops.Push(token);
                }
                else
                {
                    throw new Exception("Token inválido");
                }
            }

            while (ops.Count > 0)
            {
                var t = ops.Pop();
                if (t == "(" || t == ")") throw new Exception("Paréntesis desbalanceados");
                output.Add(t);
            }

            return output;
        }

        private double EvaluatePostfix(List<string> tokens)
        {
            var stack = new Stack<double>();

            foreach (var token in tokens)
            {
                if (double.TryParse(token, NumberStyles.Number, CultureInfo.InvariantCulture, out double num))
                {
                    stack.Push(num);
                }
                else if (IsOperator(token))
                {
                    double b = stack.Pop();
                    double a = stack.Pop();

                    double res = token switch
                    {
                        "+" => a + b,
                        "-" => a - b,
                        "*" => a * b,
                        "/" => a / b,
                        "^" => Math.Pow(a, b),
                        _ => throw new Exception("Operador inválido")
                    };

                    stack.Push(res);
                }
                else
                {
                    throw new Exception("Token inválido en evaluación");
                }
            }

            if (stack.Count != 1) throw new Exception("Expresión inválida");
            return stack.Pop();
        }
    }
}
