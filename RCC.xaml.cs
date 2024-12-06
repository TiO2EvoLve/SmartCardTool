using OfficeOpenXml;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Text.RegularExpressions;
using WindowUI.Pages;
namespace WindowUI;
public partial class RCC
{
    private string mkFileName { get; set; } // 记录MK文件名
    private List<string> MKDate{ get; set; } // 临时存储读取的MK文件的数据
    private string excelFileName{ get; set; }// 记录Excel文件名
    private MemoryStream ExcelData{ get; set; }// 临时存储读取的Excel的数据
    private string Region{ get; set; }// 下拉框选则的地区
    Microsoft.Win32.OpenFileDialog openFileDialog{ get; set; } //MK文件处理流
    Microsoft.Win32.OpenFileDialog openFileDialog2{ get; set; }//Excel文件处理流
    // 定义不需要选择MK文件的地区
    private readonly string[] disableButtonRegions = {"天津","郴州","合肥","其他地区"}; 
    public RCC()
    {
        InitializeComponent();
    }
    //打开MK文件
    private void OpenMKFile(object sender, RoutedEventArgs e)
    {

        //打开一个文件选择器，类型为任意
        openFileDialog = new Microsoft.Win32.OpenFileDialog
        {
            Filter = "All files (*.*)|*.*", // 允许选择所有文件
            Title = "选择一个文件"
        };
        if (openFileDialog.ShowDialog() == true)
        {
            if (openFileDialog.FileName == "") return;
            //将文件暂时存储到MKDate中
            MKDate = File.ReadAllLines(openFileDialog.FileName).ToList();
            //记录MK文件名
            mkFileName = Path.GetFileName(openFileDialog.FileName);
            //去掉MK文件名的前两个字符
            mkFileName = mkFileName.Substring(2);
            mk.Foreground = Brushes.LightGreen;
            mktextbox.Foreground = Brushes.Green;
            mktextbox.Text = mkFileName;
        }
    }
    //打开Excel文件
    private void OpenFile(object sender, RoutedEventArgs e)
    {
        openFileDialog2 = new Microsoft.Win32.OpenFileDialog
        {
            Filter = "Excel Files (*.xlsx)|*.xlsx;",
            Title = "选择一个文件"
        };
        if (openFileDialog2.ShowDialog() == true)
        {
            if (openFileDialog2.FileName == "") return;
            //记录Excel文件名
            excelFileName = Path.GetFileName(openFileDialog2.FileName);
            //去掉扩展名.xlsx
            excelFileName = excelFileName.Substring(0, excelFileName.Length - 5);
            //将文件暂时存储到ExcelDate中
            try
            {
                ExcelData = new MemoryStream(File.ReadAllBytes(openFileDialog2.FileName));
                data.Foreground = Brushes.LightGreen;
                datatextbox.Foreground = Brushes.Green;
                datatextbox.Text = excelFileName;
            }
            catch(IOException ex)
            {
                MessageBox.Show("文件已被占用，请关闭Excel表格。",
                    "文件占用", MessageBoxButton.OK, MessageBoxImage.Warning);
            }  
        }
    }
    //下拉框选择地区
    private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (LocationComboBox.SelectedItem is ComboBoxItem selectedItem && selectedItem.Content != null)
        {
            Region = selectedItem.Content.ToString() ?? throw new InvalidOperationException();
            // 根据选择的地区禁用或启用按钮
            if (SelectMKButton != null)
            {
                SelectMKButton.IsEnabled = Array.Exists(disableButtonRegions, region => region == Region);
                if (!SelectMKButton.IsEnabled) mk.Foreground = Brushes.LightGreen; else mk.Foreground = Brushes.Red;
            }
            //根据不同地区进行提示
            switch (Region)
            {
                case "泸州公交": tip.Text = "根据卡类型进行制作"; break;
                default:tip.Text = "该地区暂无提示"; break;
            }

        }

    }
    //点击处理文件按钮
    private void ProcessTheFile(object sender, RoutedEventArgs e)
    {
        if (ExcelData is null)
        {
            MessageBox.Show("请选择文件");
            return;
        }

        //根据不同地区处理文件
        switch (Region)
        {
            case "天津": TianJin(); break;
            case "兰州工作证": LanZhouGongZuoZheng(); break;
            case "青岛博研加气站": QingDaoBoYangJiaQiZhan(); break;
            case "抚顺夕阳红卡": FuShunXiYangHongKa(); break;
            case "郴州": ChenZhou(); break;
            case "潍坊夕阳红卡、爱心卡": WeiFang(); break;
            case "国网技术学院职工卡": GuoWang(); break;
            case "哈尔滨城市通敬老优待卡": HaErBin(); break;
            case "运城盐湖王府学校": YunCheng(); break;
            case "南通地铁": NanTong(); break;
            case "长沙公交荣誉卡": ChangSha(); break;
            case "泸州公交": LuZhou(); break;
            case "合肥通": HeFei(); break;
            case "青岛理工大学": QingDaoDaXue(); break;
            case "西安交通大学": XiAnDaXue(); break;
            case "呼和浩特": HuHeHaoTe(); break;
            case "重庆33A-A1": ChongQingA1(); break;
            case "西藏林芝": XIZang(); break;
            case "西藏拉萨": XIZangLaSa(); break;
            case "淄博公交": ZiBo(); break;
            case "平凉公交": Pingliang(); break;
            case "桂林公交": GuiLin(); break;
            case "陕西师范大学": ShanXi(); break;
            case "西安文理学院": XiAnXueYuan(); break;
            case "滨州公交" : BinZhou(); break;
            case "云南朗坤" : WangKun(); break;
            default: MessageBox.Show("请选择地区"); break;
        }
    }
    //天津的处理逻辑
    private void TianJin()
    {
        //先处理Excel文件
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // 避免出现许可证错误
        List<string> processedData = new List<string>();
        using (var package = new ExcelPackage(ExcelData))
        {
            var worksheet = package.Workbook.Worksheets[0]; // 获取第一个工作表
            int rowCount = worksheet.Dimension.Rows; //获取行数
                                                     //遍历Excel文件的每一行
            for (int row = 2; row <= rowCount; row++)
            {
                string firstColumnValue = worksheet.Cells[row, 1].Text;
                string secondColumnValue = worksheet.Cells[row, 2].Text;
                string newRow =
                    $"{firstColumnValue}      {firstColumnValue}      {secondColumnValue}              FFFFFFFFFFFFFFFFFFFF";
                processedData.Add(newRow);
            }
        }

        //处理MK文件
        //截取MK文件第二行的前42个字节
        MKDate[1] = MKDate[1].Substring(0, 42);
        //获取Excel总数据的条数
        int totalLines = processedData.Count;
        //将总数据条数转为6位数
        string totalLinesFormatted = totalLines.ToString("D6");
        //将MK文件的第二行的后6位替换为总数据条数
        MKDate[1] = MKDate[1].Substring(0, MKDate[1].Length - 6) + totalLinesFormatted;
        //将MK文件与Excel文件的数据合并

        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string fileName = $"RC{mkFileName}001";
        string filePath = Path.Combine(desktopPath, fileName);
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            writer.WriteLine(MKDate[0]);
            writer.WriteLine(MKDate[1]);

            for (int i = 0; i < processedData.Count; i++)
            {
                if (i == processedData.Count - 1)
                {
                    writer.Write(processedData[i]);
                }
                else
                {
                    writer.WriteLine(processedData[i]);
                }
            }
        }

        MessageBox.Show($"数据已合并并保存到文件: {filePath}");
    }
    //兰州工作证的处理逻辑
    private void LanZhouGongZuoZheng()
    {
        //取出Excle文件的数据
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // 避免出现许可证错误
                                                                    //取出第七列流水号数据
        List<string> processedData = new List<string>();
        using (var package = new ExcelPackage(ExcelData))
        {
            var worksheet = package.Workbook.Worksheets[0]; // 获取第一个工作表
            int rowCount = worksheet.Dimension.Rows; //获取行数
                                                     //遍历Excel文件的每一行
            for (int row = 1; row <= rowCount; row++)
            {
                string firstColumnValue = worksheet.Cells[row, 7].Text;
                processedData.Add(firstColumnValue);
            }
        }

        //将ExcelDate第三列的十六进制数转为10进制
        List<string> processedData2 = new List<string>();
        using (var package = new ExcelPackage(ExcelData))
        {
            var worksheet = package.Workbook.Worksheets[0]; // 获取第一个工作表
            int rowCount = worksheet.Dimension.Rows; //获取行数
                                                     //遍历Excel文件的每一行
            for (int row = 1; row <= rowCount; row++)
            {
                string firstColumnValue = worksheet.Cells[row, 3].Text;
                string firstColumnValue2 = Convert.ToUInt32(firstColumnValue, 16).ToString();
                Debug.WriteLine(firstColumnValue2);
                processedData2.Add(firstColumnValue2);
            }
        }

        //将processedData和processedData2合并起来，中间用','分隔，最后保存为txt文件到桌面
        List<string> mergedData = new List<string>();
        for (int i = 0; i < processedData.Count; i++)
        {
            string mergedRow = $"{processedData[i]},{processedData2[i]}";
            mergedData.Add(mergedRow);
        }

        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string fileName = excelFileName + ".txt";
        string filePath = System.IO.Path.Combine(desktopPath, fileName);
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            foreach (var line in mergedData)
            {
                writer.WriteLine(line);
            }
        }
        MessageBox.Show($"数据已合并并保存到文件: {filePath}");
    }
    //抚顺夕阳红卡的处理逻辑
    private void FuShunXiYangHongKa()
    {
        //取出Excel文件的数据
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // 避免出现许可证错误
        List<string> snData = new List<string>();
        List<string> uidData = new List<string>();

        using (var package = new ExcelPackage(ExcelData))
        {
            var worksheet = package.Workbook.Worksheets[0]; // 获取第一个工作表
            int rowCount = worksheet.Dimension.Rows; // 获取行数

            //遍历Excel文件的每一行
            for (int row = 1; row <= rowCount; row++)
            {
                string snValue = worksheet.Cells[row, 7].Text;
                string uidValue = worksheet.Cells[row, 3].Text;
                snData.Add(snValue);
                uidData.Add(uidValue);
            }
        }
        //将snDate与uidData合并起来,最后保存为txt文件到桌面
        List<string> mergedData = new List<string>();
        for (int i = 0; i < snData.Count; i++)
        {
            string mergedRow = $"{snData[i]} {uidData[i]}";
            mergedData.Add(mergedRow);
        }
        //保存为txt文件
        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string fileName = $"{excelFileName}.txt";
        string filePath = System.IO.Path.Combine(desktopPath, fileName);
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            for (int i = 0; i < mergedData.Count; i++)
            {
                if (i == mergedData.Count - 1)
                {
                    writer.Write(mergedData[i]);
                }
                else
                {
                    writer.WriteLine(mergedData[i]);
                }
            }
        }
        MessageBox.Show($"数据已处理并保存到文件: {filePath}");



    }
    //青岛博研加气站的处理逻辑
    private void QingDaoBoYangJiaQiZhan()
    {
        //取出Excel文件的数据
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // 避免出现许可证错误
        List<string> snData = new List<string>();
        List<string> uidData = new List<string>();

        using (var package = new ExcelPackage(ExcelData))
        {
            var worksheet = package.Workbook.Worksheets[0]; // 获取第一个工作表
            int rowCount = worksheet.Dimension.Rows; // 获取行数

            //遍历Excel文件的每一行
            for (int row = 1; row <= rowCount; row++)
            {
                string snValue = worksheet.Cells[row, 8].Text;
                string uidValue = worksheet.Cells[row, 3].Text;
                snData.Add(snValue);
                uidData.Add(uidValue);
            }
        }

        // 创建一个新的Excel文件
        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add(excelFileName);

            // 插入标题行
            worksheet.Cells[1, 1].Value = "SN";
            worksheet.Cells[1, 2].Value = "UID";

            // 插入数据
            for (int i = 0; i < snData.Count; i++)
            {
                worksheet.Cells[i + 2, 1].Value = snData[i];
                worksheet.Cells[i + 2, 2].Value = uidData[i];
            }

            // 保存文件到桌面
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string fileName = $"{excelFileName}.xlsx";
            string filePath = System.IO.Path.Combine(desktopPath, fileName);
            package.SaveAs(new FileInfo(filePath));
            MessageBox.Show($"数据已处理并保存到文件: {filePath}");
        }
    }
    //青岛理工大学的处理逻辑
    private void QingDaoDaXue()
    {
        //取出Excel文件的数据
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // 避免出现许可证错误
        List<string> DateData = new List<string>();
        List<string> uidData = new List<string>();
        using (var package = new ExcelPackage(ExcelData))
        {
            var worksheet = package.Workbook.Worksheets[0]; // 获取第一个工作表
            int rowCount = worksheet.Dimension.Rows; // 获取行数

            //遍历Excel文件的每一行
            for (int row = 1; row <= rowCount; row++)
            {
                string DateValue = worksheet.Cells[row, 8].Text;
                string uidValue = worksheet.Cells[row, 2].Text;
                DateData.Add(DateValue);
                uidData.Add(uidValue);
            }
        }
        // 创建一个新的Excel文件
        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add(excelFileName);

            // 插入数据
            for (int i = 0; i < DateData.Count; i++)
            {
                worksheet.Cells[i + 1, 1].Value = i + 1;
                worksheet.Cells[i + 1, 2].Value = DateData[i];
                worksheet.Cells[i + 1, 3].Value = uidData[i];
            }
            // 保存文件到桌面
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string fileName = $"{excelFileName}.xlsx";
            string filePath = System.IO.Path.Combine(desktopPath, fileName);
            package.SaveAs(new FileInfo(filePath));
            MessageBox.Show($"数据已处理并保存到文件: {filePath}");
        }
    }
    //郴州老人卡、优抚卡的处理逻辑
    private void ChenZhou()
    {
        //先处理Excel文件
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // 避免出现许可证错误
        List<string> processedData = new List<string>();
        using (var package = new ExcelPackage(ExcelData))
        {
            var worksheet = package.Workbook.Worksheets[0]; // 获取第一个工作表
            int rowCount = worksheet.Dimension.Rows; //获取行数
                                                     //遍历Excel文件的每一行
            for (int row = 2; row <= rowCount; row++)
            {
                string firstColumnValue = worksheet.Cells[row, 1].Text;
                string secondColumnValue = worksheet.Cells[row, 2].Text;
                string newRow =
                    $"{firstColumnValue}      {firstColumnValue}      {secondColumnValue}              00                         FFFFFFFFFFFFFFFFFFFF";
                processedData.Add(newRow);
            }
        }

        //处理MK文件
        //截取MK文件第二行的前42个字节
        MKDate[1] = MKDate[1].Substring(0, 42);
        //获取Excel总数据的条数
        int totalLines = processedData.Count;
        //将总数据条数转为6位数
        string totalLinesFormatted = totalLines.ToString("D6");
        //将MK文件的第二行的后6位替换为总数据条数
        MKDate[1] = MKDate[1].Substring(0, MKDate[1].Length - 6) + totalLinesFormatted;
        //将MK文件与Excel文件的数据合并

        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string fileName = $"RC{mkFileName}";
        string filePath = System.IO.Path.Combine(desktopPath, fileName);
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            writer.WriteLine(MKDate[0]);
            writer.WriteLine(MKDate[1]);

            for (int i = 0; i < processedData.Count; i++)
            {
                if (i == processedData.Count - 1)
                {
                    writer.Write(processedData[i]);
                }
                else
                {
                    writer.WriteLine(processedData[i]);
                }
            }
        }
        MessageBox.Show($"数据已合并并保存到文件: {filePath}");
    }
    //潍坊的处理逻辑
    private void WeiFang()
    {
        int rowCount;//excel文件的行数
                     //先处理Excel文件
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // 避免出现许可证错误
        List<string> processedData = new List<string>();
        using (var package = new ExcelPackage(ExcelData))
        {
            var worksheet = package.Workbook.Worksheets[0]; // 获取第一个工作表
            rowCount = worksheet.Dimension.Rows; //获取行数
                                                 //遍历Excel文件的每一行
            for (int row = 2; row <= rowCount; row++)
            {
                string firstColumnValue = worksheet.Cells[row, 2].Text;
                string secondColumnValue = worksheet.Cells[row, 8].Text;
                string newRow = $"{firstColumnValue}                {secondColumnValue}";
                processedData.Add(newRow);
            }
        }
        
        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string fileName = $"HG2610{excelFileName}01";
        string filePath = System.IO.Path.Combine(desktopPath, fileName);
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            writer.WriteLine(rowCount - 1);
            for (int i = 0; i < processedData.Count; i++)
            {
                writer.WriteLine(processedData[i]);
            }
        }
        MessageBox.Show($"数据已合并并保存到文件: {filePath},请修改文件名");
        //将文件后缀改为.RCC
        string newFilePath = filePath + ".RCC";
        File.Move(filePath, newFilePath);
    }
    //国网技术学院职工卡的处理逻辑
    private void GuoWang()
    {
        //先处理Excel文件
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // 避免出现许可证错误
        List<string> snData = new List<string>();
        List<string> uidData = new List<string>();
        using (var package = new ExcelPackage(ExcelData))
        {
            var worksheet = package.Workbook.Worksheets[0]; // 获取第一个工作表
            int rowCount = worksheet.Dimension.Rows; //获取行数
                                                     //遍历Excel文件的每一行
            for (int row = 1; row <= rowCount; row++)
            {
                string uidValue = worksheet.Cells[row, 2].Text;
                string snValue = worksheet.Cells[row, 7].Text;
                snData.Add(snValue);
                uidData.Add(uidValue);
            }
        }
        //创建一个新的Excel文件
        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add(excelFileName);
            // 插入标题行
            worksheet.Cells[1, 1].Value = "Index";
            worksheet.Cells[1, 2].Value = "SerialNumber";
            worksheet.Cells[1, 3].Value = "UID";

            // 插入数据
            for (int i = 0; i < snData.Count; i++)
            {
                worksheet.Cells[i + 2, 1].Value = i + 1;
                worksheet.Cells[i + 2, 2].Value = snData[i];
                worksheet.Cells[i + 2, 3].Value = uidData[i];
            }
            // 保存文件到桌面
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string fileName = $"{excelFileName}.xlsx";
            string filePath = System.IO.Path.Combine(desktopPath, fileName);
            package.SaveAs(new FileInfo(filePath));
            MessageBox.Show($"数据已处理并保存到文件: {filePath}");

        }
    }
    //哈尔滨城市通敬老优待卡的处理逻辑
    private void HaErBin()
    {
        int rowCount;//execl文件的行数
                     //先处理Excel文件
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // 避免出现许可证错误
        List<string> processedData = new List<string>();
        using (var package = new ExcelPackage(ExcelData))
        {
            var worksheet = package.Workbook.Worksheets[0]; // 获取第一个工作表
            rowCount = worksheet.Dimension.Rows; //获取行数
                                                 //遍历Excel文件的每一行
            for (int row = 2; row <= rowCount; row++)
            {
                string firstColumnValue = worksheet.Cells[row, 2].Text;
                string secondColumnValue = worksheet.Cells[row, 11].Text;
                string newRow = $"{firstColumnValue}|{secondColumnValue}";
                processedData.Add(newRow);
            }
        }
        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string fileName = $"HY1500{excelFileName}01.rcc";
        string filePath = System.IO.Path.Combine(desktopPath, fileName);
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            writer.WriteLine(rowCount - 1);
            for (int i = 0; i < processedData.Count; i++)
            {
                if (i == processedData.Count - 1)
                {
                    writer.Write(processedData[i]);
                }
                else
                {
                    writer.WriteLine(processedData[i]);
                }
            }
        }
        MessageBox.Show($"数据已合并并保存到文件: {filePath},请修改文件名");

    }
    //运城盐湖王府学校的处理逻辑
    private void YunCheng()
    {
        //先处理Excel文件
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // 避免出现许可证错误
        List<string> snData = new List<string>();
        List<string> uidData = new List<string>();
        using (var package = new ExcelPackage(ExcelData))
        {
            var worksheet = package.Workbook.Worksheets[0]; // 获取第一个工作表
            int rowCount = worksheet.Dimension.Rows; //获取行数
                                                     //遍历Excel文件的每一行
            for (int row = 1; row <= rowCount; row++)
            {
                string uidValue = worksheet.Cells[row, 2].Text;
                string snValue = worksheet.Cells[row, 8].Text;
                snData.Add(snValue);
                uidData.Add(uidValue);
            }
        }
        // 创建一个新的Excel文件
        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add(excelFileName);
            // 插入标题行
            worksheet.Cells[1, 1].Value = "Index";
            worksheet.Cells[1, 2].Value = "SerialNumber";
            worksheet.Cells[1, 3].Value = "UID";
            // 插入数据
            for (int i = 0; i < snData.Count; i++)
            {
                worksheet.Cells[i + 2, 1].Value = i + 1;
                worksheet.Cells[i + 2, 2].Value = snData[i];
                worksheet.Cells[i + 2, 3].Value = uidData[i];
            }
            // 保存文件到桌面
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string fileName = $"{excelFileName}.xlsx";
            string filePath = System.IO.Path.Combine(desktopPath, fileName);
            package.SaveAs(new FileInfo(filePath));
            MessageBox.Show($"数据已处理并保存到文件: {filePath},请根据制卡数据重命名RCC文件");
        }
    }
    //南通地铁的处理逻辑
    private void NanTong()
    {
        //先处理Excel文件
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // 避免出现许可证错误
        List<string> timeData = new List<string>();
        List<string> uidData = new List<string>();
        using (var package = new ExcelPackage(ExcelData))
        {
            var worksheet = package.Workbook.Worksheets[0]; // 获取第一个工作表
            int rowCount = worksheet.Dimension.Rows; //获取行数
                                                     //遍历Excel文件的每一行
            for (int row = 2; row <= rowCount; row++)
            {
                string uidValue = worksheet.Cells[row, 4].Text;
                string timeValue = worksheet.Cells[row, 12].Text;
                if (DateTime.TryParseExact(timeValue, "yyyy/MM/dd HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out DateTime dateTime))
                {
                    timeValue = "HG" + dateTime.ToString("yyyyMMdd") + uidValue;
                }
                else
                {
                    MessageBox.Show("日期格式不正确");
                }

                timeData.Add(timeValue);
                uidData.Add(uidValue);
            }
        }
        // 创建一个新的Excel文件
        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add(excelFileName);
            // 插入数据
            for (int i = 0; i < uidData.Count; i++)
            {
                worksheet.Cells[i + 1, 1].Value = uidData[i];
                worksheet.Cells[i + 1, 2].Value = timeData[i];
            }
            // 保存文件到桌面
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string fileName = $"{excelFileName}.xlsx";
            string filePath = System.IO.Path.Combine(desktopPath, fileName);
            package.SaveAs(new FileInfo(filePath));
            MessageBox.Show($"数据已处理并保存到桌面，请修改文件名");
        }
    }
    //长沙公交荣誉卡的处理逻辑
    private void ChangSha()
    {
        //先处理Excel文件
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // 避免出现许可证错误
        List<string> SerialNumData = new List<string>();
        List<string> uid_16Data = new List<string>();
        List<string> uid_16_Data = new List<string>();
        List<string> uid_10Data = new List<string>();
        List<string> uid_10_Data = new List<string>();
        using (var package = new ExcelPackage(ExcelData))
        {
            var worksheet = package.Workbook.Worksheets[0]; // 获取第一个工作表
            int rowCount = worksheet.Dimension.Rows; //获取行数
                                                     //遍历Excel文件的每一行
            for (int row = 2; row <= rowCount; row++)
            {
                string SerialNumValue = worksheet.Cells[row, 1].Text;
                string uid_16Value = worksheet.Cells[row, 3].Text;
                string uid_16_Value = worksheet.Cells[row, 4].Text;
                string uid_10Value = worksheet.Cells[row, 5].Text;
                string uid_10_Value = worksheet.Cells[row, 6].Text;
                SerialNumData.Add(SerialNumValue);
                uid_16Data.Add(uid_16Value);
                uid_16_Data.Add(uid_16_Value);
                uid_10Data.Add(uid_10Value);
                uid_10_Data.Add(uid_10_Value);
            }
        }
        // 创建一个新的Excel文件
        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add(excelFileName);
            // 插入数据

            worksheet.Cells[1, 1].Value = "SerialNum";
            worksheet.Cells[1, 2].Value = "UID_16";
            worksheet.Cells[1, 3].Value = "UID_16_";
            worksheet.Cells[1, 4].Value = "UID_10";
            worksheet.Cells[1, 5].Value = "UID_10_";
            
            for (int i = 0; i < SerialNumData.Count; i++)
            {

                worksheet.Cells[i + 2, 1].Value = SerialNumData[i];
                worksheet.Cells[i + 2, 2].Value = uid_16Data[i];
                worksheet.Cells[i + 2, 3].Value = uid_16_Data[i];
                worksheet.Cells[i + 2, 4].Value = uid_10Data[i];
                worksheet.Cells[i + 2, 5].Value = uid_10_Data[i];

            }
            // 保存文件到桌面
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string fileName = $"{excelFileName}.xlsx";
            string filePath = System.IO.Path.Combine(desktopPath, fileName);
            package.SaveAs(new FileInfo(filePath));
            MessageBox.Show($"数据已处理并保存到桌面，请修改文件名");
        }

    }
    //泸州公交的处理逻辑
    private void LuZhou()
    {
        LuZhou luzhou = new LuZhou();
        luzhou.ShowDialog();
        string cardtype = luzhou.CardType;
        if (cardtype == "") { MessageBox.Show("未选择卡类型"); return; }
        //先处理Excel文件
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // 避免出现许可证错误
        List<string> uid_10Data = new List<string>();
        List<string> cardData = new List<string>();
        using (var package = new ExcelPackage(ExcelData))
        {
            var worksheet = package.Workbook.Worksheets[0]; // 获取第一个工作表
            int rowCount = worksheet.Dimension.Rows; //获取行数
            //遍历Excel文件的每一行
            for (int row = 2; row <= rowCount; row++)
            {
                string uid_10Value = worksheet.Cells[row, 5].Text;
                string cardValue = worksheet.Cells[row, 1].Text;
                if (cardValue.Length == 19)
                {
                    cardValue = cardValue.Substring(11, 8);
                }
                cardValue = cardtype + cardValue;

                uid_10Data.Add(uid_10Value);
                cardData.Add(cardValue);
            }
        }
        // 创建一个新的Excel文件
        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add(excelFileName);
            // 插入数据
            //worksheet.Cells[1, 1].Value = "UID_10";
            // worksheet.Cells[1, 2].Value = "卡号(16位)";
            // worksheet.Cells[1, 3].Value = "卡商标志";
            for (int i = 0; i < uid_10Data.Count; i++)
            {
                worksheet.Cells[i + 1, 1].Value = uid_10Data[i];
                worksheet.Cells[i + 1, 2].Value = cardData[i];
                worksheet.Cells[i + 1, 3].Value = 0;
            }
            // 保存文件到桌面
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string fileName = $"{excelFileName}.xlsx";
            string filePath = System.IO.Path.Combine(desktopPath, fileName);
            package.SaveAs(new FileInfo(filePath));
            MessageBox.Show($"数据已处理并保存到桌面，请修改文件名");
        }
    }
    //合肥通的处理逻辑
    private void HeFei()
    {
        //先处理Excel文件
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // 避免出现许可证错误
        List<string> processedData = new List<string>();
        using (var package = new ExcelPackage(ExcelData))
        {
            var worksheet = package.Workbook.Worksheets[0]; // 获取第一个工作表
            int rowCount = worksheet.Dimension.Rows; //获取行数

            //遍历Excel文件的每一行
            for (int row = 2; row <= rowCount; row++)
            {
                string firstColumnValue = worksheet.Cells[row, 1].Text;
                string secondColumnValue = worksheet.Cells[row, 2].Text;
                string newRow =
                    $"{firstColumnValue}      {firstColumnValue}      {secondColumnValue}              00                         FFFFFFFFFFFFFFFFFFFF";
                processedData.Add(newRow);
            }
        }

        //处理MK文件
        //截取MK文件第二行的前42个字节
        MKDate[1] = MKDate[1].Substring(0, 42);
        //获取Excel总数据的条数
        int totalLines = processedData.Count;
        //将总数据条数转为6位数
        string totalLinesFormatted = totalLines.ToString("D6");
        //将MK文件的第二行的后6位替换为总数据条数
        MKDate[1] = MKDate[1].Substring(0, MKDate[1].Length - 6) + totalLinesFormatted;
        //将MK文件与Excel文件的数据合并

        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string fileName = $"RC{mkFileName}001";
        string filePath = System.IO.Path.Combine(desktopPath, fileName);
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            writer.WriteLine(MKDate[0]);
            writer.WriteLine(MKDate[1]);

            for (int i = 0; i < processedData.Count; i++)
            {
                if (i == processedData.Count - 1)
                {
                    writer.Write(processedData[i]);
                }
                else
                {
                    writer.WriteLine(processedData[i]);
                }
            }
        }

        MessageBox.Show($"数据已合并并保存到文件: {filePath}");
    }
    //西安交通大学的处理逻辑
    private void XiAnDaXue()
    {
        //取出Excle文件的数据
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // 避免出现许可证错误
        //取出第七列流水号数据
        List<string> Data = new List<string>();
        List<string> UidData = new List<string>();
        using (var package = new ExcelPackage(ExcelData))
        {
            var worksheet = package.Workbook.Worksheets[0]; // 获取第一个工作表
            int rowCount = worksheet.Dimension.Rows; //获取行数
            //遍历Excel文件的每一行
            for (int row = 1; row <= rowCount; row++)
            {
                string DataValue = worksheet.Cells[row, 7].Text;
                string UidValue = worksheet.Cells[row, 2].Text;
                UidValue = Convert.ToUInt32(UidValue, 16).ToString();
                Data.Add(DataValue);
                UidData.Add(UidValue);
            }
        }
        // 创建一个新的Excel文件
        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add(excelFileName);
            // 插入数据
            for (int i = 0; i < UidData.Count; i++)
            {
                worksheet.Cells[i + 1, 1].Value = Data[i];
                worksheet.Cells[i + 1, 2].Value = UidData[i];
            }
            // 保存文件到桌面
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string fileName = $"{excelFileName}.xlsx";
            string filePath = System.IO.Path.Combine(desktopPath, fileName);
            package.SaveAs(new FileInfo(filePath));
            MessageBox.Show($"数据已处理并保存到桌面{filePath}");
        }
    }
    //呼和浩特的处理逻辑
    private void HuHeHaoTe()
    {
        //提示
        tip.Text = "无注意事项";
        //取出Excle文件的数据
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // 避免出现许可证错误
        List<string> SNData = new List<string>();
        List<string> UidData = new List<string>();
        using (var package = new ExcelPackage(ExcelData))
        {
            var worksheet = package.Workbook.Worksheets[0]; // 获取第一个工作表
            int rowCount = worksheet.Dimension.Rows; //获取行数
            //遍历Excel文件的每一行
            for (int row = 2; row <= rowCount; row++)
            {
                string SNValue = worksheet.Cells[row, 2].Text;
                string UidValue = worksheet.Cells[row, 3].Text;
                SNData.Add(SNValue);
                UidData.Add(UidValue);

            }
        }
        // 创建一个新的Excel文件
        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add(excelFileName);
            // 插入数据
            worksheet.Cells[1, 1].Value = "发行卡号(16位)";
            worksheet.Cells[1, 2].Value = "物理卡号(8位)";
            worksheet.Cells[1, 3].Value = "物理卡号(8位) 高低字节调整";
            for (int i = 0; i < UidData.Count; i++)
            {
                worksheet.Cells[i + 2, 1].Value = SNData[i];
                worksheet.Cells[i + 2, 3].Value = UidData[i];
                worksheet.Cells[i + 2, 2].Value = SwapHexPairs(UidData[i]);
            }
            // 保存文件到桌面
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string fileName = $"{excelFileName}.xlsx";
            string filePath = System.IO.Path.Combine(desktopPath, fileName);
            package.SaveAs(new FileInfo(filePath));
            MessageBox.Show($"数据已处理并保存到桌面{filePath}");
        }
    }
    //重庆地区的331-A1模块的处理逻辑
    private void ChongQingA1()
    {
        //提示
        tip.Text = "无注意事项";
        //取出Excle文件的数据
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // 避免出现许可证错误
        List<string> SNData = new List<string>();
        List<string> ATSData = new List<string>();
        using (var package = new ExcelPackage(ExcelData))
        {
            var worksheet = package.Workbook.Worksheets[0]; // 获取第一个工作表
            int rowCount = worksheet.Dimension.Rows; //获取行数
            //遍历Excel文件的每一行
            for (int row = 2; row <= rowCount; row++)
            {
                string SNValue = worksheet.Cells[row, 11].Text;
                string ATSValue = worksheet.Cells[row, 2].Text;
                SNData.Add(SNValue);
                ATSData.Add(ATSValue);

            }
        }

        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string fileName = $"HG-{excelFileName}";
        string filePath = Path.Combine(desktopPath, fileName);
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            writer.WriteLine(ATSData.Count);
            for (int i = 0; i < ATSData.Count; i++)
            {
                if (i == ATSData.Count - 1)
                {
                    writer.Write(SNData[i] + ";" + SNData[i] + ";" + ATSData[i] + ";");
                }
                else
                {
                    writer.WriteLine(SNData[i] + ";" + SNData[i] + ";" + ATSData[i] + ";");
                }
            }
        }
        MessageBox.Show($"数据已合并并保存到文件: {filePath}");
    }
    //西藏林芝地区的处理逻辑
    private void XIZang()
    {

        //取出Excle文件的数据
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // 避免出现许可证错误
        List<string> SNData = new List<string>();
        using (var package = new ExcelPackage(ExcelData))
        {
            var worksheet = package.Workbook.Worksheets[0]; // 获取第一个工作表
            int rowCount = worksheet.Dimension.Rows; //获取行数
            //遍历Excel文件的每一行
            for (int row = 2; row <= rowCount; row++)
            {
                string SNValue = worksheet.Cells[row, 1].Text;
                SNData.Add(SNValue);
            }
        }
        string date = "20241115";
        string cardtype = "01";
        string startdate = "20241107";
        string fnishdate = "20401231";

        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string fileName = $"HP-04377740{date}165931.TXT";
        string filePath = System.IO.Path.Combine(desktopPath, fileName);
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            writer.WriteLine(SNData.Count + date);
            for (int i = 0; i < SNData.Count; i++)
            {
                if (i == SNData.Count - 1)
                {
                    writer.Write($"{SNData[i]}|04377740FFFFFFFF|{cardtype}|{startdate}|{fnishdate}|2020202020202020202020202020202020202020|2020202020202020202020202020202020202020202020202020202020202020|00|00|0000|0000000000|");
                }
                else
                {
                    writer.WriteLine($"{SNData[i]}|04377740FFFFFFFF|{cardtype}|{startdate}|{fnishdate}|2020202020202020202020202020202020202020|2020202020202020202020202020202020202020202020202020202020202020|00|00|0000|0000000000|");

                }
            }
        }
        MessageBox.Show($"数据已合并并保存到文件: {filePath}");
    }
    //西藏拉萨地区的处理逻辑
    private void XIZangLaSa()
    {

        //取出Excle文件的数据
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // 避免出现许可证错误
        List<string> SNData = new List<string>();
        using (var package = new ExcelPackage(ExcelData))
        {
            var worksheet = package.Workbook.Worksheets[0]; // 获取第一个工作表
            int rowCount = worksheet.Dimension.Rows; //获取行数
            //遍历Excel文件的每一行
            for (int row = 2; row <= rowCount; row++)
            {
                string SNValue = worksheet.Cells[row, 1].Text;
                SNData.Add(SNValue);
            }
        }
        string date = "20241115";
        string cardtype = "01";
        string startdate = "20241107";
        string finishdate = "20401231";
        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string fileName = $"HP_04357710FFFFFFFF{date}165931.txt";
        string filePath = Path.Combine(desktopPath, fileName);
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            writer.WriteLine($"{SNData.Count}|{date}|");
            for (int i = 0; i < SNData.Count; i++)
            {
                if (i == SNData.Count - 1)
                {
                    writer.Write($"{SNData[i]}|04357710FFFFFFFF|{cardtype}|{startdate}|{finishdate}||||01|0000||");
                }
                else
                {
                    writer.WriteLine($"{SNData[i]}|04357710FFFFFFFF|{cardtype}|{startdate}|{finishdate}||||01|0000||");
                }
            }
        }
        MessageBox.Show($"数据已合并并保存到文件: {filePath}");
    }
    //淄博公交地区的处理逻辑
    private void ZiBo()
    {
        string date;//日期,格式20241115112548
        string date1;//日期,格式2024-11-15
        string cardtype;//卡类型
        //打开二级窗口
        ZiBoPage zibo = new ZiBoPage();
        zibo.ShowDialog();
        //获取二级窗口的数据
        cardtype = zibo.CardType;
        date = zibo.Date14;
        date1 = zibo.Date10;
        //取出Excle文件的数据
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // 避免出现许可证错误
        List<string> xmlData = new List<string>();
        if (ExcelData == null && ExcelData.Length == 0) { MessageBox.Show("Excel数据为空"); return; }
        using (var package = new ExcelPackage(ExcelData))
        {
            var worksheet = package.Workbook.Worksheets[0]; // 获取第一个工作表
            int rowCount = worksheet.Dimension.Rows; //获取行数
            //遍历Excel文件的每一行
            for (int row = 2; row <= rowCount; row++)
            {
                string SNValue = worksheet.Cells[row, 3].Text;
                xmlData.Add(SNValue);
            }
        }
        //保存文件到桌面
        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string fileName = $"ACPU{date}_Report.xml";
        string filePath = System.IO.Path.Combine(desktopPath, fileName);
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            writer.WriteLine("<?xml version=\"1.0\" encoding=\"GB2312\"?>");
            writer.WriteLine($"<TaskBack task=\"USER CARD\" TaskId=\"{date}\">");
            writer.WriteLine("<Task>");
            writer.WriteLine("<Type>ACPU</Type>");
            writer.WriteLine("<AppType>01</AppType>");
            writer.WriteLine($"<CardType>{cardtype}</CardType>");
            writer.WriteLine($"<Amount>{xmlData.Count}</Amount>");
            writer.WriteLine($"<GoodAmount>{xmlData.Count}</GoodAmount>");
            writer.WriteLine("<BadAmount>0</BadAmount>");
            writer.WriteLine("<InitOperator>000000</InitOperator>");
            writer.WriteLine($"<IssueDate>{date1}</IssueDate>");
            writer.WriteLine("<ValidDate>2040-12-31</ValidDate>");
            writer.WriteLine($"<RepeortDate>{date1}</RepeortDate>");
            writer.WriteLine("</Task>");
            writer.WriteLine("<CardList>");
            // 提取数据
            for (int i = 0; i < xmlData.Count; i++)
            {
                string cardUid = ExtractValue(xmlData[i], "CARDUID", "APPID");
                string appId = ExtractValue(xmlData[i], "APPID", "ISSUESN");
                string issueSn = ExtractValue(xmlData[i], "ISSUESN", "ISSUETIME");
                string issueTime = ExtractValue(xmlData[i], "ISSUETIME", "STATUS");
                issueTime = date.Substring(0, 8) + issueTime.Substring(8);
                // 创建 XML 格式字符串
                string status = "Good"; // 假设默认状态为 "Good"
                xmlData[i] = $"<Card UID=\"{cardUid}\" AppID=\"{appId}\" IssueSN=\"{issueSn}\" IssueTime=\"{issueTime}\" Status=\"{status}\"/>";
                writer.WriteLine(xmlData[i]);
            }
            writer.WriteLine("</CardList>");
            writer.Write("</TaskBack>");
        }
        MessageBox.Show($"数据已合并并保存到文件: {filePath}");
    }
    //查找逻辑
    static string ExtractValue(string input, string startKey, string endKey)
    {
        // 匹配以startKey开始到endKey之前的内容
        string pattern = $@"{startKey}(.*?){endKey}";
        Match match = Regex.Match(input, pattern);
        return match.Success ? match.Groups[1].Value : string.Empty;
    }
    //平凉地区的处理逻辑
    private void Pingliang()
    {
        //取出Excle文件的数据
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // 避免出现许可证错误
        List<string> UIDData = new List<string>();
        List<string> SNData = new List<string>();
        using (var package = new ExcelPackage(ExcelData))
        {
            var worksheet = package.Workbook.Worksheets[0]; // 获取第一个工作表
            int rowCount = worksheet.Dimension.Rows; //获取行数
            //遍历Excel文件的每一行
            for (int row = 1; row <= rowCount; row++)
            {
                string UIDValue = worksheet.Cells[row, 2].Text;
                UIDValue = Convert.ToUInt32(SwapHexPairs(UIDValue), 16).ToString();
                UIDData.Add(UIDValue);
                string SNValue = worksheet.Cells[row, 8].Text;
                SNData.Add(SNValue);
            }
        }
        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string fileName = $"{excelFileName}.txt";
        string filePath = System.IO.Path.Combine(desktopPath, fileName);
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            for (int i = 0; i < SNData.Count; i++)
            {
                writer.WriteLine($"{UIDData[i]}\t74400000{SNData[i]}\t1");
            }
        }
        MessageBox.Show($"数据已合并并保存到文件: {filePath}");

    }
    //桂林公交的处理逻辑
    private void GuiLin()
    {
        //取出Excle文件的数据
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // 避免出现许可证错误
        List<string> UIDData = new List<string>();
        List<string> SNData = new List<string>();
        using (var package = new ExcelPackage(ExcelData))
        {
            var worksheet = package.Workbook.Worksheets[0]; // 获取第一个工作表
            int rowCount = worksheet.Dimension.Rows; //获取行数
            //遍历Excel文件的每一行
            for (int row = 2; row <= rowCount; row++)
            {
                string UIDValue = worksheet.Cells[row, 3].Text;
                UIDValue = "00908670" + UIDValue;
                UIDData.Add(UIDValue);
                string SNValue = worksheet.Cells[row, 2].Text;
                SNData.Add(SNValue);
            }
        }
        GuiLin guiLin= new();
        guiLin.ShowDialog();
        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        int linecount = SNData.Count;
        string total = linecount.ToString("D8");
        string fileName = $"GXJT_0{guiLin.SN.Text}_{guiLin.Count.Text}_00_V100-{SNData.Count}.rdi";
        string filePath = Path.Combine(desktopPath, fileName);
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            for (int i = 0; i < SNData.Count; i++)
            {
                if (i == SNData.Count - 1)
                {
                    writer.Write($"{UIDData[i]} {SNData[i]}"); 
                }
                else
                {
                    writer.WriteLine($"{UIDData[i]} {SNData[i]}"); 
                }
            }
        }
        MessageBox.Show($"数据保存到桌面: {filePath}"); 
    }
    //陕西师范大学
    private void ShanXi()
    {
        //取出Excle文件的数据
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // 避免出现许可证错误
        List<string> SNData = new List<string>();
        List<string> UidData = new List<string>();
        using (var package = new ExcelPackage(ExcelData))
        {
            var worksheet = package.Workbook.Worksheets[0]; // 获取第一个工作表
            int rowCount = worksheet.Dimension.Rows; //获取行数
            //遍历Excel文件的每一行
            for (int row = 2; row <= rowCount; row++)
            {
                string SNValue = worksheet.Cells[row, 1].Text;
                string UidValue = worksheet.Cells[row, 5].Text;
                SNData.Add(SNValue);
                UidData.Add(UidValue);

            }
        }
        // 创建一个新的Excel文件
        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add(excelFileName);
            for (int i = 0; i < UidData.Count; i++)
            {
                worksheet.Cells[i + 1, 1].Value = SNData[i];
                worksheet.Cells[i + 1, 2].Value = UidData[i];
               
            }
            // 保存文件到桌面
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string fileName = $"{excelFileName}.xlsx";
            string filePath = System.IO.Path.Combine(desktopPath, fileName);
            package.SaveAs(new FileInfo(filePath));
            MessageBox.Show($"数据已处理并保存到桌面{filePath}");
        } 
    }
    //西安文理学院
    private void XiAnXueYuan()
    {
        //取出Excle文件的数据
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // 避免出现许可证错误
        List<string> SNData = new List<string>();
        List<string> UidData = new List<string>();
        using (var package = new ExcelPackage(ExcelData))
        {
            var worksheet = package.Workbook.Worksheets[0]; // 获取第一个工作表
            int rowCount = worksheet.Dimension.Rows; //获取行数
            //遍历Excel文件的每一行
            for (int row = 1; row <= rowCount; row++)
            {
                string SNValue = worksheet.Cells[row, 8].Text;
                string UidValue = worksheet.Cells[row, 2].Text;
                SNData.Add(SNValue);
                UidData.Add(UidValue);

            }
        }
        // 创建一个新的Excel文件
        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add(excelFileName);
            worksheet.Cells[1, 1].Value = "Index";
            worksheet.Cells[1, 2].Value = "SerialNumber";
            worksheet.Cells[1, 3].Value = "UID";
            for (int i = 0; i < UidData.Count; i++)
            {
                worksheet.Cells[i + 2, 1].Value = i + 1;
                worksheet.Cells[i + 2, 2].Value = SNData[i];
                worksheet.Cells[i + 2, 3].Value = UidData[i];
               
            }
            // 保存文件到桌面
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string fileName = $"{excelFileName}.xlsx";
            string filePath = System.IO.Path.Combine(desktopPath, fileName);
            package.SaveAs(new FileInfo(filePath));
            MessageBox.Show($"数据已处理并保存到桌面{filePath}");
        } 
    }
    //滨州公交的处理逻辑
    private void BinZhou()
    {
        //取出Excle文件的数据
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // 避免出现许可证错误
        List<string> SNData = new List<string>();
        List<string> UidData = new List<string>();
        using (var package = new ExcelPackage(ExcelData))
        {
            var worksheet = package.Workbook.Worksheets[0]; // 获取第一个工作表
            int rowCount = worksheet.Dimension.Rows; //获取行数
            //遍历Excel文件的每一行
            for (int row = 1; row <= rowCount; row++)
            {
                string SNValue = worksheet.Cells[row, 1].Text;
                string UidValue = worksheet.Cells[row, 6].Text;
                SNData.Add(SNValue);
                UidData.Add(UidValue);

            }
        }
        // 创建一个新的Excel文件
        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add(excelFileName);
            for (int i = 0; i < UidData.Count; i++)
            {
                worksheet.Cells[i + 1, 1].Value = SNData[i];
                worksheet.Cells[i + 1, 2].Value = UidData[i];
               
            }
            // 保存文件到桌面
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string fileName = $"{excelFileName}.xlsx";
            string filePath = System.IO.Path.Combine(desktopPath, fileName);
            package.SaveAs(new FileInfo(filePath));
            MessageBox.Show($"数据已处理并保存到桌面{filePath}");
        } 
    }
    //云南朗坤
    private void WangKun()
    {
        //取出Excle文件的数据
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // 避免出现许可证错误
        List<string> SNData = new List<string>();
        List<string> UidData = new List<string>();
        List<string> Uid_Data = new List<string>();
        using (var package = new ExcelPackage(ExcelData))
        {
            var worksheet = package.Workbook.Worksheets[0]; // 获取第一个工作表
            int rowCount = worksheet.Dimension.Rows; //获取行数
            //遍历Excel文件的每一行
            for (int row = 1; row <= rowCount; row++)
            {
                string SNValue = worksheet.Cells[row, 7].Text;
                string UidValue = worksheet.Cells[row, 2].Text;
                string Uid_Value = worksheet.Cells[row, 3].Text;
                SNData.Add(SNValue);
                UidData.Add(UidValue);
                Uid_Data.Add(Uid_Value);
            }
        }
        // 创建一个新的Excel文件
        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add(excelFileName);
            for (int i = 0; i < UidData.Count; i++)
            {
                worksheet.Cells[i + 1, 1].Value = SNData[i];
                worksheet.Cells[i + 1, 2].Value = UidData[i];
                worksheet.Cells[i + 1, 3].Value = Uid_Data[i];
                worksheet.Cells[i + 1, 4].Value = Convert.ToUInt32(UidData[i], 16).ToString();
                worksheet.Cells[i + 1, 5].Value = Convert.ToUInt32(Uid_Data[i], 16).ToString();
               
            }
            // 保存文件到桌面
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string fileName = $"{excelFileName}.xlsx";
            string filePath = System.IO.Path.Combine(desktopPath, fileName);
            package.SaveAs(new FileInfo(filePath));
            MessageBox.Show($"数据已处理并保存到桌面{filePath}");
        }  
    }
    private void Test(object sender, RoutedEventArgs e)
    {
        MessageBox.Show("未开发");
    }
    //调整16进制与不调整16进制互相转换
    private string SwapHexPairs(string hex)
    {
        if (hex.Length % 2 != 0)
        {
            throw new ArgumentException("数据长度不合法");
        }
        char[] reversedHex = new char[hex.Length];
        int j = 0;
        for (int i = hex.Length - 2; i >= 0; i -= 2)
        {
            reversedHex[j++] = hex[i];
            reversedHex[j++] = hex[i + 1];
        }
        return new string(reversedHex);
    }
}
