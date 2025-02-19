using System.Text.RegularExpressions;

namespace WindowUI.Tool;

public class Tools
{
    //调整16进制与不调整16进制互相转换
    public static string ChangeHexPairs(string hex)
    {
        if (hex.Length % 2 != 0)
        {
            Message.ShowMessageBox("错误","数据不合法，可能需要删除表头行");
        }
        char[] reversedHex = new char[hex.Length];
        int j = 0;
        for (int i = hex.Length - 2; i >= 0; i -= 2)
        {
            reversedHex[j++] = hex[i];
            reversedHex[j++] = hex[i + 1];
        }
        return new string(reversedHex);
    }
    //淄博公交查找替换逻辑
    public static string ExtractValue(string input, string startKey, string endKey)
    {
        // 匹配以startKey开始到endKey之前的内容
        string pattern = $@"{startKey}(.*?){endKey}";
        Match match = Regex.Match(input, pattern);
        return match.Success ? match.Groups[1].Value : string.Empty;
    }
    //转为10进制
    public static string ChangeDecimalSystem(string input)
    {
        return Convert.ToUInt32(input, 16).ToString();
    }
    //使用LUHN算法计算校验值
    public static string Luhn(string input)
    {
        int sum = 0;
        int length = input.Length;
        for (int i = 0; i < length; i++)
        {
            int num = int.Parse(input.Substring(i, 1));
            if ((length - i) % 2 == 0)
            {
                num *= 2;
                if (num > 9)
                {
                    num -= 9;
                }
            }
            sum += num;
        }
        return (10 - sum % 10).ToString();
    }
}