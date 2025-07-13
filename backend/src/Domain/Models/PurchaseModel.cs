using Microsoft.VisualBasic;

namespace Backend.Domain.Models;

public class PurchaseModel(IEnumerable<PurchaseItemModel> purchaseItems, PaymentModel payment)
{
    public IEnumerable<PurchaseItemModel> PurchaseItems { get; } = purchaseItems;
    public PaymentModel Payment { get; } = payment;

    public decimal GetTotalPurchasePrice()
    {
        return PurchaseItems.Sum(item => item.GetTotalPrice());
    }

    public decimal GetPaidAmount()
    {
        return Payment.GetTotalAmount();
    }

    public decimal GetChangeAmount()
    {
        var paidAmount = GetPaidAmount();
        var totalPurchasePrice = GetTotalPurchasePrice();

        if (paidAmount < totalPurchasePrice)
        {
            return 0;
        }

        return paidAmount - totalPurchasePrice;
    }
}

