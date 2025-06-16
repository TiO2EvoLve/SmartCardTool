namespace WindowUI.Sort;

public class 牡丹江
{
    public static void Run(string FilePath, string excelFileName)
    {
        
        List<string> SnData = new List<string>();
        var Uid16Data = new List<string>();
        var Uid10Data = new List<string>();
        

        var sql = "select SerialNum from kahao order by SerialNum ASC";
        SnData = Mdb.Select(FilePath, sql);
        sql = "select UID_10 from kahao order by SerialNum ASC ";
        Uid10Data = Mdb.Select(FilePath, sql);
        sql = "select UID_16_ from kahao order by SerialNum ASC ";
        Uid16Data = Mdb.Select(FilePath, sql);

        // 创建一个新的Excel文件
        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add(excelFileName);
            // 插入数据
            worksheet.Cells[1, 1].Value = "流水号";
            worksheet.Cells[1, 2].Value = "芯片号";
            worksheet.Cells[1, 3].Value = "卡片打码芯片号";
            for (var i = 0; i < Uid10Data.Count; i++)
            {
                worksheet.Cells[i + 2, 1].Value = SnData[i];
                worksheet.Cells[i + 2, 2].Value = Uid16Data[i];
                worksheet.Cells[i + 2, 3].Value = Uid10Data[i];
            }

            // 保存文件到桌面
            var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var filePath = Path.Combine(desktopPath, $"{excelFileName}.xlsx");
            package.SaveAs(new FileInfo(filePath));
            Message.ShowSnack();
        }
    }
}