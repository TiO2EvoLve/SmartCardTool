using WindowUI.Pages;

namespace WindowUI.Sort;

public class 三门峡
{
    public static void Run(string FilePath, string excelFileName)
    {
        三门峡菜单 menu= new();
        menu.ShowDialog();
        var cardtype = menu.CardType;
        List<string> SNData = new List<string>();
        var UidData = new List<string>();

        var sql = "select SerialNum from kahao order by SerialNum ASC";
        SNData = Mdb.Select(FilePath, sql);

        sql = "select UID_10 from kahao order by SerialNum ASC ";
        UidData = Mdb.Select(FilePath, sql);

        // 创建一个新的TXT件
        var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        var fileName = excelFileName + ".txt";
        var filePath = Path.Combine(desktopPath, fileName);
        
        using (var writer = new StreamWriter(filePath))
        {
            for (var i = 0; i < SNData.Count; i++)
                if (i == SNData.Count - 1)
                    writer.Write($"{UidData[i].PadLeft(10, '0')}\t{cardtype}{SNData[i]}\t0");
                else
                    writer.WriteLine($"{UidData[i].PadLeft(10, '0')}\t{cardtype}{SNData[i]}\t0");
        }
        Message.ShowSnack();
        
    }
}