using WindowUI.Pages;

namespace WindowUI.Sort;

public class 徐州地铁UL
{
    public static void Run(MemoryStream ExcelData, string excelFileName)
    {
        List<string> SNData = new List<string>();
        List<string> UidData = new List<string>();


        //取出Excel文件的数据

        using (var package = new ExcelPackage(ExcelData))
        {
            var worksheet = package.Workbook.Worksheets[0]; // 获取第一个工作表
            var rowCount = worksheet.Dimension.Rows; // 获取行数

            //遍历Excel文件的每一行
            for (var row = 2; row <= rowCount; row++)
            {
                var SNValue = worksheet.Cells[row, 9].Text;
                var uidValue = worksheet.Cells[row, 2].Text;
                SNData.Add(SNValue);
                UidData.Add(uidValue);
            }
        }


        // 保存文件到桌面
        var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        var fileName = $"{excelFileName}.txt";
        var filePath = Path.Combine(desktopPath, fileName);

        using (var writer = new StreamWriter(filePath))
        {
            for (var i = 0; i < SNData.Count; i++)
                if (i == SNData.Count - 1)
                    writer.Write($"{SNData[i]}\t{UidData[i]}00");
                else
                    writer.WriteLine($"{SNData[i]}\t{UidData[i]}00");
        }

        Message.ShowSnack();
    }
}