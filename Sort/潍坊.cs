
using System.Text.RegularExpressions;

namespace WindowUI.Sort;

public class 潍坊
{
      public static void Run(string FilePath, string FileName)
    {
        int Count = 0; // 记录文件的数量
        string Date = ""; // 记录日期

        // 读取mdb文件
        List<string> ATS = new List<string>();
        List<string> CardID = new List<string>();

        // 读取ATS参数
        string sql = "SELECT ATS FROM kahao order by SerialNum ASC";
        ATS = Mdb.Select(FilePath, sql);
        if (ATS.Count == 0)
        {
            Message.ShowMessageBox("错误","ATS数据读取失败");
            return;
        }
        // 读取卡标识参数
        sql = "SELECT 卡标识 FROM kahao order by SerialNum ASC";
        CardID = Mdb.Select(FilePath, sql);
        if (CardID.Count == 0)
        {
            Message.ShowMessageBox("错误","卡标识数据读取失败");
            return;
        }
        // 使用正则表达式匹配数量和日期
        Regex regex = new Regex(@"(\d+)-(\d{8})");
        Match match = regex.Match(FileName);

        if (match.Success)
        {
            // 提取数量
            if (int.TryParse(match.Groups[1].Value, out int number))
            {
                Count = number;
            }
            else
            {
                Message.ShowMessageBox("异常","无法解析数量");
                return;
            }
            // 提取并解析日期
            string dateStr = match.Groups[2].Value;
            if (dateStr.Length == 8)
            {
                Date = dateStr;
            }
            else
            {
                Message.ShowMessageBox("异常","无效的日期格式");
                return;
            }
        }
        else
        {
            Message.ShowMessageBox("异常","未找到匹配的数量和日期");
            return;
        }
        // 生成RCC文件
        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string fileName = $"HG2610{Date}01.RCC";
        string filePath = Path.Combine(desktopPath, fileName);
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            writer.WriteLine(Count);
            for (int i = 0; i < Count; i++)
            {
                if (i < ATS.Count && i < CardID.Count)
                {
                    writer.WriteLine($"{ATS[i]}                {CardID[i]}");
                }
                else
                {
                    Message.ShowMessageBox("错误","数据行数不匹配");
                    return;
                }
            }
        }
        Message.ShowSnack();
    }
}