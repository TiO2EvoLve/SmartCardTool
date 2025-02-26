namespace WindowUI.Pages;

public partial class 桂林菜单
{
    public 桂林菜单()
    {
        InitializeComponent();
    }

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        if (SN.Text.Length != 20 || Count.Text.Length != 8)
        {
            Message.ShowMessageBox("错误","输入数据有误！");
            return;
        }
        Close();
    }
}