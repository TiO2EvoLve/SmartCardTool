using OfficeOpenXml.Style;

namespace WindowUI.Sort;

public class 成都锐视_中科
{
    public static void Run(MemoryStream ExcelData, string excelFileName)
    {
        //取出Excle文件的数据
        
        List<string> UID = new List<string>();
        List<string> SN = new List<string>();
       
        
        using (var package = new ExcelPackage(ExcelData))
        {
            var worksheet = package.Workbook.Worksheets[0]; // 获取第一个工作表
            var rowCount = worksheet.Dimension.Rows; //获取行数
            //遍历Excel文件的每一行
            for (var row = 1; row <= rowCount; row++)
            {
                var UIDValue = worksheet.Cells[row, 2].Text;
                UIDValue = Tools.ChangeHexPairs(UIDValue);
                UID.Add(UIDValue);
                var SNValue = worksheet.Cells[row, 8].Text;
                SN.Add(SNValue);
            }
        }
        var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        var fileName = $"{excelFileName}.xlsx";
        var filePath = Path.Combine(desktopPath, fileName);

        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add(excelFileName);
            for (var i = 0; i < UID.Count; i++)
            {
                worksheet.Cells[i + 1, 1].Value = Tools.ChangeDecimalSystem(UID[i]);
                worksheet.Cells[i + 1, 2].Value = $"NO.{SN[i]}";
            }
            //自动适应、居中
            worksheet.Cells.AutoFitColumns();
            worksheet.Cells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            // 保存文件到桌面
            package.SaveAs(new FileInfo(filePath));
        }

        Message.ShowSnack();
    }
}