using System.Text;
using System.Xml;

namespace WindowUI.Pages.DataParse;

public class 淮南
{
    public static async Task Parse(string XdFilePath, string KeyFilePath)
    {

        if (string.IsNullOrEmpty(XdFilePath) || string.IsNullOrEmpty(KeyFilePath))
        {
            Message.ShowMessageBox("错误", "请先选择文件");
            return;
        }

        XmlDocument document = new();
        document.Load(XdFilePath);
        var rootElem = document.DocumentElement;

        var sourceFilePath = "temple/淮南公交.mdb";
        var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        var destinationFilePath = Path.Combine(desktopPath, "淮南公交.mdb");

        try
        {
            await Task.Run(() => File.Copy(sourceFilePath, destinationFilePath, true));
        }
        catch (Exception ex)
        {
            return;
        }

        await Task.Run(() => Readout(rootElem, destinationFilePath));
        await Task.Run(() => ReadKey(KeyFilePath, destinationFilePath));
        MessageBox.Show("数据生成成功!!!");
    }

    public static void Readout(XmlElement rootElem, string destinationFilePath)
    {
        var dicryList = new List<Dictionary<string, string>>();
        var lcardData = rootElem.GetElementsByTagName("card_data");

        foreach (XmlNode node in lcardData)
        {
            var dicry = new Dictionary<string, string>();
            ProcessNode(node, "Pamater", dicry);
            ProcessNode(node, "EP", dicry, "key_group", "data_group", "F0");
            ProcessNode(node, "TC", dicry, "key_group", "data_group", "F2");
            dicryList.Add(dicry);
        }
        BatchSaveToDatabase(dicryList, destinationFilePath);
    }

    private static void ProcessNode(XmlNode node, string nodeName, Dictionary<string, string> dicry,
        string keyGroup = null, string dataGroup = null, string prefix = "")
    {
        var gradesNode = node.SelectSingleNode(nodeName);
        if (gradesNode == null) return;

        if (keyGroup != null && dataGroup != null)
        {
            ProcessGroup(gradesNode, keyGroup, dicry, prefix);
            ProcessGroup(gradesNode, dataGroup, dicry, prefix);
        }
        else
        {
            var lPamater = gradesNode.ChildNodes;
            foreach (XmlNode node1 in lPamater)
            {
                var strDGI = node1.Attributes["name"].Value;
                var strValue = node1.InnerText;
                SetValue(dicry, strDGI, strValue);
            }
        }
    }

    private static void ProcessGroup(XmlNode gradesNode, string groupName, Dictionary<string, string> dicry,
        string prefix)
    {
        var groupNode = gradesNode.SelectSingleNode(groupName);
        if (groupNode == null) return;

        var lGroup = groupNode.ChildNodes;
        foreach (XmlNode node1 in lGroup)
        {
            var strDGI = node1.Attributes["name"].Value;
            var strValue = node1.InnerText;
            SetValue(dicry, prefix + strDGI, strValue);

            if (groupName == "key_group")
            {
                var strreadout = node1.Attributes["md5"].Value;
                SetValue(dicry, prefix + strDGI + "CC", strreadout);
            }
        }
    }

    private static void BatchSaveToDatabase(List<Dictionary<string, string>> dicryList, string destinationFilePath)
    {
        var sqlList = new List<string>();
        foreach (var dicry in dicryList)
        {
            var sql = new StringBuilder(
                "insert into zhika(SN,F0主控,F0维护1,F0维护2,F0维护3,F0锁卡,F0解锁,F0消费1,F0消费2,F0圈存1,F0圈存2,F0TAC1,F0TAC2,F0圈提,F0透支,F0ef05,F0ef15,F0ef16,F0ef17,F0ef18_01,F0ef18_02,F0ef18_03,F0ef18_04,F0ef18_05,F0ef18_06,F0ef18_07,F0ef18_08,F0ef18_09,F0ef18_0A,F0ef19_01,F0ef19_02,F0ef19_03,F0ef19_04,F0ef19_05,F0ef19_06,F0ef19_07,F2主控,F2维护1,F2维护2,F2锁卡,F2解锁,F2消费1,F2消费2,F2圈存1,F2圈存2,F2TAC1,F2TAC2,F2圈提1,F2圈提2,F2透支,F2ef15,F2ef1A_01,F2ef1A_02,F2ef19_01,F2ef19_02,F2ef19_03,F2ef19_04,F2ef19_05,F2ef19_06,F2ef19_07,F0主控CC,F2主控CC)values('");
            sql.Append(dicry["卡号"]).Append("','")
                .Append(dicry["F0主控"]).Append("','")
                .Append(dicry["F0维护1"]).Append("','")
                .Append(dicry["F0维护2"]).Append("','")
                .Append(dicry["F0维护3"]).Append("','")
                .Append(dicry["F0锁卡"]).Append("','")
                .Append(dicry["F0解锁"]).Append("','")
                .Append(dicry["F0消费1"]).Append("','")
                .Append(dicry["F0消费2"]).Append("','")
                .Append(dicry["F0圈存1"]).Append("','")
                .Append(dicry["F0圈存2"]).Append("','")
                .Append(dicry["F0TAC1"]).Append("','")
                .Append(dicry["F0TAC2"]).Append("','")
                .Append(dicry["F0圈提"]).Append("','")
                .Append(dicry["F0透支"]).Append("','")
                .Append(dicry["F0ef05"]).Append("','")
                .Append(dicry["F0ef15"]).Append("','")
                .Append(dicry["F0ef16"]).Append("','")
                .Append(dicry["F0ef17"]).Append("','")
                .Append(dicry["F0ef18_01"]).Append("','")
                .Append(dicry["F0ef18_02"]).Append("','")
                .Append(dicry["F0ef18_03"]).Append("','")
                .Append(dicry["F0ef18_04"]).Append("','")
                .Append(dicry["F0ef18_05"]).Append("','")
                .Append(dicry["F0ef18_06"]).Append("','")
                .Append(dicry["F0ef18_07"]).Append("','")
                .Append(dicry["F0ef18_08"]).Append("','")
                .Append(dicry["F0ef18_09"]).Append("','")
                .Append(dicry["F0ef18_0A"]).Append("','")
                .Append(dicry["F0ef19_01"]).Append("','")
                .Append(dicry["F0ef19_02"]).Append("','")
                .Append(dicry["F0ef19_03"]).Append("','")
                .Append(dicry["F0ef19_04"]).Append("','")
                .Append(dicry["F0ef19_05"]).Append("','")
                .Append(dicry["F0ef19_06"]).Append("','")
                .Append(dicry["F0ef19_07"]).Append("','")
                .Append(dicry["F2主控"]).Append("','")
                .Append(dicry["F2维护1"]).Append("','")
                .Append(dicry["F2维护2"]).Append("','")
                .Append(dicry["F2锁卡"]).Append("','")
                .Append(dicry["F2解锁"]).Append("','")
                .Append(dicry["F2消费1"]).Append("','")
                .Append(dicry["F2消费2"]).Append("','")
                .Append(dicry["F2圈存1"]).Append("','")
                .Append(dicry["F2圈存2"]).Append("','")
                .Append(dicry["F2TAC1"]).Append("','")
                .Append(dicry["F2TAC2"]).Append("','")
                .Append(dicry["F2圈提1"]).Append("','")
                .Append(dicry["F2圈提2"]).Append("','")
                .Append(dicry["F2透支"]).Append("','")
                .Append(dicry["F2ef15"]).Append("','")
                .Append(dicry["F2ef1A_01"]).Append("','")
                .Append(dicry["F2ef1A_02"]).Append("','")
                .Append(dicry["F2ef19_01"]).Append("','")
                .Append(dicry["F2ef19_02"]).Append("','")
                .Append(dicry["F2ef19_03"]).Append("','")
                .Append(dicry["F2ef19_04"]).Append("','")
                .Append(dicry["F2ef19_05"]).Append("','")
                .Append(dicry["F2ef19_06"]).Append("','")
                .Append(dicry["F2ef19_07"]).Append("','")
                .Append(dicry["F0主控CC"]).Append("','")
                .Append(dicry["F2主控CC"]).Append("')");
            sqlList.Add(sql.ToString());
        }
        Mdb.ExecuteBatch(destinationFilePath, sqlList);
    }

    public static void SetValue(Dictionary<string, string> dicry, string key, string val)
    {
        if (dicry.ContainsKey(key))
            dicry.Remove(key);
        dicry.Add(key, val.ToUpper());
    }

    private static void ReadKey(string keyFilePath, string destinationFilePath)
    {
        if (string.IsNullOrEmpty(keyFilePath))
        {
            Message.ShowMessageBox("错误", "请先选择一个key文件");
            return;
        }

        try
        {
            var buffer = File.ReadAllBytes(keyFilePath);
            var hexString = BitConverter.ToString(buffer).Replace("-", "");
            var sql = $"update safekey set KK = '{hexString}'";
            Mdb.Execute(destinationFilePath, sql);
        }
        catch (Exception ex)
        {
            Message.ShowMessageBox("错误", "读取文件时出错: " + ex.Message);
        }
    }
}