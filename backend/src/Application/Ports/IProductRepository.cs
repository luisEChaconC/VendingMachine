namespace Backend.Application.Ports;

public interface IProductRepository : IReadOnlyProductRepository, IWriteOnlyProductRepository
{
}
