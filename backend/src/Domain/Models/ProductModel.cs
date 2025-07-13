namespace Backend.Domain.Models;

public class ProductModel(string name, decimal price, int stock, string imagePath, Guid? id = null)
{
    public Guid Id { get; set; } = id ?? Guid.NewGuid();
    public string Name { get; set; } = name;
    public decimal Price { get; set; } = price;
    public int Stock { get; set; } = stock;
    public string ImagePath { get; set; } = imagePath;
}