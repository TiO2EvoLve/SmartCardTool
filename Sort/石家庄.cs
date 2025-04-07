namespace WindowUI.Sort;

public class 石家庄
{
    public static void Run(MemoryStream ExcelData, string excelFileName)
    {
        // 第一个rcc文件，excel格式
        List<string> SNData = new List<string>();
        List<string> Uid_16_Data = new List<string>();
        using (var package = new ExcelPackage(ExcelData))
        {
            var worksheet = package.Workbook.Worksheets[0]; // 获取第一个工作表
            var rowCount = worksheet.Dimension.Rows; // 获取行数
            // 遍历Excel文件的每一行
            for (var row = 1; row <= rowCount; row++)
            {
                var SNValue = worksheet.Cells[row, 7].Text;
                var Uid_16_Value = worksheet.Cells[row, 2].Text;
                SNData.Add(SNValue);
                Uid_16_Data.Add(Uid_16_Value);
            }
        }

        //新建一个Excel文件
        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add(excelFileName);
            worksheet.Cells[1, 1].Value = "SerialNum";
            worksheet.Cells[1, 2].Value = "UID_16_";
            for (var i = 0; i < Uid_16_Data.Count; i++)
            {
                worksheet.Cells[i + 2, 1].Value = SNData[i];
                worksheet.Cells[i + 2, 2].Value = Uid_16_Data[i];
            }

            // 保存文件到桌面
            var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var fileName = $"{excelFileName}.xlsx";
            var filePath = Path.Combine(desktopPath, fileName);
            package.SaveAs(new FileInfo(filePath));
        }

        // 第二个rcc文件，txt格式
        var desktopPath1 = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        var fileName1 = $"{excelFileName}.txt";
        var filePath1 = Path.Combine(desktopPath1, fileName1);

        using (var writer = new StreamWriter(filePath1))
        {
            writer.WriteLine("SerialNum\tUID");
            for (var i = 0; i < SNData.Count; i++)
                if (i == SNData.Count - 1)
                    writer.Write($"{SNData[i]}\t{Uid_16_Data[i]}");
                else
                    writer.WriteLine($"{SNData[i]}\t{Uid_16_Data[i]}");
        }

        Message.ShowSnack();
    }
}