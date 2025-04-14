namespace WindowUI.Sort;

public class 平凉公交
{
    public static void Run(string FilePath, string excelFileName)
    {
        //取出Excle文件的数据
        
        List<string> UIDData = new List<string>();
        List<string> SNData = new List<string>();
        
        string sql = "SELECT outsidelasercode From print order by outsidelasercode ASC";
        SNData = Mdb.Select(FilePath, sql);
        sql = "SELECT insidecode From print order by outsidelasercode ASC";
        UIDData = Mdb.Select(FilePath, sql);
        
        for (int i = 0; i < UIDData.Count; i++)
        {
            UIDData[i] = Convert.ToUInt32(UIDData[i], 16).ToString();
        }
        
        // using (var package = new ExcelPackage(ExcelData))
        // {
        //     var worksheet = package.Workbook.Worksheets[0]; // 获取第一个工作表
        //     var rowCount = worksheet.Dimension.Rows; //获取行数
        //     //遍历Excel文件的每一行
        //     for (var row = 1; row <= rowCount; row++)
        //     {
        //         var UIDValue = worksheet.Cells[row, 2].Text;
        //         UIDValue = Convert.ToUInt32(Tools.ChangeHexPairs(UIDValue), 16).ToString();
        //         UIDData.Add(UIDValue);
        //         var SNValue = worksheet.Cells[row, 8].Text;
        //         SNData.Add(SNValue);
        //     }
        // }

        var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        var fileName = $"{excelFileName}.txt";
        var filePath = Path.Combine(desktopPath, fileName);
        using (var writer = new StreamWriter(filePath))
        {
            for (var i = 0; i < SNData.Count; i++)
            {
                if (i < UIDData.Count)
                {
                    writer.WriteLine($"{UIDData[i]}\t74400000{SNData[i]}\t1");
                }
                else
                {
                    writer.Write($"{SNData[i]}\t74400000{SNData[i]}\t1");
                }
            }
        }

        Message.ShowSnack();
    }
}