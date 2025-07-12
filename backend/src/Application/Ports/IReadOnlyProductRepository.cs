using Backend.Domain.Product;

namespace Backend.Application.Ports;

public interface IReadOnlyProductRepository
{
    IEnumerable<ProductModel> GetAllProducts();
}