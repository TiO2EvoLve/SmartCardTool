using System.Windows.Controls;
using CommunityToolkit.Mvvm.ComponentModel;

namespace WindowUI.Pages;

public partial class Setting : Page
{
    public Setting()
    {
        InitializeComponent();
    }
    private void ToggleSwitch_Checked(object sender, RoutedEventArgs e)
    {
        // 更新设置值
        Properties.Settings.Default.ShowYiYan = ToggleSwitch.IsChecked ?? false;
        // 保存设置
        Properties.Settings.Default.Save();
        // 获取主窗口实例并调用 LoadApiDataAsync 方法
        if (Application.Current.MainWindow is MainWindow mainWindow)
        {
            mainWindow.LoadApiDataAsync();
        }

    }

}