using WindowUI.Pages;

namespace WindowUI.Sort;

public class 哈尔滨学院
{
    public static void Run(string FilePath)
    {
        List<string> SN;
        List<string> ATS;

        哈尔滨学院菜单 harbin = new ();
        harbin.ShowDialog();
        string cardtype = harbin.CardType;
        string ZhikaTime = DateTime.Now.ToString("yyyyMMdd");
        
        string sql = "Select SerialNum From kahao order by SerialNum ASC";
        SN = Mdb.Select(FilePath,sql);
        sql = "Select ATS From kahao order by SerialNum ASC";
        ATS = Mdb.Select(FilePath,sql);
        
        var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        var fileName = $"{cardtype}1500{ZhikaTime}01.rcc";
        var filePath = Path.Combine(desktopPath, fileName);
        using (var writer = new StreamWriter(filePath))
        {
            writer.WriteLine(SN.Count);
            for (var i = 0; i < SN.Count; i++)
                if (i == SN.Count - 1)
                    writer.Write($"{ATS[i]}|{SN[i]}");
                else
                    writer.WriteLine($"{ATS[i]}|{SN[i]}");
        }

        Message.ShowSnack();
    }
}