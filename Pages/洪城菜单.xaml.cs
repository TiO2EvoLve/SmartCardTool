using System.Windows.Controls;

namespace WindowUI.Pages;

public partial class 洪城菜单 : Window
{
    public 洪城菜单()
    {
        InitializeComponent();
    }
    public string Cardtype { get; set; }
    private void ToggleButton_OnChecked(object sender, RoutedEventArgs e)
    {
        if (sender is RadioButton radioButton)
        {
            Cardtype = radioButton.DataContext.ToString();
        }
    }

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrEmpty(Cardtype))
        {
            MessageBox.Show("请先选择校区");
            return;
        }
        Close();
    }
}