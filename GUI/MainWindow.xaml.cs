#nullable enable

using Core;
using System.IO;
using System.Media;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GUI
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class Calculator : Window
    {
        private static readonly Regex Validation =
            new Regex("^(?<first>[IVXLCDM]+|(?:\\d+(?:,\\d+)?))(?:\\s*(?<op>[+\\-*/%])\\s*(?<second>[IVXLCDM]+|(?:\\d+(?:,\\d+)?))\\s*)?$", RegexOptions.Compiled);

        public Calculator()
        {
            InitializeComponent();
        }

        private void Translate(object sender, RoutedEventArgs e)
        {
            var (success, _, op, _) = Validate();
            if (!success || op != null)
            {
                File.AppendAllText("history.txt", $"Пользователь захотел перевести число из одной системы в другую, но {(!success ? "неправильно ввёл данные" : "ввёл операцию и второе число")}.\n");
                SystemSounds.Beep.Play();
                return;
            }
            var input = Input.Text;
            if (decimal.TryParse(input, out var number))
                Input.Text = new Roman(number).ToString();
            else if (Roman.TryParse(input, out var roman))
                Input.Text = roman.ToDecimal().ToString();
            File.AppendAllText("history.txt", $"Пользователь перевёл {input} в {Input.Text}.\n");
        }

        public static (bool, string?, string?, string?) ValidateInput(string input)
        {
            var match = Validation.Match(input);
            var groups = match.Groups;
            var first = groups["first"];
            var second = groups["second"];
            var op = groups["op"];
            return (match.Success,
                first.Success ? first.Value : null,
                op.Success ? op.Value : null,
                second.Success ? second.Value : null);
        }

        private (bool, string?, string?, string?) Validate()
        {
            var result = ValidateInput(Input.Text);
            if (result.Item1)
                Input.BorderBrush = Brushes.Transparent;
            else
                Input.BorderBrush = Brushes.Red;
            return result;
        }

        private void ValidateTextBox(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            Validate();
        }

        private void GenericButton(object sender, RoutedEventArgs e)
        {
            if (!(sender is Button button && button.Content is string content)) return;
            Input.AppendText(content);
            Validate();
        }

        private void Reset(object sender, RoutedEventArgs e)
        {
            Input.Clear();
        }

        private void Calculate(object sender, RoutedEventArgs e)
        {
            static Roman Into(string from)
            {
                if (decimal.TryParse(from, out var number))
                    return new Roman(number);
                else if (Roman.TryParse(from, out var roman))
                    return roman;
                else
                    return default;
            }

            var (success, first, op, second) = Validate();
            if (!success)
            {
                File.AppendAllText("history.txt", "Пользователь запросил результат, но неправильно ввёл данные.\n");
                SystemSounds.Beep.Play();
                return;
            }
            if (op is null || first is null || second is null)
            {
                File.AppendAllText("history.txt", "Пользователь запросил результат, но не ввёл операции.\n");
                return;
            }
            var firstNumb = Into(first);
            var secondNumb = Into(second);
            File.AppendAllText("history.txt", $"{Input.Text} = ");
            switch (op)
            {
                case "/":
                    Input.Text = (firstNumb / secondNumb).ToDecimal().ToString();
                    break;
                case "*":
                    Input.Text = (firstNumb * secondNumb).ToDecimal().ToString();
                    break;
                case "-":
                    Input.Text = (firstNumb - secondNumb).ToDecimal().ToString();
                    break;
                case "+":
                    Input.Text = (firstNumb + secondNumb).ToDecimal().ToString();
                    break;
                case "%":
                    Input.Text = (firstNumb % secondNumb).ToDecimal().ToString();
                    break;
            }
            File.AppendAllText("history.txt", $"{Input.Text}\n");
        }

        private void ShowHistory(object sender, RoutedEventArgs e)
        {
            string history = File.ReadAllText("history.txt");
            var window = new Window()
            {
                Title = "История",
                Content = new ScrollViewer()
                {
                    Content = new TextBox()
                    {
                        Text = history
                    }
                }
            };
            window.Show();
        }
    }
}
