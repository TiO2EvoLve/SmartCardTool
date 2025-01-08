using WindowUI.Pages;

namespace WindowUI.Sort;

public class 桂林公交
{
    public static void Run(MemoryStream ExcelData)
    {
        //取出Excle文件的数据
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // 避免出现许可证错误
        List<string> UIDData = new List<string>();
        List<string> SNData = new List<string>();
        using (var package = new ExcelPackage(ExcelData))
        {
            var worksheet = package.Workbook.Worksheets[0]; // 获取第一个工作表
            int rowCount = worksheet.Dimension.Rows; //获取行数
            //遍历Excel文件的每一行
            for (int row = 2; row <= rowCount; row++)
            {
                string UIDValue = worksheet.Cells[row, 3].Text;
                UIDValue = "00908670" + UIDValue;
                UIDData.Add(UIDValue);
                string SNValue = worksheet.Cells[row, 2].Text;
                SNData.Add(SNValue);
            }
        }
        桂林菜单 guiLin= new();
        guiLin.ShowDialog();
        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string fileName = $"GXJT_0{guiLin.SN.Text}_{guiLin.Count.Text}_00_V100-{SNData.Count}.rdi";
        string filePath = Path.Combine(desktopPath, fileName);
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            for (int i = 0; i < SNData.Count; i++)
            {
                if (i == SNData.Count - 1)
                {
                    writer.Write($"{UIDData[i]} {SNData[i]}"); 
                }
                else
                {
                    writer.WriteLine($"{UIDData[i]} {SNData[i]}"); 
                }
            }
        }
        MessageBox.Show($"数据保存到桌面: {filePath}"); 
    }
}