namespace WindowUI.Sort;

public class 抚顺
{
    public static void Run(string FilePath,MemoryStream ExcelData, string excelFileName)
    {
        List<string> SnData = new ();
        List<string> UidData = new ();
        
        //获取文件的类型
        string fileExtension = Path.GetExtension(FilePath).ToLower();
        if (fileExtension == ".mdb")
        {
            MdbExcute(FilePath);
        }else if(fileExtension == ".xlsx")
        {
            ExcelExcute(ExcelData);
        }else 
        {
            Message.ShowMessageBox("错误", "不支持的文件类型");
        }
        
        void MdbExcute(string FilePath)
        {
            string sql = "select SerialNum from kahao order by SerialNum ASC";
            SnData = Mdb.Select(FilePath, sql);
            sql = "select UID_16 from kahao order by SerialNum ASC ";
            UidData = Mdb.Select(FilePath, sql); 
        }

        void ExcelExcute(MemoryStream ExcelData)
        {
            using (var package = new ExcelPackage(ExcelData))
            {
                var worksheet = package.Workbook.Worksheets[0]; // 获取第一个工作表
                var rowCount = worksheet.Dimension.Rows; // 获取行数

                // 遍历Excel文件的每一行
                for (var row = 1; row <= rowCount; row++)
                {
                    var SNValue = worksheet.Cells[row, 7].Text;
                    var Uid16Value = worksheet.Cells[row, 3].Text;
                    SnData.Add(SNValue);
                    UidData.Add(Uid16Value);
                }
            }
        }
        
        
        //保存为txt文件
        var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        var fileName = $"{excelFileName}.txt";
        var filePath = Path.Combine(desktopPath, fileName);
        using (var writer = new StreamWriter(filePath))
        {
            for (var i = 0; i < SnData.Count; i++)
                if (i == SnData.Count - 1)
                    writer.Write($"{SnData[i]} {Tools.ChangeDecimalSystem(UidData[i])}");
                else
                    writer.WriteLine($"{SnData[i]} {Tools.ChangeDecimalSystem(UidData[i])}");
        }

        Message.ShowSnack();
    }

    
}