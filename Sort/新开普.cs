using WindowUI.Pages;

namespace WindowUI.Sort;

public class 新开普
{
   
    public static void Run(string FilePath,MemoryStream ExcelData, string FileName)
    {
        List<string> SNData = new List<string>();
        List<string> UidData = new List<string>();
        新开普菜单 page = new 新开普菜单();
        page.ShowDialog();
        
        int SN_Column = page.viewmodel.sn_Column;
        int Uid_Column = page.viewmodel.uid_Column;
        bool IsSkipFirstRow = page.viewmodel.isSkipFirstRow;
        
        //获取文件的类型
        string fileExtension = Path.GetExtension(FileName).ToLower();
        if (string.IsNullOrEmpty(fileExtension))
        {   
            return;
        }
        
        if (fileExtension == "mdb")
        {
            MdbExcute(FilePath);
        }else if(fileExtension == "xlsx")
        {
            ExcelExcute(ExcelData);
        }else 
        {
            Message.ShowMessageBox("错误", "不支持的文件类型");
        }
        //mdb文件处理逻辑
        void MdbExcute(string FilePath)
        {
            var sql = "SELECT SerialNum FROM kahao order by SerialNum ASC";
            SNData = Mdb.Select(FilePath, sql);
            sql = "SELECT UID_16_ FROM kahao order by SerialNum ASC";
            UidData = Mdb.Select(FilePath, sql);
            
        }
        //Excel文件处理逻辑
        void ExcelExcute(MemoryStream ExcelData)
        {
            using (var package = new ExcelPackage(ExcelData))
            {
                var worksheet = package.Workbook.Worksheets[0]; // 获取第一个工作表
                var rowCount = worksheet.Dimension.Rows; // 获取行数

                // 遍历Excel文件的每一行
                for (var row = IsSkipFirstRow?1:2; row <= rowCount; row++)
                {
                    var SNValue = worksheet.Cells[row, SN_Column].Text;
                    var Uid16Value = worksheet.Cells[row, Uid_Column].Text;
                    SNData.Add(SNValue);
                    UidData.Add(Uid16Value);
                }
            }
        }

        // 创建一个新的Excel文件
        var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        var fileName = $"{FileName}.xlsx";
        var filePath = Path.Combine(desktopPath, fileName);

        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add(FileName);
            // 插入数据
            worksheet.Cells[1, 1].Value = "SerialNumber";
            worksheet.Cells[1, 2].Value = "UID";
            for (var i = 0; i < SNData.Count; i++)
            {
                worksheet.Cells[i + 2, 1].Value = SNData[i];
                worksheet.Cells[i + 2, 2].Value = UidData[i];
            }
            // 保存文件到桌面
            package.SaveAs(new FileInfo(filePath));
        }
        Message.ShowSnack();
    }
   
}