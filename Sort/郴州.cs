namespace WindowUI.Sort;

public class 郴州
{
    public static void Run(List<string> MKData, string mkFileName, MemoryStream ExcelData)
    {
        List<string> SN = new List<string>();
        List<string> ATS = new List<string>();

        // 读取SN参数
        // var sql = "SELECT SerialNum FROM kahao order by SerialNum ASC";
        // SN = Mdb.Select(FilePath, sql);
        // 读取ATS参数
        // sql = "SELECT ATS FROM kahao order by SerialNum ASC";
        // ATS = Mdb.Select(FilePath, sql);
        using (var package = new ExcelPackage(ExcelData))
        {
            var worksheet = package.Workbook.Worksheets[0]; // 获取第一个工作表
            var rowCount = worksheet.Dimension.Rows; // 获取行数
            // 遍历Excel文件的每一行
            for (var row = 2; row <= rowCount; row++)
            {
                var SNValue = worksheet.Cells[row, 9].Text;
                var atsValue = worksheet.Cells[row, 4].Text;
                SN.Add(SNValue);
                ATS.Add(atsValue);
            }
        }
        
        
        //处理MK文件
        //截取MK文件第二行的前42个字节
        MKData[1] = MKData[1].Substring(0, 42);
        //获取总数据的条数
        var totalLines = SN.Count;
        //将总数据条数转为6位数
        var totalLinesFormatted = totalLines.ToString("D6");
        //将MK文件的第二行的后6位替换为总数据条数
        MKData[1] = MKData[1].Substring(0, MKData[1].Length - 6) + totalLinesFormatted;
        if (MKData[1].Length != 42)
        {
            Message.ShowMessageBox("1错误", "MK文件格式错误");
            return;
        }

        //输出rcc文件
        var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        var fileName = $"RC{mkFileName}";
        var filePath = Path.Combine(desktopPath, fileName);
        using (var writer = new StreamWriter(filePath))
        {
            writer.WriteLine(MKData[0]);
            writer.WriteLine(MKData[1]);
            for (var i = 0; i < SN.Count; i++)
                if (i == SN.Count - 1)
                    writer.Write(
                        $"{SN[i]}      {SN[i]}      {ATS[i]}              00                         FFFFFFFFFFFFFFFFFFFF");
                else
                    writer.WriteLine(
                        $"{SN[i]}      {SN[i]}      {ATS[i]}              00                         FFFFFFFFFFFFFFFFFFFF");
        }

        Message.ShowSnack();
    }
}