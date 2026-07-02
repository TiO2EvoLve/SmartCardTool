using System.Text;
using WindowUI.Pages;

namespace WindowUI.Sort;

public class 玉林
{
    private static string mkFilePath;
    private static string mkFileName;

    public static void Run(string mdbPath)
    {
        //打开菜单
        玉林菜单 YuLin = new 玉林菜单();
        YuLin.ShowDialog();
        mkFilePath = YuLin.mkFilePath;
        mkFileName = YuLin.mkFileName;
        // MDB数据
        var cardInfos = ReadCardInfos(mdbPath);
        // 客户文件
        var cards = ReadCustomerCards(mkFilePath, out var taskInfo);

        // 数量检查
        if (cardInfos.Count != cards.Count)
        {
            Message.ShowMessageBox(
                "错误",
                $"数据数量不一致！\n\n" +
                $"MDB：{cardInfos.Count}\n" +
                $"客户文件：{cards.Count}");

            return;
        }

        var datetime = mkFileName.Split('_')[6];
        // 输出文件
        var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        var outputPath = Path.Combine(desktopPath, $"BCPWB_0000_{datetime}_I.txt");
        using var writer = new StreamWriter(outputPath, false, Encoding.UTF8);

        writer.WriteLine("task_id|card_type|task_sum|bank_id|");
        writer.WriteLine("task_id|data_seq|card_no|struct_main_type|struct_child_type|bankcardno|atr|rfatr|mifare_id|");
        writer.WriteLine("----------");
        writer.WriteLine(taskInfo.TrimEnd('|'));

        for (var i = 0; i < cards.Count; i++)
        {
            var card = cards[i];
            var info = cardInfos[i];

            writer.WriteLine(
                $"{card.TaskId}|{card.DataSeq}|{info.SerialNum}|" +
                $"{card.StructMainType}|{card.StructChildType}|||" +
                $"{info.ATS}|{info.UID}");
        }

        Message.ShowSnack();
    }

    /// <summary>
    /// 读取客户文件
    /// </summary>
    private static List<SourceCard> ReadCustomerCards(
        string filePath,
        out string taskInfo)
    {
        if (!File.Exists(filePath))
            throw new Exception("客户文件不存在！");

        var lines = File.ReadAllLines(filePath);

        if (lines.Length < 5)
            throw new Exception("客户文件格式错误！");

        taskInfo = lines[3];

        var result = new List<SourceCard>();

        foreach (var line in lines.Skip(4))
        {
            if (string.IsNullOrWhiteSpace(line))
                continue;

            var cols = line.Split('|');

            if (cols.Length < 5)
            {
                throw new Exception(
                    $"客户文件格式错误：\n{line}");
            }

            result.Add(new SourceCard
            {
                TaskId = cols[0],
                DataSeq = cols[1],
                StructMainType = cols[3],
                StructChildType = cols[4]
            });
        }

        return result;
    }

    /// <summary>
    /// 读取MDB中的卡信息
    /// </summary>
    private static List<CardInfo> ReadCardInfos(
        string mdbPath)
    {
        var serialNums = Mdb.Select(
            mdbPath,
            "select SerialNum from kahao order by SerialNum ASC");

        var atsList = Mdb.Select(
            mdbPath,
            "select ATS from kahao order by SerialNum ASC");

        var uidList = Mdb.Select(
            mdbPath,
            "select UID_16 from kahao order by SerialNum ASC");

        if (serialNums.Count != atsList.Count ||
            serialNums.Count != uidList.Count)
        {
            throw new Exception(
                "MDB数据数量不一致！");
        }

        var result = new List<CardInfo>();

        for (var i = 0; i < serialNums.Count; i++)
        {
            ValidateLength(
                serialNums[i],
                19,
                "SN");

            ValidateLength(
                atsList[i],
                32,
                "ATS");

            ValidateLength(
                uidList[i],
                8,
                "芯片号");

            result.Add(new CardInfo
            {
                SerialNum = serialNums[i],
                ATS = atsList[i],
                UID = uidList[i]
            });
        }

        return result;
    }

    /// <summary>
    /// 校验字段长度
    /// </summary>
    private static void ValidateLength(
        string value,
        int length,
        string fieldName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new Exception(
                $"{fieldName}为空！");
        }

        if (value.Length != length)
        {
            throw new Exception(
                $"{fieldName}长度错误！\n" +
                $"当前值：{value}\n" +
                $"实际长度：{value.Length}\n" +
                $"要求长度：{length}");
        }
    }
}

public class CardInfo
{
    /// <summary>
    /// 卡号
    /// </summary>
    public string SerialNum { get; set; } = string.Empty;

    /// <summary>
    /// ATS
    /// </summary>
    public string ATS { get; set; } = string.Empty;

    /// <summary>
    /// UID
    /// </summary>
    public string UID { get; set; } = string.Empty;
}

public class SourceCard
{
    public string TaskId { get; set; } = string.Empty;

    public string DataSeq { get; set; } = string.Empty;

    public string StructMainType { get; set; } = string.Empty;

    public string StructChildType { get; set; } = string.Empty;
}