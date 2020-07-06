namespace TalkingCalculator
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using TalkingCalculator.Model;
    using Xamarin.Essentials;

    public class MainPageViewModel : INotifyPropertyChanged
    {
        Calculator calc = new Calculator();
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
                _displayCalculation = calcV2.GetEquation();
                onPropertyChanged(nameof(DisplayCalculation));
                _displayScreen = number;
                onPropertyChanged(nameof(DisplayNumber));
            }
        }

        public string UpdateOperator
        {
            get => operatorEntered;

            set
            {
                operatorEntered = value;

                calcV2.UpdateOperator(operatorEntered);
                _displayScreen = calcV2.GetCalculationResult();
                onPropertyChanged(nameof(DisplayNumber));
                _displayCalculation = calcV2.GetEquation();
                onPropertyChanged(nameof(DisplayCalculation));
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

        public void GetCalculationResult()
        {
            calcV2.UpdateEquation();
            _displayScreen = calcV2.GetCalculationResult();
            onPropertyChanged(nameof(DisplayNumber));
            _displayCalculation = calcV2.GetEquation();
            _displayCalculation = _displayCalculation + "= " + _displayScreen;
            onPropertyChanged(nameof(DisplayCalculation));
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
            if (!calc.isDecimalValue)
            {
                calc.isDecimalValue = true;
                _displayScreen = calc.GetNumber();
                onPropertyChanged(nameof(DisplayNumber));
                _displayCalculation = calc.GetCalculation();
                onPropertyChanged(nameof(DisplayCalculation));
            }
        }
    }
}
