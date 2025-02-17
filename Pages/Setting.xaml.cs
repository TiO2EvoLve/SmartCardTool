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
            string item = selectedItem.Content.ToString()?? throw new InvalidOperationException();
            Console.WriteLine(item);
            if (item == "Dark")
            {
                
            }
            var resourceDictionary = Application.Current.Resources.MergedDictionaries[0];
            foreach (var dictionary in resourceDictionary.MergedDictionaries)
            {
                Console.WriteLine(item);
            }
            
        }
        
           
        
    }
}