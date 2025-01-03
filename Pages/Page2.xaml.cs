using System.Windows;
using System.Windows.Controls;

namespace WindowUI.Pages;
    public partial class Page2 
    {
        private void Text_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var viewModel = DataContext as Page2ViewModel;
            if (viewModel != null)
            {
                string inputValue = viewModel.Input;
                Text2.Text = inputValue;
            }
        }
        public Page2()
        {
            InitializeComponent();
            DataContext = new Page2ViewModel();
        }
        private void ClickButton(object sender, RoutedEventArgs e)
        {
           //在同步方法里调用异步方法
              Task.Run(async () =>
              {
                await Task.Delay(1000);
                MessageBox.Show("异步方法执行完毕");
              });
        }
        
    }
    
   

