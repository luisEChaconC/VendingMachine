using Backend.Domain.Enums;
using Backend.Application.Ports;

namespace Backend.Infrastructure.Services;

public class CashInventoryService(IDictionary<DenominationEnum, int> cashInventory) : ICashInventoryService
{
    public void AddDenomination(DenominationEnum denomination, int quantity)
    {
        cashInventory[denomination] += quantity;
    }

    public void RemoveDenomination(DenominationEnum denomination, int quantity)
    {
        cashInventory[denomination] -= quantity;
    }

    public int GetDenominationCount(DenominationEnum denomination)
    {
        return cashInventory[denomination];
    }
}