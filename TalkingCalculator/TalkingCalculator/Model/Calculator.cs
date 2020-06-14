namespace TalkingCalculator
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class Calculator
    {

        // "÷"
        // "×"
        private StringBuilder numberString = new StringBuilder();
        private double leftNumber = 0;
        private double rightNumber = double.NaN;
        private double tempResult = double.NaN;
        private bool _isDecimalValue;
        private string calculateOperation = string.Empty;
        private string totalString = string.Empty;
        private string leftString = string.Empty;
        private string rightString = string.Empty;
        private string oprString = string.Empty;


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
        public string GetNumber()
        {
            if (numberString.Length == 0)
            {
                return "0";
            }
            else
            {
                if (isDecimalValue)
                {
                    return numberString.ToString();
                }
                if (double.IsNaN(rightNumber))
                {
                    return ConvertDigitToString(leftNumber);
                }
                else
                {
                    return ConvertDigitToString(rightNumber);
                }

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
            if (string.IsNullOrEmpty(calculateOperation))
            {
                leftString = numberString.ToString();
                double.TryParse(numberString.ToString(), out leftNumber);
            }
            else
            {
                rightString = numberString.ToString();
                double.TryParse(numberString.ToString(), out rightNumber);
            }
        }

        public string UpdateOperator(string oper)
        {
            if (string.IsNullOrEmpty(calculateOperation) && double.IsNaN(rightNumber))
            {

                if (double.IsNaN(leftNumber))
                {
                    if (!double.IsNaN(tempResult))
                    {
                        leftNumber = tempResult;
                        calculateOperation = oper;
                        numberString.Clear();
                        tempResult = double.NaN;
                        isDecimalValue = false;
                        leftString = ConvertDigitToString(leftNumber);
                        ClearRightNumber();
                        return ConvertDigitToString(leftNumber);
                    }
                    else
                    {
                        return "0";
                    }
                }
                else
                {
                    // No action required. Only Left side of equation provided.
                    calculateOperation = oper;
                    numberString.Clear();
                    isDecimalValue = false;
                    return ConvertDigitToString(leftNumber);
                }
            }
            else if (!double.IsNaN(rightNumber))
            {
                // Left + Right side of equation is available and Operation is available. Calculate result and return
                string temp = GetResult();
                calculateOperation = oper;
                return temp;
            }
            else if (!string.IsNullOrEmpty(calculateOperation))
            {
                // only left side of equation is available, meaning user is updating operation.
                calculateOperation = oper;
                return ConvertDigitToString(leftNumber);
            }
            else
            {
                //Error result;
                return ErrorResult;
            }
        }

        public string GetResult()
        {
            if (!string.IsNullOrEmpty(calculateOperation) && !double.IsNaN(leftNumber))
            {
                if (double.IsNaN(rightNumber))
                {
                    rightNumber = leftNumber;
                }

                leftString = ConvertDigitToString(leftNumber);
                rightString = ConvertDigitToString(rightNumber);

                if (calculateOperation.Contains("+"))
                {
                    leftNumber = leftNumber + rightNumber;
                    ClearRightNumber();
                }
                else if (calculateOperation.Contains("-"))
                {
                    leftNumber = leftNumber - rightNumber;
                    ClearRightNumber();
                }
                else if (calculateOperation.Contains("÷"))
                {
                    leftNumber = leftNumber / rightNumber;
                    ClearRightNumber();
                }
                else if (calculateOperation.Contains("×"))
                {
                    leftNumber = leftNumber * rightNumber;
                    ClearRightNumber();
                }
                else
                {
                    return ErrorResult;
                }
                totalString = ConvertDigitToString(leftNumber);
                tempResult = leftNumber;
                oprString = calculateOperation;
                return totalString;
            }
            return ErrorResult;
        }
        public string GetCalculation()
        {
            if (string.IsNullOrEmpty(oprString))
            {
                if (string.IsNullOrEmpty(totalString))
                {
                    return leftString + " " + calculateOperation.ToString() + " " + rightString;
                }
                else
                {
                    string r = leftString + " " + calculateOperation.ToString() + " " + rightString + " = " + totalString;
                    leftString = totalString;
                    rightString = totalString = string.Empty;
                    return r;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(totalString))
                {
                    return leftString + " " + oprString + " " + rightString;
                }
                else
                {
                    string r = leftString + " " + oprString + " " + rightString + " = " + totalString;
                    leftString = totalString;
                    oprString = rightString = totalString = string.Empty;
                    return r;
                }
            }

        }
        public void ClearAllData()
        {
            numberString.Clear();
            leftNumber = double.NaN;
            rightNumber = double.NaN;
            calculateOperation = string.Empty;
            isDecimalValue = false;
            totalString = rightString = leftString = string.Empty;

        }

        public void ClearRightNumber()
        {
            numberString.Clear();
            rightNumber = double.NaN;
            isDecimalValue = false;
        }
    }
}
