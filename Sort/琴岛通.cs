using System.Text.RegularExpressions;

namespace WindowUI.Sort;

public class 琴岛通
{
    public static void Run(string FilePath, string FileName)
    {
        List<string> ATS = new List<string>();
        List<string> SN = new List<string>();
        var Date = ""; // 记录日期

        var sql = "SELECT ATS FROM kahao order by SerialNum ASC";
        ATS = Mdb.Select(FilePath, sql);
        sql = "SELECT 特殊卡号 FROM kahao order by SerialNum ASC";
        SN = Mdb.Select(FilePath, sql);
        // 使用正则表达式匹配数量和日期
        var regex = new Regex(@"(\d+)-(\d{8})");
        var match = regex.Match(FileName);

        if (match.Success)
        {
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
            LogManage.AddLog("没有匹配到日期，已自动使用当前时间");
            Date = DateTime.Now.ToString("yyyyMMdd");
        }

        var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        var fileName = $"HG2660{Date}01.RCC";
        var filePath = Path.Combine(desktopPath, fileName);
        using (var writer = new StreamWriter(filePath))
        {
            writer.WriteLine(ATS.Count);
            for (var i = 0; i < ATS.Count; i++) writer.WriteLine($"{ATS[i]}{SN[i]}");
        }

        Message.ShowSnack();
    }
}