using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using MySql.Data.MySqlClient;

namespace CafeOrderManagementSystem;

public partial class LoginWindow : Window
{
    private Database _db = new Database();
    public LoginWindow()
    {
        InitializeComponent();
    }

    private void LoginBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        string sql = "select user_id from user where login = @login and password = @password";
        string login = LoginTxt.Text;
        string password = PasswordTxt.Text;
        Login(sql, login, password);
    }
    
    private void Login(string sql, string login, string password)
    {
        _db.OpenConnection();
        MySqlCommand command = new MySqlCommand(sql, _db.GetConnection());
        command.Parameters.AddWithValue("@login", login);
        command.Parameters.AddWithValue("@password", password);
        object result = command.ExecuteScalar();

        if (result != null)
        {
            
            MainWindow mainWindow = new MainWindow();
            this.Hide();
            mainWindow.Show();
        }
        else
        {
            var box = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Неверный логин или пароль", ButtonEnum.Ok,
                MsBox.Avalonia.Enums.Icon.Error);
            var rBox = box.ShowAsync();
        }
        _db.CloseConnection();
    }
}