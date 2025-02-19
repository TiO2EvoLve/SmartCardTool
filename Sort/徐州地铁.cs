namespace WindowUI.Sort;

public class 徐州地铁
{
    public static void Run(string FilePath, string excelFileName)
    {
        
        List<string> SNData = new List<string>();
        List<string> UidData = new List<string>();
        
        string sql = "SELECT NUM FROM kahao order by NUM ASC";
        SNData = Mdb.Select(FilePath, sql);
        
        sql = "SELECT UID_ FROM kahao order by NUM ASC";
        UidData = Mdb.Select(FilePath, sql);

        // 保存文件到桌面
        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string fileName = $"{excelFileName}.txt";
        string filePath = Path.Combine(desktopPath, fileName);

        using (StreamWriter writer = new StreamWriter(filePath))
        {
            for (int i = 0; i < SNData.Count; i++)
            {
                if (i == SNData.Count - 1)
                {
                    writer.Write($"{SNData[i]}\t{UidData[i]}00000000");
                }
                else
                {
                    writer.WriteLine($"{SNData[i]}\t{UidData[i]}00000000");
                }
            }
        }
        Message.ShowSnack();
    }
}