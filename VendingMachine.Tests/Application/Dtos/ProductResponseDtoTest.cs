using AutoFixture;
using Backend.Application.Dtos;
using Backend.Domain;

namespace VendingMachine.Tests.Application.Dtos;

[TestFixture]
public class ProductResponseDtoTest
{
    private Fixture _fixture;

    [SetUp]
    public void SetUp()
    {
        _fixture = new Fixture();
    }

    [Test]
    public void FromModel_ShouldMapAllProperties_WhenValidProductModel()
    {
        var productModel = _fixture.Create<ProductModel>();

        var result = ProductResponseDto.FromModel(productModel);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.EqualTo(productModel.Id));
        Assert.That(result.Name, Is.EqualTo(productModel.Name));
        Assert.That(result.Price, Is.EqualTo(productModel.Price));
        Assert.That(result.Stock, Is.EqualTo(productModel.Stock));
        Assert.That(result.ImagePath, Is.EqualTo(productModel.ImagePath));
    }
}