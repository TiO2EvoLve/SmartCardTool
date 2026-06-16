using Microsoft.Win32;

namespace WindowUI.Pages;

public partial class 南通市民卡菜单 : Window
{
    public string FileName;
    public string Order;
    public string Type;
    public string Id;
    public string StartTime;
    public string VaildTime;
    
    public 南通市民卡菜单()
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
        FileName = Path.GetFileName(openFileDialog.FileName);
        if (FileName == "") return;
        TextBlock.Text = FileName;
        //读出文件第一行数据
        var firstLine = File.ReadLines(openFileDialog.FileName).FirstOrDefault();
        if (firstLine is null) return;
        var firstLineArray = firstLine.Split("|");
        Order = firstLineArray[0];
        Type = firstLineArray[20];
        Id = firstLineArray[21];
        StartTime = firstLineArray[8];
        VaildTime = firstLineArray[9];
    }
}