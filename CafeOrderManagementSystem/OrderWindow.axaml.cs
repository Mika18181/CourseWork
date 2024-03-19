using System;
using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using CafeOrderManagementSystem.Entities;
using MySql.Data.MySqlClient;

namespace CafeOrderManagementSystem;

public partial class OrderWindow : Window
{
    private Database _db = new Database();
    private ObservableCollection<Product> _products = new ObservableCollection<Product>();
    private ObservableCollection<Table> _tables = new ObservableCollection<Table>();
    private string sql = "select * from product";
    private byte[] _imageBytes;
    
    public OrderWindow()
    {
        InitializeComponent();
        ShowTable(sql);
        LoadDataCBox();
    }

    private void ProductsDGrid_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (ProductsDGrid.SelectedItem != null)
        {
            Product selectedProduct = (Product)ProductsDGrid.SelectedItem;
            NameTBox.Text = selectedProduct.ProductName;
        }
    }
    
    private void ShowTable(string sql)
    {
        _db.OpenConnection();
        MySqlCommand command = new MySqlCommand(sql, _db.GetConnection());
        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            var currentProduct = new Product()
            {
                ProductId = reader.GetInt32("product_id"),
                ProductName = reader.GetString("product_name"),
                Price = reader.GetDecimal("price"),
                Image = reader["image"] as byte[]
            };
            _products.Add(currentProduct);
        }
        _db.CloseConnection();
        ProductsDGrid.ItemsSource = _products;
    }


    private void AddBtn_OnClickOpenBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(NameTBox.Text) && !string.IsNullOrWhiteSpace(QuantityTBox.Text))
        {
            if (int.TryParse(QuantityTBox.Text, out int quantity))
            {
                Product selectedProduct = (Product)ProductsDGrid.SelectedItem;
                decimal totalPrice = selectedProduct.Price * quantity;
                TotalTxt.Text += totalPrice.ToString() + " руб.";

                try
                {
                    _db.OpenConnection();
                    string order = "insert into orders (user, product, quantity, price, total_price, `table`) values (@user,@product, @quantity, @price, @total, @table)";
                    MySqlCommand command = new MySqlCommand(order, _db.GetConnection());
                    command.Parameters.AddWithValue("@user", 1);
                    command.Parameters.AddWithValue("@product", selectedProduct.ProductId);
                    command.Parameters.AddWithValue("@quantity", quantity);
                    command.Parameters.AddWithValue("@price", selectedProduct.Price);
                    command.Parameters.AddWithValue("@total", totalPrice);
                    command.Parameters.AddWithValue("@table", ((Table)TableCBox.SelectedItem).Number);
                    command.ExecuteNonQuery();
                    _db.CloseConnection();
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                    throw;
                }
                NameTBox.Clear();
                QuantityTBox.Clear();
            }
        }
        else
        {
            
        }
    }

    private void LoadDataCBox()
    {
        _db.OpenConnection();
        string sql = "select number from `table`";
        MySqlCommand command = new MySqlCommand(sql, _db.GetConnection());
        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            var currTable = new Table()
            {
                Number = reader.GetInt32("number")
            };
            _tables.Add(currTable);
        }
        _db.CloseConnection();
        TableCBox.ItemsSource = _tables;

    }
}