namespace WindowUI.Sort;

public class 内江
{
    public static void Run(string FilePath, string FileName)
    {
        List<string> SN = new List<string>();
        List<string> Uid = new List<string>();


        var sql = "SELECT SerialNum FROM kahao order by SerialNum ASC";
        SN = Mdb.Select(FilePath, sql);
        sql = "SELECT UID_16_ FROM kahao order by SerialNum ASC";
        Uid = Mdb.Select(FilePath, sql);
        
        for (var i = 0; i < SN.Count; i++)
        {
            SN[i] = SN[i].Substring(3);
        }
        var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        var fileName = $"{FileName}.txt";
        var filePath = Path.Combine(desktopPath, fileName);
        using (var writer = new StreamWriter(filePath))
        {
            writer.WriteLine("8H反        卡号                 厂商代码");
            for (var i = 0; i < SN.Count; i++)
                if (i == SN.Count - 1)
                    writer.Write($"{Uid[i]}\t{SN[i]}\t8670");
                else
                    writer.WriteLine($"{Uid[i]}\t{SN[i]}\t8670");
        }

        Message.ShowSnack();
    }
}