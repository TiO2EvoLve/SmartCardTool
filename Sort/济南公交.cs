namespace WindowUI.Sort;

public class 济南公交
{
    public static void Run(string FilePath, string FileName)
    {
        List<string> SNData = new List<string>();
        
        string sql = "SELECT SerialNum FROM kahao order by SerialNum ASC";
        SNData = Mdb.Select(FilePath, sql);
        
        //保存为txt文件
        var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        var fileName = $"{SNData.Count}张准备文件{DateTime.Now:yyMMdd}.txt";
        var filePath = Path.Combine(desktopPath, fileName);
        using (var writer = new StreamWriter(filePath))
        {
            for (var i = 0; i < SNData.Count; i++)
                if (i == SNData.Count - 1)
                    writer.Write($"8670{DateTime.Now:yyMMdd}{SNData[i][^6..]}");
                else
                    writer.WriteLine($"8670{DateTime.Now:yyMMdd}{SNData[i][^6..]}");
        }
        Message.ShowSnack();
    }
}