using System.Diagnostics;
using System.Windows.Documents;
using System.Windows.Input;
using Microsoft.Win32;

namespace WindowUI;

public partial class ScriptChange
{
    private string _currentFilePath = string.Empty; //文件路径
    private string fileContent = string.Empty;

    public ScriptChange()
    {
        InitializeComponent();
    }

    private void ExitWindow(object sender, RoutedEventArgs e)
    {
        //退出窗口
        Close();
    }

    private void SelectFile(object sender, RoutedEventArgs e)
    {
        var openFileDialog = new OpenFileDialog
        {
            Filter = "Script文件 | *.txt"
        };
        if (openFileDialog.ShowDialog() == true)
        {
            _currentFilePath = openFileDialog.FileName;

            ScriptName.Text = _currentFilePath; //设置文件路径到 TextBox

            using (var reader = new StreamReader(_currentFilePath, true))
            {
                fileContent = reader.ReadToEnd();
            }

            var paragraph = new Paragraph();
            // 创建并添加文本
            paragraph.Inlines.Add(new Run(fileContent));
            // 清空富文本框并插入新的段落
            Script.Document.Blocks.Clear();
            Script.Document.Blocks.Add(paragraph);
        }
    }

    private void SaveFile(object sender, RoutedEventArgs e)
    {
        if (_currentFilePath == "") return;
        // 保存富文本框内的内容到桌面
        var savePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\转换后脚本.txt";
        using (var writer = new StreamWriter(savePath))
        {
            writer.Write(new TextRange(Script.Document.ContentStart, Script.Document.ContentEnd).Text);
        }

        Message.ShowSnack();
    }

    private void Image_MouseEnter_Save(object sender, MouseEventArgs e)
    {
        var savePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\转换后脚本.txt";
        using (var writer = new StreamWriter(savePath))
        {
            writer.Write(new TextRange(Script.Document.ContentStart, Script.Document.ContentEnd).Text);
        }

        Message.ShowSnack();
    }

    private void Image_MouseDown_Open(object sender, MouseButtonEventArgs e)
    {
        var openFileDialog = new OpenFileDialog
        {
            Filter = "Script文件 | *.txt"
        };
        if (openFileDialog.ShowDialog() == true)
        {
            _currentFilePath = openFileDialog.FileName;

            ScriptName.Text = _currentFilePath; //设置文件路径到 TextBox

            using (var reader = new StreamReader(_currentFilePath, true))
            {
                fileContent = reader.ReadToEnd();
            }

            var paragraph = new Paragraph();
            //创建并添加文本
            paragraph.Inlines.Add(new Run(fileContent));
            //清空富文本框并插入新的段落
            Script.Document.Blocks.Clear();
            Script.Document.Blocks.Add(paragraph);
        }
    }

    private void Image_MouseDown_OpenScript(object sender, MouseButtonEventArgs e)
    {
        var savePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\转换后脚本.txt";
        //打开文件路径
        if (savePath != null)
        {
            var processStartInfo = new ProcessStartInfo
            {
                FileName = savePath,
                UseShellExecute = true
            };
            Process.Start(processStartInfo);
        }
    }
}