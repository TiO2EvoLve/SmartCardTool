using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace WindowUI.Tool;

public static class LogManage
{
    static LogManage()
    {
        var rccWindow = Application.Current.Windows.OfType<RCC>().FirstOrDefault();
        if (rccWindow != null) richTextBox = rccWindow.log_text;
    }

    private static RichTextBox richTextBox { get; }

    public static void Clear()
    {
        richTextBox.Document.Blocks.Clear();
        var paragraph = new Paragraph();
        paragraph.LineHeight = 5;
        paragraph.FontFamily = new FontFamily("Microsoft YaHei");
        paragraph.FontSize = 12;
        richTextBox.Document.Blocks.Add(paragraph);
    }

    public static void AddLog(string log)
    {
        var now = DateTime.Now;
        log = $"[{now:HH:mm:ss}] {log}";
        richTextBox.AppendText(log + "\n");
        richTextBox.ScrollToEnd();
    }
}