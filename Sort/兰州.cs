
using WindowUI.Pages;

namespace WindowUI.Sort;

public class 兰州
{
    public static void Run(MemoryStream ExcelData,string excelFileName,List<string> MKData,string mkFileName)
    {
        兰州菜单 lanzhou = new ();
        lanzhou.ShowDialog();
        string cardtype = lanzhou.CardType;
        
        if(cardtype == "0")
        {
            兰州公交(ExcelData,excelFileName,MKData,mkFileName,0);
        }else if (cardtype == "1")
        {
            兰州公交(ExcelData,excelFileName,MKData,mkFileName,1);
        }
    }
    private static void 兰州公交(MemoryStream ExcelData,string excelFileName,List<string> MKData,string mkFileName,int type)
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
                string firstColumnValue = worksheet.Cells[row, 1].Text;
                string secondColumnValue = worksheet.Cells[row, 2].Text;
                string newRow =
                    $"{firstColumnValue}      {firstColumnValue}      {secondColumnValue}          00                         FFFFFFFFFFFFFFFFFFFF";
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
        string fileName = $"RC{mkFileName}001";
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
        if (type == 0)
        {
            Message.ShowSnack();
            return;
        } 
        //异型卡需要两个文件
        //第二个文件
        List<string> SNData = new List<string>();
        List<string> UIDData = new List<string>();
        //取出Excle文件的数据
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // 避免出现许可证错误
        using (var package = new ExcelPackage(ExcelData))
        {
            var worksheet = package.Workbook.Worksheets[0]; // 获取第一个工作表
            int rowCount = worksheet.Dimension.Rows; //获取行数
            //遍历Excel文件的每一行
            for (int row = 1; row <= rowCount; row++)
            {
                string firstColumnValue = worksheet.Cells[row, 8].Text;
                SNData.Add(firstColumnValue);
                firstColumnValue = worksheet.Cells[row, 3].Text;
                string firstColumnValue2 = Convert.ToUInt32(firstColumnValue, 16).ToString();
                UIDData.Add(firstColumnValue2);
            }
        }
        //将processedData和processedData2合并起来，中间用','分隔，最后保存为txt文件到桌面
        List<string> mergedData = new List<string>();
        for (int i = 0; i < SNData.Count; i++)
        {
            string mergedRow = $"{SNData[i]},{UIDData[i]}";
            mergedData.Add(mergedRow);
        }
        desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        fileName = excelFileName + ".txt";
        filePath = Path.Combine(desktopPath, fileName);
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            foreach (var line in mergedData)
            {
                writer.WriteLine(line);
            }
        }
        Message.ShowSnack();
    }
}