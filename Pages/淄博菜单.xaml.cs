using System.Windows.Controls;
using Microsoft.Win32;

namespace WindowUI.Pages
{
    public partial class 淄博菜单
    {
        public 淄博菜单()
        {
            InitializeComponent();
        }

        public string CardType { set; get; }
        public string Date14 { set; get; }
        public string Date10 { set; get; }

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
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "XD文件|*.xd";
            openFileDialog.ShowDialog();
            if (String.IsNullOrEmpty(openFileDialog.FileName)) return;
            String fileName = Path.GetFileName(openFileDialog.FileName);
            if (fileName == "") return;

            if (fileName.Length < 24)
            {
                MessageBox.Show("文件名长度不足，无法处理。", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Date14 = fileName.Substring(8, 8) + fileName.Substring(17, 6);
            DateTime date = DateTime.ParseExact(Date14.Substring(0, 8), "yyyyMMdd", null);
            // 转换为目标格式
            Date10 = date.ToString("yyyy-MM-dd");
            datetext.Text = Date14;
            date2text.Text = Date10;
        }
        private void datetext_TextChanged(object sender, TextChangedEventArgs e)
        {
            Date14 = datetext.Text;
            if (Date14.Length != 14)
            {
                return;
            }
            DateTime date = DateTime.ParseExact(Date14.Substring(0, 8), "yyyyMMdd", null);
            // 转换为目标格式
            Date10 = date.ToString("yyyy-MM-dd");
            date2text.Text = Date10;
        }
    }
}