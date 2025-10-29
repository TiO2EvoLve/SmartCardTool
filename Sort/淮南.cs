using WindowUI.Pages;

namespace WindowUI.Sort;

public class 淮南
{
    public static void Run(string FilePath)
    {
        string date; //日期,格式20241115112548
        string date1; //日期,格式2024-11-15
        string cardtype; //卡类型
        //打开二级窗口
        淮南菜单 zibo = new();
        zibo.ShowDialog();
        //获取二级窗口的数据
        cardtype = zibo.CardType;
        date = zibo.Date14;
        date1 = zibo.Date10;


        List<string> SN = new List<string>();
        List<string> UID = new List<string>();
        string issueTime = "2025-06-24";
        string issueTime2 = "20250624";
        
        var sql = "SELECT SerialNum FROM kahao order by SerialNum ASC";
        SN = Mdb.Select(FilePath, sql);
        sql = "SELECT UID_16_ FROM kahao order by SerialNum ASC";
        UID = Mdb.Select(FilePath, sql);
        
        //保存文件到桌面
        var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        var fileName = $"ACPU{date}_Report.xml";
        var filePath = Path.Combine(desktopPath, fileName);
        using (var writer = new StreamWriter(filePath))
        {
            writer.WriteLine("<?xml version=\"1.0\" encoding=\"GB2312\"?>");
            writer.WriteLine($"<TaskBack task=\"USER CARD\" TaskId=\"{date}\">");
            writer.WriteLine("<Task>");
            writer.WriteLine("<Type>ACPU</Type>");
            writer.WriteLine("<AppType>01</AppType>");
            writer.WriteLine($"<CardType>{cardtype}</CardType>");
            writer.WriteLine($"<Amount>{SN.Count}</Amount>");
            writer.WriteLine($"<GoodAmount>{SN.Count}</GoodAmount>");
            writer.WriteLine("<BadAmount>0</BadAmount>");
            writer.WriteLine("<InitOperator>000000</InitOperator>");
            writer.WriteLine($"<IssueDate>{issueTime}</IssueDate>");
            writer.WriteLine("<ValidDate>2040-12-31</ValidDate>");
            writer.WriteLine($"<RepeortDate>{issueTime}</RepeortDate>");
            writer.WriteLine("</Task>");
            writer.WriteLine("<CardList>");
            // 提取数据
            for (var i = 0; i < SN.Count; i++)
            {
                SN[i] =
                    $"<Card UID=\"{UID[i]}\" AppID=\"{SN[i]}\" IssueSN=\"{SN[i]}\" IssueTime=\"{issueTime2}000000\" Status=\"Good\"/>";
                writer.WriteLine(SN[i]);
            }

            writer.WriteLine("</CardList>");
            writer.Write("</TaskBack>");
        }

        Message.ShowSnack();
    }
}