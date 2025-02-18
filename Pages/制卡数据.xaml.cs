using System.Text;
using System.Windows.Controls;
using System.Xml;
using Microsoft.Win32;

namespace WindowUI.Pages;

public partial class 制卡数据 : Page
{
    public 制卡数据()
    {
        InitializeComponent();
    }
    private string XdFilePath { get; set; }
    private string KeyFilePath { get; set; }
    private void SelectXdFile(object sender, RoutedEventArgs e)
    {
        // 注册编码提供程序
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        //打开一个文件选择器，类型为任意
        OpenFileDialog openFileDialog = new()
        {
            Filter = "xd files (*.xd)|*.xd",
            Title = "选择一个xd文件"
        };
        if (openFileDialog.ShowDialog() == true)
        {
            if (openFileDialog.FileName == "") return;
            XdFilePath = openFileDialog.FileName;
        }
    }
    private void SelectKeyFile(object sender, RoutedEventArgs e)
    {
        //打开一个文件选择器，类型为任意
        OpenFileDialog openFileDialog = new()
        {
            Filter = "xd files (*.key)|*.key",
            Title = "选择一个key文件"
        };
        if (openFileDialog.ShowDialog() == true)
        {
            if (openFileDialog.FileName == "") return;
            KeyFilePath = openFileDialog.FileName;
        }
    }
    private void CreateFile(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrEmpty(XdFilePath))
        {
            Message.ShowMessageBox("错误","请先选择一个xd文件","确认");
            return;
        }

        XmlDocument document = new XmlDocument();
        document.Load(XdFilePath);
        XmlElement rootElem = document.DocumentElement;

        //读取temple里的文件
        string sourceFilePath = "temple/淄博血站.mdb";
        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string destinationFilePath = Path.Combine(desktopPath, "淄博血站.mdb");
        //复制文件到桌面
        try
        {
            File.Copy(sourceFilePath, destinationFilePath, true);
            Console.WriteLine("File copied successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return;
        }

        readout(rootElem, destinationFilePath);
        readkey(KeyFilePath,destinationFilePath);
    }
    public static void readout(XmlElement rootElem, string destinationFilePath)
    {
        Dictionary<string, string> dicry = new Dictionary<string, string>();
        //每张卡的数据
        XmlNodeList lcard_data = rootElem.GetElementsByTagName("card_data"); //获取card_data子节点集合    

        foreach (XmlNode node in lcard_data)
        {
            //参数部分节点
            XmlNode gradesNode = node.SelectSingleNode("Pamater"); //通过SelectSingleNode方法获得当前节点下的grades子节点
            XmlNodeList lPamater = gradesNode.ChildNodes; //通过ChildNodes属性获得grades的所有一级子节点
            if (lPamater.Count > 0)
            {
                foreach (XmlNode node1 in lPamater)
                {
                    string strDGI = "";
                    string strValue = "";
                    strDGI = node1.Attributes["name"].Value;
                    strValue = node1.InnerText;
                    setValue(ref dicry, strDGI, strValue);
                }
            }


            //主目录下的密钥、数据
            gradesNode = node.SelectSingleNode("MF"); //通过SelectSingleNode方法获得当前节点下的grades子节点
            XmlNode gradesNode1 = gradesNode.SelectSingleNode("key_group");
            XmlNodeList lkey_group = gradesNode1.ChildNodes; //通过ChildNodes属性获得grades的所有一级子节点

            if (lkey_group.Count > 0)
            {
                foreach (XmlNode node1 in lkey_group)
                {
                    string strDGI = "";
                    string strValue = "";
                    //密钥
                    strDGI = node1.Attributes["name"].Value;
                    strValue = node1.InnerText;
                    setValue(ref dicry, strDGI, strValue);
                    //密钥校验值
                    string strreadout = node1.Attributes["md5"].Value;
                    setValue(ref dicry, strDGI + "CC", strreadout);
                }
            }

            XmlNode gradesNode2 = gradesNode.SelectSingleNode("data_group");
            XmlNodeList ldata_group = gradesNode2.ChildNodes; //通过ChildNodes属性获得grades的所有一级子节点   
            if (ldata_group.Count > 0)
            {
                foreach (XmlNode node1 in ldata_group)
                {
                    string strDGI = "";
                    string strValue = "";
                    //文件内容
                    strDGI = node1.Attributes["name"].Value;
                    strValue = node1.InnerText;
                    setValue(ref dicry, strDGI, strValue);
                }
            }

            //子目录EP下的密钥、文件内容
            gradesNode = node.SelectSingleNode("EP"); //通过SelectSingleNode方法获得当前节点下的grades子节点
            gradesNode1 = gradesNode.SelectSingleNode("key_group");
            lkey_group = gradesNode1.ChildNodes; //通过ChildNodes属性获得grades的所有一级子节点
            gradesNode2 = gradesNode.SelectSingleNode("data_group");
            ldata_group = gradesNode2.ChildNodes; //通过ChildNodes属性获得grades的所有一级子节点  

            if (lkey_group.Count > 0)
            {
                foreach (XmlNode node1 in lkey_group)
                {
                    string strDGI = "";
                    string strValue = "";
                    //密钥
                    strDGI = node1.Attributes["name"].Value;
                    strValue = node1.InnerText;
                    setValue(ref dicry, "F1" + strDGI, strValue);
                    //密钥校验值
                    string strreadout = node1.Attributes["md5"].Value;
                    setValue(ref dicry, "F1" + strDGI + "CC", strreadout);
                }
            }

            if (ldata_group.Count > 0)
            {
                foreach (XmlNode node1 in ldata_group)
                {
                    string strDGI = "";
                    string strValue = "";
                    //文件内容
                    strDGI = node1.Attributes["name"].Value;
                    strValue = node1.InnerText;
                    setValue(ref dicry, "F1" + strDGI, strValue);
                }
            }

            //子目录EP下的密钥、文件内容
            gradesNode = node.SelectSingleNode("TC"); //通过SelectSingleNode方法获得当前节点下的grades子节点
            gradesNode1 = gradesNode.SelectSingleNode("key_group");
            lkey_group = gradesNode1.ChildNodes; //通过ChildNodes属性获得grades的所有一级子节点
            gradesNode2 = gradesNode.SelectSingleNode("data_group");
            ldata_group = gradesNode2.ChildNodes; //通过ChildNodes属性获得grades的所有一级子节点  

            if (lkey_group.Count > 0)
            {
                foreach (XmlNode node1 in lkey_group)
                {
                    string strDGI = "";
                    string strValue = "";
                    //密钥
                    strDGI = node1.Attributes["name"].Value;
                    strValue = node1.InnerText;
                    setValue(ref dicry, "F2" + strDGI, strValue);
                    //密钥校验值
                    string strreadout = node1.Attributes["md5"].Value;
                    setValue(ref dicry, "F2" + strDGI + "CC", strreadout);
                }
            }

            if (ldata_group.Count > 0)
            {
                foreach (XmlNode node1 in ldata_group)
                {
                    string strDGI = "";
                    string strValue = "";
                    //文件内容
                    strDGI = node1.Attributes["name"].Value;
                    strValue = node1.InnerText;
                    setValue(ref dicry, "F2" + strDGI, strValue);
                }
            }

            ///////////////////////////////////////////////////////保存解析数据到数据库zhika
            string ss = dicry["卡号"];
            string ti = DateTime.Now.ToString();
            string sql =
                "insert into zhika(SN,DCCK,DCMK,EF05,EF0101,EF0102,F1DACK,F1DAMK1,F1DAMK2,F1PINUL,F1PINRE,F1DPK,F1DLK,F1TAC,F1ALOCK,F1AUNLOCK,F1PIN,F1EF15,F1EF16,F1EF1701,F1EF1702,F1EF1703,F1EF1704,F1EF1705,F1EF1706,F1EF1707,F1EF1708,F1EF1709,F2DACK,F2DAMK1,F2DAMK2,F2PINUL,F2PINRE,F2DPK,F2DLK,F2TAC,F2ALOCK,F2AUNLOCK,F2PIN,F2EF15,F2EF1701,F2EF1702,F2EF1703,F2EF1704,F2EF1705,F2EF1706,DCCK_CC,F2PIN_CC,TIME0)values('" +
                ss + "','" + dicry["主控"] + "','" + dicry["维护"] + "','" + dicry["ef05"] + "','" + dicry["ef01_01"] +
                "','" + dicry["ef01_02"] + "','" + dicry["F1主控"] + "','" + dicry["F1维护1"] + "','" + dicry["F1维护2"] +
                "','" + dicry["F1PIN解锁"] + "','" + dicry["F1PIN重装"] + "','" + dicry["F1消费"] + "','" + dicry["F1充值"] +
                "','" + dicry["F1TAC"] + "','" + dicry["F1应用锁定"] + "','" + dicry["F1应用解锁"] + "','" + dicry["F1PIN"] +
                "','" + dicry["F1ef15"] + "','" + dicry["F1ef16"] + "','" + dicry["F1ef17_01"] + "','" +
                dicry["F1ef17_02"] + "','" + dicry["F1ef17_03"] + "','" + dicry["F1ef17_04"] + "','" +
                dicry["F1ef17_05"] + "','" + dicry["F1ef17_06"] + "','" + dicry["F1ef17_07"] + "','" +
                dicry["F1ef17_08"] + "','" + dicry["F1ef17_09"] + "','" + dicry["F2主控"] + "','" + dicry["F2维护1"] +
                "','" + dicry["F2维护2"] + "','" + dicry["F2PIN解锁"] + "','" + dicry["F2PIN重装"] + "','" + dicry["F2消费"] +
                "','" + dicry["F2充值"] + "','" + dicry["F2TAC"] + "','" + dicry["F2应用锁定"] + "','" + dicry["F2应用解锁"] +
                "','" + dicry["F2PIN"] + "','" + dicry["F2ef15"] + "','" + dicry["F2ef17_01"] + "','" +
                dicry["F2ef17_02"] + "','" + dicry["F2ef17_03"] + "','" + dicry["F2ef17_04"] + "','" +
                dicry["F2ef17_05"] + "','" + dicry["F2ef17_06"] + "','" + dicry["主控CC"] + "','" + dicry["F2PINCC"] +
                "','" + ti + "')";
            Mdb.Select(destinationFilePath, sql);
            //清空字典
            dicry.Clear();
        }

        MessageBox.Show("数据已保存到桌面");
    }
    public static void setValue(ref Dictionary<string, string> dicry, string key, string val)
    {
        if (dicry.ContainsKey(key))
            dicry.Remove(key);
        dicry.Add(key, val.ToUpper());
    }
    private static void readkey(string KeyFilePath,string destinationFilePath)
    {
        //将二进制文件转为文本文件
        if (string.IsNullOrEmpty(KeyFilePath))
        {
            Console.WriteLine("Key file path is not set.");
            return;
        }
        try
        {
            byte[] buffer = File.ReadAllBytes(KeyFilePath);

            // 将字节数组转换为十六进制字符串
            string hexString = BitConverter.ToString(buffer).Replace("-", " ");
            Console.WriteLine("文件内容 (十六进制):");
            Console.WriteLine(hexString);
            string key = hexString.Replace(" ", string.Empty);
            string sql = $"update safekey set KK = '{key}'";
            Mdb.Select(destinationFilePath, sql);
        }
        catch (Exception ex)
        {
            Console.WriteLine("读取文件时出错: " + ex.Message);
        }
    }
}