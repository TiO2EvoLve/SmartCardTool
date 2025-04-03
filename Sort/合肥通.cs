namespace WindowUI.Sort;

public class 合肥通
{
    public static void Run(string FilePath, List<string> MKData, string mkFileName)
    {
        List<string> SNData = new List<string>();
        List<string> ATSData = new List<string>();

        string sql = "select SerialNum from kahao order by SerialNum ASC";
        SNData = Mdb.Select(FilePath, sql);
        sql = "select ATS from kahao order by SerialNum ASC ";
        ATSData = Mdb.Select(FilePath, sql);

        //处理MK文件
        //截取MK文件第二行的前42个字节
        MKData[1] = MKData[1].Substring(0, 42);
        //获取Excel总数据的条数
        var totalLines = SNData.Count;
        //将总数据条数转为6位数
        var totalLinesFormatted = totalLines.ToString("D6");
        //将MK文件的第二行的后6位替换为总数据条数
        MKData[1] = MKData[1].Substring(0, MKData[1].Length - 6) + totalLinesFormatted;
        //将MK文件与Excel文件的数据合并

        var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        var fileName = $"RC{mkFileName}001";
        var filePath = Path.Combine(desktopPath, fileName);
        using (var writer = new StreamWriter(filePath))
        {
            writer.WriteLine(MKData[0]);
            writer.WriteLine(MKData[1]);

            for (var i = 0; i < SNData.Count; i++)
            {
                if (i == SNData.Count - 1)
                    writer.Write($"{SNData[i]}      {SNData[i]}      {ATSData[i]}              00                         FFFFFFFFFFFFFFFFFFFF");
                else
                    writer.WriteLine($"{SNData[i]}      {SNData[i]}      {ATSData[i]}              00                         FFFFFFFFFFFFFFFFFFFF");
            }
                
        }

        Message.ShowSnack();
    }
}