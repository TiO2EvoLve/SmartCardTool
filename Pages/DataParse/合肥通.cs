using System.Text;
using System.Xml;

namespace WindowUI.Pages.DataParse;

public class 合肥通
{
    static List<string> sqlList = [];
    public static async Task Parse(string DPFFilePath)
    {
        //拷贝模板到桌面
        var sourceFilePath = "temple/合肥通.mdb";
        var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        var destinationFilePath = Path.Combine(desktopPath, "合肥通.mdb");

        try
        {
            await Task.Run(() => File.Copy(sourceFilePath, destinationFilePath, true));
        }
        catch (Exception ex)
        {
            return;
        }
        //提取数据保存到数据库
        readdpf_cs(DPFFilePath);
        if (sqlList.Count > 0)
        {
            Mdb.ExecuteBatch(destinationFilePath,sqlList);
        }
        //查找分散因子，并保存到数据库
        string parentDir = Path.GetDirectoryName(DPFFilePath)!;
        // 获取父目录下的第一个子目录名称，也就是分散因子的名称
        string? folderName = Directory
            .GetDirectories(parentDir)
            .Select(Path.GetFileName)
            .FirstOrDefault();
        
        //保存分散因子到数据库
        if (string.IsNullOrEmpty(folderName))
        {
            MessageBox.Show("未找到分散因子文件夹");
            return;
        }
        
        string sql = $"update safekey set KK='{folderName}'";
        Mdb.Execute(destinationFilePath, sql);
        
        MessageBox.Show("数据生成成功");
    }

    public static void readdpf_cs(string Path2)
    {
        Dictionary<string, string> dicry = new Dictionary<string, string>();
        BinaryReader br;
        br = new BinaryReader(new FileStream((Path2), FileMode.Open));
        byte s = br.ReadByte();
        StringBuilder sbdr = new StringBuilder();
        try
        {
            while (true)
            {
                string val = Convert.ToString(s, 16);
                val = val.PadLeft(2, '0');
                sbdr.Append(val);
                s = br.ReadByte();
            }
        }
        catch
        {
            br.Close();
        }

        //根据制卡文件说明 解析
        string valStr = sbdr.ToString();

        string dginame = valStr.Substring(17432, 612);

        //取出所有DGI 数量34
        List<string> dgiList = new List<string>();
        for (int i = 0; i < 34; i++)
        {
            string val = dginame.Substring(0, 18);
            val = val.Substring(10, 8);
            byte[] bval = strToToHexByte(val);
            ASCIIEncoding ASCIITochar = new ASCIIEncoding();
            char[] ascii = ASCIITochar.GetChars(bval);
            dgiList.Add(ascii[0].ToString() + ascii[1].ToString() + ascii[2].ToString() + ascii[3].ToString());
            //setValue(ref dicry,  dgiList[i]+"-"+i, dgiList[i]);
            dginame = dginame.Substring(18, dginame.Length - 18);
        }

        string datacount = valStr.Substring(17408, 8); //卡的个数
        datacount = datacount.Substring(6, 2) + datacount.Substring(4, 2) + datacount.Substring(2, 2) +
                    datacount.Substring(0, 2); //正式卡注意这个位置
        UInt32 icount = Convert.ToUInt32(datacount, 16);
        valStr = valStr.Substring(18044, valStr.Length - 18044);
        //第一张卡的数据以01000000E209开头，第二张的以02000000E209开头，以此类推
        string[] onecard = new string[icount]; //所有数据放在dataall里面
        for (int id = 0; id < icount; id++)
        {
            //取出每张卡的所有数据，放在onecard[id]里
            onecard[id] = valStr.Substring(id * 5072, 5072);

            //解析当前卡片制卡数据的所有具体数据：卡片短编号、数据总长度、所有DGI
            string shortnum = onecard[id].Substring(0, 8); //卡片短编号
            shortnum = shortnum.Substring(6, 2) + shortnum.Substring(4, 2) + shortnum.Substring(2, 2) +
                       shortnum.Substring(0, 2); //正式卡注意这个位置
            string serialnum = "";
            //setValue(ref dicry, shortnum, shortnum);
            onecard[id] = onecard[id].Substring(8, onecard[id].Length - 8); //去掉卡片在当前文件中的编号，如：01000000
            onecard[id] = onecard[id].Substring(4, onecard[id].Length - 4); //去掉卡片制卡数据的总长度，如：E209

            for (int i = 0; i < 34; i++)
            {
                string dgiStr = dgiList[i];
                if (dgiStr.Equals("7777"))
                {
                    dgiStr = dgiList[i];
                }

                if (onecard[id].IndexOf("86") != 0)
                {
                    continue;
                }

                string val = onecard[id].Substring(0, 14).ToUpper();
                int jjj = val.IndexOf(dgiList[i]);
                if (jjj != 4 && jjj != 6 && jjj != 8)
                    jjj = val.IndexOf(dgiList[i], jjj + 1);
                val = val.Substring(jjj + 4, 2);

                int val_length = Convert.ToInt32(val, 16);
                
                string realvalue = onecard[id].Substring(jjj + 4, val_length * 2 + 2);
                onecard[id] = onecard[id].Substring(jjj + 4 + val_length * 2 + 2,
                    onecard[id].Length - (jjj + 4 + val_length * 2 + 2));
                setValue2(ref dicry, dgiList[i], realvalue);
                if (dgiList[i] == "0101")
                {
                    //从中取出流水号
                    serialnum = realvalue.Substring(10, 19);
                }
            }

            if (dicry.Count != 34) throw new Exception("解析不通过 数量不对");


            //保存数据库
            string ti = DateTime.Now.ToString();
            string insertsql2 =
                "insert into zhikaEC(SN,shortnum,TIME2,DGIFF2E,DGI0101,DGI0102,DGI0201,DGI0202,DGI0203,DGI0204,DGI0205,DGI0206,DGI0301,DGI0302,DGI0303,DGI0401,DGI0501,DGI0801,DGI0D01,DGI0E01,DGI9200,DGI9104,DGI9207,DGI8010,DGI9010,DGI8000,DGIA001,DGI8020,DGI8201,DGI8202,DGI8203,DGI8204,DGI8205,DGI9102,DGI9103,DGI9020,DGI9000) values('" +
                serialnum + "','" + shortnum + "','" + ti + "' ,'" + dicry[dgiList[0]] + "','" + dicry[dgiList[1]] +
                "','" + dicry[dgiList[2]] + "','" + dicry[dgiList[3]] + "','" + dicry[dgiList[4]] + "','" +
                dicry[dgiList[5]] + "','" + dicry[dgiList[6]] + "','" + dicry[dgiList[7]] + "','" + dicry[dgiList[8]] +
                "','" + dicry[dgiList[9]] + "','" + dicry[dgiList[10]] + "','" + dicry[dgiList[11]] + "','" +
                dicry[dgiList[12]] + "','" + dicry[dgiList[13]] + "','" + dicry[dgiList[14]] + "','" +
                dicry[dgiList[15]] + "','" + dicry[dgiList[16]] + "','" + dicry[dgiList[17]] + "','" +
                dicry[dgiList[18]] + "','" + dicry[dgiList[19]] + "','" + dicry[dgiList[20]] + "','" +
                dicry[dgiList[21]] + "','" + dicry[dgiList[22]] + "','" + dicry[dgiList[23]] + "','" +
                dicry[dgiList[24]] + "','" + dicry[dgiList[25]] + "','" + dicry[dgiList[26]] + "','" +
                dicry[dgiList[27]] + "','" + dicry[dgiList[28]] + "','" + dicry[dgiList[29]] + "','" +
                dicry[dgiList[30]] + "','" + dicry[dgiList[31]] + "','" + dicry[dgiList[32]] + "','" +
                dicry[dgiList[33]] + "')";
            sqlList.Add(insertsql2);
        }
    }

    public static void setValue2(ref Dictionary<string, string> dicry, string key, string val)
    {
        if (dicry.ContainsKey(key))
            dicry.Remove(key);
        dicry.Add("" + key, val.ToUpper());
    }

    /// <summary> 
    /// 字符串转16进制字节数组 
    /// </summary> 
    /// <param name="hexString"></param> 
    /// <returns></returns> 
    private static byte[] strToToHexByte(string hexString)
    {
        hexString = hexString.Replace(" ", "");
        if ((hexString.Length % 2) != 0)
            hexString += " ";
        byte[] returnBytes = new byte[hexString.Length / 2];
        for (int i = 0; i < returnBytes.Length; i++)
            returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
        return returnBytes;
    }
}