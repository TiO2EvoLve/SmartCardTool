
using Microsoft.Win32;
using Tommy;

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
            string folderPath = openFolderDialog.FolderName;
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
                if (fileInfo.Length != 7123)
                {
                    Console.WriteLine($"[文件大小错误] {file}，实际大小：{fileInfo.Length}");
                }

                // 检查文件行数
                try
                {
                    var lines = File.ReadAllLines(file);
                    if (lines.Length != 46)
                    {
                        Console.WriteLine($"[行数错误] {file}（实际行数：{lines.Length}）");
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
     void ValidateFileNumberSequence(string directoryPath)
        {
            try
            {
                var files = Directory.GetFiles(directoryPath);
                List<long> validNumbers = new List<long>();

                // 1. 提取并过滤有效号码
                foreach (var file in files)
                {
                    string fileName = Path.GetFileNameWithoutExtension(file);
                    string[] parts = fileName.Split('_');

                    if (parts.Length < 5)
                    {
                        Console.WriteLine($"文件 {Path.GetFileName(file)} 命名格式错误，缺少必要部分");
                        continue;
                    }

                    string numberStr = parts[4];
                    if (!long.TryParse(numberStr, out long number))
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
                bool isSequenceValid = true;
                for (int i = 0; i < validNumbers.Count - 1; i++)
                {
                    long current = validNumbers[i];
                    long actualNext = validNumbers[i + 1];
                    long expectedNext = GetExpectedNextNumber(current);

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

        long GetExpectedNextNumber(long currentNumber)
        {
            long candidate = currentNumber + 1;
            while (candidate % 10 == 4) // 跳过所有尾号4
            {
                candidate++;
            }
            return candidate;
        }

        private void CheckLossSn(object sender, RoutedEventArgs e)
        {
            List<long> SN = new List<long>();
            OpenFileDialog open = new OpenFileDialog()
            {
                Filter = "mdb文件(*.mdb) | *.mdb",
                Title = "请选择数据库文件"
            };
            if (open.ShowDialog() == true)
            {
                string sql = "select SN from RCC order by SN ASC ";
                List<string> stringList = Mdb.Select(open.FileName, sql);
                SN = stringList.Where(s => long.TryParse(s, out _))
                    .Select(s => long.Parse(s))
                    .ToList();
            }
            // 找出不连续的号码
            List<long> nonConsecutiveNumbers = FindNonConsecutiveNumbers(SN);

            // 输出不连续的号码
            if (nonConsecutiveNumbers.Count > 0)
            {
                Console.WriteLine("不连续的号码有：");
                foreach (long number in nonConsecutiveNumbers)
                {
                    Console.WriteLine(number);
                }
            }
            else
            {
                Console.WriteLine("号码是连续的。");
            }
        }
        private List<long> FindNonConsecutiveNumbers(List<long> numbers)
        {
            List<long> nonConsecutive = new List<long>();
            if (numbers.Count <= 1)
            {
                return nonConsecutive;
            }

            for (int i = 0; i < numbers.Count - 1; i++)
            {
                long current = numbers[i];
                long next = numbers[i + 1];

                long expectedNext = GetNextExpectedNumber(current);

                if (next != expectedNext)
                {
                    nonConsecutive.Add(current);
                }
            }

            return nonConsecutive;
        }

        private long GetNextExpectedNumber(long current)
        {
            long next = current + 1;
            while (next % 10 == 4)
            {
                next++;
            }
            return next;
        }

    
}