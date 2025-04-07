namespace WindowUI.Sort;

public class 淮北
{
    public static void Run(MemoryStream ExcelData)
    {
        string cardtype;
        // 取出Excel文件的数据
        
        List<string> SNData = new List<string>();
        List<string> UIDData = new List<string>();
        string StartSN;
        string EndSN;
        using (var package = new ExcelPackage(ExcelData))
        {
            var worksheet = package.Workbook.Worksheets[0]; // 获取第一个工作表
            var rowCount = worksheet.Dimension.Rows; // 获取行数
            StartSN = worksheet.Cells[2, 1].Text;
            EndSN = worksheet.Cells[rowCount, 1].Text;
            cardtype = StartSN.Substring(8, 2);
            for (var row = 2; row <= rowCount; row++)
            {
                var SNValue = worksheet.Cells[row, 1].Text;
                var UIDValue = worksheet.Cells[row, 4].Text;
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
                SNValue += "F";
                var stre = SNValue.Substring(0, 2);
                var strf = SNValue.Substring(2, 2);
                var strg = SNValue.Substring(4, 2);
                var strh = SNValue.Substring(6, 2);
                var stri = SNValue.Substring(8, 2);
                var strj = SNValue.Substring(10, 2);
                var strk = SNValue.Substring(12, 2);
                var strl = SNValue.Substring(14, 2);
                var strm = SNValue.Substring(16, 2);
                var strn = SNValue.Substring(18, 2);
                var intnew = Convert.ToInt32(stre, 16) ^ Convert.ToInt32(strf, 16) ^ Convert.ToInt32(strg, 16) ^
                             Convert.ToInt32(strh, 16) ^ Convert.ToInt32(stri, 16) ^ Convert.ToInt32(strj, 16) ^
                             Convert.ToInt32(strk, 16) ^ Convert.ToInt32(strl, 16) ^ Convert.ToInt32(strm, 16) ^
                             Convert.ToInt32(strn, 16);
                var strXOR_2 = intnew.ToString("X").PadLeft(2, '0');
                SNValue += strXOR_2;
                SNData.Add(SNValue);
                UIDData.Add(UIDValue);
            }
        }

        var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        var fileName = $"CardNoTM{StartSN}-{EndSN}.xml";
        var filePath = Path.Combine(desktopPath, fileName);

        using (var writer = new StreamWriter(filePath))
        {
            writer.WriteLine("<?xml version = \"1.0\" encoding = \"GB2312\"?>");
            writer.WriteLine(
                $"<CardList Total = \"{SNData.Count}\" CardType = \"{cardtype}\" Start = \"{StartSN}\" End = \"{EndSN}\">");
            for (var i = 0; i < SNData.Count; i++)
                writer.WriteLine($"<Card UID=\"{UIDData[i]}\" AppID=\"{SNData[i]}\"/>");

            writer.Write("</CardList>");
        }

        Message.ShowSnack();
    }
}