namespace WindowUI.Sort;

public class 绵州通
{
    public static void Run(MemoryStream ExcelData, string FileName)
    {
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
                var UIDValue = worksheet.Cells[row, 2].Text;
                SerialNumber.Add(SNValue);
                UID.Add(UIDValue);
            }
        }

        // 创建一个新的Excel文件
        var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        var fileName = $"{FileName}.xlsx";
        var filePath = Path.Combine(desktopPath, fileName);

        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add(FileName);
            // 插入数据
            worksheet.Cells[1, 1].Value = "卡号";
            worksheet.Cells[1, 2].Value = "物理卡号";
            worksheet.Cells[1, 3].Value = "卡商名称";
            worksheet.Cells[1, 4].Value = "卡商代码";
            for (var i = 0; i < UID.Count; i++)
            {
                worksheet.Cells[i + 2, 1].Value = SerialNumber[i];
                worksheet.Cells[i + 2, 2].Value = UID[i];
                worksheet.Cells[i + 2, 3].Value = "华冠";
                worksheet.Cells[i + 2, 4].Value = "8670";
            }

            // 保存文件到桌面
            package.SaveAs(new FileInfo(filePath));
        }

        Message.ShowSnack();
    }
}