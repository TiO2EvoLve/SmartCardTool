using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WindowUI.Pages
{
    
    public partial class LuZhou : Window
    {
        public LuZhou()
        {
            InitializeComponent();
        }

        public string CardType { set; get; }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(CardType is null || CardType =="")
            {
                MessageBox.Show("请先选择卡类型");
                return;
            }
            Close();
        }
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LuZhouCardType.SelectedItem is ComboBoxItem selectedItem && selectedItem.DataContext != null)
            {
                CardType = selectedItem.DataContext.ToString();
            }
        }
    }
}
