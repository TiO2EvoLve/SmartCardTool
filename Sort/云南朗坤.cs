namespace WindowUI.Sort;

public class 云南朗坤
{
    public static void Run(MemoryStream ExcelData, string excelFileName)
    {
        // 取出Excel文件的数据
        
        List<string> SNData = new List<string>();
        List<string> Uid16Data = new List<string>();
        List<string> Uid10Data = new List<string>();

        using (var package = new ExcelPackage(ExcelData))
        {
            var worksheet = package.Workbook.Worksheets[0]; // 获取第一个工作表
            var rowCount = worksheet.Dimension.Rows; // 获取行数

            // 遍历Excel文件的每一行
            for (var row = 1; row <= rowCount; row++)
            {
                var SNValue = worksheet.Cells[row, 7].Text;
                var Uid16Value = worksheet.Cells[row, 2].Text;
                var Uid10Value = worksheet.Cells[row, 3].Text;
                SNData.Add(SNValue);
                Uid16Data.Add(Uid16Value);
                Uid10Data.Add(Uid10Value);
            }
        }

        // 创建一个新的Excel文件
        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add(excelFileName);
            worksheet.Cells[1, 1].Value = "卡号";
            worksheet.Cells[1, 2].Value = "Uid16进制";
            worksheet.Cells[1, 3].Value = "Uid16进制调整";
            worksheet.Cells[1, 4].Value = "Uid10进制";
            worksheet.Cells[1, 5].Value = "Uid10进制调整";
            for (var i = 0; i < SNData.Count; i++)
            {
                worksheet.Cells[i + 2, 1].Value = SNData[i];
                worksheet.Cells[i + 2, 2].Value = Uid16Data[i];
                worksheet.Cells[i + 2, 3].Value = Tools.ChangeHexPairs(Uid16Data[i]);
                worksheet.Cells[i + 2, 4].Value = Convert.ToUInt32(Uid16Data[i], 16).ToString();
                worksheet.Cells[i + 2, 5].Value = Uid10Data[i];
            }

            // 保存文件到桌面
            var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var fileName = $"{excelFileName}.xlsx";
            var filePath = Path.Combine(desktopPath, fileName);
            package.SaveAs(new FileInfo(filePath));
            // 显示提示消息
            Message.ShowSnack();
        }
    }
}