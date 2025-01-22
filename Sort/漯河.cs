using WindowUI.Pages;

namespace WindowUI.Sort;

public class 漯河
{
    public static void Run(MemoryStream ExcelData)
    {
        string cardtype;
        漯河菜单 window = new();
        window.ShowDialog();
        cardtype = window.CardType;
        if (window.英才卡)
        {
            Style2(ExcelData,cardtype,window.CardSN);
            return;
        }
        // 取出Excel文件的数据
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // 避免出现许可证错误
        List<string> SNData = new List<string>();
        List<string> UIDData = new List<string>();
        string StartSN;
        string EndSN;
        using (var package = new ExcelPackage(ExcelData))
        {
            var worksheet = package.Workbook.Worksheets[0]; // 获取第一个工作表
            int rowCount = worksheet.Dimension.Rows; // 获取行数
            StartSN = worksheet.Cells[2, 1].Text;
            EndSN = worksheet.Cells[rowCount, 1].Text;
            for (int row = 2; row <= rowCount; row++)
            {
                string SNValue = worksheet.Cells[row, 1].Text;
                string UIDValue = worksheet.Cells[row, 4].Text;
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
        }

        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string fileName = $"CardNoHY{StartSN} - {EndSN}.xml";
        string filePath = Path.Combine(desktopPath, fileName);

        using (StreamWriter writer = new StreamWriter(filePath))
        {
            writer.WriteLine("<?xml version=\"1.0\" encoding=\"GB2312\"?>");
            writer.WriteLine(
                $"<CardList Total=\"{SNData.Count}\" CardType=\"{cardtype}\" Start=\"{StartSN}\" End=\"{EndSN}\">");
            for (int i = 0; i < SNData.Count; i++)
            {
                writer.WriteLine($"<Card UID=\"{UIDData[i]}\" AppID=\"{SNData[i]}\"/>");
            }

            writer.Write("</CardList>");
        }

        MessageBox.Show($"文件已保存到桌面{filePath}");
    }

    private static void Style2(MemoryStream ExcelData,string cardtype,string SN)
    {
        // 取出Excel文件的数据
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // 避免出现许可证错误
        List<string> UIDData = new List<string>();
        List<string> SNData = new List<string>();
        string StartSN;
        string EndSN;
        using (var package = new ExcelPackage(ExcelData))
        {
            var worksheet = package.Workbook.Worksheets[0]; // 获取第一个工作表
            int rowCount = worksheet.Dimension.Rows; // 获取行数
            StartSN = "31050714" + SN;
            EndSN = (Convert.ToInt64(StartSN) + rowCount - 2).ToString() ;
            for (int row = 2; row <= rowCount; row++)
            {
                string SNValue = (Convert.ToInt64(StartSN) + row - 2).ToString();
                string UIDValue = worksheet.Cells[row, 4].Text;
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
        }

        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string fileName = $"CardNoHY{StartSN} - {EndSN}.xml";
        string filePath = Path.Combine(desktopPath, fileName);

        using (StreamWriter writer = new StreamWriter(filePath))
        {
            writer.WriteLine("<?xml version=\"1.0\" encoding=\"GB2312\"?>");
            writer.WriteLine(
                $"<CardList Total=\"{SNData.Count}\" CardType=\"{cardtype}\" Start=\"{StartSN}\" End=\"{EndSN}\">");
            for (int i = 0; i < SNData.Count; i++)
            {
                writer.WriteLine($"<Card UID=\"{UIDData[i]}\" AppID=\"{SNData[i]}\"/>");
            }

            writer.Write("</CardList>");
        }

        MessageBox.Show($"文件已保存到桌面{filePath}");
    }
}