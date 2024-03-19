using Avalonia.Controls;
using Avalonia.Interactivity;

namespace CafeOrderManagementSystem;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void AllProduct_OnClick(object? sender, RoutedEventArgs e)
    {
        ProductWindow productWindow = new ProductWindow();
        productWindow.Show();
    }

    private void Order_OnClick(object? sender, RoutedEventArgs e)
    {
        OrderWindow orderWindow = new OrderWindow();
        orderWindow.Show();
    }

    private void ViewOrderBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        ViewOrderWindow cartWindow = new ViewOrderWindow();
        cartWindow.Show();
    }

    private void EmployeeBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        EmployeeWindow employeeWindow = new EmployeeWindow();
        employeeWindow.Show();
    }
}