namespace WindowUI.Sort;

public class 邹平
{
    public static void Run(MemoryStream ExcelData, string excelFileName)
    {
        //取出Excle文件的数据
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // 避免出现许可证错误
        //取出第七列流水号数据
        List<string> SerialNumber = new List<string>();
        List<string> UID = new List<string>();
        using (var package = new ExcelPackage(ExcelData))
        {
            var worksheet = package.Workbook.Worksheets[0]; // 获取第一个工作表
            var rowCount = worksheet.Dimension.Rows; //获取行数
            //遍历Excel文件的每一行
            for (var row = 1; row <= rowCount; row++)
            {
                var SNValue = worksheet.Cells[row, 7].Text;
                var UIDValue = worksheet.Cells[row, 3].Text;
                UIDValue = Tools.ChangeDecimalSystem(UIDValue);
                SerialNumber.Add(SNValue);
                UID.Add(UIDValue);
            }
        }

        // 创建一个新的Excel文件
        var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        var fileName = $"{excelFileName}.xlsx";
        var filePath = Path.Combine(desktopPath, fileName);

        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add(excelFileName);
            // 插入数据
            worksheet.Cells[1, 1].Value = "SerialNumber";
            worksheet.Cells[1, 2].Value = "CUSTOMUID";
            for (var i = 0; i < UID.Count; i++)
            {
                worksheet.Cells[i + 2, 1].Value = SerialNumber[i];
                worksheet.Cells[i + 2, 2].Value = UID[i];
            }

            // 保存文件到桌面
            package.SaveAs(new FileInfo(filePath));
        }

        Message.ShowSnack();
    }
}