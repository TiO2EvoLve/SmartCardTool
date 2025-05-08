namespace WindowUI.Sort;

public class 新开普
{
    public static void Run(string FilePath,MemoryStream ExcelData, string FileName)
    {
        List<string> SNData = new List<string>();
        List<string> UidData = new List<string>();


        // var sql = "SELECT SerialNum FROM kahao order by SerialNum ASC";
        // SNData = Mdb.Select(FilePath, sql);
        // sql = "SELECT UID_16_ FROM kahao order by SerialNum ASC";
        // UidData = Mdb.Select(FilePath, sql);

        using (var package = new ExcelPackage(ExcelData))
        {
            var worksheet = package.Workbook.Worksheets[0]; // 获取第一个工作表
            var rowCount = worksheet.Dimension.Rows; // 获取行数

            // 遍历Excel文件的每一行
            for (var row = 1; row <= rowCount; row++)
            {
                var SNValue = worksheet.Cells[row, 7].Text;
                var Uid16Value = worksheet.Cells[row, 2].Text;
                SNData.Add(SNValue);
                UidData.Add(Uid16Value);
            }
        }

        // 创建一个新的Excel文件
        var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        var fileName = $"{FileName}.xlsx";
        var filePath = Path.Combine(desktopPath, fileName);

        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add(FileName);
            // 插入数据
            worksheet.Cells[1, 1].Value = "SerialNumber";
            worksheet.Cells[1, 2].Value = "UID";
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