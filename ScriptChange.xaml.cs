using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Wpf.Ui.Controls;

namespace WindowUI
{
    /// <summary>
    /// ScriptChange.xaml 的交互逻辑
    /// </summary>
    public partial class ScriptChange : Window
    {
        public ScriptChange()
        {
            InitializeComponent();
        }
        private void ExitWindow(object sender, RoutedEventArgs e)
        {
            //退出窗口
            Close();
        }

        private string _currentFilePath = string.Empty;//文件路径
        private string fileContent = string.Empty;

        private void SelectFile(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Script Files | *.txt"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                _currentFilePath = openFileDialog.FileName;

                ScriptName.Text = _currentFilePath; // 设置文件路径到 TextBox

                using (StreamReader reader = new StreamReader(_currentFilePath, true))
                {
                    fileContent = File.ReadAllText(_currentFilePath);
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
            // 保存文件
            if (string.IsNullOrEmpty(_currentFilePath))
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "Script Files | *.txt"
                };
                if (saveFileDialog.ShowDialog() == true)
                {
                    _currentFilePath = saveFileDialog.FileName;
                }
            }
        }

        private void EditorTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Console.WriteLine("文本框内容已更改");
            
        }
    }
}
