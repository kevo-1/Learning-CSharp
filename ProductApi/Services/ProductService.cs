namespace Products.Services;

using Products.Models;

public interface IProductService
{
    public List<Product> GetAll();
    public Product? GetById(int id);
    public void Create(Product product);
    public bool Update(int id, Product product);
    public bool Delete(int id);
    public int GetNextId();
}

public class ProductService: IProductService
{
    private readonly List<Product> products = [];
    private readonly ILoggerService _logger;
    public ProductService(ILoggerService logger)
    {
        _logger = logger;
    }
    public List<Product> GetAll()
    {
        return products;
    }
    public Product? GetById(int id)
    {
        var product = products.FirstOrDefault(p => p.Id == id);
        if(product is null) 
            return null;
        return product;
    }
    public void Create(Product product)
    {
        products.Add(product);
        _logger.Log($"Product created: {product.Id}, {product.Name}, {product.Price}");
    }
    public bool Update(int id, Product product)
    {
        var resProd = products.FirstOrDefault(p => p.Id == id);
        if(resProd is null) 
            return false;
        resProd.Name = product.Name;
        resProd.Price = product.Price;
        _logger.Log($"Product updated: {product.Id}, {product.Name}, {product.Price}");
        return true;
    }
    public bool Delete(int id)
    {
        var product = products.FirstOrDefault(p => p.Id == id);
        if(product is null) 
            return false;
        products.Remove(product);
        _logger.Log($"Product deleted: {product.Id}, {product.Name}, {product.Price}");
        return true;
    }

    public int GetNextId()
    {
        if(products.Count == 0) 
            return 1;
        return products.Max(p => p.Id)+1;
    }
}