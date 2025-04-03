using System.Data.OleDb;
using Wpf.Ui.Controls;

namespace WindowUI.Tool;

public class Mdb
{
    //执行查找并将数据返回
    public static List<string> Select(string filePath, string sql)
    {
        var _connectionString = $@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={filePath};";

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
}