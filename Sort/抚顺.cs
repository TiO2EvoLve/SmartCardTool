namespace WindowUI.Sort;

public class 抚顺
{
    public static void Run(string FilePath, string excelFileName)
    {
        List<string> SnData = new List<string>();
        List<string> UidData = new List<string>();
        
        string sql = "select SerialNum from kahao order by SerialNum ASC";
        SnData = Mdb.Select(FilePath, sql);
        sql = "select UID_16 from kahao order by SerialNum ASC ";
        UidData = Mdb.Select(FilePath, sql);
        
        //保存为txt文件
        var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        var fileName = $"{excelFileName}.txt";
        var filePath = Path.Combine(desktopPath, fileName);
        using (var writer = new StreamWriter(filePath))
        {
            for (var i = 0; i < SnData.Count; i++)
                if (i == SnData.Count - 1)
                    writer.Write($"{SnData[i]} {Tools.ChangeDecimalSystem(UidData[i])}");
                else
                    writer.WriteLine($"{SnData[i]} {Tools.ChangeDecimalSystem(UidData[i])}");
        }

        Message.ShowSnack();
    }
}