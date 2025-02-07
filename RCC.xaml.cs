using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Win32;
using WindowUI.Sort;

namespace WindowUI;

public partial class RCC
{
    private string mkFileName { get; set; } //  记录MK文件名
    private List<string> MKData { get; set; } // 临时存储读取的MK文件的数据
    private string excelFileName { get; set; } // 记录Excel文件名
    private MemoryStream ExcelData { get; set; } // 临时存储读取的Excel的数据
    private string Region { get; set; } // 下拉框选则的地区
    private OpenFileDialog openFileDialog { get; set; } // MK文件处理流
    private OpenFileDialog openFileDialog2 { get; set; } // Excel文件处理流
    // 定义需要MK文件的地区
    private readonly string[] disableButtonRegions = ["天津", "郴州", "合肥", "兰州菜单", "柳州公交"];
    public string value { get; set; }
    public RCC()
    {
        InitializeComponent();
    }

    //打开MK文件
    private void OpenMKFile(object sender, RoutedEventArgs e)
    {
        //打开一个文件选择器，类型为任意
        openFileDialog = new OpenFileDialog
        {
            Filter = "All files (*.*)|*.*", // 允许选择所有文件
            Title = "选择一个文件"
        };
        if (openFileDialog.ShowDialog() == true)
        {
            if (openFileDialog.FileName == "") return;
            //将文件暂时存储到MKDate中
            MKData = File.ReadAllLines(openFileDialog.FileName).ToList();
            //记录MK文件名
            mkFileName = Path.GetFileName(openFileDialog.FileName);
            //去掉MK文件名的前两个字符
            mkFileName = mkFileName.Substring(2);
            mk.Foreground = Brushes.LightGreen;
            mktextbox.Foreground = Brushes.Green;
            mktextbox.Text = mkFileName;
        }
    }

    //打开Excel文件
    private void OpenFile(object sender, RoutedEventArgs e)
    {
        openFileDialog2 = new OpenFileDialog
        {
            Filter = "Excel Files (*.xlsx)|*.xlsx;",
            Title = "选择一个文件"
        };
        if (openFileDialog2.ShowDialog() == true)
        {
            if (openFileDialog2.FileName == "") return;
            //记录Excel文件名
            excelFileName = Path.GetFileName(openFileDialog2.FileName);
            //去掉扩展名.xlsx
            excelFileName = excelFileName.Substring(0, excelFileName.Length - 5);
            //将文件暂时存储到ExcelDate中
            try
            {
                ExcelData = new MemoryStream(File.ReadAllBytes(openFileDialog2.FileName));

                datatextbox.Text = excelFileName;
            }
            catch (IOException)
            {
                MessageBox.Show("文件已被占用，请先关闭Excel表格!",
                    "文件占用", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            data.Foreground = Brushes.LightGreen;
            datatextbox.Foreground = Brushes.Green;
        }
    }

    //下拉框选择地区
    private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (LocationComboBox.SelectedItem is ComboBoxItem selectedItem && selectedItem.Content != null)
        {
            Region = selectedItem.Content.ToString() ?? throw new InvalidOperationException();
            // 根据选择的地区禁用或启用按钮
            if (SelectMKButton != null)
            {
                SelectMKButton.IsEnabled = Array.Exists(disableButtonRegions, region => region == Region);
                if (!SelectMKButton.IsEnabled)
                {
                    mk.Foreground = Brushes.LightGreen;
                    mktextbox.Foreground = Brushes.LightGreen;
                    mktextbox.Text = "无需mk文件";
                }
                else mk.Foreground = Brushes.Red;
                
            }

            //根据不同地区进行提示
            switch (Region)
            {
                case "泸州公交": tip.Text = "根据卡类型进行制作"; break;
                case "兰州菜单": tip.Text = "兰州工作证不需要MK文件，异型卡需要提供两个"; break;
                case "随州": tip.Text = "Excel文件有时列数会不对应，需自行修改"; break;
                case "洪城": tip.Text = "多个文件注意修改编号"; break;
                case "测试地区": tip.Text = "测试地区"; break;
                default: tip.Text = "该地区暂无提示"; break;
            }
        }
    }

    //点击处理文件按钮
    private void ProcessTheFile(object sender, RoutedEventArgs e)
    {
        if (ExcelData is null)
        {
            MessageBox.Show("请选择文件");
            return;
        }

        //根据不同地区处理文件
        switch (Region)
        {
            case "天津": 天津.Run(ExcelData, MKData, mkFileName); break;
            case "兰州菜单": 兰州.Run(ExcelData, excelFileName, MKData, mkFileName); break;
            case "青岛博研加气站": 青岛博研加气站.Run(ExcelData, excelFileName); break;
            case "抚顺": 抚顺.Run(ExcelData, excelFileName); break;
            case "郴州": 郴州.Run(ExcelData, MKData, mkFileName); break;
            case "潍坊": 潍坊.Run(ExcelData, excelFileName); break;
            case "国网技术学院": 国网技术学院.Run(ExcelData, excelFileName); break;
            case "哈尔滨城市通": 哈尔滨城市通.Run(ExcelData, excelFileName); break;
            case "运城盐湖王府学校": 运城盐湖王府学校.Run(ExcelData, excelFileName); break;
            case "南通地铁": 南通地铁.Run(ExcelData, excelFileName); break;
            case "长沙公交": 长沙公交.Run(ExcelData, excelFileName); break;
            case "泸州公交": 泸州公交.Run(ExcelData, excelFileName); break;
            case "合肥通": 合肥通.Run(ExcelData, MKData, mkFileName); break;
            case "青岛理工大学菜单": 青岛理工大学.Run(ExcelData, excelFileName); break;
            case "西安交通大学": 西安交通大学.Run(ExcelData, excelFileName); break;
            case "呼和浩特": 呼和浩特.Run(ExcelData, excelFileName); break;
            case "重庆33A-A1": 重庆.Run(ExcelData, excelFileName); break;
            case "西藏林芝": 西藏林芝.Run(ExcelData); break;
            case "西藏拉萨": 西藏拉萨.Run(ExcelData); break;
            case "淄博公交": 淄博公交.Run(ExcelData); break;
            case "淄博血站不开通": 淄博血站不开通.Run(ExcelData); break;
            case "平凉公交": 平凉公交.Run(ExcelData, excelFileName); break;
            case "桂林公交": 桂林公交.Run(ExcelData); break;
            case "陕西师范大学": 陕西师范大学.Run(ExcelData, excelFileName); break;
            case "西安文理学院": 西安文理学院.Run(ExcelData, excelFileName); break;
            case "滨州公交": 滨州公交.Run(ExcelData, excelFileName); break;
            case "云南朗坤": 云南朗坤.PlanB(ExcelData, excelFileName); break;
            case "盱眙": 盱眙.Run(ExcelData, excelFileName); break;
            case "柳州公交": 柳州公交.Run(ExcelData, MKData); break;
            case "漯河": 漯河.Run(ExcelData); break;
            case "随州": 随州.Run(ExcelData, excelFileName); break;
            case "昆明": 昆明.Run(ExcelData, excelFileName); break;
            case "徐州地铁": 徐州地铁.Run(ExcelData, excelFileName); break;
            case "江苏乾翔": 江苏乾翔.Run(ExcelData, excelFileName); break;
            case "石家庄": 石家庄.Run(ExcelData, excelFileName); break;
            case "淮北": 淮北.Run(ExcelData); break;
            case "山西医科大学": 山西医科大学.Run(ExcelData, excelFileName); break;
            case "济南地铁UL": 济南地铁UL.Run(ExcelData, excelFileName); break;
            case "洪城": 洪城.Run(ExcelData); break;
            case "测试地区": 测试地区.Run(ExcelData, excelFileName); break;
            default: MessageBox.Show("请选择地区"); break;
        }
    }
    private void Test(object sender, RoutedEventArgs e)
    {
        MessageBox.Show("未开发");
    }
}