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

        // 取出Excel文件的数据
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // 避免出现许可证错误
        List<string> SNData = new List<string>();
        List<string> UIDData = new List<string>();
        string StartSN;
        string EndSN;

        var sql = "SELECT SerialNum FROM kahao order by SerialNum asc";
        List<string> SN = Mdb.Select(FilePath, sql);

        sql = "SELECT UID_16_ FROM kahao order by SerialNum asc";
        List<string> UID = Mdb.Select(FilePath, sql);

        Console.WriteLine(int.MaxValue);
        if (window.英才卡)
            for (var i = 0; i < SN.Count; i++)
            {
                var number = long.Parse(window.英才卡卡号);
                number += i;
                SN[i] = "31050714" + number.ToString("D" + window.英才卡卡号.Length);
            }

        StartSN = SN[0];
        EndSN = SN[SN.Count - 1];

        for (var i = 0; i < SN.Count; i++)
        {
            var SNValue = SN[i];
            var UIDValue = UID[i];
            //计算UID校验码
            var stra = UIDValue.Substring(0, 2);
            var strb = UIDValue.Substring(2, 2);
            var strc = UIDValue.Substring(4, 2);
            var strd = UIDValue.Substring(6, 2);
            var a = Convert.ToInt32(stra, 16);
            var b = Convert.ToInt32(strb, 16);
            var c = Convert.ToInt32(strc, 16);
            var d = Convert.ToInt32(strd, 16);
            var s = a ^ b ^ c ^ d;
            UIDValue += s.ToString("X").PadLeft(2, '0');
            UIDValue = UIDValue.ToUpper();
            //计算SN校验码
            var strNUM = SNValue + "F";
            var stre = strNUM.Substring(0, 2);
            var strf = strNUM.Substring(2, 2);
            var strg = strNUM.Substring(4, 2);
            var strh = strNUM.Substring(6, 2);
            var stri = strNUM.Substring(8, 2);
            var strj = strNUM.Substring(10, 2);
            var strk = strNUM.Substring(12, 2);
            var strl = strNUM.Substring(14, 2);
            var strm = strNUM.Substring(16, 2);
            var strn = strNUM.Substring(18, 2);
            var intnew = Convert.ToInt32(stre, 16) ^ Convert.ToInt32(strf, 16) ^ Convert.ToInt32(strg, 16) ^
                         Convert.ToInt32(strh, 16) ^ Convert.ToInt32(stri, 16) ^ Convert.ToInt32(strj, 16) ^
                         Convert.ToInt32(strk, 16) ^ Convert.ToInt32(strl, 16) ^ Convert.ToInt32(strm, 16) ^
                         Convert.ToInt32(strn, 16);
            var strXOR_2 = intnew.ToString("X").PadLeft(2, '0');
            SNValue = strNUM + strXOR_2;
            SNData.Add(SNValue);
            UIDData.Add(UIDValue);
        }

        var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        var fileName = $"CardNoHY{StartSN} - {EndSN}.xml";
        var filePath = Path.Combine(desktopPath, fileName);

        using (var writer = new StreamWriter(filePath))
        {
            writer.WriteLine("<?xml version=\"1.0\" encoding=\"GB2312\"?>");
            writer.WriteLine(
                $"<CardList Total=\"{SNData.Count}\" CardType=\"{cardtype}\" Start=\"{StartSN}\" End=\"{EndSN}\">");
            for (var j = 0; j < SNData.Count; j++)
                writer.WriteLine($"<Card UID=\"{UIDData[j]}\" AppID=\"{SNData[j]}\"/>");

            writer.Write("</CardList>");
        }

        Message.ShowSnack();
    }
}