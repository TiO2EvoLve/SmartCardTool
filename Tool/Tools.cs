using System.Text.RegularExpressions;

namespace WindowUI.Tool;

public class Tools
{
    //调整16进制与不调整16进制互相转换
    public static string ChangeHexPairs(string hex)
    {
        if (hex.Length % 2 != 0) Message.ShowMessageBox("错误", "数据位数不合法");
        var reversedHex = new char[hex.Length];
        var j = 0;
        for (var i = hex.Length - 2; i >= 0; i -= 2)
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
        var pattern = $@"{startKey}(.*?){endKey}";
        var match = Regex.Match(input, pattern);
        return match.Success ? match.Groups[1].Value : string.Empty;
    }

    //转为10进制
    public static string ChangeDecimalSystem(string input)
    {
        return Convert.ToUInt32(input, 16).ToString();
    }
    
}