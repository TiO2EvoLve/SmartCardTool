namespace WindowUI.Sort;

public class 盱眙
{
    public static void Run(string FilePath, string FileName)
    {
        // 取出Excel文件的数据
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // 避免出现许可证错误
        List<string> SNData = new List<string>();
        List<string> UidData = new List<string>();
        List<string> CustomUidData = new List<string>();
        
        string sql = "SELECT SerialNum From kahao order by SerialNum ASC";
        SNData = Mdb.Select(FilePath, sql);
        sql = "SELECT UID_16_ From kahao order by SerialNum ASC";
        UidData = Mdb.Select(FilePath, sql);
        sql = "SELECT UID_16 From kahao order by SerialNum ASC";
        CustomUidData = Mdb.Select(FilePath, sql);
       

        //保存文件到桌面
        var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        var fileName = $"{FileName}.xlsx";
        var filePath = Path.Combine(desktopPath, fileName);
        // 创建一个新的Excel文件
        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add(FileName);
            worksheet.Cells[1, 1].Value = "SerialNumber";
            worksheet.Cells[1, 2].Value = "UID";
            worksheet.Cells[1, 3].Value = "CUSTOMUID";
            for (var i = 0; i < UidData.Count; i++)
            {
                worksheet.Cells[i + 2, 1].Value = SNData[i];
                worksheet.Cells[i + 2, 2].Value = UidData[i];
                worksheet.Cells[i + 2, 3].Value = CustomUidData[i];
            }
            package.SaveAs(new FileInfo(filePath));
           
        }
        Message.ShowSnack();
    }
}