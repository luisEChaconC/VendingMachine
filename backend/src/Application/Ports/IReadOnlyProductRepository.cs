using Backend.Domain;

namespace Backend.Application.Ports;

public interface IReadOnlyProductRepository
{
    IEnumerable<ProductModel> GetAllProducts();
}