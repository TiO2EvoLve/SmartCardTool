using System.Text.RegularExpressions;

namespace WindowUI.Sort;

public class 琴岛通
{
    public static void Run(string FilePath,string FileName)
    {
        
        List<string> ATS = new List<string>();
        List<string> SN = new List<string>();
        string Date = ""; // 记录日期
       
        string sql = "SELECT ATS FROM kahao order by SerialNum ASC";
        ATS = Mdb.Select(FilePath, sql);
        
        sql = "SELECT 特殊卡号 FROM kahao order by SerialNum ASC";
        SN = Mdb.Select(FilePath, sql);
            
        // 使用正则表达式匹配数量和日期
        Regex regex = new Regex(@"(\d+)-(\d{8})");
        Match match = regex.Match(FileName);

        if (match.Success)
        {
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
        
        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string fileName = $"HG2660{Date}01.rcc";
        string filePath = Path.Combine(desktopPath, fileName);
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            writer.WriteLine(ATS.Count);
            for (int i = 0; i < ATS.Count; i++)
            {
                if (i == ATS.Count - 1)
                {
                    writer.Write($"{ATS[i]}{SN[i]}");
                }
                else
                {
                    writer.WriteLine($"{ATS[i]}{SN[i]}");
                }
                
            }
        }
        Message.ShowSnack();
    }
}