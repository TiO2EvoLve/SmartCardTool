using System.Windows.Controls;
using System.Windows.Documents;

namespace WindowUI.Tool;

public static class LogManage
{
    static RichTextBox richTextBox { get; set; }
    
    static LogManage()
    {
        var rccWindow = Application.Current.Windows.OfType<RCC>().FirstOrDefault();
        if (rccWindow != null)
        {
            richTextBox = rccWindow.log_text;
        }
    }
    
    public static void Clear()
    {
        richTextBox.Document.Blocks.Clear();
    }
    
    public static void AddLog(string log)
    {
        DateTime now = DateTime.Now;
        log = $"[{now:HH:mm:ss}] {log}";
        richTextBox.AppendText(log + "\n");
        richTextBox.ScrollToEnd();
    }
}