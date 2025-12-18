namespace WindowUI.Sort;

public class 沈阳地铁
{
    public static void Run(string FilePath, string excelFileName)
    {
        // 第一个rcc文件，excel格式
        List<string> SN = new List<string>();
        List<string> UID = new List<string>();
       
        string sql = "select SerialNum from kahao order by SerialNum ASC";
        SN = Mdb.Select(FilePath, sql);
        sql = "select 打码特殊算法 from kahao order by SerialNum ASC ";
        UID = Mdb.Select(FilePath, sql);

        //新建一个Excel文件
        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add(excelFileName);
            for (var i = 0; i < UID.Count; i++)
            {
                worksheet.Cells[i + 1, 1].Value = SN[i][^9..];
                worksheet.Cells[i + 1, 2].Value = UID[i];
            }

            // 保存文件到桌面
            var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var fileName = $"{excelFileName}.xlsx";
            var filePath = Path.Combine(desktopPath, fileName);
            package.SaveAs(new FileInfo(filePath));
        }

        // 第二个rcc文件，txt格式
        var desktopPath1 = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        var fileName1 = $"{excelFileName}.txt";
        var filePath1 = Path.Combine(desktopPath1, fileName1);

        using (var writer = new StreamWriter(filePath1))
        {
            for (var i = 0; i < SN.Count; i++)
                if (i == SN.Count - 1)
                    writer.Write($"{SN[i][^9..]} {UID[i]}");
                else
                    writer.WriteLine($"{SN[i][^9..]} {UID[i]}");
        }

        Message.ShowSnack();
    }
}