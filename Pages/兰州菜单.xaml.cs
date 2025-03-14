using System.Windows.Controls;

namespace WindowUI.Pages;

public partial class 兰州菜单
{
    public 兰州菜单()
    {
        InitializeComponent();
    }

    public string CardType { set; get; } = "";

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        if (CardType is null || CardType == "")
        {
            Console.WriteLine(CardType);
            Message.ShowMessageBox("错误", "请先选择卡类型");
            return;
        }

        Close();
    }

    private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (LuZhouCardType.SelectedItem is ComboBoxItem selectedItem && selectedItem.DataContext != null)
            CardType = selectedItem.DataContext.ToString();
    }
}