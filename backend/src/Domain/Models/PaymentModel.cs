using Backend.Domain.Enums;

namespace Backend.Domain.Models;

public class PaymentModel(Dictionary<DenominationEnum, int> denominations)
{
    public Dictionary<DenominationEnum, int> Denominations { get; } = denominations;

    public decimal GetTotalAmount()
    {
        return Denominations.Sum(x => (int)x.Key * x.Value);
    }
}


