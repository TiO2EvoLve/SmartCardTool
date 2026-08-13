using System.Text;
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
    private string DpfFilePath { get; set; }

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
    //选择DPF文件
    private void SelectDpfFile(object sender, RoutedEventArgs e)
    {
        _openFileDialog.Filter = "dpf files (*.dpf)|*.dpf";
        _openFileDialog.Title = "选择一个DPF文件";
        if (_openFileDialog.ShowDialog() == true && !string.IsNullOrEmpty(_openFileDialog.FileName))
            DpfFilePath = _openFileDialog.FileName;
    }
    
    //淮南处理逻辑
    private async void 淮南解析(object sender, RoutedEventArgs e)
    {
        if(string.IsNullOrEmpty(XdFilePath) || string.IsNullOrEmpty(KeyFilePath))
        {
            Message.ShowMessageBox("错误", "请先选择xd文件和key文件");
            return;
        }
        await 淮南.Parse(XdFilePath, KeyFilePath);
    }

    private async void 淄博解析(object sender, RoutedEventArgs e)
    {
        if(string.IsNullOrEmpty(XdFilePath) || string.IsNullOrEmpty(KeyFilePath))
        {
            Message.ShowMessageBox("错误", "请先选择xd文件和key文件");
            return;
        }
        await 淄博.Parse(XdFilePath, KeyFilePath);
    }

    private async void 凯里解析(object sender, RoutedEventArgs e)
    {
        if(string.IsNullOrEmpty(XdFilePath) || string.IsNullOrEmpty(KeyFilePath))
        {
            Message.ShowMessageBox("错误", "请先选择xd文件和key文件");
            return;
        }
        await 凯里.Parse(XdFilePath, KeyFilePath);
    }
    private async void 合肥通解析(object sender, RoutedEventArgs e)
    {
        if(string.IsNullOrEmpty(DpfFilePath))
        {
            Message.ShowMessageBox("错误", "请先选择DPF文件");
            return;
        }
        await 合肥通.Parse(DpfFilePath);
    }
}