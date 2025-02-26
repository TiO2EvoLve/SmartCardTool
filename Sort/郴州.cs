namespace WindowUI.Sort;

public class 郴州
{
    public static void Run(List<string> MKData,string mkFileName,string FilePath)
    {
        
        List<string> SN = new List<string>();
        List<string> ATS = new List<string>();
        
        // 读取SN参数
        string sql = "SELECT SerialNum FROM kahao order by SerialNum ASC";
        SN = Mdb.Select(FilePath, sql);
        // 读取ATS参数
        sql = "SELECT ATS FROM kahao order by SerialNum ASC";
        ATS = Mdb.Select(FilePath, sql);
        //处理MK文件
        //截取MK文件第二行的前42个字节
        MKData[1] = MKData[1].Substring(0, 42);
        //获取总数据的条数
        int totalLines = SN.Count;
        //将总数据条数转为6位数
        string totalLinesFormatted = totalLines.ToString("D6");
        //将MK文件的第二行的后6位替换为总数据条数
        MKData[1] = MKData[1].Substring(0, MKData[1].Length - 6) + totalLinesFormatted;
        if (MKData[1].Length != 42)
        {
            Message.ShowMessageBox("错误","MK文件格式错误");
            return;
        }
        //输出rcc文件
        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string fileName = $"RC{mkFileName}";
        string filePath = Path.Combine(desktopPath, fileName);
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            writer.WriteLine(MKData[0]);
            writer.WriteLine(MKData[1]);
            for (int i = 0; i < SN.Count; i++)
            {
                if (i == SN.Count - 1)
                {
                    writer.Write($"{SN[i]}      {SN[i]}      {ATS[i]}              00                         FFFFFFFFFFFFFFFFFFFF");
                }
                else
                {
                    writer.WriteLine($"{SN[i]}      {SN[i]}      {ATS[i]}              00                         FFFFFFFFFFFFFFFFFFFF");
                }
            }
        }
        Message.ShowSnack(); 
    }
}