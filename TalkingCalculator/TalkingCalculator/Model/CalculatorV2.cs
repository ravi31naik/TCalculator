using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Transactions;

namespace TalkingCalculator.Model
{
    public class CalculatorV2
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

        public List<string> calcList = new List<string>();

        public Queue<string> inputPrimary = new Queue<string>();
        public Stack<string> inputSecondary = new Stack<string>();
        public Stack<double> calculationStack = new Stack<double>();


        public static readonly string ErrorResult = "ERROR";

        public void Test()
        {
            //string test = "3+4*2/(1-5)";
            List<string> test = new List<string>();
            test.Add("3");
            test.Add("+");
            test.Add("4");
            test.Add("*");
            test.Add("2");
            test.Add("/");
            test.Add("1");
            test.Add("-");
            test.Add("5");
            string tests = "3+4*2/1-";
            string tests1 = "3+";
            string tests2 = "3.4/5+8.8";
            string tests3 = "111.1+4+444+4.4";
            EquationToListConverter(tests);
            ConvertToPostfix(calcList);
            calcList.Clear();
            calculationStack.Clear();

            EquationToListConverter(tests1);
            ConvertToPostfix(calcList);
            calcList.Clear();
            calculationStack.Clear();

            EquationToListConverter(tests2);
            ConvertToPostfix(calcList);
            calcList.Clear();
            calculationStack.Clear();

            EquationToListConverter(tests3);
            ConvertToPostfix(calcList);
            calcList.Clear();
            calculationStack.Clear();


            //ConvertToPostfix(test);
            //CalculateResult(inputPrimary);
        }

        private void EquationToListConverter(string equation)
        {
            StringBuilder tempEquation = new StringBuilder();
            foreach (char c in equation)
            {
                if(isNumber(c.ToString()) || c.Equals('.'))
                {
                    tempEquation.Append(c);
                }
                else
                {
                    if(tempEquation.Length > 0)
                    {
                        calcList.Add(tempEquation.ToString());
                        tempEquation.Clear();
                    }
                    calcList.Add(c.ToString());
                }
            }
            if (tempEquation.Length > 0)
            {
                calcList.Add(tempEquation.ToString());
                tempEquation.Clear();
            }
        }
        private void ConvertToPostfix(List<string> inputList)
        {
            //Convert to postfix queue
            /*
             * start from left add number to Queue and add first operator to secondary stack
             * Add number to queue, check if more operator
             *        Condition 1 : is Higher Presedence then put to secondary stack
             *        Condition 2 : is Equal or lower presedence then pop operator from secondary stack and push it to primary queue
             *                      and push the current value to secondary stack
             * Once input is finished put all operator from seconday stack to primary queue
             * 
             */

            foreach(string input in inputList)
            {
                if (isNumber(input))
                {
                    inputPrimary.Enqueue(input);
                }
                else if (input.Equals("("))
                {
                    inputSecondary.Push("(");
                }
                else if (input.Equals(")"))
                {
                    while (inputSecondary.Count > 0 && inputSecondary.Peek() != "(")
                    {
                        inputPrimary.Enqueue(inputSecondary.Pop());
                    }

                    if (inputSecondary.Count > 0 && inputSecondary.Peek().Equals("("))
                    {
                        inputSecondary.Pop(); // pop out "("
                    }
                    else
                    {
                        // Throw error
                    }
                }
                else if(isOperator(input))
                {
                    if (inputSecondary.Count > 0)
                    {
                        if (inputSecondary.Peek().Equals("("))
                        {
                            //Do nothing
                        }
                        else if (isHigherPrecedence(input, inputSecondary.Peek()))
                        {
                            inputSecondary.Push(input);
                        }
                        else
                        {
                            while (inputSecondary.Count > 0 && !isHigherPrecedence(input, inputSecondary.Peek()))
                            {
                                inputPrimary.Enqueue(inputSecondary.Pop());
                            }
                            inputSecondary.Push(input);
                        }
                    }
                    else
                    {
                        inputSecondary.Push(input);
                    }
                }
                else
                {
                    // Unknown/invalid input
                }
            }
            while (inputSecondary.Count > 0)
            {
                inputPrimary.Enqueue(inputSecondary.Pop());
            }
            //Answer 342*15-/+

            //Calculate Result
            CalculateResult(inputPrimary);

        }
        private void CalculateResult(Queue<string> input)
        {
            double number = double.NaN;

            while (input.Count > 0)
            {
                string temp = input.Dequeue();

                if (isNumber(temp))
                {
                    double.TryParse(temp, out number);
                    calculationStack.Push(number);
                }
                else if(isOperator(temp))
                {
                    if(calculationStack.Count >= 2)
                    {
                        double tempRight = calculationStack.Pop();
                        double tempLeft = calculationStack.Pop();
                        calculationStack.Push(performCalculation(tempLeft, tempRight, temp));
                    }
                }
                else
                {
                    //
                }
            }
        }
        private double performCalculation(double leftNumber, double rightNumber, string operation)
        {
            switch (operation)
            {
                case "+":
                    leftNumber = leftNumber + rightNumber;
                    break;
                case "-":
                    leftNumber = leftNumber - rightNumber;
                    break;
                case "/":
                    leftNumber = leftNumber / rightNumber;
                    break;
                case "*":
                    leftNumber = leftNumber * rightNumber;
                    break;
                default:
                    leftNumber = 0;
                    break;
            }
            return leftNumber;
        }
        private bool isHigherPrecedence(string inputOne, string inputTwo)
        {
            if ((inputOne.Equals("*") || inputOne.Equals("/")))
            {
                if ((inputTwo.Equals("+") || inputTwo.Equals("-")))
                {
                    return true;
                }
                return false;
            }
            return false;
        }
        private bool isNumber(string inputData)
        {
            if (inputData.Contains("."))
            {
                double temp = double.NaN;
                double.TryParse(inputData, out temp);
                if(double.IsNaN(temp))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            // TODO: Learn how this function works/ performance
            if(inputData.All(char.IsDigit))
            {
                return true;
            }

            return false;
        }
        private bool isOperator(string inputData)
        {
            if (inputData.Equals("+") ||
                inputData.Equals("-") ||
                inputData.Equals("*") ||
                inputData.Equals("/"))
            {
                return true;
            }
            return false;
        }
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
            if (calcList.Count != 0 && calcList.Count % 3 == 0)
            {
                ConvertToPostfix(calcList);
            }
        }
        public void UpdateOperator(string oper)
        {
            if (numberString.Length > 0)
            {
                int lastIndex = numberString.Length - 1;

                //int indexTemp = calcList.Count;
                string tempLastValue = numberString[lastIndex].ToString();

                if (!isOperator(tempLastValue))
                {
                    //Add operator to list
                    calcList.Add(numberString.ToString());
                    numberString.Append(oper);
                }
                else
                {
                    //Update operator in list
                    numberString.Insert(lastIndex, oper);
                    int tempCount = calcList.Count - 1;
                    calcList.Insert(tempCount, oper);
                }
            }
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
