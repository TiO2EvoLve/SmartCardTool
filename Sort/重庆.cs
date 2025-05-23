using WindowUI.Pages;

namespace WindowUI.Sort;

public class 重庆
{
    public static void Run(string FilePath, string FileName)
    {
        重庆菜单 page = new 重庆菜单();
        page.ShowDialog();

        if (page.SelectedCampus == "交通部")
        {
            交通部(FilePath,FileName);
        }else if (page.SelectedCampus == "住建部")
        {
            住建部(FilePath);
        }

    }

    private static void 住建部(string FilePath)
    {
        //取出文件数据
        List<string> SNData = new List<string>();
        List<string> ATSData = new List<string>();
        
        var sql = "select SerialNum from kahao order by SerialNum ASC ";
        SNData = Mdb.Select(FilePath, sql);
        sql = "select ATS from kahao order by SerialNum ASC ";
        ATSData = Mdb.Select(FilePath, sql);
        
        var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        var fileName = $"HG4000{DateTime.Now:yyyyMMdd}01.rcc";
        var filePath = Path.Combine(desktopPath, fileName);
        string CardType = SNData[0].Substring(4,4);
        using (var writer = new StreamWriter(filePath))
        {
            writer.WriteLine(SNData.Count);
            for (var i = 0; i < SNData.Count; i++)
                if (i == SNData.Count - 1)
                {
                    writer.Write($"{ATSData[i]}                {SNData[i]}{CardType}40000000000000000000");
                }
                else
                    writer.WriteLine($"{ATSData[i]}                {SNData[i]}{CardType}40000000000000000000");
        }
        Message.ShowSnack();
    }

    private static void 交通部(string FilePath, string FileName)
    {
        //取出文件数据
        List<string> SNData = new List<string>();
        List<string> ATSData = new List<string>();

        var sql = "select 打码特殊算法 from kahao order by 打码特殊算法 ASC ";
        SNData = Mdb.Select(FilePath, sql);
        sql = "select ATS from kahao order by 打码特殊算法 ASC ";
        ATSData = Mdb.Select(FilePath, sql);

        var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        var fileName = $"HG-{FileName}";
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