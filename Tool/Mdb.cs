using System.Data.OleDb;
using System.Runtime.InteropServices;
using Wpf.Ui.Controls;

namespace WindowUI.Tool;

public class Mdb
{
    // 动态创建mdb数据库文件（默认委托到带标志的重载）
    public static void CreateDatabase(string filePath)
    {
        CreateDatabase(filePath, false);
    }

    // 支持创建旧版 Jet (.mdb) 的重载，用于兼容旧电脑上的旧程序
    public static void CreateDatabase(string filePath, bool useLegacyJet)
    {
        // 如果文件已存在，先删除
        if (File.Exists(filePath))
        {
            try
            {
                File.Delete(filePath);
            }
            catch (Exception ex)
            {
                throw new Exception($"无法删除已存在的文件: {ex.Message}");
            }
        }

        // 确保目录存在
        var directory = Path.GetDirectoryName(filePath);
        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        // 使用 ADOX COM 组件创建数据库
        try
        {
            // 动态创建 ADOX.Catalog COM 对象
            Type catalogType = Type.GetTypeFromProgID("ADOX.Catalog");
            if (catalogType == null)
            {
                throw new Exception("无法创建 ADOX.Catalog 对象，请确保已安装 Microsoft Access Database Engine 或 MDAC/JET");
            }

            object catalog = Activator.CreateInstance(catalogType);

            // 构建连接字符串：根据 useLegacyJet 决定使用 Jet 4.0（旧电脑兼容）还是 ACE 12.0（默认）
            string connectionString;
            if (useLegacyJet)
            {
                connectionString = $@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={filePath};Jet OLEDB:Engine Type=5";
            }
            else
            {
                connectionString = $@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={filePath};";
            }

            // 调用 Create 方法创建数据库
            catalogType.InvokeMember("Create",
                System.Reflection.BindingFlags.InvokeMethod,
                null,
                catalog,
                new object[] { connectionString });

            // 释放 COM 对象
            Marshal.ReleaseComObject(catalog);
        }
        catch (Exception ex)
        {
            throw new Exception($"创建数据库文件失败: {ex.Message}");
        }
    }

    //执行查找并将数据返回
    public static List<string> Select(string filePath, string sql)
    {
        var _connectionString = $@"Provider=Microsoft.ACE.OLEDB.4.0;Data Source={filePath};";

        using (var connection = new OleDbConnection(_connectionString))
        {
            try
            {
                connection.Open();
                var command = new OleDbCommand(sql, connection);
                var reader = command.ExecuteReader();

                if (reader.FieldCount == 0) Message.ShowMessageBox("错误", "未找到数据");
                //将结果封装到List<string>集合中
                List<string> result = new List<string>();
                while (reader.Read())
                    for (var i = 0; i < reader.FieldCount; i++)
                        result.Add(reader[i].ToString() ?? throw new InvalidOperationException());

                return result;
            }
            catch (Exception ex)
            {
                Message.ShowSnack("错误", ex.Message, ControlAppearance.Danger,
                    new SymbolIcon(SymbolRegular.ErrorCircle20), 3);
                LogManage.AddLog("数据库指令执行出错！\n" + ex.Message);
            }
            return null;
        }
    }

    //执行sql语句不返回数据，可以支持增删改
    public static void Execute(string filePath, string sql)
    {
        var _connectionString = $@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={filePath};";

        using (var connection = new OleDbConnection(_connectionString))
        {
            try
            {
                connection.Open();
                var command = new OleDbCommand(sql, connection);
                var result = command.ExecuteNonQuery();
                if (result == 0) Message.ShowMessageBox("错误", "sql执行失败");
            }
            catch (Exception ex)
            {
                Message.ShowSnack("错误", ex.Message, ControlAppearance.Danger,
                    new SymbolIcon(SymbolRegular.ErrorCircle20), 3);
                LogManage.AddLog("数据库指令执行出错！\n" + ex.Message);
            }
        }
    }

    //批量执行sql
    public static void ExecuteBatch(string filePath, List<string> sqls)
    {
        var _connectionString = $@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={filePath};";
        using (var connection = new OleDbConnection(_connectionString))
        {
            connection.Open();
            var transaction = connection.BeginTransaction();
            var command = new OleDbCommand { Connection = connection, Transaction = transaction };

            try
            {
                foreach (var sql in sqls)
                {
                    command.CommandText = sql;
                    command.ExecuteNonQuery();
                }

                transaction.Commit();
            }
            catch (Exception ex)
            {
               // transaction.Rollback();
               throw;
            }
        }
    }
}