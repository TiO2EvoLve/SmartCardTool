using System.Windows.Controls;

namespace WindowUI.Pages;

public partial class 青岛理工大学菜单 : Window
{
    public 青岛理工大学菜单()
    {
        InitializeComponent();
    }

    public string SelectedCampus { get; set; }

    private void RadioButton_Checked(object sender, RoutedEventArgs e)
    {
        if (sender is RadioButton radioButton) SelectedCampus = radioButton.Content.ToString();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrEmpty(SelectedCampus))
        {
            Message.ShowMessageBox("错误", "请先选择校区");
            return;
        }

        Close();
    }
}