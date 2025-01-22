using WindowUI.Pages;

namespace WindowUI.Sort;

public class 洪城
{
    public static void Run(MemoryStream ExcelData,List<string> MKData,string mkFileName)
    {
        //打开洪城菜单
        洪城菜单 hongCheng = new();
        hongCheng.ShowDialog();

        switch (hongCheng.Cardtype)
        {
            case "1208": 住建部(ExcelData);break;
            case "1280": 交通部(ExcelData,MKData,mkFileName);break;
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
        string date = DateTime.Now.ToString("yyyyMMddHHmmss");
        string fileName = $"住建部_回盘_山东华冠_{date}_001";
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
    }

    private static void 交通部(MemoryStream ExcelData,List<string> MKData,string mkFileName)
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
                    $"{SN}      {SN}      {ATS}              FFFFFFFFFFFFFFFFFFFF";
                processedData.Add(newRow);
            }
        }

        //处理MK文件
        //截取MK文件第二行的前42个字节
        MKData[1] = MKData[1].Substring(0, 42);
        //获取Excel总数据的条数
        int totalLines = processedData.Count;
        //将总数据条数转为6位数
        string totalLinesFormatted = totalLines.ToString("D6");
        //将MK文件的第二行的后6位替换为总数据条数
        MKData[1] = MKData[1].Substring(0, MKData[1].Length - 6) + totalLinesFormatted;
        //将MK文件与Excel文件的数据合并
        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string fileName = $"RCHG{mkFileName}001";
        string filePath = Path.Combine(desktopPath, fileName);
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            writer.WriteLine(MKData[0]);
            writer.WriteLine(MKData[1]);

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
    }
}