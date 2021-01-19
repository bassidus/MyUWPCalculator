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
        private bool textHasChanged = false;
        private bool secondWindowCreated = false;
        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            var btnText = button.Content.ToString();

            switch (btnText)
            {
                case "+":
                case "-":
                case "x":
                case "/":
                    PreCalculate(btnText);
                    return;
                case "=":
                    Calculate();
                    return;
                case "<-":
                    Erase();
                    return;
                case "BMI":
                    {
                        if (!secondWindowCreated) { OpenBMI(sender, e); secondWindowCreated = true; }
                    }
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

        private void PreCalculate(string op)
        {
            // added a var to shorten down the code a bit on the following lines
            var tbh = textBlockHistory;

            if (tbh.Text.Length > 1)
                switch (tbh.Text[tbh.Text.Length - 2])
                {
                    case '+':
                    case '-':
                    case 'x':
                    case '/':
                        if (!textHasChanged)
                        {
                            tbh.Text = tbh.Text.Substring(0, tbh.Text.Length - 3);
                            tbh.Text += $" {op} ";
                            return;
                        }
                        break;
                    default:
                        break;
                }
            newLine = true;
            textHasChanged = false;
            tbh.Text += textBlock.Text + $" {op} ";
        }

        private void Calculate()
        {

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
                    PreCalculate("+");
                    return;
                case "Subtract":
                case "189":
                    PreCalculate("-");
                    return;
                case "Multiply":
                case "191":
                    PreCalculate("x");
                    return;
                case "Divide":
                    PreCalculate("/");
                    return;
                case "Enter":
                    Calculate();
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
                    textHasChanged = true;
                }
            }
        }
    }
}
