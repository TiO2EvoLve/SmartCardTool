using System.Security.Cryptography;
using System.Text;
using System.Windows;

namespace WindowUI.Pages;

public partial class SyncTest : Window
{
    public SyncTest()
    {
        InitializeComponent();
    }

    private async void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        List<Action> actions = new List<Action>
        {
            () =>
            {
                Console.WriteLine("Action 1 started");
                Task.Delay(2000).Wait();
                Console.WriteLine("Action 1 completed");
            },
            () =>
            {
                Console.WriteLine("Action 2 started");
                Task.Delay(1000).Wait();
                Console.WriteLine("Action 2 completed");
            },
            () =>
            {
                Console.WriteLine("Action 3 started");
                Task.Delay(3000).Wait();
                Console.WriteLine("Action 3 completed");
            }
        };

        // 将所有 Action 包装为 Task 并等待完成
        List<Task> tasks = new List<Task>();
        foreach (var action in actions)
        {
            tasks.Add(Task.Run(action));
        }

        await Task.WhenAll(tasks);
        Console.WriteLine("All Actions completed");
        text.Text = "完成";
        
    }

}