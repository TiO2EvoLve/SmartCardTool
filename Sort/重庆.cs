namespace WindowUI.Sort;

public class 重庆
{
    public static void Run(string FilePath, string excelFileName)
    {
        //取出文件数据
        List<string> SNData = new List<string>();
        List<string> ATSData = new List<string>();

        var sql = "select 打码特殊算法 from kahao order by 打码特殊算法 ASC ";
        SNData = Mdb.Select(FilePath, sql);
        foreach (var item in SNData) Console.WriteLine(item);
        sql = "select ATS from kahao order by 打码特殊算法 ASC ";
        ATSData = Mdb.Select(FilePath, sql);

        var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        var fileName = $"HG-{excelFileName}";
        var filePath = Path.Combine(desktopPath, fileName);
        using (var writer = new StreamWriter(filePath))
        {
            writer.WriteLine(ATSData.Count);
            for (var i = 0; i < ATSData.Count; i++)
                if (i == ATSData.Count - 1)
                    writer.Write(SNData[i] + ";" + SNData[i] + ";" + ATSData[i] + ";");
                else
                    writer.WriteLine(SNData[i] + ";" + SNData[i] + ";" + ATSData[i] + ";");
        }
        Message.ShowSnack();
    }
}