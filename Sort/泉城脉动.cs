namespace WindowUI.Sort;

public class 泉城脉动
{
     public static void Run(MemoryStream ExcelData, string FileName)
    {
        // 取出Excel文件的数据
        
        List<string> UID = new List<string>();
        

        using (var package = new ExcelPackage(ExcelData))
        {
            var worksheet = package.Workbook.Worksheets[0]; // 获取第一个工作表
            var rowCount = worksheet.Dimension.Rows; // 获取行数
        
            // 遍历Excel文件的每一行
            for (var row = 1; row <= rowCount; row++)
            {
                var UidValue = worksheet.Cells[row, 2].Text;
                UID.Add(UidValue);
            }
        }

        var time = DateTime.Now.ToString("yyyyMMdd");
        time = time.Substring(0,6);
        // 创建一个新的Excel文件
        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add(FileName);
            worksheet.Cells[1, 1].Value = "Index";
            worksheet.Cells[1, 2].Value = "CUSTOMUID";
            worksheet.Cells[1, 3].Value = "UID";
            
            for (var i = 0; i < UID.Count; i++)
            {
                worksheet.Cells[i + 2, 1].Value = i + 1;
                worksheet.Cells[i + 2, 2].Value = "03" + time + UID[i];
                worksheet.Cells[i + 2, 3].Value = UID[i];
            }

            // 保存文件到桌面
            var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var fileName = $"{FileName}.xlsx";
            var filePath = Path.Combine(desktopPath, fileName);
            package.SaveAs(new FileInfo(filePath));
            // 显示提示消息
            Message.ShowSnack();
        }
    }
}