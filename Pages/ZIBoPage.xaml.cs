using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
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
using System.Windows.Shapes;

namespace WindowUI.Pages
{
    /// <summary>
    /// ZIBo.xaml 的交互逻辑
    /// </summary>
    public partial class ZIBoPage : Window
    {
        public ZIBoPage()
        {
            InitializeComponent();
        }

        public string CardType { set; get; }
        public string date14 { set; get; }
        public string date10 { set; get; }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Select.SelectedItem is ComboBoxItem selectedItem && selectedItem.DataContext != null)
            {
                CardType = selectedItem.DataContext.ToString();
            }
        }
        private void OpenFile(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "XD文件|*.xd";
            openFileDialog.ShowDialog();
            if (openFileDialog.FileName == null) return;
            String FileName = System.IO.Path.GetFileName(openFileDialog.FileName);
            if (FileName != null)
            {

                date14 = FileName.Substring(8,8) + FileName.Substring(17, 6);
                DateTime date = DateTime.ParseExact(date14.Substring(0,8), "yyyyMMdd", null);
                // 转换为目标格式
                date10 = date.ToString("yyyy-MM-dd");
                datetext.Text = date14;
                date2text.Text = date10;
            }
        }

        private void datetext_TextChanged(object sender, TextChangedEventArgs e)
        {
            date14 = datetext.Text;
            if (date14.Length != 14)
            {
                return;
            }
            DateTime date = DateTime.ParseExact(date14.Substring(0, 8), "yyyyMMdd", null);
            // 转换为目标格式
            date10 = date.ToString("yyyy-MM-dd");
            date2text.Text = date10;
        }
    }
}
