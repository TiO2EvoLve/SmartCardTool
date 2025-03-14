namespace WindowUI.Sort;

public class 青岛博研加气站
{
    public static void Run(MemoryStream ExcelData, string excelFileName)
    {
        //取出Excel文件的数据
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // 避免出现许可证错误
        List<string> snData = new List<string>();
        List<string> uidData = new List<string>();

        using (var package = new ExcelPackage(ExcelData))
        {
            var worksheet = package.Workbook.Worksheets[0]; // 获取第一个工作表
            var rowCount = worksheet.Dimension.Rows; // 获取行数

            //遍历Excel文件的每一行
            for (var row = 1; row <= rowCount; row++)
            {
                var snValue = worksheet.Cells[row, 8].Text;
                var uidValue = worksheet.Cells[row, 3].Text;
                snData.Add(snValue);
                uidData.Add(uidValue);
            }
        }

        // 创建一个新的Excel文件
        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add(excelFileName);

            // 插入标题行
            worksheet.Cells[1, 1].Value = "SN";
            worksheet.Cells[1, 2].Value = "UID";

            // 插入数据
            for (var i = 0; i < snData.Count; i++)
            {
                worksheet.Cells[i + 2, 1].Value = snData[i];
                worksheet.Cells[i + 2, 2].Value = uidData[i];
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