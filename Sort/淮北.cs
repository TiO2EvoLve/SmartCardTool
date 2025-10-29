namespace WindowUI.Sort;

public class 淮北
{
    public static void Run(string FilePath)
    {
        string cardtype;
        List<string> SNData = new ();
        List<string> UIDData = new ();
        string StartSN;
        string EndSN;
        
        //取出文件数据
        // using (var package = new ExcelPackage(ExcelData))
        // {
        //     var worksheet = package.Workbook.Worksheets[0]; // 获取第一个工作表
        //     var rowCount = worksheet.Dimension.Rows; // 获取行数
        //     // 遍历Excel文件的每一行
        //     for (var row = 2; row < rowCount; row++)
        //     {
        //         var SNValue = worksheet.Cells[row, 7].Text;
        //         var UidValue = worksheet.Cells[row, 2].Text;
        //         UidValue = Tools.ChangeHexPairs(UidValue);
        //         SNData.Add(SNValue);
        //         UIDData.Add(UidValue);
        //     }
        // }

        string sql = "Select SerialNum from kahao order by SerialNum asc";
        SNData = Mdb.Select(FilePath, sql);
        sql = "Select UID_16_ from kahao order by SerialNum asc";
        UIDData = Mdb.Select(FilePath, sql);

        StartSN = SNData[0];
        EndSN = SNData[^1];
        cardtype = StartSN.Substring(8, 2);
        
        for (var i = 0; i < SNData.Count; i++)
        {
            //计算UID校验码
            var stra = UIDData[i].Substring(0, 2);
            var strb = UIDData[i].Substring(2, 2);
            var strc = UIDData[i].Substring(4, 2);
            var strd = UIDData[i].Substring(6, 2);
            var a = Convert.ToInt32(stra, 16);
            var b = Convert.ToInt32(strb, 16);
            var c = Convert.ToInt32(strc, 16);
            var d = Convert.ToInt32(strd, 16);
            var s = a ^ b ^ c ^ d;
            UIDData[i] += s.ToString("X").PadLeft(2, '0');
            UIDData[i] = UIDData[i].ToUpper();
            //计算SN校验码
            SNData[i] += "F";
            var stre = SNData[i].Substring(0, 2);
            var strf = SNData[i].Substring(2, 2);
            var strg = SNData[i].Substring(4, 2);
            var strh = SNData[i].Substring(6, 2);
            var stri = SNData[i].Substring(8, 2);
            var strj = SNData[i].Substring(10, 2);
            var strk = SNData[i].Substring(12, 2);
            var strl = SNData[i].Substring(14, 2);
            var strm = SNData[i].Substring(16, 2);
            var strn = SNData[i].Substring(18, 2);
            var intnew = Convert.ToInt32(stre, 16) ^ Convert.ToInt32(strf, 16) ^ Convert.ToInt32(strg, 16) ^
                         Convert.ToInt32(strh, 16) ^ Convert.ToInt32(stri, 16) ^ Convert.ToInt32(strj, 16) ^
                         Convert.ToInt32(strk, 16) ^ Convert.ToInt32(strl, 16) ^ Convert.ToInt32(strm, 16) ^
                         Convert.ToInt32(strn, 16);
            var strXOR_2 = intnew.ToString("X").PadLeft(2, '0');
            SNData[i] += strXOR_2;
        }


        var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        var fileName = $"CardNoTM{StartSN}-{EndSN}.xml";
        var filePath = Path.Combine(desktopPath, fileName);

        using (var writer = new StreamWriter(filePath))
        {
            writer.WriteLine("<?xml version=\"1.0\" encoding=\"GB2312\"?>");
            writer.WriteLine(
                $"<CardList Total=\"{SNData.Count}\" CardType=\"{cardtype}\" Start=\"{StartSN}\" End=\"{EndSN}\">");
            for (var i = 0; i < SNData.Count; i++)
                writer.WriteLine($"<Card UID=\"{UIDData[i]}\" AppID=\"{SNData[i]}\"/>");

            writer.Write("</CardList>");
        }

        Message.ShowSnack();
    }
}