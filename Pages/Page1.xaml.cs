using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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