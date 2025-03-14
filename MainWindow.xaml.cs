using System.ComponentModel;
using System.Net.Http;
using System.Windows.Input;
using System.Windows.Threading;
using Newtonsoft.Json.Linq;
using WindowUI.Pages;
using Wpf.Ui.Controls;

namespace WindowUI;

public partial class MainWindow : INotifyPropertyChanged
{
    // 创建一个静态的HttpClient实例
    private static readonly HttpClient client = new();
    private readonly DispatcherTimer _timer;
    private string _currentTime;

    public MainWindow()
    {
        InitializeComponent();
        DataContext = this;

        _timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(1)
        };
        _timer.Tick += Timer_Tick;
        _timer.Start();

        LoadApiDataAsync();
    }

    public string CurrentTime
    {
        get => _currentTime;
        set
        {
            _currentTime = value;
            OnPropertyChanged(nameof(CurrentTime));
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void Timer_Tick(object sender, EventArgs e)
    {
        CurrentTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    }

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private void NavigationView_Loaded(object sender, RoutedEventArgs e)
    {
        // Navigate to the default page
        var navigationView = sender as NavigationView;
        navigationView?.Navigate(typeof(Home));
    }

    private async void LoadApiDataAsync()
    {
        try
        {
            var apiResponse = await GetApiDataAsync("https://api.nxvav.cn/api/yiyan/");
            //解析json
            var json = JObject.Parse(apiResponse);
            TitleTextBlock.Text = json["yiyan"]?.ToString();
        }
        catch (Exception e)
        {
            TitleTextBlock.Text = "当前无网络连接";
        }
    }

    //异步获取每日一句
    private static async Task<string> GetApiDataAsync(string url)
    {
        // 发送异步GET请求
        var response = await client.GetAsync(url);

        // 确保请求成功
        response.EnsureSuccessStatusCode();

        // 读取响应内容
        var responseBody = await response.Content.ReadAsStringAsync();
        return responseBody;
    }

    private void UIElement_OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        LoadApiDataAsync();
    }
}