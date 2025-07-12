using Backend.Application.Ports;
using Backend.Domain.Models;

namespace Backend.Infrastructure.Product;

public class ProductRepository(IDictionary<Guid, ProductModel> products) : IReadOnlyProductRepository
{
    public IEnumerable<ProductModel> GetAllProducts()
    {
        return products.Values;
    }
}