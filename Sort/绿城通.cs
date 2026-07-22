using System.Text;
using WindowUI.Pages;

namespace WindowUI.Sort;

public class 绿城通
{
    public static void Run(string FilePath)
    {
        List<string> SNData = new List<string>();
        List<string> PrintSNData = new List<string>();
        List<string> UidData = new List<string>();
        
        //打开菜单
        绿城通菜单 LC = new 绿城通菜单();
        LC.ShowDialog();
        Console.WriteLine(LC.fileContent);
        
        var sql = "SELECT SerialNum FROM kahao order by SerialNum Asc";
        SNData = Mdb.Select(FilePath, sql);
        sql = "SELECT 打码特殊算法 FROM kahao order by SerialNum Asc";
        PrintSNData = Mdb.Select(FilePath, sql);
        sql = "SELECT UID_16_ FROM kahao order by SerialNum Asc";
        UidData = Mdb.Select(FilePath, sql);
        
        // 输出文件
        var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        var time = DateTime.Now.ToString("yyyyMMdd");
        var outputPath = Path.Combine(desktopPath, $"UN{LC.fileContent.订单类型}8670{time}01_{time}01.txt");
        using var writer = new StreamWriter(outputPath, false, Encoding.UTF8);

        for (int i = 0; i < SNData.Count; i++)
        {
            writer.WriteLine($"{LC.fileContent.订单编号}\t{time}01\t{LC.fileContent.订单类型}\t{SNData[i]}\t{PrintSNData[i]}\t{UidData[i]}\t{LC.fileContent.卡片主类型}\t{LC.fileContent.卡片子类型}\t{LC.fileContent.卡片成本或押金}\t{LC.fileContent.城市代码}\t{LC.fileContent.行业代码}\t{LC.fileContent.应用版本号}\t{LC.fileContent.卡片启用标志}\t{LC.fileContent.交通部钱包应用启用标志}\t{LC.fileContent._1002钱包应用启用标志}\t{LC.fileContent._10031004钱包启用标志}\t{LC.fileContent.交通部钱包面值}\t01000000\t{LC.fileContent._1002钱包面值}\t{LC.fileContent._10031004钱包面值}\t{time}\t{DateTime.Now.AddYears(50):yyyyMMdd}\t8670\t{LC.fileContent.测试标志}\t{DateTime.Now: YYYYMMDDHHMMSS}\t0000000000000000\t00000000\t01\t \t \t00\t \t ");
        }
        
    }
}