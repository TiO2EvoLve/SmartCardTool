namespace WindowUI.Sort;

public class 西安地铁
{
    public static void Run()
    {
        string inputPath = @"您的输入路径";
        string outputPath = @"您的输出路径";

        File.WriteAllBytes(outputPath, 
            File.ReadLines(inputPath)
                .Where(line => !string.IsNullOrWhiteSpace(line))
                .SelectMany(line => Enumerable.Range(0, line.Length / 2)
                    .Select(i => Convert.ToByte(line.Substring(i * 2, 2), 16)))
                .ToArray()); 
    }

   
}