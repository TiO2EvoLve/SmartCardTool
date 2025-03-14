namespace WindowUI.Sort;

public class 新开普
{
    public static void Run(string FilePath, string FileName)
    {
        List<string> SNData = new List<string>();
        List<string> UidData = new List<string>();


        var sql = "SELECT SerialNum FROM kahao order by SerialNum ASC";
        SNData = Mdb.Select(FilePath, sql);
        sql = "SELECT UID_16_ FROM kahao order by SerialNum ASC";
        UidData = Mdb.Select(FilePath, sql);


        // 创建一个新的Excel文件
        var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        var fileName = $"{FileName}.xlsx";
        var filePath = Path.Combine(desktopPath, fileName);

        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add(FileName);
            // 插入数据
            worksheet.Cells[1, 1].Value = "SerialNumber";
            worksheet.Cells[1, 2].Value = "UID16";
            for (var i = 0; i < SNData.Count; i++)
            {
                worksheet.Cells[i + 2, 1].Value = SNData[i];
                worksheet.Cells[i + 2, 2].Value = UidData[i];
            }
            // 保存文件到桌面
            package.SaveAs(new FileInfo(filePath));
        }

        Message.ShowSnack();
    }
}