using System.Linq.Expressions;
using Microsoft.Win32;

namespace WindowUI.Pages;

public partial class 绿城通菜单 : Window
{
    public FileContent fileContent = new ();
    
    public 绿城通菜单()
    {
        InitializeComponent();
    }
    private void Close(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void OpenFile(object sender, RoutedEventArgs e)
    {
        var openFileDialog = new OpenFileDialog();
        openFileDialog.Filter = "txt文件|*.txt";
        openFileDialog.ShowDialog();
        if (string.IsNullOrEmpty(openFileDialog.FileName)) return;

        // 读取第一行并分割
        var firstLine = File.ReadLines(openFileDialog.FileName).FirstOrDefault();
        if (string.IsNullOrEmpty(firstLine)) return;

        var columns = firstLine.Split('\t');

        // 根据下标取值
        if (columns.Length > 0)
        {
            fileContent = new FileContent
            {
                订单编号 = columns[0],
                订单类型 = columns[1],
                卡片主类型 = columns[2],
                卡片子类型 = columns[3],
                卡片成本或押金 = columns[4],
                城市代码 = columns[5],
                行业代码 = columns[6],
                应用版本号 = columns[7],
                售卡日期 = columns[8],
                卡片启用标志 = columns[9],
                交通部钱包应用启用标志 = columns[10],
                交通部钱包面值 = columns[11],
                _1002钱包应用启用标志 = columns[12],
                _1002钱包面值 = columns[13],
                _10031004钱包启用标志 = columns[14],
                _10031004钱包面值 = columns[15],
                测试标志 = columns[16],
                开始逻辑卡号 = columns[17],
                结束逻辑卡号 = columns[18],
                制卡数量 = columns[19]
            };
            TextBlock.Text = Path.GetFileNameWithoutExtension(openFileDialog.FileName);
        }
        else
        {
            throw new Exception("文件为空");
        }
    }
}

public class FileContent
{
    public string 订单编号;
    public string 订单类型;
    public string 卡片主类型;
    public string 卡片子类型;
    public string 卡片成本或押金;
    public string 城市代码;
    public string 行业代码;
    public string 应用版本号;
    public string 售卡日期;
    public string 卡片启用标志;
    public string 交通部钱包应用启用标志;
    public string 交通部钱包面值;
    public string _1002钱包应用启用标志;
    public string _1002钱包面值;
    public string _10031004钱包启用标志;
    public string _10031004钱包面值;
    public string 测试标志;
    public string 开始逻辑卡号;
    public string 结束逻辑卡号;
    public string 制卡数量;

    public override string ToString()
    {
        return $"订单编号:{订单编号}, 订单类型:{订单类型}, 卡片主类型:{卡片主类型}, " +
               $"卡片子类型:{卡片子类型}, 卡片成本或押金:{卡片成本或押金}, 城市代码:{城市代码}, " +
               $"行业代码:{行业代码}, 应用版本号:{应用版本号}, 售卡日期:{售卡日期}, " +
               $"卡片启用标志:{卡片启用标志}, 交通部钱包应用启用标志:{交通部钱包应用启用标志}, " +
               $"交通部钱包面值:{交通部钱包面值}, _1002钱包应用启用标志:{_1002钱包应用启用标志}, " +
               $"_1002钱包面值:{_1002钱包面值}, _10031004钱包启用标志:{_10031004钱包启用标志}, " +
               $"_10031004钱包面值:{_10031004钱包面值}, 测试标志:{测试标志}, " +
               $"开始逻辑卡号:{开始逻辑卡号}, 结束逻辑卡号:{结束逻辑卡号}, 制卡数量:{制卡数量}";
    }
}