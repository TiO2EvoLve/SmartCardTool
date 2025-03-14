namespace WindowUI.Sort;

public class 盱眙
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
            var rowCount = worksheet.Dimension.Rows; // 获取行数

            // 异步遍历Excel文件的每一行
            for (var row = 1; row <= rowCount; row++)
            {
                var SNValue = worksheet.Cells[row, 6].Text;
                var UidValue = worksheet.Cells[row, 2].Text;
                SNData.Add(SNValue);
                UidData.Add(UidValue);
            }
        }

        // 创建一个新的Excel文件
        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add(excelFileName);
            worksheet.Cells[1, 1].Value = "SerialNumber";
            worksheet.Cells[1, 2].Value = "UID";
            worksheet.Cells[1, 3].Value = "CUSTOMUID";
            for (var i = 0; i < UidData.Count; i++)
            {
                worksheet.Cells[i + 2, 1].Value = SNData[i];
                worksheet.Cells[i + 2, 2].Value = UidData[i];
                worksheet.Cells[i + 2, 3].Value = Tools.ChangeHexPairs(UidData[i]);
            }

            //保存文件到桌面
            var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var fileName = $"{excelFileName}.xlsx";
            var filePath = Path.Combine(desktopPath, fileName);
            package.SaveAs(new FileInfo(filePath));
            // 显示提示消息
            Message.ShowSnack();
        }
    }
}