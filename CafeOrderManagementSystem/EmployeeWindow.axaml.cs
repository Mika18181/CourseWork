using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using CafeOrderManagementSystem.Entities;
using MySql.Data.MySqlClient;

namespace CafeOrderManagementSystem;

public partial class EmployeeWindow : Window
{
    private Database _db = new Database();
    private ObservableCollection<User> _users = new ObservableCollection<User>();
    private string sql = "select * from user";
    public EmployeeWindow()
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
            var currUser = new User()
            {
                UserId = reader.GetInt32("user_id"),
                Login = reader.GetString("login"),
                Password = reader.GetString("password"),
                Name = reader.GetString("name")
            };
            _users.Add(currUser);
        }
        _db.CloseConnection();
        UserDGrid.ItemsSource = _users;
    }

    private void UserDGrid_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (UserDGrid.SelectedItem != null)
        {
            User selectedUser = (User)UserDGrid.SelectedItem;
            LoginTBox.Text = selectedUser.Login;
            PasswordTBox.Text = selectedUser.Password;
            NameTBox.Text = selectedUser.Name;
        }
    }
    
    private void AddEdit(string sql)
    {
        _db.OpenConnection();
        MySqlCommand command = new MySqlCommand(sql, _db.GetConnection());
        command.Parameters.AddWithValue("@login", LoginTBox.Text);
        command.Parameters.AddWithValue("@password", PasswordTBox.Text);
        command.Parameters.AddWithValue("@name", NameTBox.Text);
        if (UserDGrid.SelectedItem != null)
        {
            User selectedUser = (User)UserDGrid.SelectedItem;
            command.Parameters.AddWithValue("@id", selectedUser.UserId);
        }
        command.ExecuteNonQuery();
        _db.CloseConnection();
    }

    private void AddBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        if (UserDGrid.SelectedItem == null)
        {
            string add = "insert into user (login, password, name) values (@login, @password, @name);";
            AddEdit(add);
        }
        else
        {
            string edit = "update user set login = @login, password = @password, name = @name where user_id = @id";
            AddEdit(edit);
        }
        RefreshData();
    }
    
    private void RefreshData()
    {
        _users.Clear();
        ShowTable(sql); // Повторно заполняем коллекцию данными из базы данных
    }

    private void DeleteBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        if (UserDGrid.SelectedItem != null)
        {
            User selectedUser = (User)UserDGrid.SelectedItem;
            string del = "delete from user where user_id = @id";
            _db.OpenConnection();
            MySqlCommand command = new MySqlCommand(del, _db.GetConnection());
            command.Parameters.AddWithValue("@id", selectedUser.UserId);
            command.ExecuteNonQuery();
            _db.CloseConnection();
            RefreshData();
        }
    }
}