using Products.Models;
using Products.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<IProductService, ProductService>();
builder.Services.AddTransient<ILoggerService, ConsoleLoggerService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/products", (IProductService service) =>
{
    var products = service.GetAll();
    return products.Select(p => new ProductResponseDto(p.Id, p.Name, p.Price));
}).WithSummary("Returns all products");

app.MapGet(
    "/products/{id}",
    (int id, IProductService service) =>
    {
        var product = service.GetById(id);
        if(product is null) 
            return Results.NotFound();
        var result = new ProductResponseDto(product.Id, product.Name, product.Price);
        return Results.Ok(result);
    }
).WithSummary("Returns product by it's Id")
.Produces<ProductResponseDto>(200)
.Produces(404);

app.MapPost(
    "/products",
    (ProductCreateDto newProduct, IProductService service) =>
    {
        var errors = new Dictionary<string, string[]>();

        if (string.IsNullOrWhiteSpace(newProduct.Name))
            errors["Name"] = new[] { "Name is required!" };
        if (newProduct.Price <= 0)
            errors["Price"] = new[] { "Price should be positive" };

        if (errors.Count() > 0)
            return Results.ValidationProblem(errors);

        var product = new Product
        {
            Id = service.GetNextId(),
            Name = newProduct.Name,
            Price = newProduct.Price
        };
        service.Create(product);

        var resProduct = new ProductResponseDto(product.Id, product.Name, product.Price);
        return Results.Created($"/products/{resProduct.Id}", resProduct);
    }
).WithSummary("Creates a new product");

app.MapPut(
    "/products/{id}",
    (int id, ProductCreateDto updatedProd, IProductService service) =>
    {
        var product = service.GetById(id);
        if (product is null)
        {
            return Results.NotFound();
        }

        var errors = new Dictionary<string, string[]>();
        if (string.IsNullOrWhiteSpace(updatedProd.Name))
            errors["Name"] = new[] { "Name is required!" };
        if (updatedProd.Price <= 0)
            errors["Price"] = new[] { "Price should be positive" };

        if (errors.Count > 0)
            return Results.ValidationProblem(errors);
        var newProd = new Product{Id= id,Name= updatedProd.Name, Price= updatedProd.Price};
        service.Update(id, newProd);
        return Results.NoContent();
    }
).WithSummary("Updates an existing product");

app.MapDelete(
    "/products/{id}",
    (int id, IProductService service) =>
    {
        var product = service.GetById(id);
        if (product is null)
        {
            return Results.NotFound();
        }

        service.Delete(id);
        return Results.NoContent();
    }
).WithSummary("Deletes an existing product");

app.Run();
