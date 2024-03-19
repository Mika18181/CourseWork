namespace CafeOrderManagementSystem.Entities;

public class Order
{
    public int OrderId { get; set; }
    public string User { get; set; }
    public string Product { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal TotalPrice { get; set; }
    public int Table { get; set; }
}