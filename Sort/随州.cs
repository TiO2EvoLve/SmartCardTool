namespace WindowUI.Sort;

public class 随州
{
    public static void Run(string FilePath, string excelFileName)
    {
        // 取出Excel文件的数据
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // 避免出现许可证错误
        List<string> SNData = new List<string>();
        List<string> UidData = new List<string>();
        
        string sql = "SELECT outsidelasercode From print order by outsidelasercode ASC";
        SNData = Mdb.Select(FilePath, sql);
        sql = "SELECT insidecode From print order by outsidelasercode ASC";
        UidData = Mdb.Select(FilePath, sql);

        // 保存文件到桌面
        var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        var fileName = $"{excelFileName}.xlsx";
        var filePath = Path.Combine(desktopPath, fileName);
        // 创建一个新的Excel文件
        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add(excelFileName);

            for (var i = 0; i < UidData.Count; i++)
            {
                worksheet.Cells[i + 1, 1].Value = SNData[i];
                worksheet.Cells[i + 1, 2].Value = Convert.ToUInt32(UidData[i], 16);
            }
            package.SaveAs(new FileInfo(filePath));
            
        }
        // 显示提示消息
        Message.ShowSnack();
    }
}