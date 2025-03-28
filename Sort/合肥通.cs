﻿namespace WindowUI.Sort;

public class 合肥通
{
    public static void Run(MemoryStream ExcelData, List<string> MKData, string mkFileName)
    {
        //先处理Excel文件
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // 避免出现许可证错误
        List<string> processedData = new List<string>();
        using (var package = new ExcelPackage(ExcelData))
        {
            var worksheet = package.Workbook.Worksheets[0]; // 获取第一个工作表
            var rowCount = worksheet.Dimension.Rows; //获取行数

            //遍历Excel文件的每一行
            for (var row = 2; row <= rowCount; row++)
            {
                var firstColumnValue = worksheet.Cells[row, 1].Text;
                var secondColumnValue = worksheet.Cells[row, 2].Text;
                var newRow =
                    $"{firstColumnValue}      {firstColumnValue}      {secondColumnValue}              00                         FFFFFFFFFFFFFFFFFFFF";
                processedData.Add(newRow);
            }
        }

        //处理MK文件
        //截取MK文件第二行的前42个字节
        MKData[1] = MKData[1].Substring(0, 42);
        //获取Excel总数据的条数
        var totalLines = processedData.Count;
        //将总数据条数转为6位数
        var totalLinesFormatted = totalLines.ToString("D6");
        //将MK文件的第二行的后6位替换为总数据条数
        MKData[1] = MKData[1].Substring(0, MKData[1].Length - 6) + totalLinesFormatted;
        //将MK文件与Excel文件的数据合并

        var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        var fileName = $"RC{mkFileName}001";
        var filePath = Path.Combine(desktopPath, fileName);
        using (var writer = new StreamWriter(filePath))
        {
            writer.WriteLine(MKData[0]);
            writer.WriteLine(MKData[1]);

            for (var i = 0; i < processedData.Count; i++)
                if (i == processedData.Count - 1)
                    writer.Write(processedData[i]);
                else
                    writer.WriteLine(processedData[i]);
        }

        Message.ShowSnack();
    }
}