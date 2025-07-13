using AutoFixture;
using Backend.Application.Commands.Purchase;
using Backend.Application.Dtos;
using Backend.Application.Errors;
using Backend.Application.Ports;
using Backend.Domain.Enums;
using Backend.Domain.Models;
using Moq;

namespace VendingMachine.Tests.Application.Commands.Purchase;

[TestFixture]
public class PurchaseProductCommandTest
{
    private Mock<IProductRepository> _productRepositoryMock;
    private Mock<ICashInventoryService> _cashInventoryServiceMock;
    private PurchaseProductCommand _command;
    private Fixture _fixture;

    [SetUp]
    public void SetUp()
    {
        _productRepositoryMock = new Mock<IProductRepository>();
        _cashInventoryServiceMock = new Mock<ICashInventoryService>();
        _command = new PurchaseProductCommand(_productRepositoryMock.Object, _cashInventoryServiceMock.Object);
        _fixture = new Fixture();
    }

    [Test]
    public void Execute_ValidPurchaseWithExactPayment_ReturnsEmptyChange()
    {
        var product = _fixture.Build<ProductModel>()
            .With(p => p.Price, 100)
            .With(p => p.Stock, 5)
            .Create();

        var purchaseRequest = new PurchaseRequestDto(
            [new PurchaseItemRequestDto(product.Id, 2)],
            new PaymentRequestDto(new Dictionary<DenominationEnum, int>
            {
                { DenominationEnum.Coin100, 2 } // Exact payment: 2 * 100 = 200
            })
        );

        _productRepositoryMock.Setup(x => x.GetProductById(product.Id)).Returns(product);
        SetupCashInventory();

        var result = _command.Execute(purchaseRequest);

        Assert.That(result.Denominations, Is.Empty);
        _productRepositoryMock.Verify(x => x.SetStock(product.Id, 3), Times.Once);
        _cashInventoryServiceMock.Verify(x => x.AddDenomination(DenominationEnum.Coin100, 2), Times.Once);
    }

    [Test]
    public void Execute_ValidPurchaseWithChange_ReturnsCorrectChange()
    {
        var product = _fixture.Build<ProductModel>()
            .With(p => p.Price, 75)
            .With(p => p.Stock, 5)
            .Create();

        var purchaseRequest = new PurchaseRequestDto(
            [new PurchaseItemRequestDto(product.Id, 1)],
            new PaymentRequestDto(new Dictionary<DenominationEnum, int>
            {
                { DenominationEnum.Coin100, 1 } // Payment: 100, Price: 75, Change: 25
            })
        );

        _productRepositoryMock.Setup(x => x.GetProductById(product.Id)).Returns(product);
        SetupCashInventory();

        var result = _command.Execute(purchaseRequest);

        Assert.That(result.Denominations[DenominationEnum.Coin25], Is.EqualTo(1));
        _productRepositoryMock.Verify(x => x.SetStock(product.Id, 4), Times.Once);
        _cashInventoryServiceMock.Verify(x => x.AddDenomination(DenominationEnum.Coin100, 1), Times.Once);
        _cashInventoryServiceMock.Verify(x => x.RemoveDenomination(DenominationEnum.Coin25, 1), Times.Once);
    }

    [Test]
    public void Execute_ProductNotFound_ThrowsPurchaseException()
    {
        var productId = _fixture.Create<Guid>();
        var purchaseRequest = new PurchaseRequestDto(
            [new PurchaseItemRequestDto(productId, 1)],
            new PaymentRequestDto(new Dictionary<DenominationEnum, int>
            {
                { DenominationEnum.Coin100, 1 }
            })
        );

        _productRepositoryMock.Setup(x => x.GetProductById(productId)).Returns((ProductModel?)null);

        var ex = Assert.Throws<PurchaseException>(() => _command.Execute(purchaseRequest));
        Assert.That(ex.ErrorType, Is.EqualTo(PurchaseErrorEnum.ProductNotFound));
        Assert.That(ex.Message, Does.Contain(productId.ToString()));
    }

    [Test]
    public void Execute_InsufficientStock_ThrowsPurchaseException()
    {
        var product = _fixture.Build<ProductModel>()
            .With(p => p.Price, 100)
            .With(p => p.Stock, 2)
            .Create();

        var purchaseRequest = new PurchaseRequestDto(
            [new PurchaseItemRequestDto(product.Id, 5)], // Requests 5 but there are only 2 available
            new PaymentRequestDto(new Dictionary<DenominationEnum, int>
            {
                { DenominationEnum.Coin500, 1 }
            })
        );

        _productRepositoryMock.Setup(x => x.GetProductById(product.Id)).Returns(product);

        var ex = Assert.Throws<PurchaseException>(() => _command.Execute(purchaseRequest));
        Assert.That(ex.ErrorType, Is.EqualTo(PurchaseErrorEnum.InsufficientStock));
        Assert.That(ex.Message, Does.Contain(product.Name));
    }

    [Test]
    public void Execute_InsufficientPayment_ThrowsPurchaseException()
    {
        var product = _fixture.Build<ProductModel>()
            .With(p => p.Price, 200)
            .With(p => p.Stock, 5)
            .Create();

        var purchaseRequest = new PurchaseRequestDto(
            [new PurchaseItemRequestDto(product.Id, 1)],
            new PaymentRequestDto(new Dictionary<DenominationEnum, int>
            {
                { DenominationEnum.Coin100, 1 } // Payment: 100, Price: 200
            })
        );

        _productRepositoryMock.Setup(x => x.GetProductById(product.Id)).Returns(product);

        var ex = Assert.Throws<PurchaseException>(() => _command.Execute(purchaseRequest));
        Assert.That(ex.ErrorType, Is.EqualTo(PurchaseErrorEnum.InsufficientPayment));
    }

    [Test]
    public void Execute_InsufficientChange_ThrowsPurchaseException()
    {
        var product = _fixture.Build<ProductModel>()
            .With(p => p.Price, 25)
            .With(p => p.Stock, 5)
            .Create();

        var purchaseRequest = new PurchaseRequestDto(
            [new PurchaseItemRequestDto(product.Id, 1)],
            new PaymentRequestDto(new Dictionary<DenominationEnum, int>
            {
                { DenominationEnum.Coin100, 1 } // Payment: 100, Price: 25, Change needed: 75
            })
        );

        _productRepositoryMock.Setup(x => x.GetProductById(product.Id)).Returns(product);

        // Setup inventory with no change available
        _cashInventoryServiceMock.Setup(x => x.GetDenominationCount(It.IsAny<DenominationEnum>())).Returns(0);

        var ex = Assert.Throws<PurchaseException>(() => _command.Execute(purchaseRequest));
        Assert.That(ex.ErrorType, Is.EqualTo(PurchaseErrorEnum.InsufficientChange));
    }

    [Test]
    public void Execute_DuplicateProducts_ThrowsPurchaseException()
    {
        var product = _fixture.Build<ProductModel>()
            .With(p => p.Price, 100)
            .With(p => p.Stock, 10)
            .Create();

        var purchaseRequest = new PurchaseRequestDto(
            [
                new PurchaseItemRequestDto(product.Id, 1),
                new PurchaseItemRequestDto(product.Id, 2) // Duplicate product
            ],
            new PaymentRequestDto(new Dictionary<DenominationEnum, int>
            {
                { DenominationEnum.Coin500, 1 }
            })
        );

        _productRepositoryMock.Setup(x => x.GetProductById(product.Id)).Returns(product);

        var ex = Assert.Throws<PurchaseException>(() => _command.Execute(purchaseRequest));
        Assert.That(ex.ErrorType, Is.EqualTo(PurchaseErrorEnum.DuplicateProduct));
        Assert.That(ex.Message, Does.Contain(product.Name));
    }

    private void SetupCashInventory()
    {
        _cashInventoryServiceMock.Setup(x => x.GetDenominationCount(DenominationEnum.Bill1000)).Returns(10);
        _cashInventoryServiceMock.Setup(x => x.GetDenominationCount(DenominationEnum.Coin500)).Returns(20);
        _cashInventoryServiceMock.Setup(x => x.GetDenominationCount(DenominationEnum.Coin100)).Returns(30);
        _cashInventoryServiceMock.Setup(x => x.GetDenominationCount(DenominationEnum.Coin50)).Returns(50);
        _cashInventoryServiceMock.Setup(x => x.GetDenominationCount(DenominationEnum.Coin25)).Returns(25);
    }
}