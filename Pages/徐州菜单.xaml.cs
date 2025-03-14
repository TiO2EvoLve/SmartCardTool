using System.Windows.Controls;

namespace WindowUI.Pages;

public partial class 徐州菜单 : Window
{
    public 徐州菜单()
    {
        InitializeComponent();
    }

    public string SelectedCampus { get; set; }

    private void RadioButton_Checked(object sender, RoutedEventArgs e)
    {
        if (sender is RadioButton radioButton)
            SelectedCampus = radioButton.Content.ToString() ?? throw new InvalidOperationException();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrEmpty(SelectedCampus))
        {
            Message.ShowMessageBox("错误", "请先选择要制作的文件格式");
            return;
        }

        Close();
    }
}