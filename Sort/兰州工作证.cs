namespace WindowUI.Sort;

public class 兰州工作证
{
    public static void Run(string FilePath, string excelFileName)
    {
        List<string> SNData = new List<string>();
        List<string> UidData = new List<string>();
        
        string sql = "SELECT NUM From kahao order by NUM ASC";
        SNData = Mdb.Select(FilePath, sql);
        sql = "SELECT UID10_ From kahao order by NUM ASC";
        UidData = Mdb.Select(FilePath, sql);

        var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        var fileName = excelFileName + ".txt";
        var filePath = Path.Combine(desktopPath, fileName);
        using (var writer = new StreamWriter(filePath))
        {
            for (var i = 0; i < SNData.Count; i++)
                if (i == SNData.Count - 1)
                    writer.Write($"{SNData[i]},{UidData[i]}");
                else
                    writer.WriteLine($"{SNData[i]},{UidData[i]}");
        }
        Message.ShowSnack();
    }
}