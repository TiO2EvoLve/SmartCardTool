using System.Globalization;

namespace WindowUI.Sort;

public class 南通地铁
{
    public static void Run(string FilePath, string excelFileName)
    {
        //先处理Excel文件
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // 避免出现许可证错误
        List<string> timeData = new List<string>();
        List<string> uidData = new List<string>();

        var sql = "SELECT UID_16_ FROM kahao order by SerialNum Asc";
        uidData = Mdb.Select(FilePath, sql);
        sql = "SELECT 其他1 FROM kahao order by SerialNum Asc";
        timeData = Mdb.Select(FilePath, sql);

        // 保存文件到桌面
        var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        var fileName = $"{excelFileName}.xlsx";
        var filePath = Path.Combine(desktopPath, fileName);
        // 创建一个新的Excel文件
        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add(excelFileName);
            // 插入数据
            for (var i = 0; i < uidData.Count; i++)
            {
                worksheet.Cells[i + 1, 1].Value = uidData[i];
                var parsedDate = DateTime.ParseExact(timeData[i], "yyyy/M/d H:mm:ss", CultureInfo.InvariantCulture);
                timeData[i] = $"HG{parsedDate.ToString("yyyyMMdd")}{uidData[i]}";
                worksheet.Cells[i + 1, 2].Value = timeData[i];
            }

            package.SaveAs(new FileInfo(filePath));
        }

        Message.ShowSnack();
    }
}