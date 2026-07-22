namespace WindowUI.Sort;

public class 哈尔滨
{
    public void Run(string FilePath)
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
        var fileName = $"HG2660{date}01.rcc";
        var filePath = Path.Combine(desktopPath, fileName);
        using (var writer = new StreamWriter(filePath))
        {
            writer.WriteLine(SN.Count);
            for (var i = 0; i < SN.Count; i++)
            {
                if (SN[i].Length != 19)
                {
                    Message.ShowMessageBox("错误", $"第{i + 1}行的卡号位数错误！");
                    return;
                }
                if (ATS[i].Length != 32)
                {
                    Message.ShowMessageBox("错误", $"第{i + 1}行的芯片使用错误！");
                    return;
                }
                if (SN_s[i].Length != 8)
                {
                    Message.ShowMessageBox("错误", $"第{i + 1}行的短号拼接错误！");
                    return;
                }
                
                if (i == SN.Count - 1)
                    writer.Write($"{ATS[i],-48}{SN_s[i],16}000033000000000000000000");
                else
                    writer.WriteLine($"{ATS[i],-48}{SN_s[i],16}000033000000000000000000");
            }
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
                    //TODO: 补齐C卡号
                    writer.Write($"{SN[i],-25}{SN[i],-25}{ATS[i],-48}FFFFFFFFFFFFFFFFFFFF");
                else
                    writer.WriteLine($"{SN[i],-25}{SN[i],-25}{ATS[i],-48}FFFFFFFFFFFFFFFFFFFF");
        }

        Message.ShowSnack();
    }
}