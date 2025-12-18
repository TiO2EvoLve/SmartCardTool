using System.Globalization;

namespace WindowUI.Sort;

public class 南通地铁
{
    //时间要和卡上印刷的一样
    public static void Run(MemoryStream ExcelData, string excelFileName)
    {
        //先处理Excel文件
        List<string> TimeData = new List<string>();
        List<string> UID = new List<string>();

        // var sql = "SELECT UID_16_ FROM kahao order by SerialNum Asc";
        // UID = Mdb.Select(FilePath, sql);
        // sql = "SELECT 其他1 FROM kahao order by SerialNum Asc";
        // TimeData = Mdb.Select(FilePath, sql);
        
        using (var package = new ExcelPackage(ExcelData))
        {
            var worksheet = package.Workbook.Worksheets[0]; // 获取第一个工作表
            var rowCount = worksheet.Dimension.Rows; //获取行数
            //遍历Excel文件的每一行
            for (var row = 1; row <= rowCount; row++)
            {
                var UidValue = worksheet.Cells[row, 2].Text;
                var TimeValue = worksheet.Cells[row, 7].Text;
                UID.Add(UidValue);
                TimeData.Add(TimeValue);
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
            // 插入数据
            for (var i = 0; i < UID.Count; i++)
            {
                worksheet.Cells[i + 1, 1].Value = UID[i];
                var parsedDate = DateTime.ParseExact(TimeData[i], "yyyy/M/d H:mm:ss", CultureInfo.InvariantCulture);
                worksheet.Cells[i + 1, 2].Value = $"HG{parsedDate.ToString("yyyyMMdd")}{UID[i]}";
                
            }

            package.SaveAs(new FileInfo(filePath));
        }
        Message.ShowSnack();
    }
}