using Backend.Application.Dtos;
using Backend.Application.Ports;

namespace Backend.Application.Queries.Product;

public class GetAllProductsQuery(IReadOnlyProductRepository productRepository) : IGetAllProductsQuery
{
    public IEnumerable<ProductResponseDto> Execute()
    {
        var products = productRepository.GetAllProducts();
        var productDtos = products.Select(ProductResponseDto.FromModel);
        return productDtos;
    }
}