using Backend.Domain.Models;

namespace Backend.Application.Dtos;

public class ProductResponseDto(Guid id, string name, decimal price, int stock, string imagePath)
{
    public Guid Id { get; } = id;
    public string Name { get; } = name;
    public decimal Price { get; } = price;
    public int Stock { get; } = stock;
    public string ImagePath { get; } = imagePath;

    public static ProductResponseDto FromModel(ProductModel model)
    {
        return new ProductResponseDto(
            model.Id,
            model.Name,
            model.Price,
            model.Stock,
            model.ImagePath
        );
    }
}
