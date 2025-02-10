using System.Windows.Controls;
using System.Windows.Input;

namespace WindowUI.Pages
{
    public partial class Page1 : Page
    {
        public Page1()
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
        
    }
}