namespace WindowUI.Sort;

public class 济南地铁UL
{
    public static void Run(MemoryStream ExcelData, string excelFileName)
    {
        // 取出Excel文件的数据
        List<string> CustomUIDData = new List<string>();
        List<string> UIDData = new List<string>();
        using (var package = new ExcelPackage(ExcelData))
        {
            var worksheet = package.Workbook.Worksheets[0]; // 获取第一个工作表
            var rowCount = worksheet.Dimension.Rows; // 获取行数
            // 遍历Excel文件的每一行
            for (var row = 2; row <= rowCount; row++)
            {
                var CustomUIDValue = worksheet.Cells[row, 11].Text;
                CustomUIDData.Add(CustomUIDValue);
                var UIDValue = CustomUIDValue.Substring(8, 14);
                UIDData.Add(UIDValue);
            }
        }

        //新建一个Excel文件
        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add(excelFileName);
            worksheet.Cells[1, 1].Value = "Index";
            worksheet.Cells[1, 2].Value = "CUSTOMUID";
            worksheet.Cells[1, 3].Value = "UID";
            for (var i = 0; i < CustomUIDData.Count; i++)
            {
                worksheet.Cells[i + 2, 1].Value = i + 1;
                worksheet.Cells[i + 2, 2].Value = CustomUIDData[i];
                worksheet.Cells[i + 2, 3].Value = UIDData[i];
            }

            // 保存文件到桌面
            var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var fileName = $"{excelFileName}.xlsx";
            var filePath = Path.Combine(desktopPath, fileName);
            package.SaveAs(new FileInfo(filePath));
        }

        Message.ShowSnack();
    }
}