using Backend.Domain.Models;

namespace Backend.Application.Ports;

public interface IReadOnlyProductRepository
{
    IEnumerable<ProductModel> GetAllProducts();
    ProductModel? GetProductById(Guid id);
}