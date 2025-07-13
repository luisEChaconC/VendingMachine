using Backend.Application.Dtos;

namespace Backend.Application.Commands.Purchase;

public interface IPurchaseProductCommand
{
    ChangeResponseDto Execute(PurchaseRequestDto purchaseRequest);
}


