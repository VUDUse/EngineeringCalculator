using System;
using System.Data;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EngineeringCalculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string input = string.Empty;
        private List<string> operationHistory = new List<string>();
        private HistoryWindow historyWindow;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var button = (System.Windows.Controls.Button)sender;
            this.input += button.Content.ToString();
            this.Display.Text = this.input;
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            this.input = string.Empty;
            this.Display.Text = string.Empty;
        }

        private void Backspace_Click(object sender, RoutedEventArgs e)
        {
            if (this.input.Length > 0)
            {
                this.input = this.input.Remove(this.input.Length - 1);
                this.Display.Text = this.input;
            }
        }

        private void UnaryOperator_Click(object sender, RoutedEventArgs e)
        {
            var button = (System.Windows.Controls.Button)sender;
            var tag = button.Tag.ToString();

            try
            {
                double number = Convert.ToDouble(new DataTable().Compute(this.input, null));
                switch (tag)
                {
                    case "sqrt":
                        this.input = FormatResult(Math.Sqrt(number).ToString());
                        break;
                    case "abs":
                        this.input = FormatResult(Math.Abs(number).ToString());
                        break;
                    case "log":
                        this.input = FormatResult(Math.Log10(number).ToString());
                        break;
                    case "ln":
                        this.input = FormatResult(Math.Log(number).ToString());
                        break;
                    case "sin":
                        this.input = FormatResult(Math.Sin(number).ToString());
                        break;
                    case "cos":
                        this.input = FormatResult(Math.Cos(number).ToString());
                        break;
                    case "tan":
                        this.input = FormatResult(Math.Tan(number).ToString());
                        break;
                    case "%":
                        this.input = FormatResult((number / 100).ToString());
                        break;
                }

                AddToHistory(this.input);
                this.Display.Text = this.input;
            }
            catch (Exception)
            {
                this.Display.Text = "Error";
            }
        }

        private void Operator_Click(object sender, RoutedEventArgs e)
        {
            var button = (System.Windows.Controls.Button)sender;
            this.input += button.Content.ToString();
            this.Display.Text = this.input;
        }

        private void Constant_Click(object sender, RoutedEventArgs e)
        {
            var button = (System.Windows.Controls.Button)sender;
            var tag = button.Tag.ToString();

            switch (tag)
            {
                case "pi":
                    this.input += Math.Round(Math.PI, 2).ToString(CultureInfo.InvariantCulture);
                    break;
            }

            this.Display.Text = this.input;
        }

        private void Equals_Click(object sender, RoutedEventArgs e)
        {
            EvaluateExpression();
        }

        private void EvaluateExpression()
        {
            try
            {
                var result = new DataTable().Compute(this.input, null).ToString();
                result = FormatResult(result);
                AddToHistory(this.input + " = " + result);
                this.input = result;
                this.Display.Text = this.input;
            }
            catch (Exception)
            {
                this.Display.Text = "Error";
            }
        }

        private string FormatResult(string result)
        {
            if (double.TryParse(result, out double number))
            {
                if (number == (int)number)
                {
                    return ((int)number).ToString();
                }
                return Math.Round(number, 2).ToString(CultureInfo.InvariantCulture);
            }
            return result;
        }

        private void AddToHistory(string text)
        {
            operationHistory.Add(text);
            if (historyWindow != null && historyWindow.IsLoaded)
            {
                historyWindow.AddToHistory(text);
            }
        }

        private void ShowHistory_Click(object sender, RoutedEventArgs e)
        {
            if (historyWindow == null || !historyWindow.IsLoaded)
            {
                historyWindow = new HistoryWindow(operationHistory);
            }
            historyWindow.Show();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                EvaluateExpression();
            }
            else if (e.Key == Key.Back)
            {
                Backspace_Click(sender, e);
            }
            else
            {
                if (Keyboard.Modifiers == ModifierKeys.Shift)
                {
                    switch (e.Key)
                    {
                        case Key.D5:
                            this.input += "%";
                            break;
                        case Key.D6:
                            this.input += "^";
                            break;
                        case Key.D8:
                            this.input += "*";
                            break;
                        case Key.D9:
                            this.input += "(";
                            break;
                        case Key.D0:
                            this.input += ")";
                            break;
                        case Key.OemPlus:
                            this.input += "+";
                            break;
                        case Key.Oem2:
                            this.input += "/";
                            break;
                    }
                }
                else
                {
                    if (e.Key >= Key.D0 && e.Key <= Key.D9)
                    {
                        this.input += (e.Key - Key.D0).ToString();
                    }
                    else if (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
                    {
                        this.input += (e.Key - Key.NumPad0).ToString();
                    }
                    else
                    {
                        switch (e.Key)
                        {
                            case Key.Add:
                                this.input += "+";
                                break;
                            case Key.Subtract:
                                this.input += "-";
                                break;
                            case Key.Multiply:
                                this.input += "*";
                                break;
                            case Key.Divide:
                                this.input += "/";
                                break;
                            case Key.Decimal:
                                this.input += ".";
                                break;
                            case Key.OemPeriod:
                                this.input += ".";
                                break;
                            case Key.OemPlus:
                                if ((Keyboard.Modifiers & ModifierKeys.Shift) != ModifierKeys.Shift)
                                {
                                    this.input += "+";
                                }
                                break;
                            case Key.OemMinus:
                                this.input += "-";
                                break;
                            case Key.Oem2:
                                this.input += "/";
                                break;
                            case Key.Oem1:
                                this.input += "*";
                                break;
                            case Key.Oem6:
                                this.input += "^";
                                break;
                        }
                    }
                }
                this.Display.Text = this.input;
            }
        }
    }
}