namespace WindowUI.Sort;

public class 潍坊
{
    public static void Run(MemoryStream ExcelData,string excelFileName)
    {
        int rowCount;//excel文件的行数
        //先处理Excel文件
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // 避免出现许可证错误
        List<string> processedData = new List<string>();
        using (var package = new ExcelPackage(ExcelData))
        {
            var worksheet = package.Workbook.Worksheets[0]; // 获取第一个工作表
            rowCount = worksheet.Dimension.Rows; //获取行数
            //遍历Excel文件的每一行
            for (int row = 2; row <= rowCount; row++)
            {
                string firstColumnValue = worksheet.Cells[row, 2].Text;
                string secondColumnValue = worksheet.Cells[row, 8].Text;
                string newRow = $"{firstColumnValue}                {secondColumnValue}";
                processedData.Add(newRow);
            }
        }
        
        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string fileName = $"HG2610{excelFileName}01";
        string filePath = Path.Combine(desktopPath, fileName);
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            writer.WriteLine(rowCount - 1);
            for (int i = 0; i < processedData.Count; i++)
            {
                writer.WriteLine(processedData[i]);
            }
        }
        MessageBox.Show($"数据已合并并保存到文件: {filePath},请修改文件名");
        //将文件后缀改为.RCC
        string newFilePath = filePath + ".RCC";
        File.Move(filePath, newFilePath);    
    }
}