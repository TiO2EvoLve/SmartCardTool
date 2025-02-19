namespace WindowUI.Sort;

public class 呼和浩特
{
    public static void Run(MemoryStream ExcelData,string excelFileName)
    {
        //取出Excle文件的数据
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // 避免出现许可证错误
        List<string> SNData = new List<string>();
        List<string> UidData = new List<string>();
        using (var package = new ExcelPackage(ExcelData))
        {
            var worksheet = package.Workbook.Worksheets[0]; // 获取第一个工作表
            int rowCount = worksheet.Dimension.Rows; //获取行数
            //遍历Excel文件的每一行
            for (int row = 2; row <= rowCount; row++)
            {
                string SNValue = worksheet.Cells[row, 2].Text;
                string UidValue = worksheet.Cells[row, 3].Text;
                SNData.Add(SNValue);
                UidData.Add(UidValue);

            }
        }
        // 创建一个新的Excel文件
        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add(excelFileName);
            // 插入数据
            worksheet.Cells[1, 1].Value = "发行卡号(16位)";
            worksheet.Cells[1, 2].Value = "物理卡号(8位)";
            worksheet.Cells[1, 3].Value = "物理卡号(8位) 高低字节调整";
            for (int i = 0; i < UidData.Count; i++)
            {
                worksheet.Cells[i + 2, 1].Value = SNData[i];
                worksheet.Cells[i + 2, 3].Value = UidData[i];
                worksheet.Cells[i + 2, 2].Value = Tools.ChangeHexPairs(UidData[i]);
            }
            // 保存文件到桌面
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string fileName = $"{excelFileName}.xlsx";
            string filePath = Path.Combine(desktopPath, fileName);
            package.SaveAs(new FileInfo(filePath));
            Message.ShowSnack();
        }
    }
}