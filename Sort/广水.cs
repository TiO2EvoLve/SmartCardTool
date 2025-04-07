namespace WindowUI.Sort;

public class 广水
{
    public static void Run(string FilePath, string excelFileName)
    {
        //先处理Excel文件
        
        List<string> SN = new List<string>();
        List<string> UID = new List<string>();

        var sql = "SELECT SerialNum FROM kahao order by SerialNum Asc";
        SN = Mdb.Select(FilePath, sql);
        sql = "SELECT UID_10 FROM kahao order by SerialNum Asc";
        UID = Mdb.Select(FilePath, sql);

        // 保存文件到桌面
        var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        var fileName = $"{excelFileName}.xlsx";
        var filePath = Path.Combine(desktopPath, fileName);
        // 创建一个新的Excel文件
        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add(excelFileName);

            worksheet.Cells[1, 1].Value = "SerialNumber";
            worksheet.Cells[1, 2].Value = "UID";
            // 插入数据
            for (var i = 0; i < UID.Count; i++)
            {
                worksheet.Cells[i + 2, 1].Value = SN[i];
                worksheet.Cells[i + 2, 2].Value = UID[i];
            }

            package.SaveAs(new FileInfo(filePath));
        }

        Message.ShowSnack();
    }
}