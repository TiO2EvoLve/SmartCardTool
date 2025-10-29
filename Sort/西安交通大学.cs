namespace WindowUI.Sort;

public class 西安交通大学
{
    public static void Run(MemoryStream ExcelData, string excelFileName)
    {
        //取出Excle文件的数据
        
        //取出第七列流水号数据
        List<string> SN = new List<string>();
        List<string> Uid10 = new List<string>();
        using (var package = new ExcelPackage(ExcelData))
        {
            var worksheet = package.Workbook.Worksheets[0]; // 获取第一个工作表
            var rowCount = worksheet.Dimension.Rows; //获取行数
            //遍历Excel文件的每一行
            for (var row = 2; row <= rowCount; row++)
            {
                var DataValue = worksheet.Cells[row, 8].Text;
                var UidValue = worksheet.Cells[row, 2].Text;
                UidValue = Convert.ToUInt32(UidValue, 16).ToString();
                SN.Add(DataValue);
                Uid10.Add(UidValue);
            }
        }

        // 创建一个新的Excel文件
        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add(excelFileName);
            // 插入数据
            for (var i = 0; i < Uid10.Count; i++)
            {
                worksheet.Cells[i + 1, 1].Value = SN[i];
                worksheet.Cells[i + 1, 2].Value = Uid10[i];
            }

            // 保存文件到桌面
            var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var fileName = $"{excelFileName}.xlsx";
            var filePath = Path.Combine(desktopPath, fileName);
            package.SaveAs(new FileInfo(filePath));
            Message.ShowSnack();
        }
    }
}