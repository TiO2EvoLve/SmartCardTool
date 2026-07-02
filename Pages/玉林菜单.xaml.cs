using Microsoft.Win32;

namespace WindowUI.Pages;

public partial class 玉林菜单 : Window
{
    public string mkFilePath;
    public string mkFileName;

    public 玉林菜单()
    {
        InitializeComponent();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void OpenFile(object sender, RoutedEventArgs e)
    {
        var openFileDialog = new OpenFileDialog();
        openFileDialog.Filter = "txt文件|*.txt";
        openFileDialog.ShowDialog();
        if (string.IsNullOrEmpty(openFileDialog.FileName)) return;
        mkFilePath = openFileDialog.FileName;
        mkFileName = Path.GetFileNameWithoutExtension(mkFilePath);
        if (mkFilePath == "") return;
        TextBlock.Text = mkFileName;
    }
}