﻿using WindowUI.Pages;

namespace WindowUI.Sort;

public class 淄博公交
{
    public static void Run(string FilePath)
    {
        string date; //日期,格式20241115112548
        string date1; //日期,格式2024-11-15
        string cardtype; //卡类型
        //打开二级窗口
        淄博菜单 zibo = new();
        zibo.ShowDialog();
        //获取二级窗口的数据
        cardtype = zibo.CardType;
        date = zibo.Date14;
        date1 = zibo.Date10;


        List<string> XmlData = new List<string>();
        var sql = "SELECT time1 FROM RCC order by SN ASC";
        XmlData = Mdb.Select(FilePath, sql);

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
            writer.WriteLine($"<Amount>{XmlData.Count}</Amount>");
            writer.WriteLine($"<GoodAmount>{XmlData.Count}</GoodAmount>");
            writer.WriteLine("<BadAmount>0</BadAmount>");
            writer.WriteLine("<InitOperator>000000</InitOperator>");
            writer.WriteLine($"<IssueDate>{date1}</IssueDate>");
            writer.WriteLine("<ValidDate>2040-12-31</ValidDate>");
            writer.WriteLine($"<RepeortDate>{date1}</RepeortDate>");
            writer.WriteLine("</Task>");
            writer.WriteLine("<CardList>");
            // 提取数据
            for (var i = 0; i < XmlData.Count; i++)
            {
                var cardUid = Tools.ExtractValue(XmlData[i], "CARDUID", "APPID");
                var appId = Tools.ExtractValue(XmlData[i], "APPID", "ISSUESN");
                var issueSn = Tools.ExtractValue(XmlData[i], "ISSUESN", "ISSUETIME");
                var issueTime = Tools.ExtractValue(XmlData[i], "ISSUETIME", "STATUS");
                issueTime = date.Substring(0, 8) + issueTime.Substring(8);
                // 创建 XML 格式字符串
                XmlData[i] =
                    $"<Card UID=\"{cardUid}\" AppID=\"{appId}\" IssueSN=\"{issueSn}\" IssueTime=\"{issueTime}\" Status=\"Good\"/>";
                writer.WriteLine(XmlData[i]);
            }

            writer.WriteLine("</CardList>");
            writer.Write("</TaskBack>");
        }

        Message.ShowSnack();
    }
}