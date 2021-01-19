using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.WindowManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace MyUWPCalculator
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(470, 400));
            ApplicationView.PreferredLaunchViewSize = new Size(470, 400);
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
            Window.Current.CoreWindow.Dispatcher.AcceleratorKeyActivated += Dispatcher_AcceleratorKeyActivated;
        }

        private bool newLine = false;
        private bool startOver = true;
        private double result = 0;

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            var btnText = button.Content.ToString();
            double c, f;
            string result;

            switch (btnText)
            {
                case "+":
                case "-":
                case "x":
                case "/":
                    Calculate(btnText);
                    return;
                case "=":
                    Calculate("=");
                    return;
                case "<-":
                    Erase();
                    return;
                case "°C":
                    double.TryParse(textBlock.Text, out c);
                    f = CelsiusToFahrenheit(c);
                    result = $"{c} °C = {f} °F";
                    textBlock.Text = result;
                    listBox.Items.Add(result);
                    newLine = true;
                    return;
                case "°F":
                    double.TryParse(textBlock.Text, out f);
                    c = FahrenheitToCelsius(f);
                    result = $"{f} °F = {c} °C";
                    textBlock.Text = result;
                    listBox.Items.Add(result);
                    newLine = true;
                    return;
                case "BMI": OpenBMI(sender, e);
                    return;
                default:
                    break;
            }

            if (textBlock.Text.Length > 15)
                return;
            if (newLine)
            {
                textBlock.Text = "";
                newLine = false;
            }
            textBlock.Text = textBlock.Text == "0" ? "" : textBlock.Text;
            textBlock.Text += button.Content;
        }

        private double FahrenheitToCelsius(double f)
        {
            
            return Math.Round((f - 32) * 5 / 9, 2);
        }

        private double CelsiusToFahrenheit(double c)
        {
            
            return Math.Round(c * 9 / 5 + 32, 2);
        }

        private async void OpenBMI(object sender, object e)
        {
            CoreApplicationView newView = CoreApplication.CreateNewView();
            int newViewId = 0;
            await newView.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                Frame frame = new Frame();
                frame.Navigate(typeof(BMI), null);
                Window.Current.Content = frame;
                // You have to activate the window in order to show it later.
                Window.Current.Activate();

                newViewId = ApplicationView.GetForCurrentView().Id;
            });
            bool viewShown = await ApplicationViewSwitcher.TryShowAsStandaloneAsync(newViewId);
        }

        private void Calculate(string op)
        {
            if (startOver)
            {
                startOver = false;
                textBlockHistory.Text = "";
            }
            double.TryParse(textBlock.Text, out var value);
            var text = textBlockHistory.Text;
            if (!startOver)
            {
                if (value == 0 && text == "")
                {
                    result = 0;
                    text = $"0 { op} ";
                }
                else if (text == "")
                {
                    result = value;
                    text = $"{ value} { op} ";
                }
                else if (value == 0)
                {
                    text = text.Substring(0, text.Length - 3) + $" { op} ";
                }
                else
                {
                    if (text.Substring(text.Length - 2, 1) == "+") result += value;
                    if (text.Substring(text.Length - 2, 1) == "-") result -= value;
                    if (text.Substring(text.Length - 2, 1) == "/") result /= value;
                    if (text.Substring(text.Length - 2, 1) == "x") result *= value;
                    if (value < 0) text += $"({ value}) { op} ";
                    else text += $"{ value} { op} ";
                }
                if (op == "=")
                {
                    text += $"{ result}";
                    listBox.Items.Add(text);
                    result = 0;
                    startOver = true;
                }
                textBlock.Text = "";
                textBlockHistory.Text = text;
            }
        }

        private void Erase()
        {
            if (textBlock.Text.Length > 1)
                textBlock.Text = textBlock.Text.Substring(0, textBlock.Text.Length - 1);
            else
                textBlock.Text = "0";
        }
        private void Dispatcher_AcceleratorKeyActivated(CoreDispatcher sender, AcceleratorKeyEventArgs args)
        {
            focusButton.Focus(FocusState.Programmatic);
            var key = args.VirtualKey.ToString();
            var shift = Window.Current.CoreWindow.GetKeyState(VirtualKey.Shift);

            if (!args.EventType.ToString().Contains("Down"))
                return;

            if (shift.HasFlag(CoreVirtualKeyStates.Down) && args.VirtualKey == VirtualKey.Number7)
                key = "Divide";

            switch (key)
            {
                case "Add":
                case "187":
                    Calculate("+");
                    return;
                case "Subtract":
                case "189":
                    Calculate("-");
                    return;
                case "Multiply":
                case "191":
                    Calculate("x");
                    return;
                case "Divide":
                    Calculate("/");
                    return;
                case "Enter":
                    Calculate("=");
                    return;
                case "Back":
                case "Delete":
                    Erase();
                    return;
                default:
                    break;
            }

            for (int i = 0; i < 10; i++)
            {
                if (key == $"Number{i}" || key == $"NumberPad{i}")
                {
                    if (textBlock.Text.Length > 15)
                        return;
                    if (newLine)
                    {
                        textBlock.Text = "";
                        newLine = false;
                    }
                    textBlock.Text = textBlock.Text == "0" ? "" : textBlock.Text;
                    textBlock.Text += i;
                }
            }
        }
    }
}
