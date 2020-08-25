namespace TalkingCalculator
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;
    using System.Linq;
    using TalkingCalculator.Model;
    using Xamarin.Essentials;
    using Xamarin.Forms;

    public class MainPageViewModel : INotifyPropertyChanged
    {
        CalculatorV2 calcV2 = new CalculatorV2();
        TextToSpeechCalc tts = new TextToSpeechCalc();
        string number = string.Empty;
        string operatorEntered = string.Empty;
        private string _displayScreen = string.Empty;
        private string _displayCalculation = string.Empty;

        public event PropertyChangedEventHandler PropertyChanged;
        void onPropertyChanged(string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public string DisplayNumber => $"{_displayScreen}";
        public string DisplayCalculation => $"{_displayCalculation}";
        public string UpdateNumber
        {
            get => number;

            set
            {
                number = value;

                calcV2.UpdateNumber(number);

                UpdateEquationDisplay(calcV2.GetEquation());
                UpdateResultDisplay(calcV2.GetLastNumberInput());
            }
        }

        public string UpdateOperator
        {
            get => operatorEntered;

            set
            {
                operatorEntered = value;

                calcV2.UpdateOperator(operatorEntered);

                UpdateEquationDisplay(calcV2.GetEquation());
                UpdateResultDisplay(calcV2.GetCalculationResult());
            }
        }

        public List<Locale> DisplaySettings()
        {
            return tts.GetAllLocaleAsync().Result.ToList();
        }
        public void SetLocale(Locale localeSelected)
        {
            tts.localePicked = localeSelected;
        }

        /// <summary>
        /// Perform all operations provided in the input
        /// </summary>
        public void GetCalculationResult()
        {
            calcV2.UpdateEquation();

            string tempResult = calcV2.GetCalculationResult();
            UpdateEquationDisplay(calcV2.GetEquation() + " = " + tempResult);
            UpdateResultDisplay(tempResult);

            calcV2.EquationCalculationCompleted();
        }

        public void ClearAll()
        {
            calcV2.ClearAllData();
            UpdateNumber = "0";
        }

        public void SayNumber()
        {
            if (!string.IsNullOrEmpty(_displayScreen))
            {
                _ = tts.SayNumberOutLoudAsync(_displayScreen);
            }
        }

        public void AddDecimalPoint()
        {
            if (!calcV2.isDecimalValue)
            {
                calcV2.isDecimalValue = true;

                UpdateEquationDisplay(calcV2.GetEquation());
                UpdateResultDisplay(calcV2.GetLastNumberInput());
            }
        }
        private void UpdateEquationDisplay(string display)
        {
            _displayCalculation = display;
            onPropertyChanged(nameof(DisplayCalculation));
        }
        private void UpdateResultDisplay(string display)
        {
            _displayScreen = display;
            onPropertyChanged(nameof(DisplayNumber));
        }
    }
}
