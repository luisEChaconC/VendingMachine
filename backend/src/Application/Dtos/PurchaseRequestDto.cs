using System.ComponentModel.DataAnnotations;
using Backend.Domain.Models;

namespace Backend.Application.Dtos;

public class PurchaseRequestDto(IEnumerable<PurchaseItemRequestDto> purchaseItems, PaymentRequestDto payment)
{
    [Required]
    public IEnumerable<PurchaseItemRequestDto> PurchaseItems { get; } = purchaseItems;

    [Required]
    public PaymentRequestDto Payment { get; } = payment;

    public PurchaseModel ToModel(IEnumerable<PurchaseItemModel> purchaseItems)
    {
        return new PurchaseModel(
            purchaseItems,
            Payment.ToModel()
        );
    }
}