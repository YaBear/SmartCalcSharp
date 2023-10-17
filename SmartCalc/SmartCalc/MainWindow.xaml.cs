using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SmartCalc
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Validator validator = new();
        private readonly Converter converter = new();
        private readonly Calculation calculation = new();

        private void Calc_Click(object sender, RoutedEventArgs e)
        {
            string inputText = expressionField.Text;
            if (inputText.IndexOf('x') != -1 && double.TryParse(xValueField.Text, out double xValue))
            {
                inputText = inputText.Replace("x", xValue.ToString());
            }
            bool isValid = validator.IsValid(inputText);
            statusLabel.Content = isValid ? "true" : "false";
            if (isValid)
            {
                string rpn = converter.InfixToRPN(inputText);
                rpnLabel.Content = rpn;
                calculation.Calculate(rpn);
                string result = calculation.GetResult();
                expressionField.Text = result;
            } else
            {
                rpnLabel.Content = "RPN";
                expressionField.Clear();
            }
        }

        private void BaseButtonClick(object sender, RoutedEventArgs e)
        {
            var buttonString = sender.ToString();
            var index = buttonString!.LastIndexOf(':');
            expressionField.AppendText(buttonString[(index + 2)..]);
        }

        private void AC_Click(object sender, RoutedEventArgs e)
        {
            expressionField.Clear();
            xValueField.Clear();
            statusLabel.Content = "Status";
            rpnLabel.Content = "RPN";
        }

        private void Backspace_Click(object sender, RoutedEventArgs e)
        {
            int len = expressionField.Text.Length;
            if (len > 0)
            {
                expressionField.Text = expressionField.Text.Remove(expressionField.Text.Length - 1);
            }
        }
        private void FuncButtonClick(object sender, RoutedEventArgs e)
        {
            var buttonString = sender.ToString();
            var index = buttonString!.LastIndexOf(':');
            expressionField.AppendText(buttonString[(index + 2)..] + "(");
        }
        public MainWindow()
        {
            string[] commandLineArgs = Environment.GetCommandLineArgs();
            InitializeComponent();
            if (commandLineArgs.Length == 1 || commandLineArgs[1] != "-debug")
            {
                rpnLabel.Visibility = Visibility.Hidden;
                statusLabel.Visibility = Visibility.Hidden;
            }
        }
    }
}
