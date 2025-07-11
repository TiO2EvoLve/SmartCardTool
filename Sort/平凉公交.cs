namespace WindowUI.Sort;

public class 平凉公交
{
    public static void Run(MemoryStream ExcelData, string excelFileName)
    {
        //取出Excle文件的数据
        
        List<string> UIDData = new List<string>();
        List<string> SNData = new List<string>();
        // string sql = "SELECT outsidelasercode From print order by outsidelasercode ASC";
        // SNData = Mdb.Select(FilePath, sql);
        // sql = "SELECT insidecode From print order by outsidelasercode ASC";
        // UIDData = Mdb.Select(FilePath, sql);
        //
        // for (int i = 0; i < UIDData.Count; i++)
        // {
        //     UIDData[i] = Convert.ToUInt32(UIDData[i], 16).ToString();
        // }
        
        using (var package = new ExcelPackage(ExcelData))
        {
            var worksheet = package.Workbook.Worksheets[0]; // 获取第一个工作表
            var rowCount = worksheet.Dimension.Rows; //获取行数
            //遍历Excel文件的每一行
            for (var row = 1; row <= rowCount; row++)
            {
                var UIDValue = worksheet.Cells[row, 3].Text;
                UIDData.Add(UIDValue);
                var SNValue = worksheet.Cells[row, 7].Text;
                SNData.Add(SNValue);
            }
        }
        var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        var fileName = $"{excelFileName}.xlsx";
        var filePath = Path.Combine(desktopPath, fileName);

        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add(excelFileName);
            for (var i = 0; i < UIDData.Count; i++)
            {
                worksheet.Cells[i + 1, 1].Value = UIDData[i];
                worksheet.Cells[i + 1, 2].Value = $"74400000{SNData[i]}";
                worksheet.Cells[i + 1, 3].Value = "1";
            }

            // 保存文件到桌面
            package.SaveAs(new FileInfo(filePath));
        }

        Message.ShowSnack();
    }
}