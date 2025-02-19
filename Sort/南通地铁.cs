
namespace WindowUI.Sort;

public class 南通地铁
{
    public static void Run(MemoryStream ExcelData,string excelFileName)
    {
        //先处理Excel文件
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // 避免出现许可证错误
        List<string> timeData = new List<string>();
        List<string> uidData = new List<string>();

        // string FilePath = "";
        // string sql = "SELECT UID FROM kahao";
        // uidData = Mdb.Select(FilePath,sql);
        // sql = "SELECT Time FROM kahao";
        // timeData = Mdb.Select(FilePath,sql);
        //
        // for (int i = 0; i < timeData.Count; i++)
        // {
        //     DateTime parsedDate = DateTime.ParseExact(timeData[i], "yyyy/MM/dd H:mm:ss", null);
        //     timeData[i] = "HG" + parsedDate.ToString("yyyyMMdd") + timeData[i];
        // }
        
        using (var package = new ExcelPackage(ExcelData))
        {
            var worksheet = package.Workbook.Worksheets[0]; // 获取第一个工作表
            int rowCount = worksheet.Dimension.Rows; //获取行数
            
            //遍历Excel文件的每一行
            for (int row = 2; row <= rowCount; row++)
            {
                string uidValue = worksheet.Cells[row, 4].Text;
                string timeValue = worksheet.Cells[row, 12].Text;
                DateTime parsedDate = DateTime.ParseExact(timeValue, "yyyy/MM/dd H:mm:ss", null);
                timeValue = "HG" + parsedDate.ToString("yyyyMMdd") + uidValue;
                timeData.Add(timeValue);
                uidData.Add(uidValue);
            }
        }
        // 创建一个新的Excel文件
        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add(excelFileName);
            // 插入数据
            for (int i = 0; i < uidData.Count; i++)
            {
                worksheet.Cells[i + 1, 1].Value = uidData[i];
                worksheet.Cells[i + 1, 2].Value = timeData[i];
            }
            // 保存文件到桌面
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string fileName = $"{excelFileName}.xlsx";
            string filePath = Path.Combine(desktopPath, fileName);
            package.SaveAs(new FileInfo(filePath));
        } 
        Message.ShowSnack();
    }
}