using Backend.Domain.Models;
using Backend.Domain.Enums;
using Backend.Application.Queries.Product;
using Backend.Application.Commands.Purchase;
using Backend.Application.Ports;
using Backend.Infrastructure.Repositories.Product;
using Backend.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowVueApp",
        builder =>
        {
            builder.WithOrigins("http://localhost:8080")
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});

// Presentation layer dependencies
// Queries
builder.Services.AddScoped<IGetAllProductsQuery, GetAllProductsQuery>();

// Commands
builder.Services.AddScoped<IPurchaseProductCommand, PurchaseProductCommand>();

// Application layer dependencies
//Repositories
builder.Services.AddScoped<IReadOnlyProductRepository, ProductRepository>();
builder.Services.AddScoped<IWriteOnlyProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();

// Infrastructure layer dependencies
// Services
builder.Services.AddScoped<ICashInventoryService, CashInventoryService>();

// Product dictionary
builder.Services.AddSingleton<IDictionary<Guid, ProductModel>>(provider =>
{
    var productDictionary = new Dictionary<Guid, ProductModel>();

    var product1 = new ProductModel("Coca Cola", 800, 10, "coca-cola.png");
    var product2 = new ProductModel("Pepsi", 750, 8, "pepsi.png");
    var product3 = new ProductModel("Fanta", 950, 10, "fanta.png");
    var product4 = new ProductModel("Sprite", 975, 15, "sprite.png");

    productDictionary.Add(product1.Id, product1);
    productDictionary.Add(product2.Id, product2);
    productDictionary.Add(product3.Id, product3);
    productDictionary.Add(product4.Id, product4);

    return productDictionary;
});

// Cash Inventory
builder.Services.AddSingleton<IDictionary<DenominationEnum, int>>(provider =>
{
    var cashInventory = new Dictionary<DenominationEnum, int>
    {
        { DenominationEnum.Bill1000, 0 },
        { DenominationEnum.Coin500, 20 },
        { DenominationEnum.Coin100, 30 },
        { DenominationEnum.Coin50, 50 },
        { DenominationEnum.Coin25, 25 }
    };

    return cashInventory;
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowVueApp");

app.MapControllers();

app.Run();
