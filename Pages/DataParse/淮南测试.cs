namespace WindowUI.Pages.DataParse;

public class 淮南测试
{
    public static async Task Parse(string XdFilePath, string KeyFilePath)
    {

        if (string.IsNullOrEmpty(XdFilePath) || string.IsNullOrEmpty(KeyFilePath))
        {
            Message.ShowMessageBox("错误", "请先选择文件");
            return;
        }
        
        MessageBox.Show("数据生成成功!!!");
    }

}