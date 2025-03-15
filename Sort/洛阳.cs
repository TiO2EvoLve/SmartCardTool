namespace WindowUI.Sort;

public class 洛阳
{
    public static void Run(string FilePath, string excelFileName)
    {
        //先处理Excel文件
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // 避免出现许可证错误
        List<string> SNData = new List<string>();
        List<string> UidData = new List<string>();

        var sql = "SELECT 打码特殊算法 FROM kahao order by SerialNum Asc";
        SNData = Mdb.Select(FilePath, sql);
        sql = "SELECT UID_16_ FROM kahao order by SerialNum Asc";
        UidData = Mdb.Select(FilePath, sql);

        // 保存文件到桌面
        var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        var fileName = $"{excelFileName}.xlsx";
        var filePath = Path.Combine(desktopPath, fileName);
        // 创建一个新的Excel文件
        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add(excelFileName);
            // 插入数据
            worksheet.Cells[1, 1].Value = "卡号";
            worksheet.Cells[1, 2].Value = "物理卡号";
            worksheet.Cells[1, 3].Value = "卡商名称";
            worksheet.Cells[1, 4].Value = "卡商代码";
            for (var i = 0; i < UidData.Count; i++)
            {
                worksheet.Cells[i + 2, 1].Value = SNData[i];
                worksheet.Cells[i + 2, 2].Value = UidData[i];
                worksheet.Cells[i + 2, 3].Value = "山东华冠智能卡";
                worksheet.Cells[i + 2, 4].Value = "8670";
            }
            package.SaveAs(new FileInfo(filePath));
        }

        Message.ShowSnack();
    }
}