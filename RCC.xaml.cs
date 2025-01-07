using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Win32;
using OfficeOpenXml;
using WindowUI.Pages;

namespace WindowUI;
public partial class RCC
{
    private string mkFileName { get; set; } //  记录MK文件名
    private List<string> MKDate{ get; set; } // 临时存储读取的MK文件的数据
    private string excelFileName{ get; set; }// 记录Excel文件名
    private MemoryStream ExcelData{ get; set; }// 临时存储读取的Excel的数据
    private string Region{ get; set; }// 下拉框选则的地区
    private OpenFileDialog openFileDialog{ get; set; } // MK文件处理流
    private OpenFileDialog openFileDialog2{ get; set; }// Excel文件处理流
    // 定义需要MK文件的地区
    private readonly string[] disableButtonRegions = ["天津","郴州","合肥","兰州","柳州公交"]; 
    public RCC()
    {
        InitializeComponent();
    }
    //打开MK文件
    private void OpenMKFile(object sender, RoutedEventArgs e)
    {

        //打开一个文件选择器，类型为任意
        openFileDialog = new OpenFileDialog
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
        openFileDialog2 = new OpenFileDialog
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
               
                datatextbox.Text = excelFileName;
            }
            catch(IOException)
            {
                MessageBox.Show("文件已被占用，请先关闭Excel表格。",
                    "文件占用", MessageBoxButton.OK, MessageBoxImage.Warning);
            }  
            data.Foreground = Brushes.LightGreen;
            datatextbox.Foreground = Brushes.Green;
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
                case "兰州":tip.Text = "只有兰州工作证不需要MK文件"; break;
                case "随州": tip.Text = "Excel文件有时列数会不对应";break;
                default:tip.Text = "该地区暂无提示"; break;
            }

        }

    }
    //点击处理文件按钮
    private async void ProcessTheFile(object sender, RoutedEventArgs e)
    {
        if (ExcelData is null)
        {
            MessageBox.Show("请选择文件");
            return;
        }
            //根据不同地区处理文件
            switch (Region)
            {
                case "天津": await 天津(); break;
                case "兰州": 兰州(); break;
                case "青岛博研加气站": 青岛博研加气站(); break;
                case "抚顺": 抚顺(); break;
                case "郴州": 郴州(); break;
                case "潍坊": 潍坊(); break;
                case "国网技术学院职工卡": 国网技术学院(); break;
                case "哈尔滨城市通": 哈尔滨(); break;
                case "运城盐湖王府学校": 运城盐湖王府学校(); break;
                case "南通地铁": 南通(); break;
                case "长沙公交荣誉卡": 长沙(); break;
                case "泸州公交": 泸州(); break;
                case "合肥通": 合肥通(); break;
                case "青岛理工大学": 青岛理工大学(); break;
                case "西安交通大学": 西安交通大学(); break;
                case "呼和浩特": 呼和浩特(); break;
                case "重庆33A-A1": 重庆(); break;
                case "西藏林芝": 西藏林芝(); break;
                case "西藏拉萨": 西藏拉萨(); break;
                case "淄博公交": 淄博公交(); break;
                case "淄博血站不开通": 淄博血站不开通();break;
                case "平凉公交": 平凉公交(); break;
                case "桂林公交": 桂林公交(); break;
                case "陕西师范大学": 陕西师范大学(); break;
                case "西安文理学院": 西安文理学院(); break;
                case "滨州公交": 滨州公交(); break;
                case "云南朗坤": await 云南朗坤(); break;
                case "盱眙": await 盱眙(); break;
                case "柳州公交" : await 柳州公交(); break; 
                case "漯河" : await 漯河(); break; 
                case "随州" : await 随州(); break; 
                case "昆明" : await 昆明(); break; 
                case "徐州地铁": await 徐州地铁(); break;
                case "江苏乾翔": await 江苏乾翔(); break;
                case "石家庄": await 石家庄(); break;
                case "淮北": await 淮北(); break;
                default: MessageBox.Show("请选择地区"); break;
            }
    }
    //天津的处理逻辑
    private async Task 天津()
    {
        //先处理Excel文件
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // 避免出现许可证错误
        List<string> processedData = new List<string>();
        using (var package = new ExcelPackage(ExcelData))
        {
            var worksheet = package.Workbook.Worksheets[0]; // 获取第一个工作表
            int rowCount = worksheet.Dimension.Rows; //获取行数
            for (int row = 2; row <= rowCount; row++)
            {
                string firstColumnValue = worksheet.Cells[row, 1].Text;
                string secondColumnValue = worksheet.Cells[row, 2].Text;
                string newRow =
                    $"{firstColumnValue}      {firstColumnValue}      {secondColumnValue}              FFFFFFFFFFFFFFFFFFFF";
                processedData.Add(newRow);
            }
        }
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
        await Task.Run(() =>
        {
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
        });
        MessageBox.Show($"数据已合并并保存到桌面: {filePath}");
    }
    //兰州的处理逻辑
    private void 兰州()
    {
        兰州 lanzhou = new ();
        lanzhou.ShowDialog();
        string cardtype = lanzhou.CardType;
        
        if (cardtype == "1")
        {
            兰州工作证();
        }else if (cardtype == "2")
        {
            兰州公交();
        }
        
    }
    private void 兰州工作证()
    {
        List<string> SNData = new List<string>();
        List<string> UIDData = new List<string>();
        //取出Excle文件的数据
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // 避免出现许可证错误
        using (var package = new ExcelPackage(ExcelData))
        {
            var worksheet = package.Workbook.Worksheets[0]; // 获取第一个工作表
            int rowCount = worksheet.Dimension.Rows; //获取行数
            //遍历Excel文件的每一行
            for (int row = 1; row <= rowCount; row++)
            {
                string firstColumnValue = worksheet.Cells[row, 8].Text;
                SNData.Add(firstColumnValue);
                firstColumnValue = worksheet.Cells[row, 3].Text;
                string firstColumnValue2 = Convert.ToUInt32(firstColumnValue, 16).ToString();
                UIDData.Add(firstColumnValue2);
            }
        }
        //将processedData和processedData2合并起来，中间用','分隔，最后保存为txt文件到桌面
        List<string> mergedData = new List<string>();
        for (int i = 0; i < SNData.Count; i++)
        {
            string mergedRow = $"{SNData[i]},{UIDData[i]}";
            mergedData.Add(mergedRow);
        }
        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string fileName = excelFileName + ".txt";
        string filePath = Path.Combine(desktopPath, fileName);
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            foreach (var line in mergedData)
            {
                writer.WriteLine(line);
            }
        }
        MessageBox.Show($"数据已合并并保存到文件: {filePath}");
    }
    private void 兰州公交()
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
                    $"{firstColumnValue}      {firstColumnValue}      {secondColumnValue}          00                         FFFFFFFFFFFFFFFFFFFF";
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
        //第二个文件
        List<string> SNData = new List<string>();
        List<string> UIDData = new List<string>();
        //取出Excle文件的数据
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // 避免出现许可证错误
        using (var package = new ExcelPackage(ExcelData))
        {
            var worksheet = package.Workbook.Worksheets[0]; // 获取第一个工作表
            int rowCount = worksheet.Dimension.Rows; //获取行数
            //遍历Excel文件的每一行
            for (int row = 1; row <= rowCount; row++)
            {
                string firstColumnValue = worksheet.Cells[row, 8].Text;
                SNData.Add(firstColumnValue);
                firstColumnValue = worksheet.Cells[row, 3].Text;
                string firstColumnValue2 = Convert.ToUInt32(firstColumnValue, 16).ToString();
                UIDData.Add(firstColumnValue2);
            }
        }
        //将processedData和processedData2合并起来，中间用','分隔，最后保存为txt文件到桌面
        List<string> mergedData = new List<string>();
        for (int i = 0; i < SNData.Count; i++)
        {
            string mergedRow = $"{SNData[i]},{UIDData[i]}";
            mergedData.Add(mergedRow);
        }
        desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        fileName = excelFileName + ".txt";
        filePath = Path.Combine(desktopPath, fileName);
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
    private void 抚顺()
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
        string filePath = Path.Combine(desktopPath, fileName);
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
    private void 青岛博研加气站()
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
            string filePath = Path.Combine(desktopPath, fileName);
            package.SaveAs(new FileInfo(filePath));
            MessageBox.Show($"数据已处理并保存到文件: {filePath}");
        }
    }
    //青岛理工大学的处理逻辑
    private void 青岛理工大学()
    {
        青岛理工大学 qingdao = new ();
        qingdao.ShowDialog();
        string campus = qingdao.SelectedCampus;
        if(campus == "青岛校区")
        {
            青岛理工大学青岛校区();
        }
        else if(campus == "临沂校区")
        {
            青岛理工大学临沂校区();
        } 
    }
    //青岛理工大学临沂校区的处理逻辑
    private void 青岛理工大学临沂校区()
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
            string filePath = Path.Combine(desktopPath, fileName);
            package.SaveAs(new FileInfo(filePath));
            MessageBox.Show($"数据已处理并保存到文件: {filePath}");
        }
    }
    //青岛理工大学青岛校区的处理逻辑
    private void 青岛理工大学青岛校区()
    {
        //取出Excel文件的数据
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // 避免出现许可证错误
        List<string> SNData = new List<string>();
        List<string> uid6Data = new List<string>();
        using (var package = new ExcelPackage(ExcelData))
        {
            var worksheet = package.Workbook.Worksheets[0]; // 获取第一个工作表
            int rowCount = worksheet.Dimension.Rows; // 获取行数

            //遍历Excel文件的每一行
            for (int row = 1; row <= rowCount; row++)
            {
                string SNValue = worksheet.Cells[row, 1].Text;
                string uid16Value = worksheet.Cells[row, 4].Text;
                SNData.Add(SNValue);
                uid6Data.Add(uid16Value);
            }
        }
        // 创建一个新的Excel文件
        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add(excelFileName);

            // 插入数据
            for (int i = 0; i < SNData.Count; i++)
            {
                worksheet.Cells[i + 1, 1].Value = SNData[i];
                worksheet.Cells[i + 1, 2].Value = uid6Data[i];
            }
            // 保存文件到桌面
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string fileName = $"{excelFileName}.xlsx";
            string filePath = Path.Combine(desktopPath, fileName);
            package.SaveAs(new FileInfo(filePath));
            MessageBox.Show($"数据已处理并保存到文件: {filePath}");
        }
    }
    //郴州的处理逻辑
    private void 郴州()
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
    //潍坊的处理逻辑
    private void 潍坊()
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
        string filePath = Path.Combine(desktopPath, fileName);
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
    private void 国网技术学院()
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
            string filePath = Path.Combine(desktopPath, fileName);
            package.SaveAs(new FileInfo(filePath));
            MessageBox.Show($"数据已处理并保存到文件: {filePath}");

        }
    }
    //哈尔滨城市通敬老优待卡的处理逻辑
    private void 哈尔滨()
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
        string filePath = Path.Combine(desktopPath, fileName);
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
    private void 运城盐湖王府学校()
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
            string filePath = Path.Combine(desktopPath, fileName);
            package.SaveAs(new FileInfo(filePath));
            MessageBox.Show($"数据已处理并保存到文件: {filePath},请根据制卡数据重命名RCC文件");
        }
    }
    //南通地铁的处理逻辑
    private void 南通()
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
                DateTime parsedDate = DateTime.ParseExact(timeValue, "yyyy/MM/dd H:mm:ss", null);
                timeValue = "HG" + parsedDate.ToString("yyyyMMdd") + uidValue;
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
            string filePath = Path.Combine(desktopPath, fileName);
            package.SaveAs(new FileInfo(filePath));
            MessageBox.Show("数据已处理并保存到桌面，请修改文件名");
        }
    }
    //长沙公交荣誉卡的处理逻辑
    private void 长沙()
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
            string filePath = Path.Combine(desktopPath, fileName);
            package.SaveAs(new FileInfo(filePath));
            MessageBox.Show("数据已处理并保存到桌面，请修改文件名");
        }

    }
    //泸州公交的处理逻辑
    private void 泸州()
    {
        泸州 luzhou = new 泸州();
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
            worksheet.Cells[1, 1].Value = "UID_10";
            worksheet.Cells[1, 2].Value = "卡号(16位)";
            worksheet.Cells[1, 3].Value = "卡商标志";
            for (int i = 0; i < uid_10Data.Count; i++)
            {
                worksheet.Cells[i + 2, 1].Value = uid_10Data[i];
                worksheet.Cells[i + 2, 2].Value = cardData[i];
                worksheet.Cells[i + 2, 3].Value = 8670;
            }
            // 保存文件到桌面
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string fileName = $"{excelFileName}.xlsx";
            string filePath = Path.Combine(desktopPath, fileName);
            package.SaveAs(new FileInfo(filePath));
            MessageBox.Show("数据已处理并保存到桌面，请修改文件名");
        }
    }
    //合肥通的处理逻辑
    private void 合肥通()
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
    //西安交通大学的处理逻辑
    private void 西安交通大学()
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
            for (int row = 2; row <= rowCount; row++)
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
            string filePath = Path.Combine(desktopPath, fileName);
            package.SaveAs(new FileInfo(filePath));
            MessageBox.Show($"数据已处理并保存到桌面{filePath}");
        }
    }
    //呼和浩特的处理逻辑
    private void 呼和浩特()
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
                worksheet.Cells[i + 2, 2].Value = ChangeHexPairs(UidData[i]);
            }
            // 保存文件到桌面
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string fileName = $"{excelFileName}.xlsx";
            string filePath = Path.Combine(desktopPath, fileName);
            package.SaveAs(new FileInfo(filePath));
            MessageBox.Show($"数据已处理并保存到桌面{filePath}");
        }
    }
    //重庆地区的331-A1模块的处理逻辑
    private void 重庆()
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
    private void 西藏林芝()
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
        string filePath = Path.Combine(desktopPath, fileName);
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
    private void 西藏拉萨()
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
    //淄博血站不开通的处理逻辑
    private async void 淄博血站不开通()
    {
        string cardtype = "0801";
        // 取出Excel文件的数据
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // 避免出现许可证错误
        List<string> SNData = new List<string>();
        List<string> UIDData = new List<string>();
        string StartSN;
        string EndSN;
        using (var package = new ExcelPackage(ExcelData))
        {
            var worksheet = package.Workbook.Worksheets[0]; // 获取第一个工作表
            int rowCount = worksheet.Dimension.Rows; // 获取行数
            StartSN = worksheet.Cells[1, 7].Text;
            EndSN = worksheet.Cells[rowCount, 7].Text;
            for (int row = 1; row <= rowCount; row++)
            {
                string SNValue = worksheet.Cells[row, 7].Text;
                string UIDValue = worksheet.Cells[row, 2].Text;
                //计算UID校验码
                string stra=UIDValue.Substring(0, 2);
                string strb=UIDValue.Substring(2, 2);
                string strc=UIDValue.Substring(4, 2);
                string strd=UIDValue.Substring(6, 2);
                int a=Convert.ToInt32(stra,16);
                int b=Convert.ToInt32(strb,16);
                int c=Convert.ToInt32(strc,16);
                int d=Convert.ToInt32(strd,16);
                int s=a^b^c^d;
                UIDValue += s.ToString("X").PadLeft(2, '0');
                UIDValue = UIDValue.ToUpper();
                //计算SN校验码
                string stre = SNValue.Substring(0, 2);
                string strf = SNValue.Substring(2, 2);
                string strg = SNValue.Substring(4, 2);
                string strh = SNValue.Substring(6, 2);
                string stri = SNValue.Substring(8, 2);
                string strj = SNValue.Substring(10, 2);
                string strk = SNValue.Substring(12, 2);
                string strl = SNValue.Substring(14, 2);
                Int32 intnew = (Convert.ToInt32(stre, 16) ^ Convert.ToInt32(strf, 16) ^ Convert.ToInt32(strg, 16) ^ Convert.ToInt32(strh, 16) ^ Convert.ToInt32(stri, 16) ^ Convert.ToInt32(strj, 16) ^ Convert.ToInt32(strk, 16) ^ Convert.ToInt32(strl, 16));
                string strXOR_2 = intnew.ToString("X").PadLeft(2, '0');
                SNValue += strXOR_2;
                SNData.Add(SNValue);
                UIDData.Add(UIDValue);
            }
        }
        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string fileName = $"{StartSN}-{EndSN}.xml";
        string filePath = Path.Combine(desktopPath, fileName);
        await Task.Run(() =>
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine("<?xml version=\"1.0\" encoding=\"GB2312\"?>");
                writer.WriteLine($"<CardList Total=\"{SNData.Count}\" CardType=\"{cardtype}\" Start=\"{StartSN}\" End=\"{EndSN}\">");
                for (int i = 0; i < SNData.Count; i++)
                {
                    writer.WriteLine($"<Card UID=\"{UIDData[i]}\" AppID=\"{SNData[i]}\"/>");
                }
                writer.Write("</CardList>");
            }
        });
        MessageBox.Show($"文件已保存到桌面{filePath}"); 
    }
    //淄博公交地区的处理逻辑
    private void 淄博公交()
    {
        string date;//日期,格式20241115112548
        string date1;//日期,格式2024-11-15
        string cardtype;//卡类型
        //打开二级窗口
        淄博 zibo = new 淄博();
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
        string filePath = Path.Combine(desktopPath, fileName);
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
    //淄博公交查找替换逻辑
    static string ExtractValue(string input, string startKey, string endKey)
    {
        // 匹配以startKey开始到endKey之前的内容
        string pattern = $@"{startKey}(.*?){endKey}";
        Match match = Regex.Match(input, pattern);
        return match.Success ? match.Groups[1].Value : string.Empty;
    }
    //平凉地区的处理逻辑
    private void 平凉公交()
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
                UIDValue = Convert.ToUInt32(ChangeHexPairs(UIDValue), 16).ToString();
                UIDData.Add(UIDValue);
                string SNValue = worksheet.Cells[row, 8].Text;
                SNData.Add(SNValue);
            }
        }
        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string fileName = $"{excelFileName}.txt";
        string filePath = Path.Combine(desktopPath, fileName);
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
    private void 桂林公交()
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
        桂林 guiLin= new();
        guiLin.ShowDialog();
        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
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
    private void 陕西师范大学()
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
            string filePath = Path.Combine(desktopPath, fileName);
            package.SaveAs(new FileInfo(filePath));
            MessageBox.Show($"数据已处理并保存到桌面{filePath}");
        } 
    }
    //西安文理学院
    private void 西安文理学院()
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
            string filePath = Path.Combine(desktopPath, fileName);
            package.SaveAs(new FileInfo(filePath));
            MessageBox.Show($"数据已处理并保存到桌面{filePath}");
        } 
    }
    //滨州公交的处理逻辑
    private void 滨州公交()
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
            string filePath = Path.Combine(desktopPath, fileName);
            package.SaveAs(new FileInfo(filePath));
            MessageBox.Show($"数据已处理并保存到桌面{filePath}");
        } 
    }
    //云南朗坤的处理逻辑
    private async Task 云南朗坤()
    {
        // 取出Excel文件的数据
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // 避免出现许可证错误
        List<string> SNData = new List<string>();
        List<string> UidData = new List<string>();
        List<string> Uid_Data = new List<string>();
        
        using (var package = new ExcelPackage(ExcelData))
        {
            var worksheet = package.Workbook.Worksheets[0]; // 获取第一个工作表
            int rowCount = worksheet.Dimension.Rows; // 获取行数

            // 异步遍历Excel文件的每一行
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
            // 异步保存文件到桌面
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string fileName = $"{excelFileName}.xlsx";
            string filePath = Path.Combine(desktopPath, fileName);
            // 使用异步的文件保存
            await Task.Run(() => package.SaveAs(new FileInfo(filePath)));
            // 显示提示消息
            MessageBox.Show($"数据已处理并保存到桌面{filePath}");
        }
    }
    //盱眙的处理逻辑
    private async Task 盱眙()
    {
        // 取出Excel文件的数据
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // 避免出现许可证错误
        List<string> SNData = new List<string>();
        List<string> UidData = new List<string>();
      
        using (var package = new ExcelPackage(ExcelData))
        {
            var worksheet = package.Workbook.Worksheets[0]; // 获取第一个工作表
            int rowCount = worksheet.Dimension.Rows; // 获取行数

            // 异步遍历Excel文件的每一行
            for (int row = 1; row <= rowCount; row++)
            {
                string SNValue = worksheet.Cells[row, 6].Text;
                string UidValue = worksheet.Cells[row, 2].Text;
                SNData.Add(SNValue);
                UidData.Add(UidValue);
            }
        }
        // 创建一个新的Excel文件
        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add(excelFileName);
            worksheet.Cells[1, 1].Value = "SerialNumber";
            worksheet.Cells[1, 2].Value = "UID";
            worksheet.Cells[1, 3].Value = "CUSTOMUID";
            for (int i = 0; i < UidData.Count; i++)
            {
                worksheet.Cells[i + 2, 1].Value = SNData[i];
                worksheet.Cells[i + 2, 2].Value = UidData[i];
                worksheet.Cells[i + 2, 3].Value = ChangeHexPairs(UidData[i]);
            }
            // 异步保存文件到桌面
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string fileName = $"{excelFileName}.xlsx";
            string filePath = Path.Combine(desktopPath, fileName);
            // 使用异步的文件保存
            await Task.Run(() => package.SaveAs(new FileInfo(filePath)));
            // 显示提示消息
            MessageBox.Show($"数据已处理并保存到桌面{filePath}");
        }
    }
    //柳州公交的处理逻辑
    private async Task 柳州公交()
    {
        //根据逗号切割MKdate
        string[] KCdata = MKDate[0].Split(';');
        Console.WriteLine(KCdata);
        string Order = KCdata[1];
        string CardBin = KCdata[5];
        string CardNumber = KCdata[4];
        string StartSN = KCdata[6];
        string EndSN = KCdata[7];
        // 取出Excel文件的数据
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // 避免出现许可证错误
        List<string> SNData = new List<string>();
        List<string> ATSData = new List<string>();

        using (var package = new ExcelPackage(ExcelData))
        {
            var worksheet = package.Workbook.Worksheets[0]; // 获取第一个工作表
            int rowCount = worksheet.Dimension.Rows; // 获取行数

            // 异步遍历Excel文件的每一行
            for (int row = 2; row <= rowCount; row++)
            {
                string SNValue = worksheet.Cells[row, 1].Text;
                string ATSValue = worksheet.Cells[row, 2].Text;
                SNData.Add(SNValue);
                ATSData.Add(ATSValue);
            }
        }

        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string date = Order.Substring(3, 8);
        string fileName = $"RC_{date}_54500000_0004_{Order}_{StartSN}_{CardNumber}";
        string filePath = Path.Combine(desktopPath, fileName);
        await Task.Run(() =>
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine($"01;{Order};{CardBin};{StartSN};{EndSN};{CardNumber};");
                for (int i = 0; i < SNData.Count; i++)
                {
                    if (i == SNData.Count - 1)
                    {
                        writer.Write($"{SNData[i]};{SNData[i]};{ATSData[i]};");
                    }
                    else
                    {
                        writer.WriteLine($"{SNData[i]};{SNData[i]};{ATSData[i]};");
                    }
                }
            }
        });
        MessageBox.Show($"文件已保存到桌面{filePath}");
    }
    //漯河的处理逻辑
    private async Task 漯河()
    {
        string cardtype;
        漯河 window = new();
        window.ShowDialog();
        cardtype = window.CardType;
        // 取出Excel文件的数据
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // 避免出现许可证错误
        List<string> SNData = new List<string>();
        List<string> UIDData = new List<string>();
        string StartSN;
        string EndSN;
        using (var package = new ExcelPackage(ExcelData))
        {
            var worksheet = package.Workbook.Worksheets[0]; // 获取第一个工作表
            int rowCount = worksheet.Dimension.Rows; // 获取行数
            StartSN = worksheet.Cells[2, 1].Text;
            EndSN = worksheet.Cells[rowCount, 1].Text;
            for (int row = 2; row <= rowCount; row++)
            {
                string SNValue = worksheet.Cells[row, 1].Text;
                string UIDValue = worksheet.Cells[row, 4].Text;
                //计算UID校验码
                string stra=UIDValue.Substring(0, 2);
                string strb=UIDValue.Substring(2, 2);
                string strc=UIDValue.Substring(4, 2);
                string strd=UIDValue.Substring(6, 2);
                int a=Convert.ToInt32(stra,16);
                int b=Convert.ToInt32(strb,16);
                int c=Convert.ToInt32(strc,16);
                int d=Convert.ToInt32(strd,16);
                int s=a^b^c^d;
                UIDValue += s.ToString("X").PadLeft(2, '0');
                UIDValue = UIDValue.ToUpper();
                //计算SN校验码
                string strNUM = SNValue + "F";
                string stre = strNUM.Substring(0, 2);
                string strf = strNUM.Substring(2, 2);
                string strg = strNUM.Substring(4, 2);
                string strh = strNUM.Substring(6, 2);
                string stri = strNUM.Substring(8, 2);
                string strj = strNUM.Substring(10, 2);
                string strk = strNUM.Substring(12, 2);
                string strl = strNUM.Substring(14, 2);
                string strm = strNUM.Substring(16, 2);
                string strn = strNUM.Substring(18, 2);
                Int32 intnew = (Convert.ToInt32(stre, 16) ^ Convert.ToInt32(strf, 16) ^ Convert.ToInt32(strg, 16) ^ Convert.ToInt32(strh, 16) ^ Convert.ToInt32(stri, 16) ^ Convert.ToInt32(strj, 16) ^ Convert.ToInt32(strk, 16) ^ Convert.ToInt32(strl, 16) ^ Convert.ToInt32(strm, 16) ^ Convert.ToInt32(strn, 16));
                string strXOR_2 = intnew.ToString("X").PadLeft(2, '0');
                SNValue = strNUM + strXOR_2;
                SNData.Add(SNValue);
                UIDData.Add(UIDValue);
            }
        }
        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string fileName = $"CardNoHY{StartSN} - {EndSN}.xml";
        string filePath = Path.Combine(desktopPath, fileName);
        await Task.Run(() =>
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine("<?xml version=\"1.0\" encoding=\"GB2312\"?>");
                writer.WriteLine($"<CardList Total=\"{SNData.Count}\" CardType=\"{cardtype}\" Start=\"{StartSN}\" End=\"{EndSN}\">");
                for (int i = 0; i < SNData.Count; i++)
                {
                    writer.WriteLine($"<Card UID=\"{UIDData[i]}\" AppID=\"{SNData[i]}\"/>");
                }
                writer.Write("</CardList>");
            }
        });
        MessageBox.Show($"文件已保存到桌面{filePath}");
    }
    //随州的处理逻辑
    private async Task 随州()
    {
        // 取出Excel文件的数据
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // 避免出现许可证错误
        List<string> SNData = new List<string>();
        List<string> UidData = new List<string>();
        using (var package = new ExcelPackage(ExcelData))
        {
            var worksheet = package.Workbook.Worksheets[0]; // 获取第一个工作表
            int rowCount = worksheet.Dimension.Rows; // 获取行数
            // 遍历Excel文件的每一行
            for (int row = 1; row <= rowCount; row++)
            {
                string SNValue = worksheet.Cells[row, 8].Text;
                string UidValue = worksheet.Cells[row, 3].Text;
                UidValue = Convert.ToUInt32(UidValue, 16).ToString();
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
            string filePath = Path.Combine(desktopPath, fileName);
            // 使用异步的文件保存
            await Task.Run(() => package.SaveAs(new FileInfo(filePath)));
            // 显示提示消息
            MessageBox.Show($"数据已处理并保存到桌面{filePath}");
        } 
    }
    //昆明的处理逻辑
    private async Task 昆明()
    {
        // 取出Excel文件的数据
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // 避免出现许可证错误
        List<string> SNData = new List<string>();
        List<string> SN16Data = new List<string>();
        List<string> Uid16Data = new List<string>();
        List<string> Uid10Data = new List<string>();
        using (var package = new ExcelPackage(ExcelData))
        {
            var worksheet = package.Workbook.Worksheets[0]; // 获取第一个工作表
            int rowCount = worksheet.Dimension.Rows; // 获取行数
            // 遍历Excel文件的每一行
            for (int row = 2; row <= rowCount; row++)
            {
                string SNValue = worksheet.Cells[row, 1].Text;
                string SN16Value = worksheet.Cells[row, 10].Text;
                string Uid16Value = worksheet.Cells[row, 4].Text;
                string Uid10Value = worksheet.Cells[row, 6].Text;
                SNData.Add(SNValue);
                SN16Data.Add(SN16Value);
                Uid16Data.Add(Uid16Value);
                Uid10Data.Add(Uid10Value);
            }
        }
        // 保存文件到桌面
        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string fileName = "1342680response867020241218.xlsx";
        string filePath = Path.Combine(desktopPath, fileName);
        // 创建一个新的Excel文件
        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add(excelFileName);
           
            for (int i = 0; i < Uid16Data.Count; i++)
            {
                worksheet.Cells[i + 1, 1].Value = Uid16Data[i];
                worksheet.Cells[i + 1, 2].Value = Uid10Data[i];
                worksheet.Cells[i + 1, 3].Value = 8684241208000000 + i;
                worksheet.Cells[i + 1, 4].Value = "8670";
                worksheet.Cells[i + 1, 5].Value = "0" + SN16Data[i];
                worksheet.Cells[i + 1, 5].Value = "ZP18010302";
            }
            // 使用异步的文件保存
            await Task.Run(() => package.SaveAs(new FileInfo(filePath)));
        } 
        fileName = $"{excelFileName}.xlsx";
        filePath = Path.Combine(desktopPath, fileName);
        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add(excelFileName);
            worksheet.Cells[1, 1].Value = "卡面号";
            worksheet.Cells[1, 2].Value = "UID_10_";
            for (int i = 0; i < Uid16Data.Count; i++)
            {
                worksheet.Cells[i + 2, 1].Value = SN16Data[i];
                worksheet.Cells[i + 2, 2].Value = Uid10Data[i];
            }
            // 使用异步的文件保存
            await Task.Run(() => package.SaveAs(new FileInfo(filePath)));
        }  
        MessageBox.Show("数据已处理并保存到桌面");
    }
    //徐州的处理逻辑
    private async Task 徐州地铁()
    {
        await Task.Run(() =>
        {
        // 取出Excel文件的数据
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // 避免出现许可证错误
        List<string> SNData = new List<string>();
        List<string> UidData = new List<string>();
        using (var package = new ExcelPackage(ExcelData))
        {
            var worksheet = package.Workbook.Worksheets[0]; // 获取第一个工作表
            int rowCount = worksheet.Dimension.Rows; // 获取行数
            // 遍历Excel文件的每一行
            for (int row = 2; row <= rowCount; row++)
            {
                string SNValue = worksheet.Cells[row, 6].Text;
                string UidValue = worksheet.Cells[row, 2].Text;
                SNData.Add(SNValue);
                UidData.Add(UidValue);
            }
        }
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
       
        MessageBox.Show($"文件已保存到桌面{filePath}");
        });
    }
    //江苏乾翔的处理逻辑
    private async Task 江苏乾翔()
    {
        await Task.Run(() =>
        {
            // 取出Excel文件的数据
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // 避免出现许可证错误
            List<string> SNData = new List<string>();
            List<string> UidData = new List<string>();
            using (var package = new ExcelPackage(ExcelData))
            {
                var worksheet = package.Workbook.Worksheets[0]; // 获取第一个工作表
                int rowCount = worksheet.Dimension.Rows; // 获取行数
                // 遍历Excel文件的每一行
                for (int row = 2; row <= rowCount; row++)
                {
                    string SNValue = worksheet.Cells[row, 7].Text;
                    string UidValue = worksheet.Cells[row, 3].Text;
                    SNData.Add(SNValue);
                    UidData.Add(UidValue);
                }
            }
            //新建一个Excel文件
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
                string filePath = Path.Combine(desktopPath, fileName);
                package.SaveAs(new FileInfo(filePath));
                MessageBox.Show($"数据已处理并保存到桌面{filePath}");
            }
        });
    }
    //石家庄的处理逻辑
    private async Task 石家庄()
    {
        await Task.Run(() =>
        {
            // 取出Excel文件的数据
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // 避免出现许可证错误
            List<string> SNData = new List<string>();
            List<string> Uid_16_Data = new List<string>();
            using (var package = new ExcelPackage(ExcelData))
            {
                var worksheet = package.Workbook.Worksheets[0]; // 获取第一个工作表
                int rowCount = worksheet.Dimension.Rows; // 获取行数
                // 遍历Excel文件的每一行
                for (int row = 1; row <= rowCount; row++)
                {
                    string SNValue = worksheet.Cells[row,7].Text;
                    string Uid_16_Value = worksheet.Cells[row, 2].Text;
                    SNData.Add(SNValue);
                    Uid_16_Data.Add(Uid_16_Value);
                }
            }
            //新建一个Excel文件
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add(excelFileName);
                worksheet.Cells[1, 1].Value = "SerialNum";
                worksheet.Cells[1, 2].Value = "UID_16_";
                for (int i = 0; i < Uid_16_Data.Count; i++)
                {
                    worksheet.Cells[i + 2, 1].Value = SNData[i];
                    worksheet.Cells[i + 2, 2].Value = Uid_16_Data[i];
                }
                // 保存文件到桌面
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string fileName = $"{excelFileName}.xlsx";
                string filePath = Path.Combine(desktopPath, fileName);
                package.SaveAs(new FileInfo(filePath));
            }
            // 保存txt文件到桌面
            string desktopPath1 = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string fileName1 = $"{excelFileName}.txt";
            string filePath1 = Path.Combine(desktopPath1, fileName1);

            using (StreamWriter writer = new StreamWriter(filePath1))
            {
                writer.WriteLine("SerialNum\tUID");
                for (int i = 0; i < SNData.Count; i++)
                {
                    if (i == SNData.Count - 1)
                    {
                        writer.Write($"{SNData[i]}\t{Uid_16_Data[i]}");
                    }
                    else
                    {
                        writer.WriteLine($"{SNData[i]}\t{Uid_16_Data[i]}");
                    }
                }
            }
        }); 
        MessageBox.Show("数据已处理并保存到桌面");
    }
    //淮北的处理逻辑
    private async Task 淮北()
    {
        string cardtype;
        // 取出Excel文件的数据
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // 避免出现许可证错误
        List<string> SNData = new List<string>();
        List<string> UIDData = new List<string>();
        string StartSN;
        string EndSN;
        using (var package = new ExcelPackage(ExcelData))
        {
            var worksheet = package.Workbook.Worksheets[0]; // 获取第一个工作表
            int rowCount = worksheet.Dimension.Rows; // 获取行数
            StartSN = worksheet.Cells[2,1].Text;
            EndSN = worksheet.Cells[rowCount, 1].Text;
            cardtype = StartSN.Substring(8,2);
            for (int row = 2; row <= rowCount; row++)
            {
                string SNValue = worksheet.Cells[row, 1].Text;
                string UIDValue = worksheet.Cells[row, 4].Text;
                //计算UID校验码
                string stra=UIDValue.Substring(0, 2);
                string strb=UIDValue.Substring(2, 2);
                string strc=UIDValue.Substring(4, 2);
                string strd=UIDValue.Substring(6, 2);
                int a=Convert.ToInt32(stra,16);
                int b=Convert.ToInt32(strb,16);
                int c=Convert.ToInt32(strc,16);
                int d=Convert.ToInt32(strd,16);
                int s=a^b^c^d;
                UIDValue += s.ToString("X").PadLeft(2, '0');
                UIDValue = UIDValue.ToUpper();
                //计算SN校验码
                SNValue += "F";
                string stre = SNValue.Substring(0, 2);
                string strf = SNValue.Substring(2, 2);
                string strg = SNValue.Substring(4, 2);
                string strh = SNValue.Substring(6, 2);
                string stri = SNValue.Substring(8, 2);
                string strj = SNValue.Substring(10, 2);
                string strk = SNValue.Substring(12, 2);
                string strl = SNValue.Substring(14, 2);
                string strm = SNValue.Substring(16, 2);
                string strn = SNValue.Substring(18, 2);
                Int32 intnew = (Convert.ToInt32(stre, 16) ^ Convert.ToInt32(strf, 16) ^ Convert.ToInt32(strg, 16) ^ Convert.ToInt32(strh, 16) ^ Convert.ToInt32(stri, 16) ^ Convert.ToInt32(strj, 16) ^ Convert.ToInt32(strk, 16) ^ Convert.ToInt32(strl, 16) ^ Convert.ToInt32(strm, 16) ^ Convert.ToInt32(strn, 16));
                string strXOR_2 = intnew.ToString("X").PadLeft(2, '0');
                SNValue += strXOR_2;
                SNData.Add(SNValue);
                UIDData.Add(UIDValue);
            }
        }
        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string fileName = $"CardNoTM{StartSN}-{EndSN}.xml";
        string filePath = Path.Combine(desktopPath, fileName);
        await Task.Run(() =>
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine("<?xml version=\"1.0\" encoding=\"GB2312\"?>");
                writer.WriteLine($"<CardList Total=\"{SNData.Count}\" CardType=\"{cardtype}\" Start=\"{StartSN}\" End=\"{EndSN}\">");
                for (int i = 0; i < SNData.Count; i++)
                {
                    writer.WriteLine($"<Card UID=\"{UIDData[i]}\" AppID=\"{SNData[i]}\"/>");
                }
                writer.Write("</CardList>");
            }
        });
        MessageBox.Show($"文件已保存到桌面{filePath}"); 
    }
    private void Test(object sender, RoutedEventArgs e)
    {
        MessageBox.Show("未开发");
    }
    //调整16进制与不调整16进制互相转换
    private string ChangeHexPairs(string hex)
    {
        if (hex.Length % 2 != 0)
        {
            MessageBox.Show("数据不合法，可能需要删除表头行");
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
