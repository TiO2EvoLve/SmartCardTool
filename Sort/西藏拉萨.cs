namespace WindowUI.Sort;

public class 西藏拉萨
{
    public static void Run(MemoryStream ExcelData)
    {
        //取出Excle文件的数据
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // 避免出现许可证错误
        var SNData = new List<string>();
        using (var package = new ExcelPackage(ExcelData))
        {
            var worksheet = package.Workbook.Worksheets[0]; // 获取第一个工作表
            var rowCount = worksheet.Dimension.Rows; //获取行数
            //遍历Excel文件的每一行
            for (var row = 2; row <= rowCount; row++)
            {
                var SNValue = worksheet.Cells[row, 1].Text;
                SNData.Add(SNValue);
            }
        }

        var date = "20241115";
        var cardtype = "01";
        var startdate = "20241107";
        var finishdate = "20401231";
        var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        var fileName = $"HP_04357710FFFFFFFF{date}165931.txt";
        var filePath = Path.Combine(desktopPath, fileName);
        using (var writer = new StreamWriter(filePath))
        {
            writer.WriteLine($"{SNData.Count}|{date}|");
            for (var i = 0; i < SNData.Count; i++)
                if (i == SNData.Count - 1)
                    writer.Write($"{SNData[i]}|04357710FFFFFFFF|{cardtype}|{startdate}|{finishdate}||||01|0000||");
                else
                    writer.WriteLine($"{SNData[i]}|04357710FFFFFFFF|{cardtype}|{startdate}|{finishdate}||||01|0000||");
        }

        Message.ShowSnack();
    }
}