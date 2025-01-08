using WindowUI.Tool;

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
            int rowCount = worksheet.Dimension.Rows; // 获取行数

            // 异步遍历Excel文件的每一行
            for (int row = 1; row <= rowCount; row++)
            {
                string SNValue = worksheet.Cells[row, 6].Text;
                string UidValue = worksheet.Cells[row, 2].Text;
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
            for (int i = 0; i < UidData.Count; i++)
            {
                worksheet.Cells[i + 2, 1].Value = SNData[i];
                worksheet.Cells[i + 2, 2].Value = UidData[i];
                worksheet.Cells[i + 2, 3].Value = Tools.ChangeHexPairs(UidData[i]);
            }

            // 异步保存文件到桌面
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string fileName = $"{excelFileName}.xlsx";
            string filePath = Path.Combine(desktopPath, fileName);
            // 使用异步的文件保存
            package.SaveAs(new FileInfo(filePath));
            // 显示提示消息
            MessageBox.Show($"数据已处理并保存到桌面{filePath}");
        }
    }
}