namespace WindowUI.Sort;

public class 徐州地铁
{
    public static void Run(MemoryStream ExcelData, string excelFileName)
    {
        // 取出Excel文件的数据
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // 避免出现许可证错误
        List<string> SNData = new List<string>();
        List<string> UidData = new List<string>();
        using (var package = new ExcelPackage(ExcelData))
        {
            var worksheet = package.Workbook.Worksheets[0]; // 获取第一个工作表
            int rowCount = worksheet.Dimension.Rows; // 获取行数
            // 遍历Excel文件的每一行
            for (int row = 2; row <= rowCount; row++)
            {
                string SNValue = worksheet.Cells[row, 6].Text;
                string UidValue = worksheet.Cells[row, 2].Text;
                SNData.Add(SNValue);
                UidData.Add(UidValue);
            }
        }

        // 保存文件到桌面
        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string fileName = $"{excelFileName}.txt";
        string filePath = Path.Combine(desktopPath, fileName);

        using (StreamWriter writer = new StreamWriter(filePath))
        {
            for (int i = 0; i < SNData.Count; i++)
            {
                if (i == SNData.Count - 1)
                {
                    writer.Write($"{SNData[i]}\t{UidData[i]}00000000");
                }
                else
                {
                    writer.WriteLine($"{SNData[i]}\t{UidData[i]}00000000");
                }
            }
        }

        MessageBox.Show($"文件已保存到桌面{filePath}");
    }
}