using System.Text.RegularExpressions;

namespace WindowUI.Tool;

public class Tools
{
    //调整16进制与不调整16进制互相转换
    public static string ChangeHexPairs(string hex)
    {
        if (hex.Length % 2 != 0)
        {
            MessageBox.Show("数据不合法，可能需要删除表头行");
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
}