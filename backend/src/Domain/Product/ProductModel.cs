namespace Backend.Domain.Product;

public class ProductModel(string name, decimal price, int stock, string imagePath, Guid? id = null)
{
    public Guid Id { get; } = id ?? Guid.NewGuid();
    public string Name { get; } = name;
    public decimal Price { get; } = price;
    public int Stock { get; } = stock;
    public string ImagePath { get; } = imagePath;
}