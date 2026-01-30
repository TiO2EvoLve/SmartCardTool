namespace WindowUI.Sort;

public class 济南交通联合
{
    public static void Run(string FilePath, string FileName)
    {
        List<string> SN = new List<string>();
        List<string> UID = new List<string>();
        
        string sql = "SELECT 打码特殊算法 FROM kahao order by SerialNum ASC";
        SN = Mdb.Select(FilePath, sql);
        sql = "SELECT UID_16_ FROM kahao order by SerialNum ASC";
        UID = Mdb.Select(FilePath, sql);
        
        
        //保存为txt文件
        var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        var fileName = $"{FileName}.txt";
        var filePath = Path.Combine(desktopPath, fileName);
        using (var writer = new StreamWriter(filePath))
        {
            for (var i = 0; i < SN.Count; i++)
                if (i == SN.Count - 1)
                    writer.Write($"{UID[i]}\t{SN[i]}\t2");
                else
                    writer.WriteLine($"{UID[i]}\t{SN[i]}\t2");
        }
        Message.ShowSnack();
    }
}