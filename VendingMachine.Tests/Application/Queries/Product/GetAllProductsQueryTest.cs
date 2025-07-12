using AutoFixture;
using Backend.Application.Queries.Product;
using Backend.Application.Ports;
using Backend.Domain;
using Moq;

namespace VendingMachine.Tests.Application.Queries.Product;

[TestFixture]
public class GetAllProductsQueryTest
{
    private Fixture _fixture;
    private Mock<IReadOnlyProductRepository> _mockRepository;
    private GetAllProductsQuery _query;

    [SetUp]
    public void SetUp()
    {
        _fixture = new Fixture();
        _mockRepository = new Mock<IReadOnlyProductRepository>();
        _query = new GetAllProductsQuery(_mockRepository.Object);
    }

    [Test]
    public void Execute_ShouldReturnProducts_WhenProductsExist()
    {
        var products = _fixture.CreateMany<ProductModel>(3).ToList();
        _mockRepository.Setup(r => r.GetAllProducts()).Returns(products);

        var result = _query.Execute();

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count(), Is.EqualTo(3));
    }

    [Test]
    public void Execute_ShouldReturnEmptyList_WhenNoProductsExist()
    {
        _mockRepository.Setup(r => r.GetAllProducts()).Returns(new List<ProductModel>());

        var result = _query.Execute();

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count(), Is.EqualTo(0));
    }

    [Test]
    public void Execute_ShouldCallRepositoryOnlyOnce_WhenExecuted()
    {
        var products = _fixture.CreateMany<ProductModel>(2).ToList();
        _mockRepository.Setup(r => r.GetAllProducts()).Returns(products);

        _query.Execute();

        _mockRepository.Verify(r => r.GetAllProducts(), Times.Once);
    }
}




