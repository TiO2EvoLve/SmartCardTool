using System.Data.OleDb;
using System.Runtime.InteropServices;
using System.Windows.Controls;
using System.Windows.Media;

namespace WindowUI.Pages;

public partial class CardRead : Page
{
    
    [DllImport("dcrf32.dll")]
    public static extern int dc_init(Int16 port, Int32 baud);
    [DllImport("dcrf32.dll")]
    public static extern short dc_beep(int icdev, uint _Msec);
    [DllImport("dcrf32.dll")]
    public static extern short dc_pro_resethex(int icdev, ref byte rlen, ref byte rbuff);
    [DllImport("dcrf32.dll")]
    public static extern short dc_card(int icdev, char _Mode, ref long Snr);
    [DllImport("dcrf32.dll")]
    public static extern short dc_pro_commandlink_hex(int icdev, byte len, ref byte sbuff, ref byte rlen, ref byte rbuff, byte tt, byte FG);
    [DllImport("dcrf32.dll")]
    public static extern short dc_reset(int icdev,int Msec);
        
    [DllImport("HDMATH20B.dll", CharSet = CharSet.Ansi)]
    private static extern UInt16 HD_3DES_Encrypt(string svHex, string svKey, short ivMode, ref byte srHex);

    int st = 0;
    string revalue = "";
    int icdev;
    public CardRead()
    {
        InitializeComponent();
    }

    private void OpenPort(object sender, RoutedEventArgs e)
    {
        string strport = port_input.Text;
        short Port = Convert.ToInt16(strport);
        int hz = 115200;
        if (LocationComboBox.SelectedItem is ComboBoxItem selectedItem && selectedItem.DataContext != null)
        {
            hz = int.Parse(selectedItem.DataContext.ToString()) ;
        }
        
        Console.WriteLine($"{Port}---{hz}");
        st = dc_init(Port, hz);
        
        if (st <= 0)
        {
            Console.WriteLine(st);
            Message.ShowMessageBox("失败","打开端口失败");
        }
        else
        {
            port_show.Foreground = Brushes.LimeGreen;
            icdev = st;
            dc_beep(icdev, 10);
        }
    }
    private void bt_start_Click(object sender, EventArgs e)
    {
        if (sn.Text == "")
        {
            Message.ShowMessageBox("警告","请先输入起始流水号");
            return;
        }
        Thread thread = new Thread(ThreadProc);
        thread.Start(); 
    }
    private void ThreadProc()
        {
            while (true)
            {
                
                st = dc_reset(icdev, 2);
                if (st != 0)
                {
                    MessageBox.Show("dc_reset error!");
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
                string strUID= Convert.ToString(icCardNo,16).PadLeft(8,'0').ToUpper();
                string strUID_ = strUID;

                string x1 = strUID.Substring(0,2);
                string x2 = strUID.Substring(2, 2);
                string x3 = strUID.Substring(4, 2);
                string x4 = strUID.Substring(6, 2);
                strUID = x4 + x3 + x2 + x1;
                
                uid16.Text = strUID;
                uid16_.Text = strUID_;

                uint iUID = uint.Parse(strUID, System.Globalization.NumberStyles.HexNumber);
                string strUID10 = iUID.ToString().PadLeft(10, '0');//10进制不调整芯片号
                uint iUID_ = uint.Parse(strUID_, System.Globalization.NumberStyles.HexNumber);
                string strUID10_ = iUID_.ToString().PadLeft(10, '0');//10进制调整芯片号

                uid10.Text = strUID10;
                uid10_.Text = strUID10_;

                ///////////////////////////////////////////////////////////
                //取复位信息
                byte crlen = 1;
                byte[] recbuff = new byte[100];
                st = dc_pro_resethex(icdev, ref crlen, ref recbuff[0]);
                if (st != 0)
                {
                    MessageBox.Show("dc_pro_reset Card Error!");
                    continue;
                }
                string textaaa = null;
                for (int w = 0; w < recbuff.Length; w++)
                {
                    textaaa += (char)recbuff[w];
                }
                textaaa = textaaa.Replace("\0", "");
                ats.Text = textaaa;
                
                //复制模板到桌面
                string sourceFilePath = "temple/淄博血站.mdb";
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string destinationFilePath = Path.Combine(desktopPath, "淄博血站.mdb");
                
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
                string _connectionString = $@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={destinationFilePath};";

                //将结果保存到数据库
                using (OleDbConnection connection = new OleDbConnection(_connectionString))
                {
                    connection.Open();
                    string strNum = sn.Text;
                    string sql = "insert into kahao(CARD_UID,CARD_UID_,CARD_UID10,CARD_UID10_,Card_ATS,NUM) values('" + strUID + "','" + strUID_ + "','" + strUID10 + "','" + strUID10_ + "','" + textaaa + "','" + strNum + "')";
                    OleDbCommand command = new OleDbCommand(sql, connection);
                    OleDbDataReader reader = command.ExecuteReader();
                }
                tip_text.Text = "OK";
                dc_beep(icdev, 10);
            }

        }
    
}