
using OfficeOpenXml.Style;

namespace WindowUI.Sort;

public class 三门峡
{
    public static void Run(string FilePath, string excelFileName)
    {
        三门峡菜单 menu= new();
        menu.ShowDialog();
        var cardtype = menu.CardType;
        List<string> SN = new List<string>();
        var UID = new List<string>();

        var sql = "select SerialNum from kahao order by SerialNum ASC";
        SN = Mdb.Select(FilePath, sql);

        sql = "select UID_10 from kahao order by SerialNum ASC ";
        UID = Mdb.Select(FilePath, sql);
        
        // 保存文件到桌面
        var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        var fileName = $"{excelFileName}.xlsx";
        var filePath = Path.Combine(desktopPath, fileName);
        
        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add(excelFileName);
            worksheet.Cells[1, 1].Value = "卡片芯号";
            worksheet.Cells[1, 2].Value = "卡号(16位)";
            worksheet.Cells[1, 3].Value = "卡商标志";
            
            for (var i = 0; i < UID.Count; i++)
            {
                worksheet.Cells[i + 2, 1].Value = UID[i];
                worksheet.Cells[i + 2, 2].Value = cardtype + SN[i];
                worksheet.Cells[i + 2, 3].Value = "8670";
            }
            worksheet.Cells.AutoFitColumns();
            worksheet.Cells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            
            package.SaveAs(new FileInfo(filePath));
        }
        Message.ShowSnack();
    }
}