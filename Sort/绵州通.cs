namespace WindowUI.Sort;

public class 绵州通
{
    public static void Run(string FilePath, string FileName)
    {
        List<string> SN = new List<string>();
        List<string> UID = new List<string>();
        
        string sql = "select SerialNum from kahao order by SerialNum ASC";
        SN = Mdb.Select(FilePath, sql);
        sql = "select UID_16_ from kahao order by SerialNum ASC ";
        UID = Mdb.Select(FilePath, sql);

        // 创建一个新的Excel文件
        var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        //获取当前日期
        var currentDate = DateTime.Now.ToString("yyyyMMdd");
        var fileName = $"绵州通华冠-数据准备文件-{currentDate}.xlsx";
        var filePath = Path.Combine(desktopPath, fileName);

        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add(FileName);
            // 插入数据
            worksheet.Cells[1, 1].Value = "卡号";
            worksheet.Cells[1, 2].Value = "物理卡号";
            worksheet.Cells[1, 3].Value = "卡商名称";
            worksheet.Cells[1, 4].Value = "卡商代码";
            for (var i = 0; i < UID.Count; i++)
            {
                worksheet.Cells[i + 2, 1].Value = SN[i];
                worksheet.Cells[i + 2, 2].Value = UID[i];
                worksheet.Cells[i + 2, 3].Value = "华冠";
                worksheet.Cells[i + 2, 4].Value = "8670";
            }

            // 保存文件到桌面
            package.SaveAs(new FileInfo(filePath));
        }

        Message.ShowSnack();
    }
}