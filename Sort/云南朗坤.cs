namespace WindowUI.Sort;

public class 云南朗坤
{
    public static void Run(MemoryStream ExcelData, string FileName)
    {
        // 取出Excel文件的数据
        
        List<string> SNData = new List<string>();
        List<string> UID16 = new List<string>();
        
        // var sql = "SELECT SerialNum FROM kahao order by SerialNum ASC";
        // SNData = Mdb.Select(FilePath, sql);
        // sql = "SELECT UID_16_ FROM kahao order by SerialNum ASC";
        // UID16 = Mdb.Select(FilePath, sql);
        

        using (var package = new ExcelPackage(ExcelData))
        {
            var worksheet = package.Workbook.Worksheets[0]; // 获取第一个工作表
            var rowCount = worksheet.Dimension.Rows; // 获取行数
        
            // 遍历Excel文件的每一行
            for (var row = 1; row <= rowCount; row++)
            {
                var SNValue = worksheet.Cells[row, 6].Text;
                var Uid16Value = worksheet.Cells[row, 2].Text;
                SNData.Add(SNValue);
                UID16.Add(Uid16Value);
            }
        }

        // 创建一个新的Excel文件
        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add(FileName);
            worksheet.Cells[1, 1].Value = "卡号";
            worksheet.Cells[1, 2].Value = "16进制";
            worksheet.Cells[1, 3].Value = "16进制调整";
            worksheet.Cells[1, 4].Value = "10进制";
            worksheet.Cells[1, 5].Value = "10进制调整";
            for (var i = 0; i < SNData.Count; i++)
            {
                string UID16_ = Tools.ChangeHexPairs(UID16[i]);
                worksheet.Cells[i + 2, 1].Value = SNData[i];
                worksheet.Cells[i + 2, 2].Value = UID16[i];
                worksheet.Cells[i + 2, 3].Value = UID16_;
                worksheet.Cells[i + 2, 4].Value = Tools.ChangeDecimalSystem(UID16[i]);
                worksheet.Cells[i + 2, 5].Value = Tools.ChangeDecimalSystem(UID16_);
            }

            // 保存文件到桌面
            var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var fileName = $"{FileName}.xlsx";
            var filePath = Path.Combine(desktopPath, fileName);
            package.SaveAs(new FileInfo(filePath));
            // 显示提示消息
            Message.ShowSnack();
        }
    }
}