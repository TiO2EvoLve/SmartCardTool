using OfficeOpenXml.Style;

namespace WindowUI.Sort;

//Excel格式，10进制调整 、NO.卡号
public class 射洪公交
{
    public static void Run(MemoryStream ExcelData, string FileName)
    {
        
        List<string> SNData = new List<string>();
        List<string> UidData = new List<string>();
        using (var package = new ExcelPackage(ExcelData))
        {
            var worksheet = package.Workbook.Worksheets[0]; // 获取第一个工作表
            var rowCount = worksheet.Dimension.Rows; // 获取行数
            // 遍历Excel文件的每一行
            for (var row = 1; row <= rowCount; row++)
            {
                var SNValue = worksheet.Cells[row, 9].Text;
                var UidValue = worksheet.Cells[row, 3].Text;
                SNData.Add(SNValue);
                UidData.Add(UidValue);
            }
        }
        //新建一个Excel文件
        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add(FileName);
            for (var i = 0; i < UidData.Count; i++)
            {
                worksheet.Cells[i + 1, 1].Value = Tools.ChangeDecimalSystem(UidData[i]);
                worksheet.Cells[i + 1, 2].Value = "NO." + SNData[i];
            }
            //自动适应、居中
            worksheet.Cells.AutoFitColumns();
            worksheet.Cells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            // 保存文件到桌面
            var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var fileName = $"{FileName}.xlsx";
            var filePath = Path.Combine(desktopPath, fileName);
            package.SaveAs(new FileInfo(filePath));
        }
        Message.ShowSnack();
        
    }
}