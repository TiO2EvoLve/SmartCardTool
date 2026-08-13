using OfficeOpenXml.Style;

namespace WindowUI.Sort;

public class 抚顺
{
    public static void Run(string FilePath,MemoryStream ExcelData, string excelFileName)
    {
        List<string> SN = new ();
        List<string> UID = new ();
        
        //获取文件的类型
        string fileExtension = Path.GetExtension(FilePath).ToLower();
        if (fileExtension == ".mdb")
        {
            MdbExcute(FilePath);
        }else if(fileExtension == ".xlsx")
        {
            ExcelExcute(ExcelData);
        }else 
        {
            Message.ShowMessageBox("错误", "不支持的文件类型");
        }
        
        void MdbExcute(string FilePath)
        {
            string sql = "select SerialNum from kahao order by SerialNum ASC";
            SN = Mdb.Select(FilePath, sql);
            sql = "select UID_10 from kahao order by SerialNum ASC ";
            UID = Mdb.Select(FilePath, sql); 
        }

        void ExcelExcute(MemoryStream ExcelData)
        {
            using (var package = new ExcelPackage(ExcelData))
            {
                var worksheet = package.Workbook.Worksheets[0]; // 获取第一个工作表
                var rowCount = worksheet.Dimension.Rows; // 获取行数

                // 遍历Excel文件的每一行
                for (var row = 1; row <= rowCount; row++)
                {
                    var SNValue = worksheet.Cells[row, 7].Text;
                    var UidValue = worksheet.Cells[row, 3].Text;
                    SN.Add(SNValue);
                    UID.Add(UidValue);
                }
            }
        }
        
        // 保存文件到桌面
        var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        var fileName = $"{excelFileName}.xlsx";
        var filePath = Path.Combine(desktopPath, fileName);
        
        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add(excelFileName);
            worksheet.Cells[1, 1].Value = "卡号";
            worksheet.Cells[1, 2].Value = "芯片号";
            
            for (var i = 0; i < UID.Count; i++)
            {
                worksheet.Cells[i + 2, 1].Value = SN[i];
                worksheet.Cells[i + 2, 2].Value = Tools.ChangeDecimalSystem(UID[i]);
            }
            worksheet.Cells.AutoFitColumns();
            worksheet.Cells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            
            package.SaveAs(new FileInfo(filePath));
        }
        Message.ShowSnack();
    }

    
}