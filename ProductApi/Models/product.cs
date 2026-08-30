namespace Products.Models;

public class Product
{
    public int Id { set; get; }
    public string Name { set; get; } = string.Empty;
    public decimal Price { set; get; }
}

public record ProductCreateDto(string Name, decimal Price);
public record ProductResponseDto(int Id, string Name, decimal Price);