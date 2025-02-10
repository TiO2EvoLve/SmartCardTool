using System.Data.OleDb;

namespace WindowUI.Tool;

public class Mdb
{
    public static List<string> Select(string filePath,string sql)
    {
        string _connectionString = $@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={filePath};";
        
        using (OleDbConnection connection = new OleDbConnection(_connectionString))
        {
            try
            {
                connection.Open();
                OleDbCommand command = new OleDbCommand(sql, connection);
                OleDbDataReader reader = command.ExecuteReader();

                //将结果封装到List<string>集合中
                List<string> result = new List<string>();
                while (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        result.Add(reader[i].ToString());
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            return null;
        }
    }
}