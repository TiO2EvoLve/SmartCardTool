namespace WindowUI.Sort;

public class 哈尔滨城市通
{
    public static void Run(string FilePath)
    {
        List<string> SN = new();
        List<string> ATS = new();
        
        string sql = "select 打码特殊算法 from kahao order by SerialNum ASC";
        SN = Mdb.Select(FilePath, sql);
        sql = "select ATS from kahao order by SerialNum ASC";
        ATS = Mdb.Select(FilePath, sql);

        var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        var fileName = $"HY1500{DateTime.Now:yyyyMMdd}01.rcc";
        var filePath = Path.Combine(desktopPath, fileName);
        using (var writer = new StreamWriter(filePath))
        {
            writer.WriteLine(SN.Count - 1);
            for (var i = 0; i < SN.Count; i++)
                if (i == SN.Count - 1)
                    writer.Write($"{ATS[i]}|{SN[i]}");
                else
                    writer.WriteLine($"{ATS[i]}|{SN[i]}");
        }
        Message.ShowSnack();
    }
}