
using Microsoft.Win32;
using System.Windows.Forms;
namespace WindowUI.Pages;

public partial class DataCheck
{
    public DataCheck()
    {
        InitializeComponent();
    }

    private void CheckZhiKaFile(object sender, RoutedEventArgs e)
    {
        var openFolderDialog = new OpenFolderDialog();
        if (openFolderDialog.ShowDialog() == true)
        {
            string folderPath = openFolderDialog.FolderName;
            var files = Directory.GetFiles(folderPath);

            foreach (var file in files)
            {
                var fileInfo = new FileInfo(file);
            
                // 检查文件大小（7KB = 7123字节）
                if (fileInfo.Length != 7123)
                {
                    Console.WriteLine($"[文件大小错误] {file}，实际大小：{fileInfo.Length}");
                    continue;
                }

                // 检查文件行数
                try
                {
                    var lines = File.ReadAllLines(file);
                    if (lines.Length != 46)
                    {
                        Console.WriteLine($"[行数错误] {file}（实际行数：{lines.Length}）");
                        continue;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[读取失败] {file} - {ex.Message}");
                }
            }
        }

    }
}