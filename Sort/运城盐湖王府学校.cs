using Wpf.Ui.Controls;

namespace WindowUI.Sort;

public class 运城盐湖王府学校
{
    public static void Run(MemoryStream ExcelData, string excelFileName)
    {
        //先处理Excel文件
        
        List<string> snData = new List<string>();
        List<string> uidData = new List<string>();
        using (var package = new ExcelPackage(ExcelData))
        {
            var worksheet = package.Workbook.Worksheets[0]; // 获取第一个工作表
            var rowCount = worksheet.Dimension.Rows; //获取行数
            //遍历Excel文件的每一行
            for (var row = 1; row <= rowCount; row++)
            {
                var uidValue = worksheet.Cells[row, 2].Text;
                var snValue = worksheet.Cells[row, 8].Text;
                snData.Add(snValue);
                uidData.Add(uidValue);
            }
        }

        // 创建一个新的Excel文件
        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add(excelFileName);
            // 插入标题行
            worksheet.Cells[1, 1].Value = "Index";
            worksheet.Cells[1, 2].Value = "SerialNumber";
            worksheet.Cells[1, 3].Value = "UID";
            // 插入数据
            for (var i = 0; i < snData.Count; i++)
            {
                worksheet.Cells[i + 2, 1].Value = i + 1;
                worksheet.Cells[i + 2, 2].Value = snData[i];
                worksheet.Cells[i + 2, 3].Value = uidData[i];
            }

            // 保存文件到桌面
            var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var fileName = $"{excelFileName}.xlsx";
            var filePath = Path.Combine(desktopPath, fileName);
            package.SaveAs(new FileInfo(filePath));
            Message.ShowSnack("成功", "文件已保存到桌面,请根据制卡数据重命名RCC文件", ControlAppearance.Success,
                new SymbolIcon(SymbolRegular.Checkmark20), 3);
        }
    }
}