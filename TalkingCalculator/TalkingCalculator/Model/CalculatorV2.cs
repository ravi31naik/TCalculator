﻿using System;
using System.Collections.Generic;
using System.Globalization;
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
        private bool _isResultDecimal;
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
                    isResultDecimal = true;
                }
            }
        }
        public bool isResultDecimal
        {
            get { return _isResultDecimal; }
            set { _isResultDecimal = value; }
        }
        public string ConvertDigitToString(double number)
        {
            if (number < 1000000000000000)
            {
                if (isResultDecimal)
                {
                    //return string.Format("{0:N}", number);
                    return number.ToString();
                }
                else
                {
                    //return string.Format("{0:N0}", number);
                    return number.ToString("#,#", CultureInfo.InvariantCulture);
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
                //string resultOutput = string.Empty;
                var outputArray = numberString.ToString().ToArray();
                for(int i = 0; i < outputArray.Length; i++)
                {
                    if (calculatorHelper.isNumber(outputArray[i].ToString()))
                    {
                        tempOutput.Append(outputArray[i].ToString());
                        continue;
                    }
                    //resultOutput = ConvertExponentialToDigits(tempOutput.ToString());
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
            isResultDecimal = false;
        }
        public void EquationCalculationCompleted()
        {
            inputList.Clear();
            numberString.Clear();
            //inputList.Add(tempResult.ToString());
            //numberString.Append(tempResult.ToString());

            inputList.Add(ConvertExponentialToDigits(tempResult.ToString()));
            numberString.Append(ConvertExponentialToDigits(tempResult.ToString()));
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
                if (isResultDecimal)
                {
                    return inputList[tempLastIndex].ToString();
                }
                else
                {
                    return ConvertDigitToString(ConvertStringToNumber(inputList[tempLastIndex]));
                }
            }
        }

        private string ConvertExponentialToDigits(string inputNumber)
        {
            if (isResultDecimal)
            {
                return double.Parse(inputNumber, CultureInfo.InvariantCulture).ToString("F10");
            }
            else
            {
                return double.Parse(inputNumber, CultureInfo.InvariantCulture).ToString("F0");
            }
        }
        private double ConvertStringToNumber(string inputNumber)
        {
            return double.Parse(inputNumber);
        }
    }
}
