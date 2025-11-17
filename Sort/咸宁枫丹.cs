namespace WindowUI.Sort;

public class 咸宁枫丹
{
    public static void Run(string FilePath, string FileName)
    {
        List<string> SN = new List<string>();
        List<string> Uid = new List<string>();


        var sql = "SELECT SerialNum FROM kahao order by SerialNum ASC";
        SN = Mdb.Select(FilePath, sql);
        sql = "SELECT UID_16 FROM kahao order by SerialNum ASC";
        Uid = Mdb.Select(FilePath, sql);
        
        var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        var fileName = $"{FileName}.txt";
        var filePath = Path.Combine(desktopPath, fileName);
        using (var writer = new StreamWriter(filePath))
        {
            for (var i = 0; i < SN.Count; i++)
                writer.WriteLine($"{SN[i]} {Uid[i]}");
        }

        Message.ShowSnack();
    }
}