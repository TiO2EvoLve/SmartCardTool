using System.Text;
using System.Windows.Controls;
using System.Xml;
using Microsoft.Win32;
namespace WindowUI.Pages;

public partial class 制卡数据 : Page
{
    private readonly OpenFileDialog _openFileDialog = new();

    public 制卡数据()
    {
        InitializeComponent();
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
    }

    private string XdFilePath { get; set; }
    private string KeyFilePath { get; set; }

    //选择xd文件
    private void SelectXdFile(object sender, RoutedEventArgs e)
    {
        _openFileDialog.Filter = "xd files (*.xd)|*.xd";
        _openFileDialog.Title = "选择一个xd文件";
        if (_openFileDialog.ShowDialog() == true && !string.IsNullOrEmpty(_openFileDialog.FileName))
            XdFilePath = _openFileDialog.FileName;
    }
    //选择key文件
    private void SelectKeyFile(object sender, RoutedEventArgs e)
    {
        _openFileDialog.Filter = "xd files (*.key)|*.key";
        _openFileDialog.Title = "选择一个key文件";
        if (_openFileDialog.ShowDialog() == true && !string.IsNullOrEmpty(_openFileDialog.FileName))
            KeyFilePath = _openFileDialog.FileName;
    }
    //复制数据库到桌面
    private async void CreateFile(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrEmpty(XdFilePath) || string.IsNullOrEmpty(KeyFilePath))
        {
            Message.ShowMessageBox("错误", "请先选择文件");
            return;
        }

        XmlDocument document = new();
        document.Load(XdFilePath);
        var rootElem = document.DocumentElement;

        var sourceFilePath = "temple/淄博血站.mdb";
        var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        var destinationFilePath = Path.Combine(desktopPath, "淄博血站.mdb");

        try
        {
            await Task.Run(() => File.Copy(sourceFilePath, destinationFilePath, true));
            Console.WriteLine("文件已复制到桌面");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return;
        }

        await Task.Run(() => Readout(rootElem, destinationFilePath));
        await Task.Run(() => ReadKey(KeyFilePath, destinationFilePath));
    }

    public static void Readout(XmlElement rootElem, string destinationFilePath)
    {
        var dicryList = new List<Dictionary<string, string>>();
        var lcardData = rootElem.GetElementsByTagName("card_data");

        foreach (XmlNode node in lcardData)
        {
            var dicry = new Dictionary<string, string>();
            ProcessNode(node, "Pamater", dicry);
            ProcessNode(node, "MF", dicry, "key_group", "data_group");
            ProcessNode(node, "EP", dicry, "key_group", "data_group", "F1");
            ProcessNode(node, "TC", dicry, "key_group", "data_group", "F2");
            dicryList.Add(dicry);
        }

        BatchSaveToDatabase(dicryList, destinationFilePath);
        MessageBox.Show("数据生成成功!!!");
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
                "insert into zhika(SN,DCCK,DCMK,EF05,EF0101,EF0102,F1DACK,F1DAMK1,F1DAMK2,F1PINUL,F1PINRE,F1DPK,F1DLK,F1TAC,F1ALOCK,F1AUNLOCK,F1PIN,F1EF15,F1EF16,F1EF1701,F1EF1702,F1EF1703,F1EF1704,F1EF1705,F1EF1706,F1EF1707,F1EF1708,F1EF1709,F2DACK,F2DAMK1,F2DAMK2,F2PINUL,F2PINRE,F2DPK,F2DLK,F2TAC,F2ALOCK,F2AUNLOCK,F2PIN,F2EF15,F2EF1701,F2EF1702,F2EF1703,F2EF1704,F2EF1705,F2EF1706,DCCK_CC,F2PIN_CC,TIME0)values('");
            sql.Append(dicry["卡号"]).Append("','")
                .Append(dicry["主控"]).Append("','")
                .Append(dicry["维护"]).Append("','")
                .Append(dicry["ef05"]).Append("','")
                .Append(dicry["ef01_01"]).Append("','")
                .Append(dicry["ef01_02"]).Append("','")
                .Append(dicry["F1主控"]).Append("','")
                .Append(dicry["F1维护1"]).Append("','")
                .Append(dicry["F1维护2"]).Append("','")
                .Append(dicry["F1PIN解锁"]).Append("','")
                .Append(dicry["F1PIN重装"]).Append("','")
                .Append(dicry["F1消费"]).Append("','")
                .Append(dicry["F1充值"]).Append("','")
                .Append(dicry["F1TAC"]).Append("','")
                .Append(dicry["F1应用锁定"]).Append("','")
                .Append(dicry["F1应用解锁"]).Append("','")
                .Append(dicry["F1PIN"]).Append("','")
                .Append(dicry["F1ef15"]).Append("','")
                .Append(dicry["F1ef16"]).Append("','")
                .Append(dicry["F1ef17_01"]).Append("','")
                .Append(dicry["F1ef17_02"]).Append("','")
                .Append(dicry["F1ef17_03"]).Append("','")
                .Append(dicry["F1ef17_04"]).Append("','")
                .Append(dicry["F1ef17_05"]).Append("','")
                .Append(dicry["F1ef17_06"]).Append("','")
                .Append(dicry["F1ef17_07"]).Append("','")
                .Append(dicry["F1ef17_08"]).Append("','")
                .Append(dicry["F1ef17_09"]).Append("','")
                .Append(dicry["F2主控"]).Append("','")
                .Append(dicry["F2维护1"]).Append("','")
                .Append(dicry["F2维护2"]).Append("','")
                .Append(dicry["F2PIN解锁"]).Append("','")
                .Append(dicry["F2PIN重装"]).Append("','")
                .Append(dicry["F2消费"]).Append("','")
                .Append(dicry["F2充值"]).Append("','")
                .Append(dicry["F2TAC"]).Append("','")
                .Append(dicry["F2应用锁定"]).Append("','")
                .Append(dicry["F2应用解锁"]).Append("','")
                .Append(dicry["F2PIN"]).Append("','")
                .Append(dicry["F2ef15"]).Append("','")
                .Append(dicry["F2ef17_01"]).Append("','")
                .Append(dicry["F2ef17_02"]).Append("','")
                .Append(dicry["F2ef17_03"]).Append("','")
                .Append(dicry["F2ef17_04"]).Append("','")
                .Append(dicry["F2ef17_05"]).Append("','")
                .Append(dicry["F2ef17_06"]).Append("','")
                .Append(dicry["主控CC"]).Append("','")
                .Append(dicry["F2PINCC"]).Append("','")
                .Append(DateTime.Now.ToString()).Append("')");
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