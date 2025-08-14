using Microsoft.Win32;

namespace WindowUI.Pages;

public partial class DataCheck
{
    public DataCheck()
    {
        InitializeComponent();
    }

    //盐城制卡数据检验
    private void CheckZhiKaFile(object sender, RoutedEventArgs e)
    {
        var openFolderDialog = new OpenFolderDialog();
        if (openFolderDialog.ShowDialog() == true)
        {
            var folderPath = openFolderDialog.FolderName;
            var files = Directory.GetFiles(folderPath);

            //文件数量
            Console.WriteLine($"文件数量：{files.Count()}");
            //检查文件号码段是否连续
            ValidateFileNumberSequence(folderPath);

            //检查文件大小和行数
            foreach (var file in files)
            {
                var fileInfo = new FileInfo(file);

                // 检查文件大小（7KB = 7123字节）
                if (fileInfo.Length != 7123) Console.WriteLine($"[文件大小错误] {file}，实际大小：{fileInfo.Length}");

                // 检查文件行数
                try
                {
                    using var stream = new StreamReader(file);
                    string? line;
                    int lineCount = 0;
                    while ((line = stream.ReadLine()) != null)
                    {
                        lineCount++;
                    }
                    if (lineCount != 46) 
                    {
                        Console.WriteLine($"[行数错误] {file}（实际行数：{lineCount}）");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[读取失败] {file} - {ex.Message}");
                }
            }

            Console.WriteLine("检查完毕！");
        }
    }

    private void ValidateFileNumberSequence(string directoryPath)
    {
        try
        {
            var files = Directory.GetFiles(directoryPath);
            var validNumbers = new List<long>();

            // 1. 提取并过滤有效号码
            foreach (var file in files)
            {
                var fileName = Path.GetFileNameWithoutExtension(file);
                string[] parts = fileName.Split('_');

                if (parts.Length < 5)
                {
                    Console.WriteLine($"文件 {Path.GetFileName(file)} 命名格式错误，缺少必要部分");
                    continue;
                }

                var numberStr = parts[4];
                if (!long.TryParse(numberStr, out var number))
                {
                    Console.WriteLine($"文件 {Path.GetFileName(file)} 包含无效号码: {numberStr}");
                    continue;
                }

                if (number % 10 == 4)
                {
                    Console.WriteLine($"文件 {Path.GetFileName(file)} 包含不吉利的尾号4: {number}");
                    continue;
                }

                validNumbers.Add(number);
            }

            // 2. 排序号码
            validNumbers.Sort();

            // 3. 验证连续性
            var isSequenceValid = true;
            for (var i = 0; i < validNumbers.Count - 1; i++)
            {
                var current = validNumbers[i];
                var actualNext = validNumbers[i + 1];
                var expectedNext = GetExpectedNextNumber(current);

                if (actualNext != expectedNext)
                {
                    Console.WriteLine($"发现断号: {current} 之后应为 {expectedNext}，但实际是 {actualNext}");
                    isSequenceValid = false;
                }
            }

            Console.WriteLine(isSequenceValid
                ? "所有文件号码连续有效"
                : "发现不连续号码");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"处理过程中发生了错误: {ex.Message}");
        }
    }

    private long GetExpectedNextNumber(long currentNumber)
    {
        var candidate = currentNumber + 1;
        while (candidate % 10 == 4) // 跳过所有尾号4
            candidate++;
        return candidate;
    }
    
}