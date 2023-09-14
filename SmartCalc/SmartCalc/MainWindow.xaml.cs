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
        private readonly Calculation calculation = new Calculation();

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            string inputText = textBox.Text;
            bool isValid = validator.IsValid(inputText);
            label1.Content = isValid ? "true" : "false";
            if (isValid)
            {
                string rpn = converter.InfixToRPN(inputText);
                label2.Content = rpn;
                calculation.Calculate(rpn);
                string result = calculation.GetResult();
                label3.Content = result;
            } else
            {
                label2.Content = "RPN";
                label3.Content = "Result";
            }
        }
        public MainWindow()
        {
            InitializeComponent();
        }
    }
}
