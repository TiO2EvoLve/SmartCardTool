namespace WindowUI.Sort;

public class 山西医科大学
{
    public static void Run(MemoryStream ExcelData, string FileName)
    {
        
        List<string> SNData = new List<string>();
        List<string> UidData = new List<string>();
        using (var package = new ExcelPackage(ExcelData))
        {
            var worksheet = package.Workbook.Worksheets[0]; // 获取第一个工作表
            var rowCount = worksheet.Dimension.Rows; // 获取行数
            // 遍历Excel文件的每一行
            for (var row = 1; row <= rowCount; row++)
            {
                var SNValue = worksheet.Cells[row, 8].Text;
                var UidValue = worksheet.Cells[row, 2].Text;
                UidValue = Convert.ToUInt32(UidValue, 16).ToString();
                SNData.Add(SNValue);
                UidData.Add(UidValue);
            }
        }
        // string Sql = "SELECT SerialNum FROM kahao order by SerialNum ASC";
        // SNData = Mdb.Select(FilePath, Sql);
        // Sql = "SELECT UID_10_ FROM kahao order by SerialNum ASC";
        // UidData = Mdb.Select(FilePath, Sql);
        
        
        
        //新建一个Excel文件
        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add(FileName);
            for (var i = 0; i < UidData.Count; i++)
            {
                worksheet.Cells[i + 1, 1].Value = SNData[i];
                worksheet.Cells[i + 1, 2].Value = UidData[i];
            }

            // 保存文件到桌面
            var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var fileName = $"{FileName}.xlsx";
            var filePath = Path.Combine(desktopPath, fileName);
            package.SaveAs(new FileInfo(filePath));
            
        }
        Message.ShowSnack();
        
    }
}