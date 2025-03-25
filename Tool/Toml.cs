using Tommy;

namespace WindowUI.Tool;

public class Toml
{
    public static string GetToml(string root,string key)
    {
        var configPath = "Config/config.toml";
        TextReader tomlText = new StreamReader(configPath);
        var table = TOML.Parse(tomlText);
        return table[root][key];
    }
}