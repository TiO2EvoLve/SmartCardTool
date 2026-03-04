using System.Windows.Controls;

namespace WindowUI.Pages;

public partial class 凯里菜单 : Window
{
    public string CardType = ""; //标识卡类型
    public 凯里菜单()
    {
        InitializeComponent();
    }
    private void Button_Click(object sender, RoutedEventArgs e)
    {
        if (CardType is null || CardType == "")
        {
            Message.ShowMessageBox("警告", "请先选择卡类型");
            return;
        }
        
        Close();
    }

    private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (LuoHeCardType.SelectedItem is ComboBoxItem selectedItem && selectedItem.DataContext != null)
        {
            CardType = selectedItem.DataContext.ToString();
        }
    }
    
}