namespace Movie.Domain.Entities;

public class Movie
{
    public int Id {set; get;}
    public required string Title {set; get;}
    public required string Genre {set; get;}
    public required int ReleaseYear {set; get;} 
    public List<Review> Reviews { get; set; } = new();
}

public record MovieCreateDto(string Title, string Genre, int ReleaseYear);