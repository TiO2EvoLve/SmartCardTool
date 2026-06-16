using WindowUI.Pages;

namespace WindowUI.Sort;

public class 南通市民卡
{
    public static void Run(string FilePath)
    {
        List<string> SN = new List<string>();
        List<string> UID = new List<string>();

        string sql = "select SerialNum from kahao order by SerialNum ASC";
        SN = Mdb.Select(FilePath, sql);
        sql = "select UID_16_ from kahao order by SerialNum ASC ";
        UID = Mdb.Select(FilePath, sql);

        南通市民卡菜单 Nt = new ();
        Nt.ShowDialog();
        
        var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        var fileName = $"{Nt.FileName}.result";
        var filePath = Path.Combine(desktopPath, fileName);
        var Time = DateTime.Now.ToString("yyyyMMddHH");
        using (var writer = new StreamWriter(filePath))
        {
            for (var i = 0; i < SN.Count; i++)
            {
                if (i == SN.Count - 1)
                    writer.Write($"{Nt.Order}|{SN[i]}||{Nt.Type}|{Nt.Id}|{Nt.Id}|{UID[i]}|{Time}|{Nt.StartTime}|{Nt.VaildTime}|09|4|||||||0|0|0");
                else
                    writer.WriteLine($"{Nt.Order}|{SN[i]}||{Nt.Type}|{Nt.Id}|{Nt.Id}|{UID[i]}|{Time}|{Nt.StartTime}|{Nt.VaildTime}|09|4|||||||0|0|0");
            }
                
        }
        Message.ShowSnack();
    }
}