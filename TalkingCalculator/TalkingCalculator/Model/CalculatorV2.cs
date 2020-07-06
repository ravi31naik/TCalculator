using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace TalkingCalculator.Model
{
    public class CalculatorV2
    {
        // "÷"
        // "×"
        private StringBuilder numberString = new StringBuilder();
        private double tempResult = double.NaN;
        private bool _isDecimalValue;
        CalculatorHelper calculatorHelper = new CalculatorHelper();
        List<string> inputList = new List<string>();

        public static readonly string ErrorResult = "ERROR";

        public bool isDecimalValue
        {
            get { return _isDecimalValue; }
            set
            {
                _isDecimalValue = value;
                if (_isDecimalValue)
                {
                    if (!(numberString.Length == 0))
                    {
                        numberString.Append(".");
                    }
                    else
                    {
                        numberString.Append("0.");
                    }
                }
            }
        }
        public string ConvertDigitToString(double number)
        {
            if (number < 1000000000000000)
            {
                if (isDecimalValue)
                {
                    //return string.Format("{0:N}", number);
                    return number.ToString();
                }
                else
                {
                    //return string.Format("{0:N0}", number);
                    return number.ToString();
                }
            }
            else
            {
                return string.Format("{0:E3}", number);
            }
        }
        public string GetEquation()
        {

            StringBuilder tempOutput = new StringBuilder();
            if(numberString.Length < 1)
            {
                return "0";
            }

            var outputArray = inputList.ToArray();
            foreach (var item in outputArray)
            {
                if (calculatorHelper.isNumber(item))
                {
                    double temp = double.NaN;
                    double.TryParse(item, out temp);
                    tempOutput.Append(ConvertDigitToString(temp));
                }
                else if (calculatorHelper.isOperator(item))
                {
                    tempOutput.Append(item);
                }
                else
                {
                    return ErrorResult;
                }
                tempOutput.Append(" ");
            }

            return tempOutput.ToString();
        }
        public void UpdateNumber(string number)
        {
            if (isDecimalValue)
            {
                numberString.Append(number);
            }
            else
            {
                if (!(numberString.Length == 0 && number.Equals("0")))
                {
                    numberString.Append(number);
                }
            }
            //Calculate results
            UpdateTheResults(numberString.ToString());
        }
        public void UpdateOperator(string oper)
        {
            if (oper.Equals("×"))
            {
                oper = "*";
            }
            if (oper.Equals("÷"))
            {
                oper = "/";
            }
            if (numberString.Length > 0)
            {
                int lastIndex = numberString.Length - 1;
                string tempLastValue = numberString[lastIndex].ToString();

                if(calculatorHelper.isOperator(tempLastValue))
                {
                    //Update the last operator
                    numberString.Remove(lastIndex, 1);
                    numberString.Append(oper);
                }
                else
                {
                    numberString.Append(oper);
                }

                //Calculate results
                UpdateTheResults(numberString.ToString());
            }
            
        }
        public string GetCalculationResult()
        {
            return ConvertDigitToString(tempResult);
        }
        private void UpdateTheResults(string input)
        {
            // Update the result
            inputList = calculatorHelper.EquationToListConverter(input);
            tempResult = calculatorHelper.GetEquationResult(inputList);
        }
        public void ClearAllData()
        {
            numberString.Clear();
            isDecimalValue = false;
            inputList.Clear();
            tempResult = 0;
        }
        public void EquationCalculationCompleted()
        {
            inputList.Clear();
            numberString.Clear();
            inputList.Add(tempResult.ToString());
            numberString.Append(tempResult.ToString());
        }
        public void UpdateEquation()
        {
            if (numberString.Length > 0)
            {
                int lastIndex = numberString.Length - 1;
                string tempLastValue = numberString[lastIndex].ToString();

                if (calculatorHelper.isOperator(tempLastValue))
                {
                    numberString.Append(tempResult);
                    UpdateTheResults(numberString.ToString());
                }
            }
        }
    }
}
