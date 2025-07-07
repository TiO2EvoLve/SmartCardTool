namespace WindowUI.Sort;

public class 昆明
{
    public static void Run(MemoryStream ExcelData, string excelFileName)
    {
        // 取出Excel文件的数据
        
        List<string> SNData = new List<string>();
        List<string> SN16Data = new List<string>();
        List<string> Uid16Data = new List<string>();
        List<string> Uid10Data = new List<string>();
        using (var package = new ExcelPackage(ExcelData))
        {
            var worksheet = package.Workbook.Worksheets[0]; // 获取第一个工作表
            var rowCount = worksheet.Dimension.Rows; // 获取行数
            // 遍历Excel文件的每一行
            for (var row = 2; row <= rowCount; row++)
            {
                var SNValue = worksheet.Cells[row, 1].Text;
                var SN16Value = worksheet.Cells[row, 10].Text;
                var Uid16Value = worksheet.Cells[row, 4].Text;
                var Uid10Value = worksheet.Cells[row, 6].Text;
                SNData.Add(SNValue);
                SN16Data.Add(SN16Value);
                Uid16Data.Add(Uid16Value);
                Uid10Data.Add(Uid10Value);
            }
        }

        // 保存文件到桌面
        var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        var fileName = "1342680response867020250113.xlsx";
        var filePath = Path.Combine(desktopPath, fileName);
        // 创建一个新的Excel文件
        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add(fileName);

            for (var i = 0; i < Uid16Data.Count; i++)
            {
                worksheet.Cells[i + 1, 1].Value = Uid16Data[i];
                worksheet.Cells[i + 1, 2].Value = Uid10Data[i];
                worksheet.Cells[i + 1, 3].Value = (8684250113000001 + i).ToString();
                worksheet.Cells[i + 1, 4].Value = "8670";
                worksheet.Cells[i + 1, 5].Value = "0" + SNData[i];
                worksheet.Cells[i + 1, 6].Value = "ZP18010302";
            }

            package.SaveAs(new FileInfo(filePath));
        }

        fileName = $"{excelFileName}.xlsx";
        filePath = Path.Combine(desktopPath, fileName);
        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add(excelFileName);
            worksheet.Cells[1, 1].Value = "卡面号";
            worksheet.Cells[1, 2].Value = "UID_10_";
            for (var i = 0; i < Uid16Data.Count; i++)
            {
                worksheet.Cells[i + 2, 1].Value = SN16Data[i];
                worksheet.Cells[i + 2, 2].Value = Uid10Data[i];
            }

            package.SaveAs(new FileInfo(filePath));
        }
        Message.ShowSnack();
    }
}