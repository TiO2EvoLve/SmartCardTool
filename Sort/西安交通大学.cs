﻿namespace WindowUI.Sort;

public class 西安交通大学
{
    public static void Run(MemoryStream ExcelData,string excelFileName)
    {
        //取出Excle文件的数据
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // 避免出现许可证错误
        //取出第七列流水号数据
        List<string> Data = new List<string>();
        List<string> UidData = new List<string>();
        using (var package = new ExcelPackage(ExcelData))
        {
            var worksheet = package.Workbook.Worksheets[0]; // 获取第一个工作表
            int rowCount = worksheet.Dimension.Rows; //获取行数
            //遍历Excel文件的每一行
            for (int row = 1; row <= rowCount; row++)
            {
                string DataValue = worksheet.Cells[row, 7].Text;
                string UidValue = worksheet.Cells[row, 2].Text;
                UidValue = Convert.ToUInt32(UidValue, 16).ToString();
                Data.Add(DataValue);
                UidData.Add(UidValue);
            }
        }
        // 创建一个新的Excel文件
        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add(excelFileName);
            // 插入数据
            for (int i = 0; i < UidData.Count; i++)
            {
                worksheet.Cells[i + 1, 1].Value = Data[i];
                worksheet.Cells[i + 1, 2].Value = UidData[i];
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