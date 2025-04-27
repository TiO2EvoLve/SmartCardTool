using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Win32;
using WindowUI.Sort;
using Wpf.Ui.Controls;

namespace WindowUI;

public partial class RCC
{
    public RCC()
    {
        InitializeComponent();
    }

    private string mkFileName { get; set; } = null!; //  记录MK文件名
    private List<string> MKData { get; set; } = null!; // 临时存储读取的MK文件的数据
    private string FileName { get; set; } = null!; // 记录文件名
    private MemoryStream ZhikaStream { get; set; } = null!; // 临时存储读取的文件数据
    private string FilePath { get; set; } = null!; // 记录文件的路径
    private string Region { get; set; } = null!; // 下拉框选则的地区
    private OpenFileDialog openFileDialog { get; set; } = null!; // MK文件处理流
    private OpenFileDialog openFileDialog2 { get; set; } = null!; // 数据文件处理流

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
            LogManage.AddLog($"成功选择MK文件:{openFileDialog.FileName}");
            //如果什么也没有就返回
            if (openFileDialog.FileName == "") return;
            //判断文件名是否以MK或者KC开头,如果不是就不是MK文件
            if (!Path.GetFileName(openFileDialog.FileName).StartsWith("MK") &&
                !Path.GetFileName(openFileDialog.FileName).StartsWith("KC"))
            {
                Message.ShowMessageBox("错误", "请选择正确的MK文件");
                return;
            }
            
            try
            {
                //将文件暂时存储到MKDate中
                MKData = File.ReadAllLines(openFileDialog.FileName).ToList();
                //记录MK文件名
                mkFileName = Path.GetFileName(openFileDialog.FileName);
                //去掉MK文件名的前两个字符"MK"或"KC"
                mkFileName = mkFileName.Substring(2);
                mk.Foreground = Brushes.LightGreen;
                mktextbox.Foreground = Brushes.LimeGreen;
                mktextbox.Text = mkFileName;
            }
            catch
            {
                Message.ShowMessageBox("错误", "MK文件读取错误");
            }
            mk.Foreground = Brushes.LightGreen;
            mktextbox.Foreground = Brushes.LimeGreen;
        }
    }

    //打开Excel或Mdb文件
    private void OpenFile(object sender, RoutedEventArgs e)
    {
        string file = Toml.GetToml(Region, "file");
        openFileDialog2 = new OpenFileDialog
        {
            Filter = $"数据文件(*.{file})|*.{file}",
            Title = "选择一个文件"
        };
        if (openFileDialog2.ShowDialog() == true)
        {
            LogManage.AddLog($"成功选择数据文件:{openFileDialog2.FileName}");
            if (openFileDialog2.FileName == "") return;
            //记录文件路径
            FilePath = openFileDialog2.FileName;
            //记录Excel文件名
            FileName = Path.GetFileNameWithoutExtension(openFileDialog2.FileName);
            FileName = FileName.Replace("kahao", "");
            FileName = FileName.Replace("_", "");
            //暂时存储文件流
            try
            {
                ZhikaStream = new MemoryStream(File.ReadAllBytes(openFileDialog2.FileName));
            }
            catch (IOException)
            {
                Message.ShowMessageBox("错误", "文件已被占用，请先关闭其他程序");
                return;
            }

            datatextbox.Text = FileName;
            data.Foreground = Brushes.LightGreen;
            datatextbox.Foreground = Brushes.LimeGreen;
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
                try
                {
                    if (Convert.ToBoolean(Toml.GetToml(Region, "mk")))
                    {
                        SelectMKButton.IsEnabled = true;
                        mk.Foreground = Brushes.Red;
                        mktextbox.Foreground = Brushes.Red;
                    }
                    else
                    {
                        SelectMKButton.IsEnabled = false;
                        mk.Foreground = Brushes.LightGreen;
                        mktextbox.Foreground = Brushes.LightGreen;
                    }
                }
                catch
                {
                    Message.ShowMessageBox("提示", "请选择地区");
                    return;
                }

                LogManage.AddLog($"选择地区为：{Region}");
                string tips = Toml.GetToml(Region, "tip");

                tip.Text = tips;
                string file = Toml.GetToml(Region, "file");
                LogManage.AddLog($"{Region}地区需要{file}文件格式");
                
            }
        }
    }

    //点击处理文件按钮
    private void ProcessTheFile(object sender, RoutedEventArgs e)
    {
        if (ZhikaStream is null || ZhikaStream.Length == 0)
        {
            Message.ShowMessageBox("错误", "未选择数据文件");
            return;
        }

        LogManage.AddLog("开始处理文件...");
        //根据不同地区处理文件
        try
        {
            switch (Region)
            {
                case "天津": 天津.Run(FilePath, MKData, mkFileName); break;
                case "兰州": 兰州.Run(FilePath, FileName, MKData, mkFileName); break;
                case "兰州工作证": 兰州工作证.Run(FilePath, FileName); break;
                case "青岛博研加气站": 青岛博研加气站.Run(ZhikaStream, FileName); break;
                case "抚顺": 抚顺.Run(FilePath, FileName); break;
                case "郴州": 郴州.Run(MKData, mkFileName, FilePath); break;
                case "潍坊": 潍坊.Run(FilePath, FileName); break;
                case "国网技术学院": 国网技术学院.Run(ZhikaStream, FileName); break;
                case "哈尔滨城市通": 哈尔滨城市通.Run(ZhikaStream, FileName); break;
                case "哈尔滨学院": 哈尔滨学院.Run(FilePath); break;
                case "运城盐湖王府学校": 运城盐湖王府学校.Run(ZhikaStream, FileName); break;
                case "南通地铁": 南通地铁.Run(FilePath, FileName); break;
                case "长沙公交": 长沙公交.Run(ZhikaStream, FileName); break;
                case "泸州公交": 泸州公交.Run(FilePath, FileName); break;
                case "合肥通": 合肥通.Run(FilePath, MKData, mkFileName); break;
                case "青岛理工大学": 青岛理工大学.Run(ZhikaStream, FileName); break;
                case "西安交通大学": 西安交通大学.Run(ZhikaStream, FileName); break;
                case "呼和浩特": 呼和浩特.Run(ZhikaStream, FileName); break;
                case "重庆": 重庆.Run(FilePath, FileName); break;
                case "西藏林芝": 西藏林芝.Run(FilePath); break;
                case "西藏拉萨": 西藏拉萨.Run(FilePath); break;
                case "淄博公交": 淄博公交.Run(FilePath); break;
                case "淄博血站不开通": 淄博血站不开通.Run(ZhikaStream); break;
                case "平凉公交": 平凉公交.Run(FilePath, FileName); break;
                case "桂林公交": 桂林公交.Run(FilePath); break;
                case "陕西师范大学": 陕西师范大学.Run(ZhikaStream, FileName); break;
                case "西安文理学院": 西安文理学院.Run(ZhikaStream, FileName); break;
                case "滨州公交": 滨州公交.Run(FilePath, FileName); break;
                case "云南朗坤": 云南朗坤.Run(ZhikaStream, FileName); break;
                case "盱眙": 盱眙.Run(FilePath, FileName); break;
                case "柳州公交": 柳州公交.Run(FilePath, MKData); break;
                case "漯河": 漯河.Run(FilePath); break;
                case "随州": 随州.Run(FilePath, FileName); break;
                case "昆明": 昆明.Run(ZhikaStream, FileName); break;
                case "徐州地铁": 徐州地铁.Run(FilePath, ZhikaStream, FileName); break;
                case "江苏乾翔": 江苏乾翔.Run(ZhikaStream, FileName); break;
                case "石家庄": 石家庄.Run(ZhikaStream, FileName); break;
                case "淮北": 淮北.Run(ZhikaStream); break;
                case "山西医科大学": 山西医科大学.Run(FilePath, FileName); break;
                case "济南地铁UL": 济南地铁UL.Run(ZhikaStream, FileName); break;
                case "洪城": 洪城.Run(ZhikaStream); break;
                case "第一医科大学": 第一医科大学.Run(ZhikaStream, FileName); break;
                case "邹平": 邹平.Run(ZhikaStream, FileName); break;
                case "盐城": 盐城.Run(FilePath, FileName); break;
                case "穆棱": 穆棱.Run(FilePath, FileName); break;
                case "上海树维": 上海树维.Run(FilePath, FileName); break;
                case "琴岛通": 琴岛通.Run(FilePath, FileName); break;
                case "琴岛通1280": 琴岛通1280.Run(MKData, mkFileName, FilePath, FileName); break;
                case "广水": 广水.Run(FilePath, FileName); break;
                case "洛阳": 洛阳.Run(FilePath, FileName); break;
                case "新开普": 新开普.Run(FilePath, ZhikaStream, FileName); break;
                case "济南员工卡": 济南员工卡.Run(ZhikaStream, FileName); break;
                default: Message.ShowMessageBox("警告", "请先选择地区"); break;
            }
        }
        catch (Exception exception)
        {
            Message.ShowMessageBox("错误", exception.Message);
            LogManage.AddLog($"处理文件出错，错误信息：{exception.Message}");
        }
    }

    private void Test(object sender, RoutedEventArgs e)
    {
        Message.ShowSnack("警告", "该功能未开发", ControlAppearance.Caution, new SymbolIcon(SymbolRegular.DismissSquare20), 3);
        LogManage.AddLog("未开发");
    }

    private void ClearLog(object sender, MouseButtonEventArgs e)
    {
        LogManage.Clear();
    }
}