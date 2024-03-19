using System.Data;
using MySql.Data.MySqlClient;

namespace CafeOrderManagementSystem;

public class Database
{
    private MySqlConnection _connection =
        new MySqlConnection(@"server=localhost;database=cafe1;port=3306;User Id=root;password=IGraf123*");

    public void OpenConnection()
    {
        if (_connection.State == ConnectionState.Closed)
        {
            _connection.Open();
        }
    }

    public void CloseConnection()
    {
        if (_connection.State == ConnectionState.Open)
        {
            _connection.Close();
        }
    }

    public MySqlConnection GetConnection()
    {
        return _connection;
    }
}