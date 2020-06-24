namespace TalkingCalculator
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using TalkingCalculator.Model;
    using Xamarin.Essentials;
    using Xamarin.Forms;

    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        MainPageViewModel mainViewModel;
        private Label _displayLabel;
        public MainPage()
        {
            InitializeComponent();
            _displayLabel = this.FindByName<Label>("DisplayWindow");
            mainViewModel = BindingContext as MainPageViewModel;
            mainViewModel.ClearAll();
            CalculatorV2 v2 = new CalculatorV2();
            v2.Test();
        }

        private void ButtonNumber_Clicked(object sender, EventArgs e)
        {
            int textLength = _displayLabel.Text.Length;
            if (textLength < 10)
            {
                _displayLabel.FontSize = 60;
            }
            else if (textLength >= 10 && textLength <= 13)
            {
                _displayLabel.FontSize = 50;
            }
            else if (textLength >=14 && textLength <=17)
            {
                _displayLabel.FontSize = 40;
            }
            else
            {
                _displayLabel.FontSize = 30;
            }
            Button numberClicked = (sender as Button);
            mainViewModel.UpdateNumber = numberClicked.Text;
        }

        private void ButtonOperator_Clicked(object sender, EventArgs e)
        {
            // Get second number
            // Clear the display
            // show new number

            Button operationClicked = (sender as Button);
            mainViewModel.UpdateOperator = operationClicked.Text;
        }
        private void ButtonCalculateResult_Clicked(object sender, EventArgs e)
        {
            mainViewModel.GetCalculationResult();
        }
        private void ButtonClearAll_Clicked(object sender, EventArgs e)
        {
            mainViewModel.ClearAll();
        }
        private void ButtonTalk_Clicked(object sender, EventArgs e)
        {
            mainViewModel.SayNumber();
        }

        private async void ButtonSettings_ClickedAsync(object sender, EventArgs e)
        {
            List<Locale> allLocal = mainViewModel.DisplaySettings();

            var action = await DisplayActionSheet("Select Language", "Cancel", null, allLocal.Select(m => m.Name + " " + m.Language + " " + m.Country).ToArray());
            if(action != null)
            {
                mainViewModel.SetLocale(allLocal.Find(x => x.Name.Equals(action)));
            }
        }

        private void ButtonDecimalPoint_Clicked(object sender, EventArgs e)
        {
            mainViewModel.AddDecimalPoint();
        }
    }
}
