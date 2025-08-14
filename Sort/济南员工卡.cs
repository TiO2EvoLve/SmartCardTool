namespace WindowUI.Sort;

public class 济南员工卡
{
    public static void Run(string FilePath, string FileName)
    {
        List<string> SNData = new List<string>();
        List<string> UidData = new List<string>();
        
        string sql = "SELECT SerialNum FROM kahao order by SerialNum ASC";
        SNData = Mdb.Select(FilePath, sql);
        sql = "SELECT UID_16_ FROM kahao order by SerialNum ASC";
        UidData = Mdb.Select(FilePath, sql);
        
        //保存为txt文件
        var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        var fileName = $"{FileName}.txt";
        var filePath = Path.Combine(desktopPath, fileName);
        using (var writer = new StreamWriter(filePath))
        {
            for (var i = 0; i < SNData.Count; i++)
                if (i == SNData.Count - 1)
                    writer.Write($"{UidData[i]}|{UidData[i]}|{SNData[i]}");
                else
                    writer.WriteLine($"{UidData[i]}|{UidData[i]}|{SNData[i]}");
        }
        Message.ShowSnack();
    }
}