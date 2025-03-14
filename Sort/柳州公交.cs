namespace WindowUI.Sort;

public class 柳州公交
{
    public static void Run(string FilePath, List<string> MKData)
    {
        //根据逗号切割MKdate
        string[] KCdata = MKData[0].Split(';');
        var Order = KCdata[1];
        var CardBin = KCdata[5];
        var CardNumber = KCdata[4];
        var StartSN = KCdata[6];
        var EndSN = KCdata[7];
        // 取出文件的数据
        List<string> SNData = new List<string>();
        var ATSData = new List<string>();

        var sql = "select SerialNum from kahao order by SerialNum ASC ";
        SNData = Mdb.Select(FilePath, sql);
        if (SNData.Count == 0)
        {
            Message.ShowMessageBox("错误", "卡号读取失败");
            return;
        }

        sql = "select ATS from kahao order by SerialNum ASC ";
        ATSData = Mdb.Select(FilePath, sql);
        if (ATSData.Count == 0)
        {
            Message.ShowMessageBox("错误", "ATS读取失败");
            return;
        }

        var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        var date = Order.Substring(3, 8);
        var fileName = $"RC_{date}_54500000_0004_{Order}_{StartSN}_{CardNumber}";
        var filePath = Path.Combine(desktopPath, fileName);

        using (var writer = new StreamWriter(filePath))
        {
            writer.WriteLine($"01;{Order};{CardBin};{StartSN};{EndSN};{CardNumber};");
            for (var i = 0; i < SNData.Count; i++)
                if (i == SNData.Count - 1)
                    writer.Write($"{SNData[i]};{SNData[i]};{ATSData[i]};");
                else
                    writer.WriteLine($"{SNData[i]};{SNData[i]};{ATSData[i]};");
        }

        Message.ShowSnack();
    }
}