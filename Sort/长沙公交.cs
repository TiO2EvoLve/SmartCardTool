namespace WindowUI.Sort;

public class 长沙公交
{
    public static void Run(MemoryStream ExcelData, string excelFileName)
    {
        //先处理Excel文件
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // 避免出现许可证错误
        List<string> SerialNumData = new List<string>();
        List<string> uid_16Data = new List<string>();
        List<string> uid_16_Data = new List<string>();
        List<string> uid_10Data = new List<string>();
        List<string> uid_10_Data = new List<string>();
        using (var package = new ExcelPackage(ExcelData))
        {
            var worksheet = package.Workbook.Worksheets[0]; // 获取第一个工作表
            var rowCount = worksheet.Dimension.Rows; //获取行数
            //遍历Excel文件的每一行
            for (var row = 2; row <= rowCount; row++)
            {
                var SerialNumValue = worksheet.Cells[row, 1].Text;
                var uid_16Value = worksheet.Cells[row, 3].Text;
                var uid_16_Value = worksheet.Cells[row, 4].Text;
                var uid_10Value = worksheet.Cells[row, 5].Text;
                var uid_10_Value = worksheet.Cells[row, 6].Text;
                SerialNumData.Add(SerialNumValue);
                uid_16Data.Add(uid_16Value);
                uid_16_Data.Add(uid_16_Value);
                uid_10Data.Add(uid_10Value);
                uid_10_Data.Add(uid_10_Value);
            }
        }

        // 创建一个新的Excel文件
        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add(excelFileName);
            // 插入数据

            worksheet.Cells[1, 1].Value = "SerialNum";
            worksheet.Cells[1, 2].Value = "UID_16";
            worksheet.Cells[1, 3].Value = "UID_16_";
            worksheet.Cells[1, 4].Value = "UID_10";
            worksheet.Cells[1, 5].Value = "UID_10_";

            for (var i = 0; i < SerialNumData.Count; i++)
            {
                worksheet.Cells[i + 2, 1].Value = SerialNumData[i];
                worksheet.Cells[i + 2, 2].Value = uid_16Data[i];
                worksheet.Cells[i + 2, 3].Value = uid_16_Data[i];
                worksheet.Cells[i + 2, 4].Value = uid_10Data[i];
                worksheet.Cells[i + 2, 5].Value = uid_10_Data[i];
            }

            // 保存文件到桌面
            var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var fileName = $"{excelFileName}.xlsx";
            var filePath = Path.Combine(desktopPath, fileName);
            package.SaveAs(new FileInfo(filePath));
            Message.ShowSnack();
        }
    }
}