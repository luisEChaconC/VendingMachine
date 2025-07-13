using System;
using Backend.Application.Dtos;
using Backend.Application.Ports;
using Backend.Domain.Models;
using Backend.Domain.Enums;
using Backend.Application.Errors;
using System.Collections.Generic;
using System.Linq;

namespace Backend.Application.Commands.Purchase;

public class PurchaseProductCommand(IProductRepository productRepository, ICashInventoryService cashInventoryService) : IPurchaseProductCommand
{

    public ChangeResponseDto Execute(PurchaseRequestDto purchaseRequest)
    {
        var purchase = this.BuildPurchaseModel(purchaseRequest);

        ValidateNoDuplicateProducts(purchase.PurchaseItems);

        VerifyStockAvailability(purchase.PurchaseItems);
        VerifyPaymentAmount(purchase);

        var changeDenominationQuantities = this.CalculateChangeDenominationQuantities(purchase.Payment, purchase.GetChangeAmount());

        this.AddPaymentToInventory(purchase.Payment);
        this.UpdateProductStock(purchase.PurchaseItems);
        this.RemoveChangeFromInventory(changeDenominationQuantities);

        return new ChangeResponseDto(changeDenominationQuantities);
    }

    private PurchaseModel BuildPurchaseModel(PurchaseRequestDto purchaseRequest)
    {
        var purchaseItems = this.BuildPurchaseItemModels(purchaseRequest.PurchaseItems);
        return purchaseRequest.ToModel(purchaseItems);
    }

    private IEnumerable<PurchaseItemModel> BuildPurchaseItemModels(IEnumerable<PurchaseItemRequestDto> purchaseItems)
    {
        foreach (var purchaseItem in purchaseItems)
        {
            var product = productRepository.GetProductById(purchaseItem.ProductId);

            if (product == null)
            {
                throw new PurchaseException(PurchaseErrorEnum.ProductNotFound, $"Could not find product with id {purchaseItem.ProductId}");
            }

            yield return purchaseItem.ToModel(product);
        }
    }

    static private void VerifyStockAvailability(IEnumerable<PurchaseItemModel> purchaseItems)
    {
        foreach (var purchaseItem in purchaseItems)
        {
            if (purchaseItem.Product.Stock < purchaseItem.Quantity)
            {
                throw new PurchaseException(PurchaseErrorEnum.InsufficientStock, $"Product {purchaseItem.Product.Name} does not have enough stock");
            }
        }
    }

    static private void VerifyPaymentAmount(PurchaseModel purchase)
    {
        if (purchase.GetTotalPurchasePrice() > purchase.GetPaidAmount())
        {
            throw new PurchaseException(PurchaseErrorEnum.InsufficientPayment, "The paid amount is less than the total purchase price");
        }
    }

    private void UpdateProductStock(IEnumerable<PurchaseItemModel> purchaseItems)
    {
        foreach (var purchaseItem in purchaseItems)
        {
            var newStockValue = purchaseItem.Product.Stock - purchaseItem.Quantity;
            productRepository.SetStock(purchaseItem.Product.Id, newStockValue);
        }
    }

    private void AddPaymentToInventory(PaymentModel payment)
    {
        foreach (var denomination in payment.Denominations)
        {
            var denominationType = denomination.Key;
            var quantity = denomination.Value;
            cashInventoryService.AddDenomination(denominationType, quantity);
        }
    }

    private Dictionary<DenominationEnum, int> CalculateChangeDenominationQuantities(PaymentModel payment, decimal changeAmount)
    {
        var simulatedInventory = this.GetSimulatedInventory(payment);

        var change = new Dictionary<DenominationEnum, int>();

        DenominationEnum[] denominations = [
            DenominationEnum.Bill1000,
            DenominationEnum.Coin500,
            DenominationEnum.Coin100,
            DenominationEnum.Coin50,
            DenominationEnum.Coin25
        ];

        foreach (var denomination in denominations)
        {
            var denominationValue = (int)denomination;
            var requiredQuantity = (int)Math.Floor(changeAmount / denominationValue);
            var availableQuantity = simulatedInventory[denomination];
            var quantityToUse = Math.Min(requiredQuantity, availableQuantity);

            if (quantityToUse > 0)
            {
                change[denomination] = quantityToUse;
                changeAmount -= denominationValue * quantityToUse;
            }

            if (changeAmount == 0)
            {
                break;
            }
        }

        if (changeAmount > 0)
        {
            throw new PurchaseException(PurchaseErrorEnum.InsufficientChange, "Not enough cash in inventory to give change");
        }

        return change;
    }

    private Dictionary<DenominationEnum, int> GetSimulatedInventory(PaymentModel payment)
    {
        var simulatedInventory = Enum.GetValues<DenominationEnum>()
            .ToDictionary(denomination => denomination, denomination => cashInventoryService.GetDenominationCount(denomination));

        foreach (var denomination in payment.Denominations)
        {
            simulatedInventory[denomination.Key] += denomination.Value;
        }

        return simulatedInventory;
    }

    private void RemoveChangeFromInventory(Dictionary<DenominationEnum, int> changeDenominationQuantities)
    {
        foreach (var pair in changeDenominationQuantities)
        {
            var denomination = pair.Key;
            var quantity = pair.Value;
            cashInventoryService.RemoveDenomination(denomination, quantity);
        }
    }

    private static void ValidateNoDuplicateProducts(IEnumerable<PurchaseItemModel> purchaseItems)
    {
        var seen = new HashSet<Guid>();
        foreach (var item in purchaseItems)
        {
            if (!seen.Add(item.Product.Id))
            {
                throw new PurchaseException(PurchaseErrorEnum.DuplicateProduct, $"Duplicate product detected in purchase request: {item.Product.Name} (id: {item.Product.Id})");
            }
        }
    }
}


