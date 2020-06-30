using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TalkingCalculator.Model
{
    public class CalculatorHelper
    {
        public List<string> EquationToListConverter(string equation)
        {
            List<string> calcList = new List<string>();
            StringBuilder tempEquation = new StringBuilder();
            foreach (char c in equation)
            {
                if (isNumber(c.ToString()) || c.Equals('.'))
                {
                    tempEquation.Append(c);
                }
                else
                {
                    if (tempEquation.Length > 0)
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
            }

            return calcList;
        }

        public double GetEquationResult(List<string> calcList)
        {
            Queue<string> postfixEquation = ConvertToPostfix(calcList);
            Stack<double> computedResult = CalculateResult(postfixEquation);

            if (computedResult != null && computedResult.Count > 0)
            {
                return computedResult.Pop();
            }

            return double.NaN;
        }

        private Queue<string> ConvertToPostfix(List<string> inputList)
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

            Queue<string> inputPrimary = new Queue<string>();
            Stack<string> inputSecondary = new Stack<string>();

            foreach (string input in inputList)
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
                else if (isOperator(input))
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

            return inputPrimary;
        }

        private Stack<double> CalculateResult(Queue<string> input)
        {
            double number = double.NaN;
            Stack<double> calculationStack = new Stack<double>();

            while (input.Count > 0)
            {
                string temp = input.Dequeue();

                if (isNumber(temp))
                {
                    double.TryParse(temp, out number);
                    calculationStack.Push(number);
                }
                else if (isOperator(temp))
                {
                    if (calculationStack.Count >= 2)
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

            return calculationStack;
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

        public bool isNumber(string inputData)
        {
            if (inputData.Contains("."))
            {
                double temp = double.NaN;
                double.TryParse(inputData, out temp);
                if (double.IsNaN(temp))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            // TODO: Learn how this function works/ performance
            if (inputData.All(char.IsDigit))
            {
                return true;
            }

            return false;
        }
        
        public bool isOperator(string inputData)
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

    }
}
