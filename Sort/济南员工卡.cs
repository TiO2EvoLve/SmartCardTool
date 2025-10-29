namespace WindowUI.Sort;

public class 济南员工卡
{
    public static void Run(string FilePath, string FileName)
    {
        List<string> SNData = new List<string>();
        List<string> UidData = new List<string>();
        
            string sql = "SELECT SerialNum FROM kahao order by SerialNum ASC";
        SNData = Mdb.Select(FilePath, sql);
        sql = "SELECT UID_16_ FROM kahao order by SerialNum ASC";
        UidData = Mdb.Select(FilePath, sql);
        // using (var package = new ExcelPackage(ExcelData))
        // {
        //     var worksheet = package.Workbook.Worksheets[0]; // 获取第一个工作表
        //     var rowCount = worksheet.Dimension.Rows; //获取行数
        //     //遍历Excel文件的每一行
        //     for (var row = 1; row <= rowCount; row++)
        //     {
        //         var SNValue = worksheet.Cells[row, 7].Text;
        //         var UidValue = worksheet.Cells[row, 2].Text;
        //         SNData.Add(SNValue);
        //         UidData.Add(UidValue);
        //     }
        // }
        
        //保存为txt文件
        var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        var fileName = $"{FileName}.txt";
        var filePath = Path.Combine(desktopPath, fileName);
        using (var writer = new StreamWriter(filePath))
        {
            for (var i = 0; i < SNData.Count; i++)
                if (i == SNData.Count - 1)
                    writer.Write($"{UidData[i]}|{UidData[i]}|{SNData[i]}");
                else
                    writer.WriteLine($"{UidData[i]}|{UidData[i]}|{SNData[i]}");
        }
        Message.ShowSnack();
    }
}