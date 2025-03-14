using System.Windows.Controls;
using System.Windows.Input;

namespace WindowUI.Pages;

public partial class Home : Page
{
    public Home()
    {
        InitializeComponent();
    }

    private void OpenMKtoRC_Window(object sender, MouseButtonEventArgs e)
    {
        new RCC().Show();
    }

    private void OpenScriptWindow(object sender, MouseButtonEventArgs e)
    {
        new ScriptChange().Show();
    }

    private void Test(object sender, MouseButtonEventArgs e)
    {
        Message.ShowMessageBox("提示", "暂未开发");
    }

    private void OpenCheckWindow(object sender, MouseButtonEventArgs e)
    {
        DataCheck check = new();
        check.ShowDialog();
    }
}