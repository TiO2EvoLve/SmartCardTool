namespace WindowUI.Sort;

public class 南京功夫豆
{
    public static void Run(string FilePath,  string excelFileName)
    {
        List<string> SNData = new List<string>();
        List<string> UidData = new List<string>();

     
        var sql = "SELECT SerialNum FROM kahao order by SerialNum ASC";
        SNData = Mdb.Select(FilePath, sql);
        sql = "SELECT UID_16 FROM kahao order by SerialNum ASC";
        UidData = Mdb.Select(FilePath, sql);

        // 保存文件到桌面
        var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        var fileName = $"{excelFileName}.txt";
        var filePath = Path.Combine(desktopPath, fileName);

        using (var writer = new StreamWriter(filePath))
        {
            for (var i = 0; i < SNData.Count; i++)
                if (i == SNData.Count - 1)
                    writer.Write($"{SNData[i]}\t{UidData[i]}00");
                else
                    writer.WriteLine($"{SNData[i]}\t{UidData[i]}00");
        }

        Message.ShowSnack();
    }
}