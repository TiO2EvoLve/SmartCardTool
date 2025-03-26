using Tommy;

namespace WindowUI.Tool;

public class Toml
{
    public static string GetToml(string root,string key)
    {
        var configPath = "Config/config.toml";
        string toml = "";
        try
        {
            TextReader tomlText = new StreamReader(configPath);
            var table = TOML.Parse(tomlText);
            toml =  table[root][key];
        }catch
        {
            Message.ShowMessageBox("错误", "可能未找到该数据");
        }

        return toml;
    }
}