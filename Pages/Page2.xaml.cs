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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // 获取今天的日期
            DateTime today = DateTime.Today;
            Console.WriteLine("今天的日期是: " + today.ToString("yyyy-MM-dd"));

            // 计算4个月后的日期
            DateTime fourMonthsLater = today.AddMonths(4);
            Console.WriteLine("4个月后的日期是: " + fourMonthsLater.ToString("yyyy-MM-dd"));
            MessageBox.Show("4个月后的日期是: " + fourMonthsLater.ToString("yyyy-MM-dd"));
        }
    }
}
