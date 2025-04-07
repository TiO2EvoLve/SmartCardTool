namespace WindowUI.Sort;

public class 哈尔滨城市通
{
    public static void Run(MemoryStream ExcelData, string excelFileName)
    {
        int rowCount; //execl文件的行数
        //先处理Excel文件
        
        List<string> processedData = new List<string>();
        using (var package = new ExcelPackage(ExcelData))
        {
            var worksheet = package.Workbook.Worksheets[0]; // 获取第一个工作表
            rowCount = worksheet.Dimension.Rows; //获取行数
            //遍历Excel文件的每一行
            for (var row = 2; row <= rowCount; row++)
            {
                var firstColumnValue = worksheet.Cells[row, 2].Text;
                var secondColumnValue = worksheet.Cells[row, 11].Text;
                var newRow = $"{firstColumnValue}|{secondColumnValue}";
                processedData.Add(newRow);
            }
        }

        var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        var fileName = $"HY1500{excelFileName}01.rcc";
        var filePath = Path.Combine(desktopPath, fileName);
        using (var writer = new StreamWriter(filePath))
        {
            writer.WriteLine(rowCount - 1);
            for (var i = 0; i < processedData.Count; i++)
                if (i == processedData.Count - 1)
                    writer.Write(processedData[i]);
                else
                    writer.WriteLine(processedData[i]);
        }

        Message.ShowSnack();
    }
}