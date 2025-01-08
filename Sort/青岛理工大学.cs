using WindowUI.Pages;

namespace WindowUI.Sort;

public class 青岛理工大学
{
    public static void Run(MemoryStream ExcelData,string excelFileName)
    {
        青岛理工大学菜单 qingdao = new ();
        qingdao.ShowDialog();
        string campus = qingdao.SelectedCampus;
        if(campus == "青岛校区")
        {
            青岛理工大学青岛校区(ExcelData,excelFileName);
        }
        else if(campus == "临沂校区")
        {
            青岛理工大学临沂校区(ExcelData,excelFileName);
        }      
    }
    private static void 青岛理工大学临沂校区(MemoryStream ExcelData,string excelFileName)
    {
        //取出Excel文件的数据
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // 避免出现许可证错误
        List<string> DateData = new List<string>();
        List<string> uidData = new List<string>();
        using (var package = new ExcelPackage(ExcelData))
        {
            var worksheet = package.Workbook.Worksheets[0]; // 获取第一个工作表
            int rowCount = worksheet.Dimension.Rows; // 获取行数

            //遍历Excel文件的每一行
            for (int row = 1; row <= rowCount; row++)
            {
                string DateValue = worksheet.Cells[row, 8].Text;
                string uidValue = worksheet.Cells[row, 2].Text;
                DateData.Add(DateValue);
                uidData.Add(uidValue);
            }
        }
        // 创建一个新的Excel文件
        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add(excelFileName);

            // 插入数据
            for (int i = 0; i < DateData.Count; i++)
            {
                worksheet.Cells[i + 1, 1].Value = i + 1;
                worksheet.Cells[i + 1, 2].Value = DateData[i];
                worksheet.Cells[i + 1, 3].Value = uidData[i];
            }
            // 保存文件到桌面
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string fileName = $"{excelFileName}.xlsx";
            string filePath = Path.Combine(desktopPath, fileName);
            package.SaveAs(new FileInfo(filePath));
            MessageBox.Show($"数据已处理并保存到文件: {filePath}");
        }
    }
    //青岛理工大学青岛校区的处理逻辑
    private static void 青岛理工大学青岛校区(MemoryStream ExcelData,string excelFileName)
    {
        //取出Excel文件的数据
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // 避免出现许可证错误
        List<string> SNData = new List<string>();
        List<string> uid6Data = new List<string>();
        using (var package = new ExcelPackage(ExcelData))
        {
            var worksheet = package.Workbook.Worksheets[0]; // 获取第一个工作表
            int rowCount = worksheet.Dimension.Rows; // 获取行数

            //遍历Excel文件的每一行
            for (int row = 1; row <= rowCount; row++)
            {
                string SNValue = worksheet.Cells[row, 1].Text;
                string uid16Value = worksheet.Cells[row, 4].Text;
                SNData.Add(SNValue);
                uid6Data.Add(uid16Value);
            }
        }
        // 创建一个新的Excel文件
        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add(excelFileName);

            // 插入数据
            for (int i = 0; i < SNData.Count; i++)
            {
                worksheet.Cells[i + 1, 1].Value = SNData[i];
                worksheet.Cells[i + 1, 2].Value = uid6Data[i];
            }
            // 保存文件到桌面
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string fileName = $"{excelFileName}.xlsx";
            string filePath = Path.Combine(desktopPath, fileName);
            package.SaveAs(new FileInfo(filePath));
            MessageBox.Show($"数据已处理并保存到文件: {filePath}");
        }
    }
}