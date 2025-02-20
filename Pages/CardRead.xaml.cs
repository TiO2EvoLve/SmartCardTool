using System.Windows.Controls;

namespace WindowUI.Pages;

public partial class CardRead : Page
{
    public CardRead()
    {
        InitializeComponent();
    }

    private void OpenPort(object sender, RoutedEventArgs e)
    {
        Message.ShowMessageBox("失败","打开端口失败");
    }
}