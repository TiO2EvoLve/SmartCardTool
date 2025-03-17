using WindowUI.Pages;

namespace WindowUI.Sort;

public class 泸州公交
{
    public static void Run(string FilePath, string excelFileName)
    {
        泸州菜单 luzhou = new();
        luzhou.ShowDialog();
        var cardtype = luzhou.CardType;

        List<string> SnData = new List<string>();
        var Uid10Data = new List<string>();

        var sql = "select SerialNum from kahao order by SerialNum ASC";
        SnData = Mdb.Select(FilePath, sql);

        for (var i = 0; i < SnData.Count; i++)
        {
            if (SnData[i].Length == 8)
            {
                SnData[i] = $"{cardtype}{SnData[i]}";
            }
            else
            {
                SnData[i] = SnData[i].Substring(3, 16); 
            }
            if (!SnData[i].StartsWith(cardtype))
            {
                Message.ShowMessageBox("错误", "检查到卡类型不正确");
                return;
            }
        }

        sql = "select UID_10 from kahao order by SerialNum ASC ";
        Uid10Data = Mdb.Select(FilePath, sql);

        // 创建一个新的Excel文件
        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add(excelFileName);
            // 插入数据
            worksheet.Cells[1, 1].Value = "UID_10";
            worksheet.Cells[1, 2].Value = "卡号(16位)";
            worksheet.Cells[1, 3].Value = "卡商标志";
            for (var i = 0; i < Uid10Data.Count; i++)
            {
                worksheet.Cells[i + 2, 1].Value = Uid10Data[i];
                worksheet.Cells[i + 2, 2].Value = SnData[i];
                worksheet.Cells[i + 2, 3].Value = 8670;
            }

            // 保存文件到桌面
            var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var filePath = Path.Combine(desktopPath, $"{excelFileName}.xlsx");
            package.SaveAs(new FileInfo(filePath));
            Message.ShowSnack();
        }
    }
}