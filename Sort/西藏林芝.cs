namespace WindowUI.Sort;

public class 西藏林芝
{
    public static void Run(MemoryStream ExcelData)
    {
        //取出Excle文件的数据
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // 避免出现许可证错误
        List<string> SNData = new List<string>();
        using (var package = new ExcelPackage(ExcelData))
        {
            var worksheet = package.Workbook.Worksheets[0]; // 获取第一个工作表
            int rowCount = worksheet.Dimension.Rows; //获取行数
            //遍历Excel文件的每一行
            for (int row = 2; row <= rowCount; row++)
            {
                string SNValue = worksheet.Cells[row, 1].Text;
                SNData.Add(SNValue);
            }
        }
        string date = "20241115";
        string cardtype = "01";
        string startdate = "20241107";
        string fnishdate = "20401231";

        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string fileName = $"HP-04377740{date}165931.TXT";
        string filePath = Path.Combine(desktopPath, fileName);
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            writer.WriteLine(SNData.Count + date);
            for (int i = 0; i < SNData.Count; i++)
            {
                if (i == SNData.Count - 1)
                {
                    writer.Write($"{SNData[i]}|04377740FFFFFFFF|{cardtype}|{startdate}|{fnishdate}|2020202020202020202020202020202020202020|2020202020202020202020202020202020202020202020202020202020202020|00|00|0000|0000000000|");
                }
                else
                {
                    writer.WriteLine($"{SNData[i]}|04377740FFFFFFFF|{cardtype}|{startdate}|{fnishdate}|2020202020202020202020202020202020202020|2020202020202020202020202020202020202020202020202020202020202020|00|00|0000|0000000000|");

                }
            }
        }
        MessageBox.Show($"数据已合并并保存到文件: {filePath}");    
    }
}