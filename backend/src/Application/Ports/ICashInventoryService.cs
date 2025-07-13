using Backend.Domain.Enums;

namespace Backend.Application.Ports;

public interface ICashInventoryService
{
    void AddDenomination(DenominationEnum denomination, int quantity);
    void RemoveDenomination(DenominationEnum denomination, int quantity);
    int GetDenominationCount(DenominationEnum denomination);
}

