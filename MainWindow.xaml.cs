
using System.ComponentModel;
using System.Windows;
using System.Windows.Threading;
using WindowUI.Pages;
using Wpf.Ui.Controls;

namespace WindowUI;

public partial class MainWindow : Window,INotifyPropertyChanged
{
    private string _currentTime;
    private DispatcherTimer _timer;

    public event PropertyChangedEventHandler PropertyChanged;

    public string CurrentTime
    {
        get => _currentTime;
        set
        {
            _currentTime = value;
            OnPropertyChanged(nameof(CurrentTime));
        }
    }

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
    }
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
        navigationView?.Navigate(typeof(Page1));
    }
   

}