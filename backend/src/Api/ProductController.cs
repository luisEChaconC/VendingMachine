using Backend.Application.Queries.Product;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Api;

[ApiController]
[Route("api/products")]
public class ProductController(IGetAllProductsQuery getAllProductsQuery) : ControllerBase
{
    [HttpGet]
    public IActionResult GetAllProducts()
    {
        var products = getAllProductsQuery.Execute();
        return Ok(products);
    }
}
