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
            case "1208": 住建部(ExcelData);break;
            case "1280": 交通部(ExcelData);break;
            case "all": 住建部(ExcelData);交通部(ExcelData);break;
            default:return;
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
            int rowCount = worksheet.Dimension.Rows; //获取行数
            //遍历Excel文件的每一行
            for (int row = 2; row <= rowCount; row++)
            {
                string ATS = worksheet.Cells[row, 2].Text;
                string code = worksheet.Cells[row, 11].Text;
                string newRow =
                    $"{ATS}                {code}        000033000000000000000000";
                processedData.Add(newRow);
            }
        }
        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string date = DateTime.Now.ToString("yyyyMMdd");
        string fileName = $"住建部_回盘_山东华冠_{date}_001.rcc";
        string filePath = Path.Combine(desktopPath, fileName);
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            writer.WriteLine(processedData.Count);
            for (int i = 0; i < processedData.Count; i++)
            {
                if (i == processedData.Count - 1)
                {
                    writer.Write(processedData[i]);
                }
                else
                {
                    writer.WriteLine(processedData[i]);
                }
            }
        }
        MessageBox.Show("住建部文件已保存到桌面");
    }

    private static void 交通部(MemoryStream ExcelData)
    {
        //先处理Excel文件
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // 避免出现许可证错误
        List<string> processedData = new List<string>();
        using (var package = new ExcelPackage(ExcelData))
        {
            var worksheet = package.Workbook.Worksheets[0]; // 获取第一个工作表
            int rowCount = worksheet.Dimension.Rows; //获取行数
            //遍历Excel文件的每一行
            for (int row = 2; row <= rowCount; row++)
            {
                string SN = worksheet.Cells[row, 1].Text;
                string ATS = worksheet.Cells[row, 2].Text;
                string newRow =
                    $"{SN}      {SN}      {ATS}                FFFFFFFFFFFFFFFFFFFF";
                processedData.Add(newRow);
            }
        }
        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string date = DateTime.Now.ToString("yyyyMMddHHmmss");
        string fileName = $"RCHG{date}000001.rcc";
        string filePath = Path.Combine(desktopPath, fileName);
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            writer.WriteLine("01");
            string number = processedData.Count.ToString().PadLeft(6, '0');
            writer.WriteLine($"ORD202401250912301542024012400020016{number}");

            for (int i = 0; i < processedData.Count; i++)
            {
                if (i == processedData.Count - 1)
                {
                    writer.Write(processedData[i]);
                }
                else
                {
                    writer.WriteLine(processedData[i]);
                }
            }
        }
        MessageBox.Show("交通部文件已保存到桌面");
    }
}