using System.Text.RegularExpressions;

namespace WindowUI.Sort;

public partial class 青岛公交
{
    [GeneratedRegex(@"(\d+)-(\d{8})")]
    private static partial Regex MyRegex();
    
    public static void Run(string FilePath, string FileName)
    {
        int Count = 1; // 记录文件的数量
        string Date = "00000000"; // 记录日期

        // 读取mdb文件
        List<string> ATS = new List<string>();
        var CardID = new List<string>();

        // 读取ATS参数
        string sql = "SELECT ATS FROM kahao order by SerialNum ASC";
        ATS = Mdb.Select(FilePath, sql);

        // 读取特殊卡号参数
        sql = "SELECT 特殊卡号 FROM kahao order by SerialNum ASC";
        CardID = Mdb.Select(FilePath, sql);

        // 使用正则表达式匹配数量和日期
        var regex = MyRegex();
        var match = regex.Match(FileName);

        if (match.Success)
        {
            // 提取数量
            if (int.TryParse(match.Groups[1].Value, out var number))
            {
                Count = number;
            }
            else
            {
                Message.ShowMessageBox("异常", "无法解析数量");
                return;
            }
        
            // 提取并解析日期
            var dateStr = match.Groups[2].Value;
            if (dateStr.Length == 8)
            {
                Date = dateStr;
            }
            else
            {
                Message.ShowMessageBox("异常", "无效的日期格式");
                return;
            }
        }
        else
        {
            Message.ShowMessageBox("异常", "未找到匹配的数量和日期");
            return;
        }

        // 生成RCC文件
        var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        var fileName = $"HG2660{Date}01.rcc";
        var filePath = Path.Combine(desktopPath, fileName);
        using (var writer = new StreamWriter(filePath))
        {
            writer.WriteLine(Count);
            for (var i = 0; i < ATS.Count; i++)
                if (i == ATS.Count - 1)
                {
                    writer.WriteLine($"{ATS[i]}                {CardID[i]}");
                }
                else
                {
                    writer.Write($"{ATS[i]}                {CardID[i]}");
                }
            
        }
        Message.ShowSnack();
    }

    
}