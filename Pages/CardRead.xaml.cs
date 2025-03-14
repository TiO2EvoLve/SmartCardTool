using System.Data.OleDb;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows.Controls;
using System.Windows.Media;

namespace WindowUI.Pages;

public partial class CardRead : Page
{
    private int hz = 115200;
    private int icdev;
    private string revalue = "";

    private int st;

    public CardRead()
    {
        InitializeComponent();
    }

    [DllImport("dcrf32.dll")]
    public static extern int dc_init(short port, int baud);

    [DllImport("dcrf32.dll")]
    public static extern short dc_beep(int icdev, uint _Msec);

    [DllImport("dcrf32.dll")]
    public static extern short dc_pro_resethex(int icdev, ref byte rlen, ref byte rbuff);

    [DllImport("dcrf32.dll")]
    public static extern short dc_card(int icdev, char _Mode, ref long Snr);

    [DllImport("dcrf32.dll")]
    public static extern short dc_pro_commandlink_hex(int icdev, byte len, ref byte sbuff, ref byte rlen,
        ref byte rbuff, byte tt, byte FG);

    [DllImport("dcrf32.dll")]
    public static extern short dc_reset(int icdev, int Msec);

    [DllImport("HDMATH20B.dll", CharSet = CharSet.Ansi)]
    private static extern ushort HD_3DES_Encrypt(string svHex, string svKey, short ivMode, ref byte srHex);

    private void OpenPort(object sender, RoutedEventArgs e)
    {
        try
        {
            var strport = port_input.Text;
            var Port = Convert.ToInt16(strport);

            st = dc_init(Port, hz);
            Console.WriteLine(st);
            if (st < 0)
            {
                Message.ShowMessageBox("失败", "打开端口失败");
            }
            else
            {
                port_show.Foreground = Brushes.LimeGreen;
                icdev = st;
                dc_beep(icdev, 10);
            }
        }
        catch (DllNotFoundException)
        {
            Message.ShowMessageBox("错误", "还没有设置dcrf32.dll");
        }
    }

    private void bt_start_Click(object sender, EventArgs e)
    {
        if (sn.Text == "")
        {
            Message.ShowMessageBox("警告", "请先输入起始流水号");
            return;
        }

        var thread = new Thread(ThreadProc);
        thread.Start();
    }

    private void ThreadProc()
    {
        while (true)
        {
            st = dc_reset(icdev, 2);
            if (st != 0)
            {
                Message.ShowMessageBox("错误", "dc_reset error!");
                return;
            }

            ////////////////////////////////////////////////////////////
            //寻卡
            long icCardNo = 0;
            st = dc_card(icdev, '0', ref icCardNo);
            if (st != 0)
            {
                tip_text.Text = "未寻到卡！";
                continue;
            }

            var strUID = Convert.ToString(icCardNo, 16).PadLeft(8, '0').ToUpper();
            var strUID_ = strUID;

            var x1 = strUID.Substring(0, 2);
            var x2 = strUID.Substring(2, 2);
            var x3 = strUID.Substring(4, 2);
            var x4 = strUID.Substring(6, 2);
            strUID = x4 + x3 + x2 + x1;

            uid16.Text = strUID;
            uid16_.Text = strUID_;

            var iUID = uint.Parse(strUID, NumberStyles.HexNumber);
            var strUID10 = iUID.ToString().PadLeft(10, '0'); //10进制不调整芯片号
            var iUID_ = uint.Parse(strUID_, NumberStyles.HexNumber);
            var strUID10_ = iUID_.ToString().PadLeft(10, '0'); //10进制调整芯片号

            uid10.Text = strUID10;
            uid10_.Text = strUID10_;

            ///////////////////////////////////////////////////////////
            //取复位信息
            byte crlen = 1;
            var recbuff = new byte[100];
            st = dc_pro_resethex(icdev, ref crlen, ref recbuff[0]);
            if (st != 0)
            {
                Message.ShowMessageBox("错误", "dc_pro_reset Card Error!");
                continue;
            }

            string textaaa = null;
            for (var w = 0; w < recbuff.Length; w++) textaaa += (char)recbuff[w];
            textaaa = textaaa.Replace("\0", "");
            ats.Text = textaaa;

            //复制模板到桌面
            var sourceFilePath = "temple/淄博血站.mdb";
            var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var destinationFilePath = Path.Combine(desktopPath, "淄博血站.mdb");

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

            var _connectionString = $@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={destinationFilePath};";

            //将结果保存到数据库
            using (var connection = new OleDbConnection(_connectionString))
            {
                connection.Open();
                var strNum = sn.Text;
                var sql = "insert into kahao(CARD_UID,CARD_UID_,CARD_UID10,CARD_UID10_,Card_ATS,NUM) values('" +
                          strUID + "','" + strUID_ + "','" + strUID10 + "','" + strUID10_ + "','" + textaaa + "','" +
                          strNum + "')";
                var command = new OleDbCommand(sql, connection);
                var reader = command.ExecuteReader();
            }

            tip_text.Text = "OK";
            dc_beep(icdev, 10);
        }
    }

    private void LocationComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (LocationComboBox.SelectedItem is ComboBoxItem selectedItem && selectedItem.Content != null)
            hz = int.Parse(selectedItem.Content.ToString());
    }
}