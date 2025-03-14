namespace WindowUI.Sort;

public class 抚顺
{
    public static void Run(MemoryStream ExcelData, string excelFileName)
    {
        //取出Excel文件的数据
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // 避免出现许可证错误
        List<string> SnData = new List<string>();
        List<string> UidData = new List<string>();

        using (var package = new ExcelPackage(ExcelData))
        {
            var worksheet = package.Workbook.Worksheets[0]; // 获取第一个工作表
            var rowCount = worksheet.Dimension.Rows; // 获取行数

            //遍历Excel文件的每一行
            for (var row = 1; row <= rowCount; row++)
            {
                var snValue = worksheet.Cells[row, 7].Text;
                var uidValue = worksheet.Cells[row, 3].Text;
                uidValue = Tools.ChangeDecimalSystem(uidValue);
                SnData.Add(snValue);
                UidData.Add(uidValue);
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
                    writer.Write($"{SnData[i]} {UidData[i]}");
                else
                    writer.WriteLine($"{SnData[i]} {UidData[i]}");
        }

        Message.ShowSnack();
    }
}