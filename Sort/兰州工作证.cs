// ReSharper disable All
namespace WindowUI.Sort;


//格式说明
//文件名：兰州工作证.txt
//内容格式：
//3110102501019537,4071323218
//3110102501019538,4071323314
//3110102501019539,4071323410
public class 兰州工作证
{
    public static void Run(MemoryStream ExcelData,string FilePath, string excelFileName)
    {
        List<string> SNData = new List<string>();
        List<string> UidData = new List<string>();
        
        //Mdb处理
        // string sql = "SELECT NUM From kahao order by NUM ASC";
        // SNData = Mdb.Select(FilePath, sql);
        // sql = "SELECT UID10_ From kahao order by NUM ASC";
        // UidData = Mdb.Select(FilePath, sql);

        //Excel处理
        using (var package = new ExcelPackage(ExcelData))
        {
            var worksheet = package.Workbook.Worksheets[0]; // 获取第一个工作表
            var rowCount = worksheet.Dimension.Rows; //获取行数
            //遍历Excel文件的每一行
            for (var row = 1; row <= rowCount; row++)
            {
                var SNValue = worksheet.Cells[row, 2].Text;
                var UidValue = worksheet.Cells[row, 4].Text;
                SNData.Add(SNValue);
                UidData.Add(UidValue);
            }
        }
        var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        var fileName = excelFileName + ".txt";
        var filePath = Path.Combine(desktopPath, fileName);
        using (var writer = new StreamWriter(filePath))
        {
            for (var i = 0; i < SNData.Count; i++)
                if (i == SNData.Count - 1)
                    writer.Write($"{SNData[i]},{Tools.ChangeDecimalSystem(UidData[i])}");
                else
                    writer.WriteLine($"{SNData[i]},{Tools.ChangeDecimalSystem(UidData[i])}");
        }
        Message.ShowSnack();
    }
    
}