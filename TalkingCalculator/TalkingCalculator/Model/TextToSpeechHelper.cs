namespace TalkingCalculator.Model
{
    public class TextToSpeechHelper
    {
        private string one(long num)
        {
            switch (num)
            {
                case 1: return "One";
                case 2: return "Two";
                case 3: return "Three";
                case 4: return "Four";
                case 5: return "Five";
                case 6: return "Six";
                case 7: return "Seven";
                case 8: return "Eight";
                case 9: return "Nine";
            }
            return "";
        }
        private string twoLessThan20(long num)
        {
            switch (num)
            {
                case 10: return "Ten";
                case 11: return "Eleven";
                case 12: return "Twelve";
                case 13: return "Thirteen";
                case 14: return "Fourteen";
                case 15: return "Fifteen";
                case 16: return "Sixteen";
                case 17: return "Seventeen";
                case 18: return "Eighteen";
                case 19: return "Nineteen";
            }
            return "";
        }
        private string ten(long num)
        {
            switch (num)
            {
                case 2: return "Twenty";
                case 3: return "Thirty";
                case 4: return "Forty";
                case 5: return "Fifty";
                case 6: return "Sixty";
                case 7: return "Seventy";
                case 8: return "Eighty";
                case 9: return "Ninety";
            }
            return "";
        }
        private string two(long num)
        {
            if (num == 0)
                return "";
            else if (num < 10)
                return one(num);
            else if (num < 20)
                return twoLessThan20(num);
            else
            {
                long tenner = num / 10;
                long rest = num - tenner * 10;
                if (rest != 0)
                    return ten(tenner) + " " + one(rest);
                else
                    return ten(tenner);
            }
        }
        private string three(long num)
        {
            long hundred = num / 100;
            long rest = num - hundred * 100;
            string res = "";
            if (hundred * rest != 0)
                res = one(hundred) + " Hundred " + two(rest);
            else if ((hundred == 0) && (rest != 0))
                res = two(rest);
            else if ((hundred != 0) && (rest == 0))
                res = one(hundred) + " Hundred";
            return res;
        }
        public string ConvertNumberToEnglish(string number)
        {
            long num = 0;
            long.TryParse(number.Replace(",", ""), out num);

            if (num == 0)
                return "Zero";

            long trillion = num / 1000000000000;
            long billion = (num - (trillion * 1000000000000)) / 1000000000;
            long million = (num - trillion * 1000000000000 - billion * 1000000000) / 1000000;
            long thousand = (num - trillion * 1000000000000 - billion * 1000000000 - million * 1000000) / 1000;
            long rest = num - trillion * 1000000000000 - billion * 1000000000 - million * 1000000 - thousand * 1000;

            string result = "";
            if (trillion != 0)
            {
                result = three(trillion) + " Trillion";
            }
            if (billion != 0)
            {
                if (!string.IsNullOrEmpty(result))
                    result += " ";
                result += three(billion) + " Billion";
            }
            if (million != 0)
            {
                if (!string.IsNullOrEmpty(result))
                    result += " ";
                result += three(million) + " Million";
            }
            if (thousand != 0)
            {
                if (!string.IsNullOrEmpty(result))
                    result += " ";
                result += three(thousand) + " Thousand";
            }
            if (rest != 0)
            {
                if (!string.IsNullOrEmpty(result))
                    result += " ";
                result += three(rest);
            }
            return result;
        }
    }
}
