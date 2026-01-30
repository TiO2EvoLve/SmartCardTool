namespace WindowUI.Sort;

public class 随州
{
    public static void Run(string FilePath,MemoryStream ExcelData, string excelFileName)
    {
        // 取出Excel文件的数据
        List<string> SNData = new List<string>();
        List<string> UidData = new List<string>();
        
        // string sql = "SELECT SerialNum From kahao order by SerialNum ASC";
        // SNData = Mdb.Select(FilePath, sql);
        // sql = "SELECT UID_10 From kahao order by SerialNum ASC";
        // UidData = Mdb.Select(FilePath, sql);
        using (var package = new ExcelPackage(ExcelData))
        {
            var worksheet = package.Workbook.Worksheets[0]; // 获取第一个工作表
            var rowCount = worksheet.Dimension.Rows; //获取行数
            //遍历Excel文件的每一行
            for (var row = 1; row <= rowCount; row++)
            {
                var UIDValue = worksheet.Cells[row, 2].Text;
                UidData.Add(UIDValue);
                var SNValue = worksheet.Cells[row, 6].Text;
                SNData.Add(SNValue);
            }
        }

        // 保存文件到桌面
        var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        var fileName = $"{excelFileName}.xlsx";
        var filePath = Path.Combine(desktopPath, fileName);
        // 创建一个新的Excel文件
        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add(excelFileName);
            for (var i = 0; i < UidData.Count; i++)
            {
                worksheet.Cells[i + 1, 1].Value = SNData[i];
                worksheet.Cells[i + 1, 2].Value =Tools.ChangeDecimalSystem(UidData[i]);
            }
            package.SaveAs(new FileInfo(filePath));
        }
        // 显示提示消息
        Message.ShowSnack();
    }
}