using System.ComponentModel.DataAnnotations;
using Backend.Domain.Models;

namespace Backend.Application.Dtos;

public class PurchaseItemRequestDto(Guid productId, int quantity)
{
    [Required]
    public Guid ProductId { get; } = productId;

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
    public int Quantity { get; } = quantity;

    public PurchaseItemModel ToModel(ProductModel product)
    {
        return new PurchaseItemModel(product, Quantity);
    }
}


