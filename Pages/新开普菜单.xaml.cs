using System.Windows.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using Tommy;

namespace WindowUI.Pages;

public partial class 新开普菜单
{
    public 新开普菜单()
    {
        InitializeComponent();
        DataContext = new ViewModel();//设置绑定模型
        LoadComboBoxItems();//初始化复选框内容
    }

    public string Region;//选择地区
    public ViewModel viewmodel => DataContext as ViewModel;
    
    //下拉框选择地区逻辑
    private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (LocationComboBox.SelectedItem is ComboBoxItem selectedItem && selectedItem.Content != null)
        {
            Region = selectedItem.Content.ToString() ?? throw new InvalidOperationException();
            LocationComboBox.Text = Region;
            LoadRegionProperty(Region);
        }
    }

    private void OnClick(object sender, RoutedEventArgs e)
    {
        
    }
    //点击确认按钮逻辑
    private void OK(object sender, RoutedEventArgs e)
    {
        Close();
    }
    //加载复选框内容
    private void LoadComboBoxItems()
    {
        var tomlFilePath = "Config/新开普.toml";
        if (File.Exists(tomlFilePath))
        {
            TextReader reader = new StreamReader(tomlFilePath);
            var table = TOML.Parse(reader);

            foreach (var key in table.Keys)
            {
                LocationComboBox.Items.Add(new ComboBoxItem { Content = key });
            }
        }
    }
    //加载地区配置
    private void LoadRegionProperty(string Region)
    {
        var configPath = "Config/新开普.toml";
        try
        {
            TextReader tomlText = new StreamReader(configPath);
            var table = TOML.Parse(tomlText);
            viewmodel.Sn_Column=  table[Region]["SN_Column"];
            viewmodel.Uid_Column=  table[Region]["Uid_Column"];
            viewmodel.IsSkipFirstRow=  table[Region]["SkipRow"];
                
        }catch
        {
            Message.ShowMessageBox("错误", "未找到该数据");
        }
    }
    

}
public partial class ViewModel : ObservableObject
{
    [ObservableProperty]
    public int sn_Column = 2;//卡号所在列
    [ObservableProperty]
    public int uid_Column = 7;//芯片号所在列
    [ObservableProperty]
    public bool isSkipFirstRow = true;//是否跳过首行
    [ObservableProperty]
    private string selectedRegion;


}