using Backend.Domain.Enums;

namespace Backend.Application.Dtos;

public class ChangeResponseDto(Dictionary<DenominationEnum, int> denominations)
{
    public Dictionary<DenominationEnum, int> Denominations { get; } = denominations;
}


