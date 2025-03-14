using WindowUI.Pages;

namespace WindowUI.Sort;

public class 洪城
{
    public static void Run(MemoryStream ExcelData)
    {
        洪城菜单 hongCheng = new();
        hongCheng.ShowDialog();
        switch (hongCheng.Cardtype)
        {
            case "1208": 住建部(ExcelData); break;
            case "1280": 交通部(ExcelData); break;
            case "all":
                住建部(ExcelData);
                交通部(ExcelData);
                break;
            default: return;
        }
    }

    private static void 住建部(MemoryStream ExcelData)
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
                var ATS = worksheet.Cells[row, 2].Text;
                var code = worksheet.Cells[row, 11].Text;
                var newRow =
                    $"{ATS}                {code}        000033000000000000000000";
                processedData.Add(newRow);
            }
        }

        var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        var date = DateTime.Now.ToString("yyyyMMdd");
        var fileName = $"住建部_回盘_山东华冠_{date}_001.rcc";
        var filePath = Path.Combine(desktopPath, fileName);
        using (var writer = new StreamWriter(filePath))
        {
            writer.WriteLine(processedData.Count);
            for (var i = 0; i < processedData.Count; i++)
                if (i == processedData.Count - 1)
                    writer.Write(processedData[i]);
                else
                    writer.WriteLine(processedData[i]);
        }

        Message.ShowSnack();
    }

    private static void 交通部(MemoryStream ExcelData)
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
                var SN = worksheet.Cells[row, 1].Text;
                var ATS = worksheet.Cells[row, 2].Text;
                var newRow =
                    $"{SN}      {SN}      {ATS}                FFFFFFFFFFFFFFFFFFFF";
                processedData.Add(newRow);
            }
        }

        var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        var date = DateTime.Now.ToString("yyyyMMddHHmmss");
        var fileName = $"RCHG{date}000001.rcc";
        var filePath = Path.Combine(desktopPath, fileName);
        using (var writer = new StreamWriter(filePath))
        {
            writer.WriteLine("01");
            var number = processedData.Count.ToString().PadLeft(6, '0');
            writer.WriteLine($"ORD202401250912301542024012400020016{number}");

            for (var i = 0; i < processedData.Count; i++)
                if (i == processedData.Count - 1)
                    writer.Write(processedData[i]);
                else
                    writer.WriteLine(processedData[i]);
        }

        Message.ShowSnack();
    }
}