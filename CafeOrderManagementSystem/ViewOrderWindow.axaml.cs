using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using CafeOrderManagementSystem.Entities;
using MySql.Data.MySqlClient;

namespace CafeOrderManagementSystem;

public partial class ViewOrderWindow : Window
{
    private Database _db = new Database();
    private ObservableCollection<Order> _orders = new ObservableCollection<Order>();
    private string sql = "select order_id, name, number, product_name, quantity, orders.price, total_price from orders\njoin cafe1.product p on p.product_id = orders.product\njoin cafe1.user u on u.user_id = orders.user\njoin cafe1.`table` t on t.table_id = orders.`table`";
    
    public ViewOrderWindow()
    {
        InitializeComponent();
        ShowTable(sql);
    }

    private void ShowTable(string sql)
    {
        _db.OpenConnection();
        MySqlCommand command = new MySqlCommand(sql, _db.GetConnection());
        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            var currentOrder = new Order()
            {
                OrderId = reader.GetInt32("order_id"),
                User = reader.GetString("name"),
                Table = reader.GetInt32("number"),
                Product = reader.GetString("product_name"),
                Quantity = reader.GetInt32("quantity"),
                Price = reader.GetDecimal("price"),
                TotalPrice = reader.GetDecimal("total_price")
            };
            _orders.Add(currentOrder);
        }
        _db.CloseConnection();
        CartDGrid.ItemsSource = _orders;
    }
}