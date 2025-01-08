using WindowUI.Pages;
using WindowUI.Tool;

namespace WindowUI.Sort;

public class 淄博公交
{
    public static void Run(MemoryStream ExcelData)
    {
        string date;//日期,格式20241115112548
        string date1;//日期,格式2024-11-15
        string cardtype;//卡类型
        //打开二级窗口
        淄博菜单 zibo = new ();
        zibo.ShowDialog();
        //获取二级窗口的数据
        cardtype = zibo.CardType;
        date = zibo.Date14;
        date1 = zibo.Date10;
        //取出Excle文件的数据
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // 避免出现许可证错误
        List<string> xmlData = new List<string>();
        if (ExcelData == null && ExcelData.Length == 0) { MessageBox.Show("Excel数据为空"); return; }
        using (var package = new ExcelPackage(ExcelData))
        {
            var worksheet = package.Workbook.Worksheets[0]; // 获取第一个工作表
            int rowCount = worksheet.Dimension.Rows; //获取行数
            //遍历Excel文件的每一行
            for (int row = 2; row <= rowCount; row++)
            {
                string SNValue = worksheet.Cells[row, 3].Text;
                xmlData.Add(SNValue);
            }
        }
        //保存文件到桌面
        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string fileName = $"ACPU{date}_Report.xml";
        string filePath = Path.Combine(desktopPath, fileName);
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            writer.WriteLine("<?xml version=\"1.0\" encoding=\"GB2312\"?>");
            writer.WriteLine($"<TaskBack task=\"USER CARD\" TaskId=\"{date}\">");
            writer.WriteLine("<Task>");
            writer.WriteLine("<Type>ACPU</Type>");
            writer.WriteLine("<AppType>01</AppType>");
            writer.WriteLine($"<CardType>{cardtype}</CardType>");
            writer.WriteLine($"<Amount>{xmlData.Count}</Amount>");
            writer.WriteLine($"<GoodAmount>{xmlData.Count}</GoodAmount>");
            writer.WriteLine("<BadAmount>0</BadAmount>");
            writer.WriteLine("<InitOperator>000000</InitOperator>");
            writer.WriteLine($"<IssueDate>{date1}</IssueDate>");
            writer.WriteLine("<ValidDate>2040-12-31</ValidDate>");
            writer.WriteLine($"<RepeortDate>{date1}</RepeortDate>");
            writer.WriteLine("</Task>");
            writer.WriteLine("<CardList>");
            // 提取数据
            for (int i = 0; i < xmlData.Count; i++)
            {
                string cardUid = Tools.ExtractValue(xmlData[i], "CARDUID", "APPID");
                string appId = Tools.ExtractValue(xmlData[i], "APPID", "ISSUESN");
                string issueSn = Tools.ExtractValue(xmlData[i], "ISSUESN", "ISSUETIME");
                string issueTime = Tools.ExtractValue(xmlData[i], "ISSUETIME", "STATUS");
                issueTime = date.Substring(0, 8) + issueTime.Substring(8);
                // 创建 XML 格式字符串
                string status = "Good"; // 假设默认状态为 "Good"
                xmlData[i] = $"<Card UID=\"{cardUid}\" AppID=\"{appId}\" IssueSN=\"{issueSn}\" IssueTime=\"{issueTime}\" Status=\"{status}\"/>";
                writer.WriteLine(xmlData[i]);
            }
            writer.WriteLine("</CardList>");
            writer.Write("</TaskBack>");
        }
        MessageBox.Show($"数据已合并并保存到文件: {filePath}");
    }
}