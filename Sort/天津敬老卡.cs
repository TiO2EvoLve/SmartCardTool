namespace WindowUI.Sort;

public class 天津敬老卡
{
    public static void Run(MemoryStream ExcelData)
    {
        List<string> 身份证号 = new List<string>();
        List<string> 老年卡号 = new List<string>();
        List<string> 申请订单号 = new List<string>();
        List<string> 卡面号 = new List<string>();
        using (var package = new ExcelPackage(ExcelData))
        {
            var worksheet = package.Workbook.Worksheets[0]; // 获取第一个工作表
            var rowCount = worksheet.Dimension.Rows; // 获取行数
            // 遍历Excel文件的每一行
            for (var row = 2; row <= rowCount; row++)
            {
                var 身份证号数据 = worksheet.Cells[row, 3].Text;
                var 老年卡号数据 = worksheet.Cells[row, 4].Text;
                var 卡号数据 = worksheet.Cells[row, 9].Text;
                var 订单号数据 = worksheet.Cells[row, 7].Text;
                
                身份证号.Add(身份证号数据.PadRight(18,' '));
                老年卡号.Add(老年卡号数据.PadRight(8,' '));
                卡面号.Add(卡号数据.Replace(" ","").PadRight(20,' '));
                申请订单号.Add(订单号数据.PadRight(25,' '));
            }
        }
        
        var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        var fileTime = DateTime.Now.ToString("yyyyMMdd");
        var 卡商批次号 = $"1{fileTime.Substring(3,5)}";
        var fileName = $"IMP-{卡商批次号}-{fileTime}.txt";
        var filePath = Path.Combine(desktopPath, fileName);
        using (var writer = new StreamWriter(filePath))
        {
            for (var i = 0; i < 身份证号.Count; i++)
                if (i == 身份证号.Count - 1)
                    writer.Write($"{身份证号[i]}\t{老年卡号[i]}\t{卡面号[i]}\t{申请订单号[i]}\t{卡商批次号,-20}");
                else
                    writer.WriteLine($"{身份证号[i]}\t{老年卡号[i]}\t{卡面号[i]}\t{申请订单号[i]}\t{卡商批次号,-20}");
        }
        Message.ShowSnack();
        
        
        
       
        
    }
}