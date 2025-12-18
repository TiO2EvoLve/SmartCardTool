namespace WindowUI.Sort;

public class 滨州公交
{
    public static void Run(string FilePath, string excelFileName)
    {
        List<string> SNData = new List<string>();
        List<string> UidData = new List<string>();

        var sql = "select SerialNum from kahao order by SerialNum ASC";
        SNData = Mdb.Select(FilePath, sql);
        sql = "select UID_10 from kahao order by SerialNum ASC";
        UidData = Mdb.Select(FilePath, sql);
        
        // using (var package = new ExcelPackage(ExcelData))
        // {
        //     var worksheet = package.Workbook.Worksheets[0]; // 获取第一个工作表
        //     var rowCount = worksheet.Dimension.Rows; // 获取行数
        //     // 遍历Excel文件的每一行
        //     for (var row = 1; row <= rowCount; row++)
        //     {
        //         var SNValue = worksheet.Cells[row, 7].Text;
        //         var UIDValue = worksheet.Cells[row, 2].Text;
        //         SNData.Add(SNValue);
        //         UidData.Add(UIDValue);
        //     }
        // }

        // 创建一个新的Excel文件
        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add(excelFileName);
            worksheet.Cells[1, 1].Value = "SerialNum";
            worksheet.Cells[1, 2].Value = "UID_10";
            for (var i = 0; i < UidData.Count; i++)
            {
                worksheet.Cells[i + 2, 1].Value = SNData[i];
                worksheet.Cells[i + 2, 2].Value = UidData[i];
            }

            // 保存文件到桌面
            var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var filePath = Path.Combine(desktopPath, $"{excelFileName}.xlsx");
            package.SaveAs(new FileInfo(filePath));
        }

        Message.ShowSnack();
    }
}