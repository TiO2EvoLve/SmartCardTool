using System.Windows.Documents;
using WindowUI.Pages;

namespace WindowUI.Sort;

public class 漯河
{
    public static void Run(string FilePath)
    {
        string cardtype;
        漯河菜单 window = new();
        window.ShowDialog();
        cardtype = window.CardType;
        //判断是否是英才卡
        
        // 取出Excel文件的数据
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // 避免出现许可证错误
        List<string> SNData = new List<string>();
        List<string> UIDData = new List<string>();
        string StartSN;
        string EndSN;

        string sql = "SELECT SerialNum FROM kahao";
        List<string> SN = Mdb.Select(FilePath, sql);

        sql = "SELECT UID_16_ FROM kahao";
        List<string> UID = Mdb.Select(FilePath, sql);

        if (window.英才卡)
        {
            for (int i = 0; i < SN.Count; i++)
            {
                SN[i] = window.英才卡卡号 + i + 1;
            }
        }

        StartSN = SN[0];
        EndSN = SN[SN.Count - 1];

        for (int i = 0; i < SN.Count; i++)
        {
            string SNValue = SN[i];
            string UIDValue = UID[i];
            //计算UID校验码
            string stra = UIDValue.Substring(0, 2);
            string strb = UIDValue.Substring(2, 2);
            string strc = UIDValue.Substring(4, 2);
            string strd = UIDValue.Substring(6, 2);
            int a = Convert.ToInt32(stra, 16);
            int b = Convert.ToInt32(strb, 16);
            int c = Convert.ToInt32(strc, 16);
            int d = Convert.ToInt32(strd, 16);
            int s = a ^ b ^ c ^ d;
            UIDValue += s.ToString("X").PadLeft(2, '0');
            UIDValue = UIDValue.ToUpper();
            //计算SN校验码
            string strNUM = SNValue + "F";
            string stre = strNUM.Substring(0, 2);
            string strf = strNUM.Substring(2, 2);
            string strg = strNUM.Substring(4, 2);
            string strh = strNUM.Substring(6, 2);
            string stri = strNUM.Substring(8, 2);
            string strj = strNUM.Substring(10, 2);
            string strk = strNUM.Substring(12, 2);
            string strl = strNUM.Substring(14, 2);
            string strm = strNUM.Substring(16, 2);
            string strn = strNUM.Substring(18, 2);
            Int32 intnew = (Convert.ToInt32(stre, 16) ^ Convert.ToInt32(strf, 16) ^ Convert.ToInt32(strg, 16) ^
                            Convert.ToInt32(strh, 16) ^ Convert.ToInt32(stri, 16) ^ Convert.ToInt32(strj, 16) ^
                            Convert.ToInt32(strk, 16) ^ Convert.ToInt32(strl, 16) ^ Convert.ToInt32(strm, 16) ^
                            Convert.ToInt32(strn, 16));
            string strXOR_2 = intnew.ToString("X").PadLeft(2, '0');
            SNValue = strNUM + strXOR_2;
            SNData.Add(SNValue);
            UIDData.Add(UIDValue);
        }

        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string fileName = $"CardNoHY{StartSN} - {EndSN}.xml";
        string filePath = Path.Combine(desktopPath, fileName);

        using (StreamWriter writer = new StreamWriter(filePath))
        {
            writer.WriteLine("<?xml version=\"1.0\" encoding=\"GB2312\"?>");
            writer.WriteLine(
                $"<CardList Total=\"{SNData.Count}\" CardType=\"{cardtype}\" Start=\"{StartSN}\" End=\"{EndSN}\">");
            for (int j = 0; j < SNData.Count - 1; j++)
            {
                writer.WriteLine($"<Card UID=\"{UIDData[j]}\" AppID=\"{SNData[j]}\"/>");
            }

            writer.Write("</CardList>");
        }

        MessageBox.Show($"文件已保存到桌面{filePath}");
    }
}