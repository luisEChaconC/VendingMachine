using Backend.Domain.Models;

namespace Backend.Application.Ports;

public interface IWriteOnlyProductRepository
{
    void SetStock(Guid productId, int newStockValue);
}

