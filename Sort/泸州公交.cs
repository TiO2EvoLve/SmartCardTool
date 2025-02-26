using WindowUI.Pages;

namespace WindowUI.Sort;

public class 泸州公交
{
    public static void Run(MemoryStream ExcelData,string excelFileName)
    {
        泸州菜单 luzhou = new();
        luzhou.ShowDialog();
        string cardtype = luzhou.CardType;
        if (cardtype == "") { Message.ShowMessageBox("错误","未选择卡类型"); return; }
        //先处理Excel文件
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // 避免出现许可证错误
        List<string> uid_10Data = new List<string>();
        List<string> cardData = new List<string>();
        using (var package = new ExcelPackage(ExcelData))
        {
            var worksheet = package.Workbook.Worksheets[0]; // 获取第一个工作表
            int rowCount = worksheet.Dimension.Rows; //获取行数
            //遍历Excel文件的每一行
            for (int row = 2; row <= rowCount; row++)
            {
                string uid_10Value = worksheet.Cells[row, 5].Text;
                string cardValue = worksheet.Cells[row, 1].Text;
                if (cardValue.Length == 19)
                {
                    cardValue = cardValue.Substring(11, 8);
                }
                cardValue = cardtype + cardValue;

                uid_10Data.Add(uid_10Value);
                cardData.Add(cardValue);
            }
        }
        // 创建一个新的Excel文件
        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add(excelFileName);
            // 插入数据
            worksheet.Cells[1, 1].Value = "UID_10";
            worksheet.Cells[1, 2].Value = "卡号(16位)";
            worksheet.Cells[1, 3].Value = "卡商标志";
            for (int i = 0; i < uid_10Data.Count; i++)
            {
                worksheet.Cells[i + 2, 1].Value = uid_10Data[i];
                worksheet.Cells[i + 2, 2].Value = cardData[i];
                worksheet.Cells[i + 2, 3].Value = 8670;
            }
            // 保存文件到桌面
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string fileName = $"{excelFileName}.xlsx";
            string filePath = Path.Combine(desktopPath, fileName);
            package.SaveAs(new FileInfo(filePath));
            Message.ShowSnack();
        }    
    }
}