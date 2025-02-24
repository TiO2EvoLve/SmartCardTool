using WindowUI.Pages;

namespace WindowUI.Sort;

public class 桂林公交
{
    public static void Run(string FilePath)
    {
        //取出文件的数据
        List<string> SNData = new List<string>();
        List<string> UIDData = new List<string>();
        
        string sql = "select SN from RCC order by SN ASC ";
        SNData = Mdb.Select(FilePath,sql);
        sql = "select UID from RCC order by SN ASC ";
        UIDData = Mdb.Select(FilePath,sql);
        
        桂林菜单 guiLin= new();
        guiLin.ShowDialog();
        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string fileName = $"GXJT_{guiLin.SN.Text}_{guiLin.Count.Text}_00_V100.rdi";
        string filePath = Path.Combine(desktopPath, fileName);
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            for (int i = 0; i < SNData.Count; i++)
            {
                if (i == SNData.Count - 1)
                {
                    writer.Write($"00908670{UIDData[i]} {SNData[i]}"); 
                }
                else
                {
                    writer.WriteLine($"00908670{UIDData[i]} {SNData[i]}"); 
                }
            }
        }
        Message.ShowSnack();
    }
}