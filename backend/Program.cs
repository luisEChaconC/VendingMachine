using Backend.Domain.Product;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();
