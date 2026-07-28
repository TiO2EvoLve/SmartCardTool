namespace WindowUI.Sort;

public class 昆明
{
    public static void Run(string FilePath)
    {

        //可能得改成制卡数据首位文件名
        long ZhikaFileName = 8684250113000001;
        // 取出Excel文件的数据
        
        List<string> SN = [];
        List<string> SN16 = [];
        List<string> UID16 = [];
        List<string> UID10 = [];
        // using (var package = new ExcelPackage(ExcelData))
        // {
        //     var worksheet = package.Workbook.Worksheets[0]; // 获取第一个工作表
        //     var rowCount = worksheet.Dimension.Rows; // 获取行数
        //     // 遍历Excel文件的每一行
        //     for (var row = 2; row <= rowCount; row++)
        //     {
        //         var SNValue = worksheet.Cells[row, 1].Text;
        //         var SN16Value = worksheet.Cells[row, 10].Text;
        //         var Uid16Value = worksheet.Cells[row, 4].Text;
        //         var Uid10Value = worksheet.Cells[row, 6].Text;
        //         SNData.Add(SNValue);
        //         SN16Data.Add(SN16Value);
        //         Uid16Data.Add(Uid16Value);
        //         Uid10Data.Add(Uid10Value);
        //     }
        // }
        var sql = "SELECT SerialNum FROM kahao order by SerialNum ASC";
        SN = Mdb.Select(FilePath, sql);
        sql = "SELECT UID_16 FROM kahao order by SerialNum ASC";
        UID16 = Mdb.Select(FilePath, sql);
        sql = "SELECT UID_10 FROM kahao order by SerialNum ASC";
        UID10 = Mdb.Select(FilePath, sql);
        
        // 保存文件到桌面
        var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        var fileName = "1342680response867020250113.xlsx";
        var filePath = Path.Combine(desktopPath, fileName);
        // 创建一个新的Excel文件
        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add(fileName);

            for (var i = 0; i < UID16.Count; i++)
            {
                worksheet.Cells[i + 1, 1].Value = UID16[i];
                worksheet.Cells[i + 1, 2].Value = UID10[i];
                worksheet.Cells[i + 1, 3].Value = (8684250113000001 + i).ToString();
                worksheet.Cells[i + 1, 4].Value = "8670";
                worksheet.Cells[i + 1, 5].Value = "0" + SN[i];
                worksheet.Cells[i + 1, 6].Value = "ZP18010302";
            }

            package.SaveAs(new FileInfo(filePath));
        }

        var FileName =  Path.GetFileNameWithoutExtension(FilePath);
        fileName = $"{FileName}.xlsx";
        filePath = Path.Combine(desktopPath, fileName);
        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add(FileName);
            worksheet.Cells[1, 1].Value = "卡面号";
            worksheet.Cells[1, 2].Value = "UID_10_";
            for (var i = 0; i < UID16.Count; i++)
            {
                worksheet.Cells[i + 2, 1].Value = SN[i].Substring(3,16);
                worksheet.Cells[i + 2, 2].Value = UID10[i];
            }
            package.SaveAs(new FileInfo(filePath));
        }
        Message.ShowSnack();
    }
}