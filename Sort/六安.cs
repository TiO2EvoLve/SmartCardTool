namespace WindowUI.Sort;

public class 六安
{
    public static void Run(string FilePath, string FileName)
    {
        //先处理Excel文件
        var SN = new List<string>();
        var UID = new List<string>();

        var sql = "SELECT SN FROM RCC order by SN Asc";
        SN = Mdb.Select(FilePath, sql);
        sql = "SELECT UID FROM RCC order by SN Asc";
        UID = Mdb.Select(FilePath, sql);
        
        // 保存文件到桌面
        var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        var fileName = $"{FileName}.xlsx";
        var filePath = Path.Combine(desktopPath, fileName);
        // 创建一个新的Excel文件
        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add(FileName);
            // 插入数据
            worksheet.Cells[1, 1].Value = "卡片应用主账号";
            worksheet.Cells[1, 2].Value = "卡片芯片号";
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