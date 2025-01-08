using System.Windows;

namespace WindowUI.Pages;

public partial class 桂林菜单
{
    public 桂林菜单()
    {
        InitializeComponent();
    }

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        if (SN.Text.Length != 19 || Count.Text.Length != 8)
        {
            MessageBox.Show("输入数据有误！");
            return;
        }
        Close();
    }
}