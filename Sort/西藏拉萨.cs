namespace WindowUI.Sort;

//昌都同理
public class 西藏拉萨
{
    public static void Run(string FilePath)
    {
        string date;
        var cardtype = "01";
        var startdate = DateTime.Now.ToString("yyyyMMdd");
        var finishdate = "20401231";
        
        List<string> SNData = new List<string>();
        var sql = "SELECT SN FROM RCC order by SN Asc";
        SNData = Mdb.Select(FilePath, sql);
        sql = "select time1 from RCC";
        date = Mdb.Select(FilePath, sql)[0];
        DateTime time = DateTime.Parse(date);
        date = time.ToString("yyyyMMddhhmmss");
     
        var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        var fileName = $"HP_04357710FFFFFFFF{date}.txt";
        var filePath = Path.Combine(desktopPath, fileName);
        using (var writer = new StreamWriter(filePath))
        {
            writer.WriteLine($"{SNData.Count}|{date.Substring(0,8)}|");
            for (var i = 0; i < SNData.Count; i++)
                if (i == SNData.Count - 1)
                    writer.Write($"{SNData[i]}|04357710FFFFFFFF|{cardtype}|{startdate}|{finishdate}||||01|0000||");
                else
                    writer.WriteLine($"{SNData[i]}|04357710FFFFFFFF|{cardtype}|{startdate}|{finishdate}||||01|0000||");
        }

        Message.ShowSnack();
    }
}