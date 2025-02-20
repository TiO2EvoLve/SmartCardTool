using System.Windows.Controls;

namespace WindowUI.Pages;

public partial class 漯河菜单 : Window
{
    public 漯河菜单()
    {
        InitializeComponent();
    }
    public string CardType = "";//标识卡类型
    public bool 英才卡 = false;//标识是否是英才卡
    public string 英才卡卡号 = "0";//标识英才卡首位卡号

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        if(CardType is null || CardType =="")
        {
            Message.ShowMessageBox("警告","请先选择卡类型");
            return;
        }
        if(英才卡 && 英才卡卡号.Length != 11)
        {
            Message.ShowMessageBox("警告","请填写正确的卡号");
            return;
        }
        Close();
    }
    private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (LuoHeCardType.SelectedItem is ComboBoxItem selectedItem && selectedItem.DataContext != null)
        {
            CardType = selectedItem.DataContext.ToString();
            if (selectedItem.Content.ToString() == "英才卡" )
            {
                英才卡 = true;
                SN.IsEnabled = true;
            }
            else
            {
                英才卡 = false;
                if (SN != null) // 检查SN是否初始化
                {
                    SN.IsEnabled = false;
                }
            }
        }
    }

    private void SN_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        英才卡卡号 = SN.Text;
    }
}