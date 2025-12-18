namespace WindowUI.entity;

public class Region(string name, string tips, bool mk, string file)
{
    public string Name { get; set; } = name;
    public string Tips { get; set; } = tips;
    public bool mk { get; set; } = mk;
    public string file { get; set; } = file;
}