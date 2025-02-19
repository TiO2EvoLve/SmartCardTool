namespace WindowUI.Sort;

public class 重庆
{
    public static void Run(MemoryStream ExcelData,string excelFileName)
    {
        //取出Excle文件的数据
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // 避免出现许可证错误
        List<string> SNData = new List<string>();
        List<string> ATSData = new List<string>();
        using (var package = new ExcelPackage(ExcelData))
        {
            var worksheet = package.Workbook.Worksheets[0]; // 获取第一个工作表
            int rowCount = worksheet.Dimension.Rows; //获取行数
            //遍历Excel文件的每一行
            for (int row = 2; row <= rowCount; row++)
            {
                string SNValue = worksheet.Cells[row, 11].Text;
                string ATSValue = worksheet.Cells[row, 2].Text;
                SNData.Add(SNValue);
                ATSData.Add(ATSValue);

            }
        }

        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string fileName = $"HG-{excelFileName}";
        string filePath = Path.Combine(desktopPath, fileName);
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            writer.WriteLine(ATSData.Count);
            for (int i = 0; i < ATSData.Count; i++)
            {
                if (i == ATSData.Count - 1)
                {
                    writer.Write(SNData[i] + ";" + SNData[i] + ";" + ATSData[i] + ";");
                }
                else
                {
                    writer.WriteLine(SNData[i] + ";" + SNData[i] + ";" + ATSData[i] + ";");
                }
            }
        }
        Message.ShowSnack();       
    }
}