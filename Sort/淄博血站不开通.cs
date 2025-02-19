namespace WindowUI.Sort;

public class 淄博血站不开通
{
    public static void Run(MemoryStream ExcelData)
    {
        string cardtype = "0801";
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
            StartSN = worksheet.Cells[1, 8].Text;
            EndSN = worksheet.Cells[rowCount, 8].Text;
            for (int row = 1; row <= rowCount; row++)
            {
                string SNValue = worksheet.Cells[row, 8].Text;
                string UIDValue = worksheet.Cells[row, 2].Text;
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
                string stre = SNValue.Substring(0, 2);
                string strf = SNValue.Substring(2, 2);
                string strg = SNValue.Substring(4, 2);
                string strh = SNValue.Substring(6, 2);
                string stri = SNValue.Substring(8, 2);
                string strj = SNValue.Substring(10, 2);
                string strk = SNValue.Substring(12, 2);
                string strl = SNValue.Substring(14, 2);
                Int32 intnew = (Convert.ToInt32(stre, 16) ^ Convert.ToInt32(strf, 16) ^ Convert.ToInt32(strg, 16) ^
                                Convert.ToInt32(strh, 16) ^ Convert.ToInt32(stri, 16) ^ Convert.ToInt32(strj, 16) ^
                                Convert.ToInt32(strk, 16) ^ Convert.ToInt32(strl, 16));
                string strXOR_2 = intnew.ToString("X").PadLeft(2, '0');
                SNValue += strXOR_2;
                SNData.Add(SNValue);
                UIDData.Add(UIDValue);
                
            }
        }

        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string fileName = $"{StartSN}-{EndSN}.xml";
        string filePath = Path.Combine(desktopPath, fileName);
        //保存文件到桌面
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
        Message.ShowMessageBox();
    }
}