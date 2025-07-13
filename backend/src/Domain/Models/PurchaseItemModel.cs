namespace Backend.Domain.Models;

public class PurchaseItemModel(ProductModel product, int quantity)
{
    public ProductModel Product { get; } = product;
    public int Quantity { get; } = quantity;

    public decimal GetTotalPrice()
    {
        return Product.Price * Quantity;
    }
}


