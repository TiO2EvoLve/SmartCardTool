namespace WindowUI.Sort;

public class 柳州公交
{
    public static void Run(MemoryStream ExcelData,List<string> MKData)
    {
        //根据逗号切割MKdate
        string[] KCdata = MKData[0].Split(';');
        Console.WriteLine(KCdata);
        string Order = KCdata[1];
        string CardBin = KCdata[5];
        string CardNumber = KCdata[4];
        string StartSN = KCdata[6];
        string EndSN = KCdata[7];
        // 取出Excel文件的数据
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // 避免出现许可证错误
        List<string> SNData = new List<string>();
        List<string> ATSData = new List<string>();

        using (var package = new ExcelPackage(ExcelData))
        {
            var worksheet = package.Workbook.Worksheets[0]; // 获取第一个工作表
            int rowCount = worksheet.Dimension.Rows; // 获取行数

            // 异步遍历Excel文件的每一行
            for (int row = 2; row <= rowCount; row++)
            {
                string SNValue = worksheet.Cells[row, 1].Text;
                string ATSValue = worksheet.Cells[row, 2].Text;
                SNData.Add(SNValue);
                ATSData.Add(ATSValue);
            }
        }

        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string date = Order.Substring(3, 8);
        string fileName = $"RC_{date}_54500000_0004_{Order}_{StartSN}_{CardNumber}";
        string filePath = Path.Combine(desktopPath, fileName);

        using (StreamWriter writer = new StreamWriter(filePath))
        {
            writer.WriteLine($"01;{Order};{CardBin};{StartSN};{EndSN};{CardNumber};");
            for (int i = 0; i < SNData.Count; i++)
            {
                if (i == SNData.Count - 1)
                {
                    writer.Write($"{SNData[i]};{SNData[i]};{ATSData[i]};");
                }
                else
                {
                    writer.WriteLine($"{SNData[i]};{SNData[i]};{ATSData[i]};");
                }
            }
        }
        Message.ShowSnack();
    }
}