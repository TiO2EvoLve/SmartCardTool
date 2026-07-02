using System.Text;
using WindowUI.Pages;

namespace WindowUI.Sort;

public class 西安地铁
{
    public static void Run(string FilePath)
    {
        List<string> UID = new List<string>();
        List<string> SN = new List<string>();

        string 订单ID;
        string 卡片厂商ID;
        string 卡片介质;
        string 卡片数量;
        string 起始卡号;

        //打开菜单
        西安地铁菜单 XiAn = new 西安地铁菜单();
        XiAn.ShowDialog();
        订单ID = XiAn.订单ID;
        卡片厂商ID = XiAn.卡片厂商ID;
        卡片介质 = XiAn.卡片介质;
        卡片数量 = XiAn.卡片数量;
        起始卡号 = XiAn.起始卡号;
        //读取数据库
        var sql = "SELECT UID_16_ FROM kahao order by SerialNum ASC";
        UID = Mdb.Select(FilePath, sql);
        sql = "SELECT 特殊卡号 FROM kahao order by SerialNum ASC";
        SN = Mdb.Select(FilePath, sql);

        // 输出文件
        var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        var outputPath = Path.Combine(desktopPath, $"Deliver.{卡片厂商ID}.{订单ID}.{卡片介质}.{DateTime.Now:yyyyMMdd}.001");
        using var writer = new StreamWriter(outputPath, false, Encoding.UTF8);

        writer.WriteLine($"{卡片厂商ID}{DecimalToHexReversed(订单ID)}{卡片介质}{DecimalToHexReversed(卡片数量.PadLeft(4, '0'))}{起始卡号}");

        for (int i = 0; i < UID.Count; i++)
        {
            if(int.Parse(起始卡号) + i != int.Parse(SN[i]))
            {
                throw new Exception($"卡号不匹配！");
            }
            writer.WriteLine($"{卡片厂商ID}{UID[i].PadLeft(14, '0')}{DecimalToHexReversed(起始卡号 + i)}");
        }

        Message.ShowSnack();
    }

    /// <summary>
    /// 十进制字符串转十六进制（反转字节序）
    /// </summary>
    public static string DecimalToHexReversed(string decimalString)
    {
        // 转成无符号整数（支持大数字）
        uint value = Convert.ToUInt32(decimalString);

        // 转成字节数组
        byte[] bytes = BitConverter.GetBytes(value);

        // 反转字节序
        Array.Reverse(bytes);

        // 转成十六进制字符串
        return Convert.ToHexString(bytes);
    }
}