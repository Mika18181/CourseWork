using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using CafeOrderManagementSystem.Entities;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using MySql.Data.MySqlClient;

namespace CafeOrderManagementSystem;

public partial class ProductWindow : Window
{
    private Database _db = new Database();
    private ObservableCollection<Product> _products = new ObservableCollection<Product>();
    private string sql = "select * from product";
    private byte[] _imageBytes;
    
    
    public ProductWindow()
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
    
    

    private async void OpenBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        var topLevel = TopLevel.GetTopLevel(this);
        
        var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Изображение",
            AllowMultiple = false
        });

        if (files.Count >= 1)
        {
            await using var imageStream = await files[0].OpenReadAsync();
            _imageBytes = await ConvertStreamToBytesAsync(imageStream);
        }
    }
    
    private async Task<byte[]> ConvertStreamToBytesAsync(Stream stream)
    {
        using (MemoryStream memoryStream = new MemoryStream())
        {
            await stream.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }
    }
    private void ProductsDGrid_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (ProductsDGrid.SelectedItem != null)
        {
            Product selectedProduct = (Product)ProductsDGrid.SelectedItem;
            NameTBox.Text = selectedProduct.ProductName;
            PriceTBox.Text = selectedProduct.Price.ToString();
        }
    }
    
    private void AddEdit(string sql)
    {
        _db.OpenConnection();
        MySqlCommand command = new MySqlCommand(sql, _db.GetConnection());
        command.Parameters.AddWithValue("@name", NameTBox.Text);
        command.Parameters.AddWithValue("@price", PriceTBox.Text);
        command.Parameters.AddWithValue("@image", _imageBytes);
        if (ProductsDGrid.SelectedItem != null)
        {
            Product selectedProduct = (Product)ProductsDGrid.SelectedItem;
            command.Parameters.AddWithValue("@id", selectedProduct.ProductId);
        }
        command.ExecuteNonQuery();
        _db.CloseConnection();
    }

    private void AddBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        if (ProductsDGrid.SelectedItem == null)
        {
            string add = "insert into product (product_name, price, image) values (@name, @price, @image);";
            AddEdit(add);
        }
        else
        {
            string edit = "update product set product_name = @name, price = @price, image = @image where product_id = @id";
            AddEdit(edit);
        }
        RefreshData();
    }

    private void DeleteBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        if (ProductsDGrid.SelectedItem != null)
        {
            Product selectedProduct = (Product)ProductsDGrid.SelectedItem;
            string del = "delete from product where product_id = @id";
            _db.OpenConnection();
            MySqlCommand command = new MySqlCommand(del, _db.GetConnection());
            command.Parameters.AddWithValue("@id", selectedProduct.ProductId);
            command.ExecuteNonQuery();
            _db.CloseConnection();
            RefreshData();
        }
    }
    
    private void RefreshData()
    {
        _products.Clear();
        ShowTable(sql); // Повторно заполняем коллекцию данными из базы данных
    }
}