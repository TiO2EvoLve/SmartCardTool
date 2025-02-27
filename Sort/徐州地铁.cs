using WindowUI.Pages;

namespace WindowUI.Sort;

public class 徐州地铁
{
    public static void Run(string FilePath,MemoryStream ExcelData, string excelFileName)
    {
        
        List<string> SNData = new List<string>();
        List<string> UidData = new List<string>();
        
        徐州菜单 xuzhou = new();
        xuzhou.ShowDialog();
        string filetype = xuzhou.SelectedCampus;
        if (filetype == ".mdb")
        {
            string sql = "SELECT NUM FROM kahao order by NUM ASC";
            SNData = Mdb.Select(FilePath, sql);
            sql = "SELECT UID_ FROM kahao order by NUM ASC";
            UidData = Mdb.Select(FilePath, sql);
        }else if (filetype == ".xlsx")
        {
            //取出Excel文件的数据
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // 避免出现许可证错误
            using (var package = new ExcelPackage(ExcelData))
            {
                var worksheet = package.Workbook.Worksheets[0]; // 获取第一个工作表
                int rowCount = worksheet.Dimension.Rows; // 获取行数

                //遍历Excel文件的每一行
                for (int row = 1; row <= rowCount; row++)
                {
                    string SNValue = worksheet.Cells[row, 8].Text;
                    string uidValue = worksheet.Cells[row, 2].Text;
                    SNData.Add(SNValue);
                    UidData.Add(uidValue);
                }
            }
        }
        
        // 保存文件到桌面
        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string fileName = $"{excelFileName}.txt";
        string filePath = Path.Combine(desktopPath, fileName);

        using (StreamWriter writer = new StreamWriter(filePath))
        {
            for (int i = 0; i < SNData.Count; i++)
            {
                if (i == SNData.Count - 1)
                {
                    writer.Write($"{SNData[i]}\t{UidData[i]}00000000");
                }
                else
                {
                    writer.WriteLine($"{SNData[i]}\t{UidData[i]}00000000");
                }
            }
        }
        Message.ShowSnack();
    }
}