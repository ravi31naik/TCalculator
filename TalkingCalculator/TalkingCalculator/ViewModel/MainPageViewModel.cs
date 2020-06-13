namespace TalkingCalculator
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using Xamarin.Essentials;

    public class MainPageViewModel : INotifyPropertyChanged
    {
        Calculator calc = new Calculator();
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
                calc.UpdateNumber(number);
                _displayScreen = calc.GetNumber();
                onPropertyChanged(nameof(DisplayNumber));
                _displayCalculation = calc.GetCalculation();
                onPropertyChanged(nameof(DisplayCalculation));
            }
        }

        public string UpdateOperator
        {
            get => operatorEntered;

            set
            {
                operatorEntered = value;
                _displayScreen = calc.UpdateOperator(operatorEntered);
                onPropertyChanged(nameof(DisplayNumber));
                _displayCalculation = calc.GetCalculation();
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
            _displayScreen = calc.GetResult();
            onPropertyChanged(nameof(DisplayNumber));
            _displayCalculation = calc.GetCalculation();
            onPropertyChanged(nameof(DisplayCalculation));
            calc.ClearAllData();
        }

        public void ClearAll()
        {
            calc.ClearAllData();
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
