using System.ComponentModel.DataAnnotations;
using Backend.Domain.Enums;
using Backend.Domain.Models;

namespace Backend.Application.Dtos;

public class PaymentRequestDto(Dictionary<DenominationEnum, int> denominations)
{
    [Required]
    [ValidDenominationQuantities]
    public Dictionary<DenominationEnum, int> Denominations { get; } = denominations;

    public PaymentModel ToModel()
    {
        return new PaymentModel(Denominations);
    }
}

public class ValidDenominationQuantitiesAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not Dictionary<DenominationEnum, int> denominations)
            return ValidationResult.Success!;

        var invalidValues = denominations.Where(pair => pair.Value <= 0).ToList();
        if (invalidValues.Count > 0)
        {
            return new ValidationResult($"The following denominations have invalid quantities (must be positive): {string.Join(", ", invalidValues.Select(pair => pair.Key))}");
        }

        return ValidationResult.Success!;
    }
}


