
using WindowUI.Tool;

namespace WindowUI.Sort;

public class 云南朗坤
{
    public static void Run(MemoryStream ExcelData,string excelFileName)
    {
        // 取出Excel文件的数据
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // 避免出现许可证错误
        List<string> SNData = new List<string>();
        List<string> UidData = new List<string>();
        List<string> Uid_Data = new List<string>();
        
        using (var package = new ExcelPackage(ExcelData))
        {
            var worksheet = package.Workbook.Worksheets[0]; // 获取第一个工作表
            int rowCount = worksheet.Dimension.Rows; // 获取行数

            // 遍历Excel文件的每一行
            for (int row = 1; row <= rowCount; row++)
            {
                string SNValue = worksheet.Cells[row, 7].Text;
                string UidValue = worksheet.Cells[row, 2].Text;
                string Uid_Value = worksheet.Cells[row, 3].Text;
                SNData.Add(SNValue);
                UidData.Add(UidValue);
                Uid_Data.Add(Uid_Value);
            }
        }
        // 创建一个新的Excel文件
        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add(excelFileName);
            for (int i = 0; i < UidData.Count; i++)
            {
                worksheet.Cells[i + 1, 1].Value = SNData[i];
                worksheet.Cells[i + 1, 2].Value = UidData[i];
                worksheet.Cells[i + 1, 3].Value = Uid_Data[i];
                worksheet.Cells[i + 1, 4].Value = Convert.ToUInt32(UidData[i], 16).ToString();
                worksheet.Cells[i + 1, 5].Value = Convert.ToUInt32(Uid_Data[i], 16).ToString();
            }
            // 保存文件到桌面
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string fileName = $"{excelFileName}.xlsx";
            string filePath = Path.Combine(desktopPath, fileName);
            package.SaveAs(new FileInfo(filePath));
            // 显示提示消息
            MessageBox.Show($"数据已处理并保存到桌面{filePath}");
        }    
    }
    
    public static void PlanB(MemoryStream ExcelData, string excelFileName)
    {
        // 取出Excel文件的数据
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // 避免出现许可证错误
        List<string> SNData = new List<string>();
        List<string> Uid16Data = new List<string>();
        List<string> Uid10Data = new List<string>();
        
        
        using (var package = new ExcelPackage(ExcelData))
        {
            var worksheet = package.Workbook.Worksheets[0]; // 获取第一个工作表
            int rowCount = worksheet.Dimension.Rows; // 获取行数

            // 遍历Excel文件的每一行
            for (int row = 1; row <= rowCount; row++)
            {
                string SNValue = worksheet.Cells[row, 7].Text;
                string Uid16Value = worksheet.Cells[row, 2].Text;
                string Uid10Value = worksheet.Cells[row, 3].Text;
                SNData.Add(SNValue);
                Uid16Data.Add(Uid16Value);
                Uid10Data.Add(Uid10Value);
            }
        } 
        // 创建一个新的Excel文件
        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add(excelFileName);
            for (int i = 0; i < SNData.Count; i++)
            {
                worksheet.Cells[i + 1, 1].Value = SNData[i];
                worksheet.Cells[i + 1, 2].Value = Uid16Data[i];
                worksheet.Cells[i + 1, 3].Value = Tools.ChangeHexPairs(Uid16Data[i]);
                worksheet.Cells[i + 1, 4].Value = Convert.ToUInt32(Uid16Data[i], 16).ToString();
                worksheet.Cells[i + 1, 5].Value = Uid10Data[i];
            }
            // 保存文件到桌面
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string fileName = $"{excelFileName}.xlsx";
            string filePath = Path.Combine(desktopPath, fileName);
            package.SaveAs(new FileInfo(filePath));
            // 显示提示消息
            MessageBox.Show($"数据已处理并保存到桌面{filePath}");
        }     
    }
}