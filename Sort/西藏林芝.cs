namespace WindowUI.Sort;

public class 西藏林芝
{
    public static void Run(string FilePath)
    {
        List<string> SNData = new List<string>();
        var sql = "SELECT SN FROM RCC order by SN Asc";
        SNData = Mdb.Select(FilePath, sql);
        
        var date = "20241115";
        var cardtype = "01";
        var startdate = "20241107";
        var fnishdate = "20401231";

        var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        var fileName = $"HP-04377740{date}165931.TXT";
        var filePath = Path.Combine(desktopPath, fileName);
        using (var writer = new StreamWriter(filePath))
        {
            writer.WriteLine(SNData.Count + date);
            for (var i = 0; i < SNData.Count; i++)
                if (i == SNData.Count - 1)
                    writer.Write(
                        $"{SNData[i]}|04377740FFFFFFFF|{cardtype}|{startdate}|{fnishdate}|2020202020202020202020202020202020202020|2020202020202020202020202020202020202020202020202020202020202020|00|00|0000|0000000000|");
                else
                    writer.WriteLine(
                        $"{SNData[i]}|04377740FFFFFFFF|{cardtype}|{startdate}|{fnishdate}|2020202020202020202020202020202020202020|2020202020202020202020202020202020202020202020202020202020202020|00|00|0000|0000000000|");
        }

        Message.ShowSnack();
    }
}