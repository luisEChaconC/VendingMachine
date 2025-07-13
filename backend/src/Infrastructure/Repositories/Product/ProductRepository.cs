using Backend.Application.Ports;
using Backend.Domain.Models;

namespace Backend.Infrastructure.Repositories.Product;

public class ProductRepository(IDictionary<Guid, ProductModel> products) : IProductRepository
{
    public IEnumerable<ProductModel> GetAllProducts()
    {
        return products.Values;
    }

    public ProductModel? GetProductById(Guid id)
    {
        return products.TryGetValue(id, out var product) ? product : null;
    }

    public void SetStock(Guid productId, int newStockValue)
    {
        if (!products.TryGetValue(productId, out var product))
        {
            throw new ArgumentException($"Product with id {productId} not found", nameof(productId));
        }

        product.Stock = newStockValue;
    }
}