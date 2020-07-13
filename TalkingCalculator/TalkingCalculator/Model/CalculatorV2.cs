using System;
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
                    int tempLastIndex = numberString.Length - 1;
                    string inputString = numberString.ToString();
                    if (numberString.Length != 0 && calculatorHelper.isNumber(inputString[tempLastIndex].ToString()))
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
            if (numberString.Length < 1)
            {
                return "0";
            }
            else
            {
                StringBuilder tempOutput = new StringBuilder();
                var outputArray = numberString.ToString().ToArray();
                for(int i = 0; i < outputArray.Length; i++)
                {
                    if (calculatorHelper.isNumber(outputArray[i].ToString()))
                    {
                        tempOutput.Append(outputArray[i].ToString());
                        continue;
                    }
                    tempOutput.Append(" ");
                    tempOutput.Append(outputArray[i].ToString());
                    tempOutput.Append(" ");
                }
                return tempOutput.ToString();
            }
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
                else if (tempLastValue.Equals("."))
                {
                    // Update input where only number and a decimal point (0.) is provided
                    numberString.Append(0);
                    numberString.Append(oper);
                }
                else
                {
                    numberString.Append(oper);
                }

                //Calculate results
                UpdateTheResults(numberString.ToString());
            }
            if (isDecimalValue)
            {
                isDecimalValue = false;
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

        /// <summary>Adds right side of equation when required.</summary>
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
                // Update input where only number and a decimal point (0.) is provided
                if (tempLastValue.Equals("."))
                {
                    numberString.Append(0);
                }
            }
        }
        public string GetLastNumberInput()
        {

            int tempLastIndex = inputList.Count - 1;
            while (tempLastIndex >= 0 && calculatorHelper.isOperator(inputList[tempLastIndex]))
            {
                tempLastIndex -= 1;
            }
            if (tempLastIndex < 0)
            {
                return "0";
            }
            else
            {
                return inputList[tempLastIndex].ToString();
            }
            //if (inputList.Count > 0)
            //{
            //    return inputList[inputList.Count - 1].ToString();
            //}
            //return "0";
        }
    }
}
