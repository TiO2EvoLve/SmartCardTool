using Microsoft.Win32;

namespace WindowUI.Pages;

public partial class 西安地铁菜单 : Window
{
    public string FilePath;//Order文件路径
    public string FileName;//Ordel文件名
    public string 订单ID;//订单ID
    public string 卡片厂商ID;//卡片厂商ID
    public string 卡片介质;//卡片介质
    public string 卡片数量;//卡片数量
    public string 起始卡号;//逻辑ID起始编号

    public 西安地铁菜单()
    {
        InitializeComponent();
    }
    
    private void Button_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void OpenFile(object sender, RoutedEventArgs e)
    {
        var openFileDialog = new OpenFileDialog
        {
            Filter = "Excel文件|*.xlsx"
        };
        openFileDialog.ShowDialog();
        if (string.IsNullOrEmpty(openFileDialog.FileName)) return;
        FilePath = openFileDialog.FileName;
        if (FilePath == "") return;
        var ExcelData = new MemoryStream(File.ReadAllBytes(FilePath));
        
        using (var package = new ExcelPackage(ExcelData))
        {
            var worksheet = package.Workbook.Worksheets[0]; // 获取第一个工作表
            订单ID = worksheet.Cells[2, 1].Text;
            if (worksheet.Cells[2, 6].Text == "中安特")
            {
                卡片厂商ID = "04";
            }else
            {
                throw new Exception("卡片厂商ID不正确");
            }
            卡片介质 = worksheet.Cells[2, 2].Text == "CPU卡" ? "02" : "03";
            卡片数量 = worksheet.Cells[2, 8].Text;
            起始卡号 = worksheet.Cells[2, 7].Text;
        }
    }
}