namespace WindowUI.Sort;

public class 昆明轨道
{
      public static void Run(string FilePath, string excelFileName)
    {
        // 取出Excel文件的数据
        
        List<string> SNData = new List<string>();
        List<string> UID_16_ = new List<string>();
        
        string sql = "select 打码特殊算法 from kahao order by SerialNum ASC";
        SNData = Mdb.Select(FilePath, sql);
        sql = "select UID_16_ from kahao order by SerialNum ASC ";
        UID_16_ = Mdb.Select(FilePath, sql);

        // 保存文件到桌面
        var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        var fileName = $"{excelFileName}.xlsx";
        var filePath = Path.Combine(desktopPath, fileName);
        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add(excelFileName);
            worksheet.Cells[1, 1].Value = "卡面号";
            worksheet.Cells[1, 2].Value = "UID_16_";
            for (var i = 0; i < SNData.Count; i++)
            {
                worksheet.Cells[i + 2, 1].Value = SNData[i];
                worksheet.Cells[i + 2, 2].Value = UID_16_[i];
            }

            package.SaveAs(new FileInfo(filePath));
        }

        Message.ShowSnack();
    }
}