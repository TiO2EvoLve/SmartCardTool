using System.Text.RegularExpressions;

namespace WindowUI.Sort;

public class 琴岛通1280
{
    public static void Run(List<string> MKData, string mkFileName, string FilePath, string FileName)
    {
        List<string> ATS = new List<string>();
        List<string> SN = new List<string>();
        List<string> SNN = new List<string>();
        var Date = ""; // 记录日期

        var sql = "SELECT ATS FROM kahao order by SerialNum ASC";
        ATS = Mdb.Select(FilePath, sql);
        sql = "SELECT 卡类型 FROM kahao order by SerialNum ASC";
        SN = Mdb.Select(FilePath, sql);
        sql = "SELECT 特殊卡号 FROM kahao order by SerialNum ASC";
        SNN = Mdb.Select(FilePath, sql);

        //处理MK文件
        //截取MK文件第二行的前42个字节
        MKData[1] = MKData[1].Substring(0, 42);
        //获取总数据的条数
        var totalLines = SN.Count;
        //将总数据条数转为6位数
        var totalLinesFormatted = totalLines.ToString("D6");
        //将MK文件的第二行的后6位替换为总数据条数
        MKData[1] = MKData[1].Substring(0, MKData[1].Length - 6) + totalLinesFormatted;
        if (MKData[1].Length != 42)
        {
            Message.ShowMessageBox("错误", "MK文件格式错误");
            return;
        }

        // 使用正则表达式匹配数量和日期
        var regex = new Regex(@"(\d+)-(\d{8})");
        var match = regex.Match(FileName);

        if (match.Success)
        {
            // 提取并解析日期
            var dateStr = match.Groups[2].Value;
            if (dateStr.Length == 8)
            {
                Date = dateStr;
            }
            else
            {
                Message.ShowMessageBox("异常", "无效的日期格式");
                return;
            }
        }
        else
        {
            LogManage.AddLog("没有匹配到日期，已自动使用当前时间");
            Date = DateTime.Now.ToString("yyyyMMdd");
        }

        var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        var fileName = $"HG2660{Date}01.RCC";
        var filePath = Path.Combine(desktopPath, fileName);
        using (var writer = new StreamWriter(filePath))
        {
            writer.WriteLine(ATS.Count);
            for (var i = 0; i < ATS.Count; i++) writer.WriteLine($"{ATS[i]}              {SN[i]}");
        }

        //第二个文件
        //处理SNN内容
        for (var i = 0; i < SNN.Count; i++)
        {
            var pattern = @"^(.*?)XXYYZZ(.*?)AABBCC(.*)$";

            var m = Regex.Match(SNN[i], pattern);

            if (m.Success)
            {
                var firstPart = m.Groups[1].Value;
                var secondPart = m.Groups[2].Value;
                var thirdPart = m.Groups[3].Value;
                SNN[i] = $"{firstPart}      {firstPart}      {secondPart}              {thirdPart}";
            }
            else
            {
                MessageBox.Show("错误", "正则表达式未匹配到对应的数据");
                return;
            }
        }

        fileName = $"RC{mkFileName}";
        filePath = Path.Combine(desktopPath, fileName);
        using (var writer = new StreamWriter(filePath))
        {
            writer.WriteLine(MKData[0]);
            writer.WriteLine(MKData[1]);
            for (var i = 0; i < SNN.Count; i++)
                if (i == SNN.Count - 1)
                    writer.Write(SNN[i]);
                else
                    writer.WriteLine(SNN[i]);
        }

        Message.ShowSnack();
    }
}