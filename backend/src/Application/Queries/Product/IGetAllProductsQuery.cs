using Backend.Application.Dtos;

namespace Backend.Application.Queries.Product;

public interface IGetAllProductsQuery
{
    IEnumerable<ProductResponseDto> Execute();
}
