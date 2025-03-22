using WindowUI.Pages;

namespace WindowUI.Sort;

public class 兰州
{
    public static void Run(string FilePath, string FileName, List<string> MKData, string mkFileName)
    {
        兰州菜单 lanzhou = new();
        lanzhou.ShowDialog();
        var cardtype = lanzhou.CardType;

        if (cardtype == "0")
            兰州公交(FilePath, FileName, MKData, mkFileName, 0);
        else if (cardtype == "1") 兰州公交(FilePath, FileName, MKData, mkFileName, 1);
    }

    private static void 兰州公交(string FilePath, string excelFileName, List<string> MKData, string mkFileName,
        int type)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // 避免出现许可证错误
        List<string> SnData = new List<string>();
        List<string> ATSData = new List<string>();
        string sql = "select SerialNum from kahao order by SerialNum ASC";
        SnData = Mdb.Select(FilePath, sql);
        sql = "select ATS from kahao order by SerialNum ASC ";
        ATSData = Mdb.Select(FilePath, sql);
        
        //处理MK文件
        //截取MK文件第二行的前42个字节
        MKData[1] = MKData[1].Substring(0, 42);
        //获取Excel总数据的条数
        var totalLines = SnData.Count;
        //将总数据条数转为6位数
        var totalLinesFormatted = totalLines.ToString("D6");
        //将MK文件的第二行的后6位替换为总数据条数
        MKData[1] = MKData[1].Substring(0, MKData[1].Length - 6) + totalLinesFormatted;
        //将MK文件与Excel文件的数据合并
        var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        var fileName = $"RC{mkFileName}001";
        var filePath = Path.Combine(desktopPath, fileName);
        using (var writer = new StreamWriter(filePath))
        {
            writer.WriteLine(MKData[0]);
            writer.WriteLine(MKData[1]);

            for (var i = 0; i < SnData.Count; i++)
                if (i == SnData.Count - 1)
                    writer.Write($"{SnData[i]}      {SnData[i]}      {ATSData[i]}          00                         FFFFFFFFFFFFFFFFFFFF");
                else
                    writer.WriteLine($"{SnData[i]}      {SnData[i]}      {ATSData[i]}          00                         FFFFFFFFFFFFFFFFFFFF");
        }

        if (type == 0)
        {
            Message.ShowSnack();
            return;
        }

        //异型卡需要两个文件
        //第二个文件
        List<string> UidData = new List<string>();
        sql = "select UID_10 from kahao order by SerialNum ASC ";
        UidData = Mdb.Select(FilePath, sql);

        desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        fileName = excelFileName + ".txt";
        filePath = Path.Combine(desktopPath, fileName);
        using (var writer = new StreamWriter(filePath))
        {
            for (var i = 0; i < SnData.Count; i++)
                if (i == SnData.Count - 1)
                    writer.Write($"{SnData[i]},{UidData[i]}");
                else
                    writer.WriteLine($"{SnData[i]},{UidData[i]}");
        }

        Message.ShowSnack();
    }
}