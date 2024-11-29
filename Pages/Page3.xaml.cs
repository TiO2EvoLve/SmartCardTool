using System.ComponentModel;
using System.Windows.Controls;

namespace WindowUI.Pages
{
    public partial class Page3 : Page, INotifyPropertyChanged
    {
        public Page3()
        {
            InitializeComponent();
            this.DataContext = this;  //设置 DataContext 为当前 Page 实例
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged(nameof(Name));  // 通知 UI 更新
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void TextBoxBase_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            Console.WriteLine(Name);  // 输出绑定的 Name 属性
        }
    }
}