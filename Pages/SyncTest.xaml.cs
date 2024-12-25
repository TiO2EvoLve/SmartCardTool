using System.DirectoryServices.ActiveDirectory;
using System.Windows;

namespace WindowUI.Pages;

public partial class SyncTest
{
    public SyncTest()
    {
        InitializeComponent();
    }
    private async void Button_OnClick(object sender, RoutedEventArgs e)
    {
        text.Text = "开始任务";
        List<Action> actions = new List<Action>
        {
            () =>
            {
                Console.WriteLine("任务一开始");
                Task.Delay(1000).Wait();
                Console.WriteLine("任务一完成");
            },
            () =>
            {
                Console.WriteLine("任务二开始");
                Task.Delay(2000).Wait();
                Console.WriteLine("任务二完成");
            },
            () =>
            {
                Console.WriteLine("任务三开始");
                Task.Delay(3000).Wait();
                Console.WriteLine("任务三完成");
                
            }
        };
        // 将所有 Action 包装为 Task 并等待完成
        List<Task> tasks = new List<Task>();
        foreach (var action in actions)
        { 
            tasks.Add(Task.Run(action));
            
        }
        await Task.WhenAny(tasks);
        Console.WriteLine("至少有一个完成");
        await Task.WhenAll(tasks);
        Console.WriteLine("已全部完成");
        text.Text = "已全部完成";
    }
}