
namespace WindowUI.Sort;

public class 洪城
{
    public static void Run(string FilePath)
    {
        List<string> SN = new();
        List<string> SN_s = new();
        List<string> ATS = new();
        
        string sql = "select 特殊卡号 from kahao order by SerialNum ASC";
        SN = Mdb.Select(FilePath, sql);
        sql = "select 打码特殊算法 from kahao order by SerialNum ASC";
        SN_s = Mdb.Select(FilePath, sql);
        sql = "select ATS from kahao order by SerialNum ASC";
        ATS = Mdb.Select(FilePath, sql);
        
        //住建部文件
        var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        var date = DateTime.Now.ToString("yyyyMMdd");
        var fileName = $"住建部_回盘_山东华冠_{date}_001.rcc";
        var filePath = Path.Combine(desktopPath, fileName);
        using (var writer = new StreamWriter(filePath))
        {
            writer.WriteLine(SN.Count);
            for (var i = 0; i < SN.Count; i++)
                if (i == SN.Count - 1)
                    writer.Write($"{ATS[i]}                {SN_s[i]}        000033000000000000000000");
                else
                    writer.WriteLine($"{ATS[i]}                {SN_s[i]}        000033000000000000000000");
        }
        //交通部文件
        date = DateTime.Now.ToString("yyyyMMddHHmmss");
        fileName = $"RCHG{date}000000.rcc";
        filePath = Path.Combine(desktopPath, fileName);
        using (var writer = new StreamWriter(filePath))
        {
            writer.WriteLine("01");
            var number = SN.Count.ToString().PadLeft(6, '0');
            writer.WriteLine($"ORD202401250912301542024012400020016{number}");

            for (var i = 0; i < SN.Count; i++)
                if (i == SN.Count - 1)
                    writer.Write($"{SN[i]}      {SN[i]}      {ATS[i]}                FFFFFFFFFFFFFFFFFFFF");
                else
                    writer.WriteLine($"{SN[i]}      {SN[i]}      {ATS[i]}                FFFFFFFFFFFFFFFFFFFF");
        }

        Message.ShowSnack();
    }
}