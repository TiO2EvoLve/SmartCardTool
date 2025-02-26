namespace WindowUI.Sort;

public class 盐城
{
    public static void Run(string FilePath, string FileName)
    {
        
        List<string> SNData = new List<string>();
        List<string> UidData = new List<string>();
        
        string sql = "SELECT SerialNum FROM kahao order by SerialNum ASC";
        SNData = Mdb.Select(FilePath, sql);
        
        sql = "SELECT UID_16_ FROM kahao order by SerialNum ASC";
        UidData = Mdb.Select(FilePath, sql);

        
        // 创建一个新的Excel文件
        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add(FileName);
            worksheet.Cells[1, 1].Value = "SN";
            worksheet.Cells[1, 2].Value = "UID";
            worksheet.Cells[1, 3].Value = "厂家";
            
            for (int i = 0; i < UidData.Count; i++)
            {
                worksheet.Cells[i + 2, 1].Value = SNData[i];
                worksheet.Cells[i + 2, 2].Value = UidData[i];
                worksheet.Cells[i + 2, 3].Value = "山东华冠智能卡";
            }
            // 保存文件到桌面
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string filePath = Path.Combine(desktopPath, $"{FileName}.xlsx");
            package.SaveAs(new FileInfo(filePath));
            // 显示提示消息
            Message.ShowSnack();
        }    
    }
}