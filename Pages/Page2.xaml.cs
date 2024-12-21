
using System.Windows;
using System.Windows.Controls;

namespace WindowUI.Pages
{
    public partial class Page2 : Page
    {
        public Page2()
        {
            InitializeComponent();
        }
      
        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Button clicked!");
        }
    }
    
   
}
