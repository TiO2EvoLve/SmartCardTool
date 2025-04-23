namespace WindowUI.Sort;

public class 济南员工卡
{
    public static void Run(MemoryStream ExcelData, string excelFileName)
    {
        List<string> SNData = new List<string>();
        List<string> UidData = new List<string>();
        
        using (var package = new ExcelPackage(ExcelData))
        {
            var worksheet = package.Workbook.Worksheets[0]; // 获取第一个工作表
            var rowCount = worksheet.Dimension.Rows; // 获取行数
            // 遍历Excel文件的每一行
            for (var row = 2; row <= rowCount; row++)
            {
                var SNValue = worksheet.Cells[row, 7].Text;
                var Uid_Value = worksheet.Cells[row, 2].Text;
                SNData.Add(SNValue);
                UidData.Add(Uid_Value);
            }
        }
        
        //保存为txt文件
        var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        var fileName = $"{excelFileName}.txt";
        var filePath = Path.Combine(desktopPath, fileName);
        using (var writer = new StreamWriter(filePath))
        {
            writer.WriteLine("芯片号|复位信息|卡片逻辑卡号");
            for (var i = 0; i < SNData.Count; i++)
                if (i == SNData.Count - 1)
                    writer.Write($"{UidData[i]}|{UidData[i]}|90000001{SNData[i]}");
                else
                    writer.WriteLine($"{UidData[i]}|{UidData[i]}|90000001{SNData[i]}");
        }
        Message.ShowSnack();
    }
}