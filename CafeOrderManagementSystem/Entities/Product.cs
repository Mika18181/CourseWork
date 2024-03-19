﻿namespace CafeOrderManagementSystem.Entities;

public class Product
{
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public decimal Price { get; set; }
    public byte[]? Image { get; set; }
}