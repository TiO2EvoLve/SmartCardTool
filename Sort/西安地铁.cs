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
        Console.WriteLine($"订单ID: {订单ID}, 卡片厂商ID: {卡片厂商ID}, 卡片介质: {卡片介质}, 卡片数量: {卡片数量}, 起始卡号: {起始卡号}");
        //读取数据库
        var sql = "SELECT UID_16_ FROM kahao order by SerialNum ASC";
        UID = Mdb.Select(FilePath, sql);
        sql = "SELECT SerialNum FROM kahao order by SerialNum ASC";
        SN = Mdb.Select(FilePath, sql);

        //转换数据
        订单ID = DecimalToHexReversed(订单ID);
        卡片数量 = CardNumberClampAndReversed(卡片数量);
        
        // 输出文件
        var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        var outputPath = Path.Combine(desktopPath, $"Deliver.{卡片厂商ID}.{订单ID}.{卡片介质}.{DateTime.Now:yyyyMMdd}.001");
        using var writer = new StreamWriter(outputPath, false, Encoding.UTF8);

        writer.WriteLine($"{卡片厂商ID}{订单ID}{卡片介质}{卡片数量}");

        for (int i = 0; i < UID.Count;i++)
        {
            // if(int.Parse(起始卡号) + i != int.Parse(SN[i]))
            // {
            //     throw new Exception($"卡号不匹配！");
            // }
            writer.WriteLine($"{UID[i].PadLeft(14, '0')}{DecimalToReversedHex(起始卡号)}");
        }

        Message.ShowSnack();
    }
    
    //十进制字符串转十六进制（反转字节序）
    public static string DecimalToHexReversed(string decimalString)
    {
        uint value = Convert.ToUInt32(decimalString);
    
        // 直接获取4个字节（大端序）
        byte[] bytes = BitConverter.GetBytes(value);
        if (BitConverter.IsLittleEndian)
            Array.Reverse(bytes);
    
        // 转为十六进制字符串并拼接
        return string.Concat(bytes.Select(b => b.ToString("X2")));
    }
    public static string CardNumberClampAndReversed(string decimalString)
    {
        // 转成无符号整数
        uint value = Convert.ToUInt32(decimalString);
    
        // 转为4位十六进制字符串（保持四位）
        string hex = value.ToString("X4");
        
        string reversed = hex.Substring(2, 2) + hex.Substring(0, 2);
    
        return reversed;
    }
    //还原16进制卡号并翻转
    public static string DecimalToReversedHex(string decimalString)
    {
        // 使用 UInt32（无符号32位整数）支持更大的数值
        uint value = uint.Parse(decimalString);
        // 或者使用 Convert.ToUInt32(decimalString);
    
        // 获取字节数组（小端序）
        byte[] bytes = BitConverter.GetBytes(value);
        // bytes = [0x75, 0xAE, 0x6B, 0xEE]
    
        // 直接转为十六进制字符串（已经是反转后的顺序）
        return BitConverter.ToString(bytes).Replace("-", "");
    }
}