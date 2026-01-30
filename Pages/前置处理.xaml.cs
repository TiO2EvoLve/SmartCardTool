using Microsoft.Win32;
using OfficeOpenXml.Style;

namespace WindowUI.Pages;

public partial class 前置处理 : Window
{
    public 前置处理()
    {
        InitializeComponent();
    }
    List<string> name = new();
    List<string> id = new();
    List<string> sn = new();
    List<string> order = new();
    List<string> date = new();
    
    

    private void OpenFile(object sender, RoutedEventArgs e)
    {
        OpenFileDialog openFileDialog = new()
        {
            Filter = "xd files (*.TXT)|*.TXT",
            Title = "选择一个TXT文件"
        };
        openFileDialog.ShowDialog();
        if (openFileDialog.FileName == "") return;
        //读取文件，处理每一行数据
        foreach (string line in File.ReadLines(openFileDialog.FileName))
        {
            var parts = line.Split("\t");
            if (parts.Length >= 8)
            {
                name.Add(parts[0]);
                id.Add(parts[2]);
                sn.Add(parts[3]);
                order.Add(parts[6]);
                date.Add(parts[7]);
            }
        }
        //生成excel表格
        var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        var fileName = $"天津敬老卡表格.xlsx";
        var filePath = Path.Combine(desktopPath, fileName);

        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add("敬老卡信息");
            worksheet.Cells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells[1, 1].Value = "姓名";
            worksheet.Cells[1, 2].Value = "身份证号";
            worksheet.Cells[1, 3].Value = "卡号";
            worksheet.Cells[1, 4].Value = "订单号";
            worksheet.Cells[1, 5].Value = "发卡日期";
            for (var i = 0; i < sn.Count; i++)
            {
                worksheet.Cells[i + 2, 1].Value = name[i];
                worksheet.Cells[i + 2, 2].Value = id[i];
                worksheet.Cells[i + 2, 3].Value = sn[i];
                worksheet.Cells[i + 2, 4].Value = order[i];
                worksheet.Cells[i + 2, 5].Value = date[i];
            }

            worksheet.Cells.AutoFitColumns();
            // 保存文件到桌面
            package.SaveAs(new FileInfo(filePath));
            MessageBox.Show("文件已保存到桌面！");
        }
    }
}