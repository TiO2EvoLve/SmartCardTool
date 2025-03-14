namespace WindowUI.Sort;

public class 滨州公交
{
    public static void Run(string FilePath, string excelFileName)
    {
        List<string> SNData = new List<string>();
        List<string> UidData = new List<string>();

        var sql = "select SerialNum from kahao order by SerialNum ASC";
        SNData = Mdb.Select(FilePath, sql);
        sql = "select UID_10_ from kahao order by SerialNum ASC";
        UidData = Mdb.Select(FilePath, sql);

        // 创建一个新的Excel文件
        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add(excelFileName);
            for (var i = 0; i < UidData.Count; i++)
            {
                worksheet.Cells[i + 1, 1].Value = SNData[i];
                worksheet.Cells[i + 1, 2].Value = UidData[i];
            }

            // 保存文件到桌面
            var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var filePath = Path.Combine(desktopPath, $"{excelFileName}.xlsx");
            package.SaveAs(new FileInfo(filePath));
        }

        Message.ShowSnack();
    }
}