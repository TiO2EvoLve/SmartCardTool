using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace WindowUI
{
    public partial class ScriptChange : Window
    {
        private string _currentFilePath = string.Empty;//文件路径
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
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Script文件 | *.txt"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                _currentFilePath = openFileDialog.FileName;

                ScriptName.Text = _currentFilePath; //设置文件路径到 TextBox

                using (StreamReader reader = new StreamReader(_currentFilePath, true))
                {
                    fileContent = reader.ReadToEnd();
                }
                Paragraph paragraph = new Paragraph();
                // 创建并添加文本
                paragraph.Inlines.Add(new Run(fileContent));
                // 清空富文本框并插入新的段落
                Script.Document.Blocks.Clear();
                Script.Document.Blocks.Add(paragraph);
            }
        }
        private void SaveFile(object sender, RoutedEventArgs e)
        {
            // 保存富文本框内的内容到桌面
            string savePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\转换后脚本.txt";
            using (StreamWriter writer = new StreamWriter(savePath))
            {
                writer.Write(new TextRange(Script.Document.ContentStart, Script.Document.ContentEnd).Text);
            }
            System.Windows.MessageBox.Show("保存成功！");
        }
        private void Image_MouseEnter_Save(object sender, System.Windows.Input.MouseEventArgs e)
        {
            string savePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\转换后脚本.txt";
            using (StreamWriter writer = new StreamWriter(savePath))
            {
                writer.Write(new TextRange(Script.Document.ContentStart, Script.Document.ContentEnd).Text);
            }
            System.Windows.MessageBox.Show("保存成功！");
        }
        private void Image_MouseDown_Open(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Script文件 | *.txt"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                _currentFilePath = openFileDialog.FileName;

                ScriptName.Text = _currentFilePath; //设置文件路径到 TextBox

                using (StreamReader reader = new StreamReader(_currentFilePath, true))
                {
                    fileContent = reader.ReadToEnd();
                }
                Paragraph paragraph = new Paragraph();
                // 创建并添加文本
                paragraph.Inlines.Add(new Run(fileContent));
                // 清空富文本框并插入新的段落
                Script.Document.Blocks.Clear();
                Script.Document.Blocks.Add(paragraph);
            }
        }
        private void Image_MouseDown_OpenScript(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            string savePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\转换后脚本.txt";
            //打开文件路径
            if (savePath != null)
            {
                var processStartInfo = new System.Diagnostics.ProcessStartInfo
                {
                    FileName = savePath,
                    UseShellExecute = true
                };
                System.Diagnostics.Process.Start(processStartInfo);
            }
        }
    }
}
