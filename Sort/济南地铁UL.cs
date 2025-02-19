
namespace WindowUI.Sort;

public class 济南地铁UL
{
    public static void Run(MemoryStream ExcelData,string excelFileName)
    {
        // 取出Excel文件的数据
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // 避免出现许可证错误
        List<string> CustomUIDData = new List<string>();
        List<string> UIDData = new List<string>();
        using (var package = new ExcelPackage(ExcelData))
        {
            var worksheet = package.Workbook.Worksheets[0]; // 获取第一个工作表
            int rowCount = worksheet.Dimension.Rows; // 获取行数
            // 遍历Excel文件的每一行
            for (int row = 2; row <= rowCount; row++)
            {
                string CustomUIDValue = worksheet.Cells[row, 11].Text;
                CustomUIDData.Add(CustomUIDValue);
                string UIDValue = CustomUIDValue.Substring(8,14);
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
            for (int i = 0; i < CustomUIDData.Count; i++)
            {
                worksheet.Cells[i + 2, 1].Value = i + 1; 
                worksheet.Cells[i + 2, 2].Value = CustomUIDData[i];
                worksheet.Cells[i + 2, 3].Value = UIDData[i];
            }

            // 保存文件到桌面
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string fileName = $"{excelFileName}.xlsx";
            string filePath = Path.Combine(desktopPath, fileName);
            package.SaveAs(new FileInfo(filePath));
            
        }
        Message.ShowSnack();
    }
}