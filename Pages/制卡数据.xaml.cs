using System.Text;
using System.Windows.Controls;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Microsoft.Win32;
using WindowUI.Pages.DataParse;

namespace WindowUI.Pages;

public partial class 制卡数据
{
    private readonly OpenFileDialog _openFileDialog = new();

    public 制卡数据()
    {
        InitializeComponent();
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
    }

    private string XdFilePath { get; set; }
    private string KeyFilePath { get; set; }

    //选择xd文件
    private void SelectXdFile(object sender, RoutedEventArgs e)
    {
        _openFileDialog.Filter = "xd files (*.xd)|*.xd";
        _openFileDialog.Title = "选择一个xd文件";
        if (_openFileDialog.ShowDialog() == true && !string.IsNullOrEmpty(_openFileDialog.FileName))
            XdFilePath = _openFileDialog.FileName;
    }
    //选择key文件
    private void SelectKeyFile(object sender, RoutedEventArgs e)
    {
        _openFileDialog.Filter = "xd files (*.key)|*.key";
        _openFileDialog.Title = "选择一个key文件";
        if (_openFileDialog.ShowDialog() == true && !string.IsNullOrEmpty(_openFileDialog.FileName))
            KeyFilePath = _openFileDialog.FileName;
    }
    
    
    //淮南处理逻辑
    private async void 淮南解析(object sender, RoutedEventArgs e)
    {
        await 淮南.Parse(XdFilePath, KeyFilePath);
    }

    private async void 淄博解析(object sender, RoutedEventArgs e)
    {
        await 淄博.Parse(XdFilePath, KeyFilePath);
    }
}