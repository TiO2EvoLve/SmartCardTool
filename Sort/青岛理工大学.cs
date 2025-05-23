﻿using WindowUI.Pages;

namespace WindowUI.Sort;

public class 青岛理工大学
{
    public static void Run(MemoryStream ExcelData, string excelFileName)
    {
        青岛理工大学菜单 qingdao = new();
        qingdao.ShowDialog();
        var campus = qingdao.SelectedCampus;
        if (campus == "青岛校区")
            青岛理工大学青岛校区(ExcelData, excelFileName);
        else if (campus == "临沂校区")
            青岛理工大学临沂校区(ExcelData, excelFileName);
        else
            Message.ShowMessageBox("错误", "请选择校区");
    }

    private static void 青岛理工大学临沂校区(MemoryStream ExcelData, string excelFileName)
    {
        //取出Excel文件的数据
        
        List<string> DateData = new List<string>();
        var uidData = new List<string>();
        using (var package = new ExcelPackage(ExcelData))
        {
            var worksheet = package.Workbook.Worksheets[0]; // 获取第一个工作表
            var rowCount = worksheet.Dimension.Rows; // 获取行数

            //遍历Excel文件的每一行
            for (var row = 1; row <= rowCount; row++)
            {
                var DateValue = worksheet.Cells[row, 8].Text;
                var uidValue = worksheet.Cells[row, 2].Text;
                DateData.Add(DateValue);
                uidData.Add(uidValue);
            }
        }

        // 创建一个新的Excel文件
        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add(excelFileName);

            // 插入数据
            for (var i = 0; i < DateData.Count; i++)
            {
                worksheet.Cells[i + 1, 1].Value = i + 1;
                worksheet.Cells[i + 1, 2].Value = DateData[i];
                worksheet.Cells[i + 1, 3].Value = uidData[i];
            }

            // 保存文件到桌面
            var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var fileName = $"{excelFileName}.xlsx";
            var filePath = Path.Combine(desktopPath, fileName);
            package.SaveAs(new FileInfo(filePath));
            Message.ShowSnack();
        }
    }

    //青岛理工大学青岛校区的处理逻辑
    private static void 青岛理工大学青岛校区(MemoryStream ExcelData, string excelFileName)
    {
        //取出Excel文件的数据
        
        List<string> SNData = new List<string>();
        var uid6Data = new List<string>();
        using (var package = new ExcelPackage(ExcelData))
        {
            var worksheet = package.Workbook.Worksheets[0]; // 获取第一个工作表
            var rowCount = worksheet.Dimension.Rows; // 获取行数

            //遍历Excel文件的每一行
            for (var row = 1; row <= rowCount; row++)
            {
                var SNValue = worksheet.Cells[row, 8].Text;
                var uid16Value = worksheet.Cells[row, 2].Text;
                SNData.Add(SNValue);
                uid6Data.Add(uid16Value);
            }
        }

        // 创建一个新的Excel文件
        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add(excelFileName);

            // 插入数据
            for (var i = 0; i < SNData.Count; i++)
            {
                worksheet.Cells[i + 1, 1].Value = SNData[i];
                worksheet.Cells[i + 1, 2].Value = uid6Data[i];
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