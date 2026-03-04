
namespace WindowUI.Sort;

public class 海口
{
    public static void Run(string FilePath, string FileName)
    {
        
        
        List<string> SN = new List<string>();
        List<string> ATS = new List<string>();
        List<string> 卡标识 = new List<string>();
        string 订单号;
        string 首位卡号;
        string 末位卡号;
        
        var sql = "SELECT SerialNum FROM kahao order by SerialNum ASC";
        SN = Mdb.Select(FilePath, sql);
        首位卡号 = SN.First();
        末位卡号 = SN.Last();
        sql = "SELECT ATS FROM kahao order by SerialNum ASC";
        ATS = Mdb.Select(FilePath, sql);
        sql = "SELECT 卡标识 FROM kahao order by SerialNum ASC";
        卡标识 = Mdb.Select(FilePath, sql);
        sql = "SELECT 厂商代码 FROM kahao order by SerialNum ASC";
        订单号 = Mdb.Select(FilePath, sql).First();
        
        var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        var fileName = $"ICA05316431FFF{DateTime.Now:yyyyMMdd}160001.txt";
        var filePath = Path.Combine(desktopPath, fileName);
        using (var writer = new StreamWriter(filePath))
        {
            writer.WriteLine($"03{订单号}{首位卡号} {末位卡号} {SN.Count.ToString().PadLeft(5,'0').PadRight(25,'F')}");
            for (var i = 0; i < SN.Count; i++)
                writer.WriteLine($"000000000000000000{SN[i]} {ATS[i]}                              {卡标识[i]}           FFFFFFFFFF");
        }
        Message.ShowSnack();
    }
}