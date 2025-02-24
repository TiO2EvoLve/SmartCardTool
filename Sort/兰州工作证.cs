namespace WindowUI.Sort;

public class 兰州工作证
{
    public static void Run(MemoryStream ExcelData, string excelFileName)
    {
        List<string> SNData = new List<string>();
        List<string> UIDData = new List<string>();
        
        //取出Excle文件的数据
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // 避免出现许可证错误
        using (var package = new ExcelPackage(ExcelData))
        {
            var worksheet = package.Workbook.Worksheets[0]; // 获取第一个工作表
            int rowCount = worksheet.Dimension.Rows; //获取行数
            //遍历Excel文件的每一行
            Console.WriteLine(rowCount);
            for (int row = 2; row <= rowCount; row++)
            {
                string SNValue = worksheet.Cells[row, 7].Text;
                SNData.Add(SNValue);
                string UIDValue = worksheet.Cells[row, 3].Text;
                UIDValue = Tools.ChangeDecimalSystem(UIDValue);
                UIDData.Add(UIDValue);
            }
        }
       
        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string fileName = excelFileName + ".txt";
        string filePath = Path.Combine(desktopPath, fileName);
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            for (int i = 0; i < SNData.Count; i++)
            {
                if (i == SNData.Count - 1)
                {
                    writer.Write($"{SNData[i]},{UIDData[i]}");
                }
                else
                {
                    writer.WriteLine($"{SNData[i]},{UIDData[i]}");  
                }
            }
        }
        Message.ShowSnack();
    }
}