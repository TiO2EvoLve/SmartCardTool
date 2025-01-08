namespace WindowUI.Sort;

public class 石家庄
{
    public static void Run(MemoryStream ExcelData, string excelFileName)
    {
        // 第一个rcc文件，excel格式
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // 避免出现许可证错误
        List<string> SNData = new List<string>();
        List<string> Uid_16_Data = new List<string>();
        using (var package = new ExcelPackage(ExcelData))
        {
            var worksheet = package.Workbook.Worksheets[0]; // 获取第一个工作表
            int rowCount = worksheet.Dimension.Rows; // 获取行数
            // 遍历Excel文件的每一行
            for (int row = 1; row <= rowCount; row++)
            {
                string SNValue = worksheet.Cells[row, 7].Text;
                string Uid_16_Value = worksheet.Cells[row, 2].Text;
                SNData.Add(SNValue);
                Uid_16_Data.Add(Uid_16_Value);
            }
        }
        //新建一个Excel文件
        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add(excelFileName);
            worksheet.Cells[1, 1].Value = "SerialNum";
            worksheet.Cells[1, 2].Value = "UID_16_";
            for (int i = 0; i < Uid_16_Data.Count; i++)
            {
                worksheet.Cells[i + 2, 1].Value = SNData[i];
                worksheet.Cells[i + 2, 2].Value = Uid_16_Data[i];
            }
            // 保存文件到桌面
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string fileName = $"{excelFileName}.xlsx";
            string filePath = Path.Combine(desktopPath, fileName);
            package.SaveAs(new FileInfo(filePath));
        }

        // 第二个rcc文件，txt格式
        string desktopPath1 = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string fileName1 = $"{excelFileName}.txt";
        string filePath1 = Path.Combine(desktopPath1, fileName1);

        using (StreamWriter writer = new StreamWriter(filePath1))
        {
            writer.WriteLine("SerialNum\tUID");
            for (int i = 0; i < SNData.Count; i++)
            {
                if (i == SNData.Count - 1)
                {
                    writer.Write($"{SNData[i]}\t{Uid_16_Data[i]}");
                }
                else
                {
                    writer.WriteLine($"{SNData[i]}\t{Uid_16_Data[i]}");
                }
            }
        }
        MessageBox.Show("数据已处理并保存到桌面");
    }
}