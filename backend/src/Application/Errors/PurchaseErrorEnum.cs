namespace Backend.Application.Errors;

public enum PurchaseErrorEnum
{
    ProductNotFound,
    InsufficientStock,
    InsufficientPayment,
    InsufficientChange,
    DuplicateProduct
}