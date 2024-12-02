using System.Windows;

namespace WindowUI.Pages;

public partial class GuiLin
{
    public GuiLin()
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