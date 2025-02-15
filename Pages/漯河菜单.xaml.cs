using System.Windows;
using System.Windows.Controls;

namespace WindowUI.Pages;

public partial class 漯河菜单 : Window
{
    public 漯河菜单()
    {
        InitializeComponent();
    }
    public string CardType { set; get; } = "";
    public bool 英才卡 = false;
    public string 英才卡卡号;

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        if(CardType is null || CardType =="")
        {
            MessageBox.Show("请先选择卡类型");
            return;
        }
        Close();
    }
    private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (LuZhouCardType.SelectedItem is ComboBoxItem selectedItem && selectedItem.DataContext != null)
        {
            CardType = selectedItem.DataContext.ToString();
            if (selectedItem.Content.ToString() == "英才卡")
            {
                英才卡 = true;
                SN.IsEnabled = true;
            }
        }
    }

    private void SN_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        英才卡卡号 ="31050714" + SN.Text;
    }
}