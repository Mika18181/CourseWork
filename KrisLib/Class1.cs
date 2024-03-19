using CafeOrderManagementSystem;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using MySql.Data.MySqlClient;

namespace KrisLib;


public class Class1
{
    Database _db = new Database();
    
    public bool Login(string sql, string login, string password)
    {
        _db.OpenConnection();
        MySqlCommand command = new MySqlCommand(sql, _db.GetConnection());
        command.Parameters.AddWithValue("@login", login);
        command.Parameters.AddWithValue("@password", password);
        object result = command.ExecuteScalar();

        if (result != null)
        {
            return true;
        }
        else
        {
            return false;
        }
        _db.CloseConnection();
    }

    public bool AddOrder(int user, int product, int quantity, decimal price, decimal totalPrice, int table)
    {
        try
        {
            _db.OpenConnection();
            string order = "insert into orders (user, product, quantity, price, total_price, `table`) values (@user,@product, @quantity, @price, @total, @table)";
            MySqlCommand command = new MySqlCommand(order, _db.GetConnection());
            command.Parameters.AddWithValue("@user", user);
            command.Parameters.AddWithValue("@product", product);
            command.Parameters.AddWithValue("@quantity", quantity);
            command.Parameters.AddWithValue("@price", price);
            command.Parameters.AddWithValue("@total", totalPrice);
            command.Parameters.AddWithValue("@table", table);
            command.ExecuteNonQuery();
            _db.CloseConnection();
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
        
    }
}