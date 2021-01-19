using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
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
            ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(470, 345));
            ApplicationView.PreferredLaunchViewSize = new Size(470, 345);
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
            Window.Current.CoreWindow.Dispatcher.AcceleratorKeyActivated += Dispatcher_AcceleratorKeyActivated;
        }

        private bool newLine = false;
        private bool textHasChanged = false;
        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;

            switch (button.Content)
            {
                case "+":
                case "-":
                case "*":
                case "/":
                    PreCalculate(button.Content.ToString());
                    return;
                case "=":
                    Calculate();
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
            textHasChanged = true;
        }

        private void PreCalculate(string op)
        {
            if (textBlockHistory.Text.Length > 1)
                switch (textBlockHistory.Text[textBlockHistory.Text.Length - 2])
                {
                    case '+':
                    case '-':
                    case '*':
                    case '/':
                        if (!textHasChanged)
                        {
                            textBlockHistory.Text = textBlockHistory.Text.Substring(0, textBlockHistory.Text.Length - 3);
                            textBlockHistory.Text += $" {op} ";
                            return;
                        }
                        break;
                    default:
                        break;
                }
            newLine = true;
            textHasChanged = false;
            textBlockHistory.Text += textBlock.Text + $" {op} ";
        }

        private void Calculate()
        {

        }

        private void Dispatcher_AcceleratorKeyActivated(CoreDispatcher sender, AcceleratorKeyEventArgs args)
        {
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
                    PreCalculate("+");
                    return;
                case "Subtract":
                case "189":
                    PreCalculate("-");
                    return;
                case "Multiply":
                case "191":
                    PreCalculate("*");
                    return;
                case "Divide":
                    PreCalculate("/");
                    return;
                case "Enter":
                    textBlock.Text = key;
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
                    textHasChanged = true;
                }
            }
        }
    }
}
