namespace WindowUI.Sort;

public class 穆棱
{
    public static void Run(string FilePath, string excelFileName)
    {
        //先处理Excel文件
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // 避免出现许可证错误
        List<string> SnData = new List<string>();
        List<string> UidData = new List<string>();
        List<string> Uid10Data = new List<string>();

        var sql = "SELECT SerialNum FROM kahao order by SerialNum Asc";
        SnData = Mdb.Select(FilePath, sql);
        sql = "SELECT UID_16_ FROM kahao order by SerialNum Asc";
        UidData = Mdb.Select(FilePath, sql);
        sql = "SELECT UID_10 FROM kahao order by SerialNum Asc";
        Uid10Data = Mdb.Select(FilePath, sql);


        // 保存文件到桌面
        var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        var fileName = $"{excelFileName}.xlsx";
        var filePath = Path.Combine(desktopPath, fileName);
        // 创建一个新的Excel文件
        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add(excelFileName);
            // 插入数据
            worksheet.Cells[1, 1].Value = "流水号";
            worksheet.Cells[1, 2].Value = "芯片号";
            worksheet.Cells[1, 3].Value = "卡面打码芯片号";
            for (var i = 0; i < SnData.Count; i++)
            {
                worksheet.Cells[i + 2, 1].Value = SnData[i];
                worksheet.Cells[i + 2, 2].Value = UidData[i];
                worksheet.Cells[i + 2, 3].Value = Uid10Data[i];
            }

            package.SaveAs(new FileInfo(filePath));
        }

        Message.ShowSnack();
    }
}