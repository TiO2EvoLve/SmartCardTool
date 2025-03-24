using System.Windows.Controls;

namespace WindowUI.Pages;

public partial class Setting : Page
{
    public Setting()
    {
        InitializeComponent();
    }

    private void ChangeTheme(object sender, RoutedEventArgs e)
    {
        if (ThemeComboBox.SelectedItem is ComboBoxItem selectedItem && selectedItem.Content != null)
        {
        }
    }
}